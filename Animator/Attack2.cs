using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerAttack.GetInstance().NormalAttack();
    }
}
