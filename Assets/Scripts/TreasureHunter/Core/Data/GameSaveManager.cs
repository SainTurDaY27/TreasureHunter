using System;
using System.Collections;
using System.IO;
using TreasureHunter.Gameplay.Player;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Core.Data
{
    public class GameSaveManager
    {
        private SaveGameData _saveGameData;

        private const string _SAVE_GAME_SLOT_ONE_PATH = "/SavedGameDataSlotOne.json";
        private const string _SAVE_GAME_SLOT_TWO_PATH = "/SavedGameDataSlotTwo.json";
        private const string _SAVE_GAME_SLOT_THREE_PATH = "/SavedGameDataSlotThree.json";
        public event Action OnSaveGameDataChanged;

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
            string path = GetSaveGamePath(saveGameSlot);
            GameData gameData = DataManager.Instance.GameData;
            PlayerData playerData = DataManager.Instance.PlayerData;
            SaveGameData saveGameData = new SaveGameData
            {
                currentMapArea = gameData.CurrentMapArea,
                playerPosition = playerData.GetPlayerPosition(),
                obtainedSkill = playerData.GetObtainedSkills(),
                startingSkill =  playerData.GetStartingSkills(),
                lastPlayedTime = gameData.GetLastPlayedTime().ToFileTime(),
                collectTreasures = gameData.GetCollectedTreasures(),
                trueBoolStates =  gameData.GetTrueBoolStates(),
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
                    _saveGameData = JsonUtility.FromJson<SaveGameData>(saveGameDataJson);
                }
            }
            return _saveGameData;
        }

        public bool IsSaveGameExist(SaveGameSlot saveGameSlot)
        {
            string path = GetSaveGamePath(saveGameSlot);
            return File.Exists(path);
        }

        public void DeleteSaveGame(SaveGameSlot saveGameSlot)
        {
            string path = GetSaveGamePath(saveGameSlot);
            if (File.Exists(path))
            {
                File.Delete(path);
                OnSaveGameDataChangedHandler();
            }
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

        public void OnSaveGameDataChangedHandler()
        {
            OnSaveGameDataChanged?.Invoke();
        }
    }

    public enum SaveGameSlot
    {
        SlotOne = 1,
        SlotTwo = 2,
        SlotThree = 3
    }
}