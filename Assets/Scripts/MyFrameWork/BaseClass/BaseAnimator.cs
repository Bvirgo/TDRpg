using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZFrameWork
{
    [RequireComponent(typeof(Animator))]
    /// <summary>
    /// Base Animator 
    /// </summary>
    public class BaseAnimator : MonoBehaviour
    {
        protected Animator m_animator;
        protected ActionType m_actType;

        void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_actType = ActionType.idel;
            OnAwake();
        }

        protected virtual void OnAwake() { }


        public virtual ActionType AnmType
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

    }
}

