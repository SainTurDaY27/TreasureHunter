using UnityEngine;

namespace TreasureHunter.Core.StateMachines
{
    public class SetFloatBehavior : StateMachineBehaviour
    {
        public string floatName;

        public bool updateOnStateEnter, updateOnStateExit;
        public bool updateOnStateMachineEnter, updateOnStateMachineExit;

        public float valueOnEnter, valueOnExit;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (updateOnStateEnter)
            {
                animator.SetFloat(floatName, valueOnEnter);
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        // public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        // {
        //     if (updateOnStateExit)
        //     {
        //         animator.SetFloat(floatName, valueOnExit);
        //     }
        // }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (updateOnStateExit)
            {
                animator.SetFloat(floatName, valueOnExit);
            }
        }

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (updateOnStateMachineEnter)
            {
                animator.SetFloat(floatName, valueOnEnter);
            }
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            if (updateOnStateMachineExit)
            {
                animator.SetFloat(floatName, valueOnExit);
            }
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}    }
    }
}