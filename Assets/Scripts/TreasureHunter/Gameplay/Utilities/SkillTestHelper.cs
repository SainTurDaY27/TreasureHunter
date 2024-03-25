using System;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.State.GameState;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Utilities;
using UnityEngine;

namespace TreasureHunter.Gameplay.Utilities
{
    public class SkillTestHelper : MonoSingleton<SkillTestHelper>
    {
        public event Action OnSkillChanged;

        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        // TODO: Remove this later, just use for print out the key to toggle skill
        public override void Awake()
        {
            base.Awake();
            Debug.Log("Toggle skill button list\n" +
                "Alpha1 : DoubleJump\n" +
                "Alpha2 : Dash\n" +
                "Alpha3 : WallJump\n" +
                "Alpha4 : FireBall\n" +
                "Alpha5 : Shrink\n");
        }
        #endif

        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        private void Update()
        {
            if (GameStateManager.Instance.CurrentState.StateID != (int)GameStates.State.Game)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ToggleSkill(SkillKey.DoubleJump);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ToggleSkill(SkillKey.Dash);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ToggleSkill(SkillKey.WallJump);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ToggleSkill(SkillKey.Fireball);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ToggleSkill(SkillKey.Shrink);
            }

            // Test save and load game
            if (Input.GetKeyDown(KeyCode.O))
            {
                DataManager.Instance.SaveGame(SaveGameSlot.SlotOne);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                DataManager.Instance.LoadSavedGame(SaveGameSlot.SlotOne);
            }
        }
        #endif

        private void ToggleSkill(SkillKey skillKey)
        {
            bool toggleStatus = DataManager.Instance.PlayerData.ToggleSkill(skillKey);
            Debug.Log($"{skillKey} set to {toggleStatus}");
            OnSkillChangedHandler();
        }

        public void OnSkillChangedHandler()
        {
            OnSkillChanged?.Invoke();
        }
    }
}