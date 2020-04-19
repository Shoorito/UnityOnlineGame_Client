using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(PlayerAttack.GetInstance() == null)
        {
            Debug.LogError("플레이어 공격 클래스가 생성되어 있지 않습니다!");

            return;
        }

        PlayerAttack.GetInstance().NormalAttack();
    }
}
