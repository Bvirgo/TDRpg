using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine.UI;
using ZFrameWork;
using System;

public class PlayerMove : MonoBehaviour
{
    [Header("Speed:")]
    public int m_nSpeed = 15;
    private RoleAnimator m_roleAnmCtr;
    private Rigidbody m_rigibody;
    private CharacterController m_charCtr;
    private UnityEngine.AI.NavMeshAgent m_nav;
    private GameObject m_camera;
    private Vector3 vOffset;
    private Vector3 vTarget;
    private Transform m_Target;
    private float fY;

    private EffectCtr[] m_pEffectCtr;
    private Action m_actArriaved;
    private int m_nEffectIndex;
    void Start()
    {
        m_roleAnmCtr = GetComponent<RoleAnimator>();
        if (m_roleAnmCtr == null)
        {
            m_roleAnmCtr = gameObject.AddComponent<RoleAnimator>();
        }
        m_charCtr = transform.GetComponent<CharacterController>();
        m_nav = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_nav.enabled = false;
        m_camera = Camera.main.gameObject;
        vOffset = m_camera.transform.position - transform.position;
        fY = transform.position.y;
        Transform tf = transform.Find("Skill1");
        m_nEffectIndex = 0;
        if (tf != null)
        {
            m_pEffectCtr = gameObject.GetComponentsInChildren<EffectCtr>();
        }
      
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (m_roleAnmCtr.IsAttack)
        {
            return;
        }

	    Vector3 vOld = transform.position;
        float hDir = Input.GetAxis("Horizontal");
        float vDir = Input.GetAxis("Vertical");

        /// **************************
        /// 这里特殊处理一下，按理说，h是x正向，v是Z正向
        /// 调整地图轴向和角色轴向一致
        /// 但是，这个地图把出生地放在了h的反向
        /// **************************
        hDir = -hDir;
        
        /// **************************
        ///	是否落地判断:如果没有落地，需要自己处理重力，SimpleMove处理重力太轻飘飘 
        /// **************************
        if (!m_charCtr.SimpleMove(new Vector3(vDir * m_nSpeed, transform.localPosition.y, hDir * m_nSpeed)))
        {
            RaycastHit hit;
            //--检测角色离开地面的高度，获取角色正下方的地面点，每一帧都同步一下这个Y值，让角色一直贴着地面
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000f, LayerMask.GetMask("Ground")))
            {
                Vector3 vPos = hit.point;
                transform.position = new Vector3(transform.position.x,vPos.y,transform.position.z);
            }
        }
        
        Vector3 vSpeed = new Vector3(vDir, 0, hDir);
        if (Mathf.Abs(hDir) > 0.1f || Mathf.Abs(vDir) > 0.1f)
        {
            m_roleAnmCtr.AnmType = ActionType.run;
            transform.rotation = Quaternion.LookRotation(vSpeed);
            m_nav.enabled = false;
        }
        else
        {
            m_roleAnmCtr.AnmType = ActionType.idel;
        }

	    if (m_nav.enabled)
	    {
	        float f = Vector3.Distance(transform.position, vTarget);
            if (f> 3)
            {
                m_roleAnmCtr.AnmType = ActionType.run;
            }
            else
            {
                m_nav.isStopped = true;
                m_nav.enabled = false;
                m_roleAnmCtr.AnmType = ActionType.idel;
                if (m_actArriaved != null)
                {
                    m_actArriaved();
                }
            }
	    }
	    m_camera.transform.position = transform.position + vOffset;

        //测试技能特效
        //if(Input.GetMouseButtonDown(0) && m_pEffectCtr != null && m_pEffectCtr.Length > 1)
        //{
        //    m_nEffectIndex++;
        //    m_nEffectIndex = m_nEffectIndex >= m_pEffectCtr.Length ? 0 : m_nEffectIndex;
        //    m_nEffectIndex = m_nEffectIndex < 0 ? m_pEffectCtr.Length : m_nEffectIndex;
        //    TestAttackEffect();
        //}
	}

    private void TestAttackEffect()
    {
        m_pEffectCtr = gameObject.GetComponentsInChildren<EffectCtr>();
        m_pEffectCtr[m_nEffectIndex].Show();
    }

    public void SetTarget(Vector3 _vPos,Action _cbDone = null)
    {
        if (!m_nav.enabled)
        {
            m_nav.enabled = true;
        }
        m_actArriaved = _cbDone;
        vTarget = _vPos;
        m_nav.SetDestination(_vPos);
        m_nav.speed = 20;
    }
}
