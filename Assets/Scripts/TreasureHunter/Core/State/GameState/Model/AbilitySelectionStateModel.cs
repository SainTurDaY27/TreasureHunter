using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

namespace TreasureHunter.Core.State.GameState
{
    public class AbilitySelectionStateModel : StateModel
    {
        private AbilitySelectionPanel _abilitySelectionPanel;

        public AbilitySelectionStateModel() : base((int)GameStates.State.AbilitySelection, nameof(AbilitySelectionStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
            fromState: StateID,
            toState: (int)GameStates.State.Game));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn();
            _abilitySelectionPanel = (AbilitySelectionPanel)UIManager.Instance.Show(UIKey.AbilitySelection);
            _abilitySelectionPanel.OnPlayButtonClicked += PlayGame;
            _abilitySelectionPanel.OnSkillSelected += AddSelectedSkill;
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.Hide(UIKey.AbilitySelection);
            _abilitySelectionPanel.OnPlayButtonClicked -= PlayGame;
            _abilitySelectionPanel.OnSkillSelected -= AddSelectedSkill;
        }

        private void AddSelectedSkill(SkillKey skillKey)
        {
            DataManager.Instance.PlayerData.ObtainSkill(skillKey);
        }

        private void PlayGame()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Game);
        }
    }
}