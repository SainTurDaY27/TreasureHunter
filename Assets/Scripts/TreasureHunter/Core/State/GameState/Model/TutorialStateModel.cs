using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;

namespace TreasureHunter.Core.State.GameState
{
    public class TutorialStateModel : StateModel
    {
        private TutorialPanel _tutorialPanel;

        public TutorialStateModel() : base((int)GameStates.State.Tutorial, nameof(TutorialStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Game));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Menu));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn();
            GameManager.Instance.PauseGame(true);
            _tutorialPanel = (TutorialPanel)UIManager.Instance.Show(UIKey.Tutorial);
            _tutorialPanel.OnBackToGameButtonClicked += BackToGame;
            _tutorialPanel.OnMainMenuButtonClicked += () => SetActivePopUpWarning(true);
            _tutorialPanel.OnConfirmPopUpButtonClicked += BackToMenu;
            _tutorialPanel.OnBackPopUpButtonClicked += () => SetActivePopUpWarning(false);
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            GameManager.Instance.PauseGame(false);
            UIManager.Instance.Hide(UIKey.Tutorial);
            _tutorialPanel.OnBackToGameButtonClicked -= BackToGame;
            _tutorialPanel.OnMainMenuButtonClicked -= () => SetActivePopUpWarning(true);
            _tutorialPanel.OnConfirmPopUpButtonClicked -= BackToMenu;
            _tutorialPanel.OnBackPopUpButtonClicked -= () => SetActivePopUpWarning(false);
        }

        private void BackToGame()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Game, BackToGameMethod.ContinueGame);
        }

        private void BackToMenu()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
        }

        private void SetActivePopUpWarning(bool isActive)
        {
            if (isActive)
            {
                _tutorialPanel.SetTutorialPanelButtonInteractable(false);
                _tutorialPanel.SetPopUpPanelActive(true);
            }
            else
            {
                _tutorialPanel.SetTutorialPanelButtonInteractable(true);
                _tutorialPanel.SetPopUpPanelActive(false);
            }
        }
    }
}