using System;
using System.Collections;
using System.IO;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Player;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

namespace TreasureHunter.Core.Data
{
    public class GameSaveManager
    {
        //private List<SaveGameData> _saveGameData;
        private SaveGameData _saveGameData;

        private const string _SAVE_GAME_SLOT_ONE_PATH = "/SavedGameDataSlotOne.json";
        private const string _SAVE_GAME_SLOT_TWO_PATH = "/SavedGameDataSlotTwo.json";
        private const string _SAVE_GAME_SLOT_THREE_PATH = "/SavedGameDataSlotThree.json";
        private MapPanel _mapPanel;

        public GameSaveManager()
        {
            _saveGameData = new SaveGameData();
        }

        /// <summary>
        /// Save game data to JSON file in the given slot.
        /// </summary>
        /// <param name="saveGameSlot">Slot to save</param>
        public void SaveGameData(SaveGameSlot saveGameSlot)
        {
            if (UIManager.Instance.TryGetUIByKey(UIKey.Map, out IBaseUI ui) && (ui is MapPanel panel))
            {
                _mapPanel = panel;
            }

            // _mapPanel.UpdateMapMarkerData();

            string path = GetSaveGamePath(saveGameSlot);
            GameData gameData = DataManager.Instance.GameData;
            PlayerData playerData = DataManager.Instance.PlayerData;
            Debug.Log("Save game to " + path);
            //Debug.Log("Current map area: " + gameData.CurrentMapArea);
            //Debug.Log("Obtained skill: " + playerData.GetObtainedSkills());
            Debug.Log("Last played time: " + gameData.GetLastPlayedTime());
            //Debug.Log("Explored map area: " + gameData.ExploredMapAreas);
            Debug.Log("Map marker data: " + gameData.GetMapMarkerData());
            foreach (var mapMarker in gameData.GetMapMarkerData())
            {
                Debug.Log(mapMarker);
            }
            //Debug.Log("Remaining map marker: " + gameData.GetRemainingMapMarker());
            SaveGameData saveGameData = new SaveGameData
            {
                currentMapArea = gameData.CurrentMapArea,
                obtainedSkill = playerData.GetObtainedSkills(),
                lastPlayedTime = gameData.GetLastPlayedTime().ToFileTime(),

                collectTreasures = gameData.GetCollectedTreasures(),

                exploredMapArea = gameData.ExploredMapAreas,
                mapMarkerData = gameData.GetMapMarkerData(),
                remainingMapMarker = gameData.GetRemainingMapMarker()
            };
            string saveGameDataJson = JsonUtility.ToJson(saveGameData);
            File.WriteAllText(path, saveGameDataJson);
            Debug.Log("Saved game data: " + saveGameDataJson);
        }

        public SaveGameData LoadGameData(SaveGameSlot saveGameSlot)
        {
            if (IsSaveGameExist(saveGameSlot))
            {
                _saveGameData = new SaveGameData();
                Debug.Log("Load saved game data from slot " + saveGameSlot);
                string path = GetSaveGamePath(saveGameSlot);
                if (File.Exists(path))
                {
                    var saveGameDataJson = File.ReadAllText(path);

                    // check if json can deserialize
                    if (string.IsNullOrEmpty(saveGameDataJson))
                    {
                        Debug.Log("Saved game data is empty");
                        return _saveGameData;
                    }

                    //_saveGameData.Add(JsonUtility.FromJson<SaveGameData>(saveGameDataJson));
                    _saveGameData = JsonUtility.FromJson<SaveGameData>(saveGameDataJson);
                }

                // debug all elements in the list
                //foreach (var saveGameData in _saveGameData)
                //{
                //    Debug.Log("Saved game data: " + saveGameData);
                //    //Debug.Log("Current map area: " + saveGameData.GetMapAreaKey());
                //    //Debug.Log("Obtained skill: " + saveGameData.GetObtainedSkills());
                //    //Debug.Log("Last played time: " + saveGameData.GetLastPlayedTime());
                //    //Debug.Log("Explored map area: " + saveGameData.GetExploredMapAreas());
                //    //Debug.Log("Map marker data: " + saveGameData.GetMapMarkerData());
                //    //Debug.Log("Remaining map marker: " + saveGameData.GetRemainingMapMarker());
                //}
                Debug.Log("Current map area: " + _saveGameData.GetMapAreaKey());
                Debug.Log("Obtained skill: " + _saveGameData.GetObtainedSkills());
                Debug.Log("Last played time: " + DateTime.FromFileTime(_saveGameData.GetLastPlayedTime()));
                Debug.Log("Explored map area: " + _saveGameData.GetExploredMapAreas());
                Debug.Log("Map marker data: " + _saveGameData.GetMapMarkerData());
                foreach (var mapMarker in _saveGameData.GetMapMarkerData())
                {
                    Debug.Log(mapMarker);
                }
                Debug.Log("Remaining map marker: " + _saveGameData.GetRemainingMapMarker());
                
                
            }

            return _saveGameData;
        }

        public bool IsSaveGameExist(SaveGameSlot saveGameSlot)
        {
            string path = GetSaveGamePath(saveGameSlot);
            return File.Exists(path);
        }

        private string GetSaveGamePath(SaveGameSlot saveGameSlot)
        {
            string path = "";
            switch (saveGameSlot)
            {
                case SaveGameSlot.SlotOne:
                    path = Application.persistentDataPath + _SAVE_GAME_SLOT_ONE_PATH;
                    break;
                case SaveGameSlot.SlotTwo:
                    path = Application.persistentDataPath + _SAVE_GAME_SLOT_TWO_PATH;
                    break;
                case SaveGameSlot.SlotThree:
                    path = Application.persistentDataPath + _SAVE_GAME_SLOT_THREE_PATH;
                    break;
            }

            return path;
        }
    }

    public enum SaveGameSlot
    {
        SlotOne = 1,
        SlotTwo = 2,
        SlotThree = 3
    }
}