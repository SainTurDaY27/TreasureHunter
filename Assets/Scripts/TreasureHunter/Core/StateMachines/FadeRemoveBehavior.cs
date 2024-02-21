using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunter.Core.StateMachines
{
    public class FadeRemoveBehavior : StateMachineBehaviour
    {
        public float fadeTime = 0.5f;
        public float fadeDelay = 0f;

        private float fadeDelayElapsed = 0f;

        private float timeElapsed = 0f;

        private SpriteRenderer spriteRenderer;

        private GameObject objToRemove;

        private Color startColor;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timeElapsed = 0;
            spriteRenderer = animator.GetComponent<SpriteRenderer>();
            startColor = spriteRenderer.color;
            objToRemove = animator.gameObject;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (fadeDelay > fadeDelayElapsed)
            {
                fadeDelayElapsed += Time.deltaTime;
            }
            else
            {
                timeElapsed += Time.deltaTime;
                float newAlpha = startColor.a * (1 - timeElapsed / fadeTime);
                spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
                if (timeElapsed > fadeTime)
                {
                    Destroy(objToRemove);
                }
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}