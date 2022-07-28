using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuityAttack : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("Attack", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Attack3"))
        {
            animator.ResetTrigger("Attack");
        }
    }
}
