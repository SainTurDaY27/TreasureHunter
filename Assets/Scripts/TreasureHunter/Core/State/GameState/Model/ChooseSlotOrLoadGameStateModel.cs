using System.Diagnostics;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;

namespace TreasureHunter.Core.State.GameState
{
    public class ChooseSlotOrLoadGameStateModel : StateModel
    {
        private LoadGamePanel _loadGamePanel;
        private ChooseSaveSlotPanel _chooseSaveSlotPanel;
        private OnStateInCondition _condition;

        public ChooseSlotOrLoadGameStateModel() : base((int)GameStates.State.ChooseSlotOrLoadGame, nameof(ChooseSlotOrLoadGameStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
            fromState: StateID,
            toState: (int)GameStates.State.Game));
            GameStateManager.Instance.AddTransition(new StateTransition(
            fromState: StateID,
            toState: (int)GameStates.State.Menu));
            GameStateManager.Instance.AddTransition(new StateTransition(
            fromState: StateID,
            toState: (int)GameStates.State.SkillSelection));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn(args);

            _condition = OnStateInCondition.NewGame;
            if (args.Length > 0)
            {
                _condition = (OnStateInCondition)args[0];
            }

            if (_condition == OnStateInCondition.NewGame)
            {
                _chooseSaveSlotPanel = (ChooseSaveSlotPanel)UIManager.Instance.Show(UIKey.ChooseSaveSlot);
                _chooseSaveSlotPanel.OnContinueButtonClicked += ContinueToAbilitySelection;
                _chooseSaveSlotPanel.OnBackButtonClicked += BackToMenu;
            }
            else
            {
                _loadGamePanel = (LoadGamePanel)UIManager.Instance.Show(UIKey.LoadGame);
                _loadGamePanel.OnPlayButtonClicked += PlayLoadGame;
                _loadGamePanel.OnBackButtonClicked += BackToMenu;
            }
            UpdateSavedGameDataUI();
            DataManager.Instance.GameSaveManager.OnSaveGameDataChanged += UpdateSavedGameDataUI;
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            if (_condition == OnStateInCondition.NewGame)
            {
                UIManager.Instance.Hide(UIKey.ChooseSaveSlot);
                _chooseSaveSlotPanel.OnContinueButtonClicked -= ContinueToAbilitySelection;
                _chooseSaveSlotPanel.OnBackButtonClicked -= BackToMenu;
                _chooseSaveSlotPanel.DeselectAllButton();
            }
            else
            {
                UIManager.Instance.Hide(UIKey.LoadGame);
                _loadGamePanel.OnPlayButtonClicked -= PlayLoadGame;
                _loadGamePanel.OnBackButtonClicked -= BackToMenu;
                _loadGamePanel.DeselectAllButton();
            }
            DataManager.Instance.GameSaveManager.OnSaveGameDataChanged -= UpdateSavedGameDataUI;
        }

        private void PlayLoadGame()
        {
            var loadGameSlot = _loadGamePanel.GetSelectedSaveGameSlot();
            DataManager.Instance.GameData.SetCurrentSaveGameSlot(loadGameSlot);

            // TODO: Remove this line later
            UnityEngine.Debug.Log("Set save slot to: " + DataManager.Instance.GameData.GetCurrentSaveGameSlot());

            GameStateManager.Instance.GoToState((int)GameStates.State.Game, BackToGameMethod.LoadGame);
        }

        private void BackToMenu()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
        }

        private void ContinueToAbilitySelection()
        {
            ChooseSaveSlot();
            GameStateManager.Instance.GoToState((int)GameStates.State.SkillSelection);
        }

        private void ChooseSaveSlot()
        {
            SaveGameSlot saveGameSlot = _chooseSaveSlotPanel.GetSelectedSaveGameSlot();
            DataManager.Instance.GameData.SetCurrentSaveGameSlot(saveGameSlot);
        }

        private void UpdateSavedGameDataUI()
        {
            if (_condition == OnStateInCondition.NewGame)
            {
                _chooseSaveSlotPanel.UpdateSavedGameDataUI();
            }
            else
            {
                _loadGamePanel.UpdateSavedGameDataUI();
            }
        }
    }

    public enum OnStateInCondition
    {
        NewGame,
        LoadGame
    }
}