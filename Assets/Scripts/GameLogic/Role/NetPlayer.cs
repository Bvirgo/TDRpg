using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class NetPlayer : BaseActor 
{
    public void MsgAttack(int _nPos)
    {
        ActionType at = ActionType.attack;
        int nPos = _nPos;
        switch (nPos)
        {
            case 1:
                at = ActionType.sk_1;
                break;
            case 2:
                at = ActionType.sk_2;
                break;
            case 3:
                at = ActionType.sk_3;
                break;
            default:
                break;
        }

        var role = RoleManager.Instance.OnGetMainPlayer();
        AttackAnimator(role.transform, at);
    }

    private void AttackAnimator(Transform _tf, ActionType _at)
    {
        RoleAnimator ra = _tf.GetComponent<RoleAnimator>();
        if (ra != null)
        {
            ra.AnmType = _at;
        }
    }
}
