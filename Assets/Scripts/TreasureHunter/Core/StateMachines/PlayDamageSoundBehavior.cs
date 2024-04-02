using System;
using System.Collections.Generic;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Core.StateMachines
{
    [Serializable]
    public struct DamageSound
    {
        public int damageReceived;
        public AudioClip soundToPlay;
        public float volume;
    }

    public class PlayDamageSoundBehavior : StateMachineBehaviour
    {
        public List<DamageSound> damageSoundList;



        // Delay timer


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var damageSound in damageSoundList)
            {
                if (animator.GetInteger(AnimationStrings.DamageReceived) >= damageSound.damageReceived)
                {
                    var soundToPlay = damageSound.soundToPlay;
                    var volume = damageSound.volume;
                    AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
                }
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}