using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

[RequireComponent(typeof(Animator))]
public class RoleAnimator : BaseAnimator
{
  
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

