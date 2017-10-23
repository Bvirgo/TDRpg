using System;
using UnityEngine;
using System.Collections;
using System.Security.AccessControl;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BossMove : MonoBehaviour
{
    private Animator m_animator;
    private CharacterController m_charCtr;
    private int m_nSpeed=5;
    private List<Vector3> m_pList = new List<Vector3>();
    private Vector3 m_vTarget;

	// Use this for initialization
	void Start ()
	{
	    m_animator = transform.GetComponent<Animator>();
	    m_charCtr = transform.GetComponent<CharacterController>();
	    m_vTarget = transform.position;
        InitPos();
	}

    private void InitPos()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Cruise-Boss");

        for (int i = 0;i < obj.transform.childCount -1 ;++i)
        {
            Vector3 vPos = obj.transform.GetChild(i).position;
            vPos = new Vector3(vPos.x, transform.position.y, vPos.z);
            m_pList.Add(vPos);
        }
        Debug.Log("获取点："+m_pList.Count);
    }

    private Vector3 GetRandomPos()
    {
        int i = Random.Range(0, m_pList.Count -1);
        return m_pList[i];
    }

    // Update is called once per frame
	void Update ()
	{
	    float fDis = Vector3.Distance(m_vTarget, transform.position);
	    if (fDis > 3)
	    {
            transform.LookAt(m_vTarget);
	        m_charCtr.SimpleMove(transform.forward*m_nSpeed);
	        if (m_animator.GetInteger("Action") != 2)
	        {
                m_animator.SetInteger("Action", 2);
	        }
	       
	    }
	    else
	    {
	        if (m_animator.GetInteger("Action") != 1)
	        {
                m_animator.SetInteger("Action", 1);
	        }       
	        m_vTarget = GetRandomPos();
	    }
	}
}
