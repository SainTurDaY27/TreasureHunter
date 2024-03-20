using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

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
            _mapPanel.ResetAllMarkers();
            _mapPanel.OnMapMarkerPlaced += AddMapMarker;
            _mapPanel.OnMapMarkerRemoved += RemoveMapMarker;
            UpdateInGameMap();
            UpdateTresureUI();
            UpdateRemainingMarkerAmount();
            _gameManager.PauseGame(true);
        }
        

        public override void OnStateOut()
        {
            base.OnStateOut();
            _dataManager.GameData.OnMapMarkerChanged -= UpdateRemainingMarkerAmount;
            _dataManager.GameData.OnMapAreaExplored -= UpdateInGameMap;
            _mapPanel.ResetAllMarkers();
            _mapPanel.OnMapMarkerPlaced -= AddMapMarker;
            _mapPanel.OnMapMarkerRemoved -= RemoveMapMarker;
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
            _mapPanel.LoadMapMarkerFromData();
        }

        private void UpdateRemainingMarkerAmount()
        {
            _markerCount = DataManager.Instance.GameData.RemainingMapMarker;
            _mapPanel.SetMapMarkerRemaining(_markerCount);
        }

        private void AddMapMarker(Vector2 mapMarker)
        {
            _dataManager.GameData.AddMapMarker(mapMarker);
            UpdateRemainingMarkerAmount();
        }

        private void RemoveMapMarker(Vector2 mapMarker)
        {
            _dataManager.GameData.RemoveMapMarker(mapMarker);
            UpdateRemainingMarkerAmount();
        }
    }
}