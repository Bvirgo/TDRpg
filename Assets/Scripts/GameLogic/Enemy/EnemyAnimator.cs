using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class EnemyAnimator : BaseAnimator
{
    public override ActionType AnmType
    {
        get
        {
            return m_actType ;    
        }

        set
        {
            m_actType = value;
            switch (m_actType)
            {
                case ActionType.idel:
                    m_animator.SetInteger("iAction", 1001);
                    break;
                case ActionType.attack:
                    m_animator.SetTrigger("atk");
                    break;
                case ActionType.run:
                    m_animator.SetInteger("iAction", 1003);
                    break;
                case ActionType.die:
                    m_animator.SetTrigger("die");
                    break;
                case ActionType.hurt:
                    m_animator.SetTrigger("hurt");
                    break;
                case ActionType.sk_1:
                    m_animator.SetInteger("iAction", 2);
                    break;
                case ActionType.sk_2:
                    break;
                case ActionType.sk_3:
                    break;
                default:
                    break;
            }
        }
    }
}
