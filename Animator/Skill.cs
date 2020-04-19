using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerAttack.GetInstance().SkillAttack();
    }
}
