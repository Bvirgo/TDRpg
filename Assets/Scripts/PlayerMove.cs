using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine.UI;
public class PlayerMove : MonoBehaviour
{

    public int m_nSpeed = 15;
    private Animator m_animator;
    private Rigidbody m_rigibody;
    private CharacterController m_charCtr;
    private UnityEngine.AI.NavMeshAgent m_nav;
    private GameObject m_camera;
    private Vector3 vOffset;
    private Vector3 vTarget;
    public Transform m_Target;
    public GameObject m_npc1;
    public GameObject m_npc2;
    private float fY;

    private EffectCtr m_effectCtr;
	// Use this for initialization
    void Awake()
    {
        m_animator = transform.GetComponent<Animator>();
        //m_rigibody = transform.GetComponent<Rigidbody>();
        m_charCtr = transform.GetComponent<CharacterController>();
        m_nav = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_nav.enabled = false;
        m_camera = Camera.main.gameObject;
        vOffset = m_camera.transform.position - transform.position;
        fY = transform.position.y;
        Transform tf = transform.Find("Skill1");
        if (tf != null)
        {
            m_effectCtr = tf.GetComponent<EffectCtr>();
            gameObject.GetComponentInChildren<EffectCtr>();
        }
      
    }
	
	// Update is called once per frame
	void Update ()
	{
	    Vector3 vOld = transform.position;
        float hDir = Input.GetAxis("Horizontal");
        float vDir = Input.GetAxis("Vertical");

        //--是否落地判断:如果没有落地，需要自己处理重力，SimpleMove处理重力太轻飘飘
        if(!m_charCtr.SimpleMove(new Vector3(-vDir * m_nSpeed, transform.localPosition.y, hDir * m_nSpeed)))
        {
            RaycastHit hit;
            //--检测角色离开地面的高度，获取角色正下方的地面点，每一帧都同步一下这个Y值，让角色一直贴着地面
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000f, LayerMask.GetMask("Ground")))
            {
                Vector3 vPos = hit.point;
                transform.position = new Vector3(transform.position.x,vPos.y,transform.position.z);
            }
        }

        Vector3 vSpeed = new Vector3(-vDir,0, hDir);
        //transform.rotation = Quaternion.LookRotation(vSpeed);
        //if (vSpeed.magnitude > 0.05f)
        //{
        //    m_animator.SetBool("bRun", true);
        //}
        //else
        //{
        //    m_animator.SetBool("bRun", false);
        //}
        if (Mathf.Abs(hDir) > 0.1f || Mathf.Abs(vDir) > 0.1f)
        {
            m_animator.SetBool("bRun", true);

            transform.rotation = Quaternion.LookRotation(vSpeed);
            m_nav.enabled = false;
        }
        else
        {
            m_animator.SetBool("bRun", false);
        }

	    if (m_nav.enabled)
	    {
	        float f = Vector3.Distance(transform.position, vTarget);
            if (f> 3)
            {
                m_animator.SetBool("bRun", true);
            }
            else
            {
                m_nav.Stop();
                m_nav.enabled = false;
                m_animator.SetBool("bRun", false);
            }
	    }
        //transform.position = new Vector3(transform.position.x,fY,transform.position.z);
	    m_camera.transform.position = transform.position + vOffset;

        //测试
        if(Input.GetMouseButtonDown(0) && m_effectCtr != null)
        {
            m_effectCtr.Show();
            //DG.Tweening.ShortcutExtensions.DOMove(transform, transform.position + Vector3.forward, 0.3f);
        }
	}

    private void SetTarget(Vector3 _vPos)
    {
        if (!m_nav.enabled)
        {
            m_nav.enabled = true;
        }
        m_nav.SetDestination(_vPos);
        m_nav.speed = 20;
    }

    public void SetNPC1()
    {
        vTarget = m_npc1.transform.position;
       SetTarget(m_npc1.transform.position);
    }

    public void SetNPC2()
    {
        vTarget = m_npc2.transform.position;
        SetTarget(m_npc2.transform.position);
    }
}
