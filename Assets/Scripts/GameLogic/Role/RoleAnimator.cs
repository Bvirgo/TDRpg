using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

[RequireComponent(typeof(Animator))]
public class RoleAnimator : MonoBehaviour
{
    private Animator m_animator;
    private ActionType m_actType;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_actType = ActionType.idel;
    }

    public ActionType AnmType
    {
       get { return m_actType; }
       set
        {
            m_actType = value;
            switch (m_actType)
            {
                case ActionType.idel:
                    m_animator.SetBool("bRun", false);
                    break;
                case ActionType.run:
                    m_animator.SetBool("bRun", true);
                    break;
                case ActionType.attack:
                    m_animator.SetTrigger("atk");
                    break;
                case ActionType.die:
                    m_animator.SetTrigger("bDie");
                    break;
                case ActionType.hurt:
                    m_animator.SetTrigger("bHit");
                    break;
                case ActionType.sk_1:
                    m_animator.SetTrigger("s1");
                    break;
                case ActionType.sk_2:
                    m_animator.SetTrigger("s2");
                    break;
                case ActionType.sk_3:
                    m_animator.SetTrigger("s3");
                    break;
                default:
                    break;
            }
        }
    }

    private bool m_bCanMove;

    /// **************************
    ///	When Attack Dont Move 
    /// **************************
    public bool IsAttack
    {
        get
        {
            if (m_animator.layerCount > 1)
            {
                m_bCanMove = m_animator.GetCurrentAnimatorStateInfo(1).IsName("EmptyStatus");
            }
            else
            {
                m_bCanMove = true;
            }

            return !m_bCanMove;
        }
        
    }

}

public enum ActionType
{
    idel,
    attack,
    run,
    die,
    hurt,
    sk_1,
    sk_2,
    sk_3,    
}
