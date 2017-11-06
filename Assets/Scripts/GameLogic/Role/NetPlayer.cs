using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class NetPlayer : BaseActor 
{

    #region AsyncMove
    public void AsyncMove(Vector3 nPos, Vector3 nRot)
    {
        NetPlayerMove npm = gameObject.GetComponent<NetPlayerMove>();
        if (npm != null)
        {
            npm.NetForecastInfo(nPos, nRot);
        }
    }

    #endregion
    #region AsyncSkill
    public void AsyncAttack(int _nPos)
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

        AttackAnimator(transform, at);
    }

    private void AttackAnimator(Transform _tf, ActionType _at)
    {
        RoleAnimator ra = _tf.GetComponent<RoleAnimator>();
        if (ra != null)
        {
            ra.AnmType = _at;
        }
    }
    #endregion

}
