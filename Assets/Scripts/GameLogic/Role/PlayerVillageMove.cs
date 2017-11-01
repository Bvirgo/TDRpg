using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;
using ZFrameWork;
public class PlayerVillageMove : MonoBehaviour
{

    private float velocity = 15;
    private float smoothing = 10;

    private RoleAnimator m_roleAnmCtr;
    private NavMeshAgent m_nav;
    private Vector3 offset;
    private Transform mainCamera;
    
    private Vector3 vTarget;
    private Action m_actArriaved;

    void Start()
    {
        m_roleAnmCtr = GetComponent<RoleAnimator>();
        if (m_roleAnmCtr == null)
        {
            m_roleAnmCtr = gameObject.AddComponent<RoleAnimator>();
        }
        m_nav = this.GetComponent<NavMeshAgent>();
        m_nav.enabled = false;
        mainCamera = Camera.main.transform;
        offset = mainCamera.position - transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f)
        {
            ManaulMove(h,v);
            m_nav.enabled = false;
        }
        else
        {
            m_roleAnmCtr.AnmType = ActionType.idel;
        }

        if (m_nav.enabled)
        {
            NavMove();
        }
    }

    private void ManaulMove(float _fH,float _fV)
    {
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        GetComponent<Rigidbody>().velocity = new Vector3(_fH * velocity, vel.y, _fV * velocity);

        Quaternion qTarget = Quaternion.LookRotation(new Vector3(_fH, 0, _fV));
        transform.rotation = Quaternion.Lerp(transform.rotation, qTarget, smoothing * Time.deltaTime);

        m_roleAnmCtr.AnmType = ActionType.run;
    }

    private void NavMove()
    {
        float f = Vector3.Distance(transform.position, vTarget);
        if (f > 3)
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
    
    void FixedUpdate()
    {
        Vector3 targetPos = transform.position + offset;
        mainCamera.position = Vector3.Lerp(mainCamera.position, targetPos, smoothing * Time.deltaTime);
    }
    public void SetTarget(Vector3 _vPos, Action _cbDone = null)
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