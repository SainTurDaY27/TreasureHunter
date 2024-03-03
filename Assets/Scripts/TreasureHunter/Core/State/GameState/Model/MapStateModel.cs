using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;

namespace TreasureHunter.Core.State.GameState
{
    public class MapStateModel : StateModel
    {
        private MapPanel _mapPanel;
        private GameManager _gameManager;
        private DataManager _dataManager;
        private int _treasureCount;
        private int _markerCount;

        public MapStateModel() : base((int)GameStates.State.Map, nameof(MapStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Game));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn();
            _gameManager = GameManager.Instance;
            _dataManager = DataManager.Instance;
            _mapPanel = (MapPanel)UIManager.Instance.Show(UIKey.Map);
            _dataManager.GameData.OnMapMarkerChanged += UpdateRemainingMarkerAmount;
            _dataManager.GameData.OnMapAreaExplored += UpdateInGameMap;
            UpdateTresureUI();
            UpdateRemainingMarkerAmount();
            _gameManager.PauseGame(true);
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            _dataManager.GameData.OnMapMarkerChanged -= UpdateRemainingMarkerAmount;
            _dataManager.GameData.OnMapAreaExplored -= UpdateInGameMap;
            UIManager.Instance.Hide(UIKey.Map);
            _gameManager.PauseGame(false);
        }

        private void UpdateTresureUI()
        {
            _treasureCount = DataManager.Instance.GameData.TreasureCount;
            _mapPanel.SetTreasureImage(_treasureCount);
        }

        private void UpdateInGameMap()
        {
            _mapPanel.UpdateMapUI(DataManager.Instance.GameData.ExploredMapAreas);
        }

        private void UpdateRemainingMarkerAmount()
        {
            _markerCount = DataManager.Instance.GameData.MapMarker;
            _mapPanel.SetMapMarkerRemaining(_markerCount);
        }
    }
}