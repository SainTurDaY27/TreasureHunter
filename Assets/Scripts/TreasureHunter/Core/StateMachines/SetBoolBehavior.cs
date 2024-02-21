using UnityEngine;
using UnityEngine.Animations;

namespace TreasureHunter.Core.StateMachines
{
    public class SetBoolBehavior : StateMachineBehaviour
    {
        public string boolName;

        public bool updateOnState;
        public bool updateOnStateMachine;
        public bool valueOnEnter, valueOnExit;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (updateOnState) animator.SetBool(boolName, valueOnEnter);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (updateOnState) animator.SetBool(boolName, valueOnExit);
        }

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
        {
            if (updateOnStateMachine) animator.SetBool(boolName, valueOnEnter);
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
        {
            if (updateOnStateMachine) animator.SetBool(boolName, valueOnExit);
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
        //}
    }
}