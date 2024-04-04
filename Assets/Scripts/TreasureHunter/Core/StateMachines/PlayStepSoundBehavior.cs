using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Core.StateMachines
{
    public class PlayStepSoundBehavior : StateMachineBehaviour
    {
        public AudioClip soundToPlay;
        public float volume = 1f;
        public float playDelay = 0.25f;

        private float playTimer = 0;
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            playTimer = 0f;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            playTimer += Time.deltaTime;
            if (playTimer >= playDelay)
            {
                if (animator.GetBool(AnimationStrings.IsGrounded))
                {
                    AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
                }

                playTimer = 0f;
            }
        }
    }
}