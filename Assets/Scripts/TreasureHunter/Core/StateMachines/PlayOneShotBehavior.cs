using UnityEngine;

namespace TreasureHunter.Core.StateMachines
{
    public class PlayOneShotBehavior : StateMachineBehaviour
    {
        public AudioClip soundToPlay;

        public float volume = 1f;

        public bool playOnEnter = true;

        public bool playOnExit = false;

        public bool playAfterDelay = false;

        // Delay timer
        public float playDelay = 0.25f;

        private float timeSinceEntered = 0;

        private bool hasDelayedSoundPlayer = false;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (playOnEnter)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
            }

            timeSinceEntered = 0;
            hasDelayedSoundPlayer = false;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (playAfterDelay && !hasDelayedSoundPlayer)
            {
                timeSinceEntered += Time.deltaTime;
                if (timeSinceEntered >= playDelay)
                {
                    AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
                    hasDelayedSoundPlayer = true;
                }
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (playOnExit)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
            }
        
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
