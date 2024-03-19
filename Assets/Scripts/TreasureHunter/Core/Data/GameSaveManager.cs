using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Map;
using TreasureHunter.Gameplay.Player;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;
using UnityEngine;
using static TreasureHunter.Gameplay.System.GameData;

namespace TreasureHunter.Core.Data
{
    public class SaveGameData
    {
        public MapAreaKey CurrentMapArea;
        public List<SkillKey> ObtainedSkill;
        public DateTime LastPlayedTime;

        // TODO: Add treasure save data
        public HashSet<string> CollectTreasures;

        public MapAreaKey[] ExploredMapArea;
        public List<MapMarkerData> MapMarkerData;
        public int RemainingMapMarker;

        public MapAreaKey GetMapAreaKey()
        {
            return CurrentMapArea;
        }

        public List<SkillKey> GetObtainedSkills()
        {
            // debug all elements in the list
            foreach (var skill in ObtainedSkill)
            {
                Debug.Log("Obtained skill: " + skill);
            }

            return ObtainedSkill;
        }

        public DateTime GetLastPlayedTime()
        {
            return LastPlayedTime;
        }

        public HashSet<string> GetCollectedTreasures()
        {
            foreach (var treasure in CollectTreasures)
            {
                Debug.Log("Treasure collected: " + treasure);
            }

            return CollectTreasures;
        }

        public MapAreaKey[] GetExploredMapAreas()
        {
            foreach (var mapAreaKey in ExploredMapArea)
            {
                Debug.Log("Explored map area: " + mapAreaKey);
            }
            return ExploredMapArea;
        }

        public List<MapMarkerData> GetMapMarkerDatas()
        {
            foreach (var markerData in MapMarkerData)
            {
                if (markerData != null)
                {
                    Debug.Log("Map marker data: " + markerData);
                }
            }
            return MapMarkerData;
        }

        public int GetRemainingMapMarker()
        {
            return RemainingMapMarker;
        }
    }

    public class GameSaveManager
    {
        private List<SaveGameData> _saveGameDataList;
        private const string _SAVE_GAME_SLOT_ONE_PATH = "/SavedGameDataSlotOne.json";
        private const string _SAVE_GAME_SLOT_TWO_PATH = "/SavedGameDataSlotTwo.json";
        private const string _SAVE_GAME_SLOT_THREE_PATH = "/SavedGameDataSlotThree.json";
        private MapPanel _mapPanel;

        public GameSaveManager()
        {
            _saveGameDataList = new List<SaveGameData>();
            //if (UIManager.Instance.TryGetUIByKey(UIKey.Map, out IBaseUI ui) && (ui is MapPanel panel))
            //{
            //    _mapPanel = panel;
            //    Debug.Log("Map panel found: " + _mapPanel);
            //}
        }

        public void SaveGameData(SaveGameSlot saveGameSlot)
        {
            if (UIManager.Instance.TryGetUIByKey(UIKey.Map, out IBaseUI ui) && (ui is MapPanel panel))
            {
                _mapPanel = panel;
            }
            _mapPanel.UpdateMapMarkerData();

            string path = GetSaveGamePath(saveGameSlot);
            GameData gameData = DataManager.Instance.GameData;
            PlayerData playerData = DataManager.Instance.PlayerData;
            Debug.Log("Save game to " + path);
            Debug.Log("Current map area: " + gameData.CurrentMapArea);
            Debug.Log("Obtained skill: " + playerData.GetObtainedSkills());
            Debug.Log("Last played time: " + gameData.GetLastPlayedTime());
            Debug.Log("Explored map area: " + gameData.ExploredMapAreas);
            Debug.Log("Map marker data: " + gameData.GetMapMarkerData());
            Debug.Log("Remaining map marker: " + gameData.GetRemainingMapMarker());
            SaveGameData saveGameData = new SaveGameData
            {
                CurrentMapArea = gameData.CurrentMapArea,
                ObtainedSkill = playerData.GetObtainedSkills(),
                LastPlayedTime = gameData.GetLastPlayedTime(),

                // TODO: Add treasure save data
                //CollectTreasures = gameData.GetCollectedTreasures(),

                ExploredMapArea = gameData.ExploredMapAreas,
                MapMarkerData = gameData.GetMapMarkerData(),
                RemainingMapMarker = gameData.GetRemainingMapMarker()
            };
            string saveGameDataJson = JsonUtility.ToJson(saveGameData);
            File.WriteAllText(path, saveGameDataJson);
            Debug.Log("Saved game data: " + saveGameDataJson);
        }

        public List<SaveGameData> LoadGameData(SaveGameSlot saveGameSlot)
        {
            if (IsSaveGameExist(saveGameSlot))
            {
                _saveGameDataList.Clear();
                Debug.Log("Load saved game data from slot " + saveGameSlot);
                Debug.Log("Path: " + GetSaveGamePath(saveGameSlot));
                string path = GetSaveGamePath(saveGameSlot);
                if (File.Exists(path))
                {
                    var saveGameDataJson = File.ReadAllText(path);

                    // check if json can deserialize
                    if (string.IsNullOrEmpty(saveGameDataJson))
                    {
                        Debug.Log("Saved game data is empty");
                        return _saveGameDataList;
                    }
                    _saveGameDataList.Add(JsonUtility.FromJson<SaveGameData>(saveGameDataJson));
                }

                // debug all elements in the list
                foreach (var saveGameData in _saveGameDataList)
                {
                    Debug.Log("Current map area: " + saveGameData.GetMapAreaKey());
                    Debug.Log("Obtained skill: " + saveGameData.GetObtainedSkills());
                    Debug.Log("Last played time: " + saveGameData.GetLastPlayedTime());
                    Debug.Log("Explored map area: " + saveGameData.GetExploredMapAreas());
                    Debug.Log("Map marker data: " + saveGameData.GetMapMarkerDatas());
                    Debug.Log("Remaining map marker: " + saveGameData.GetRemainingMapMarker());
                }
            }
            return _saveGameDataList;
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