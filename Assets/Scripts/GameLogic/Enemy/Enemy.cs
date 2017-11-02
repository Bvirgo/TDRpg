using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class Enemy : BaseActor
{
    private int m_nHp;
    private int m_nCurHp;

    private EnemyAnimator m_anm;
    
    protected override void InitContainer()
    {
        base.InitContainer();
        m_nHp = 100;
        m_nCurHp = m_nHp;
        m_anm = GetComponent<EnemyAnimator>();
    }

    protected override void OnReady()
    {
        base.OnReady(); 
    }

    //收到攻击调用这个方法
    // 0,收到多少伤害
    // 1,后退的距离
    // 2,浮空的高度
    void TakeDamage(Message  _msg)
    {
        if (m_nCurHp <= 0)
        {
            Dead();
            return;
        } 

        // 减去伤害值
        int damage = (int)_msg["damage"];
        m_nCurHp -= damage;
        float fHpPro = (float)m_nCurHp / m_nHp;

        // 受到攻击的动画
        m_anm.AnmType = ActionType.hurt;

        // 浮空和后退
        float backDistance = (float)_msg["bDis"];
        float jumpHeight = (float)_msg["jDis"];
        Vector3 vDir = (Vector3)_msg["dir"];
        vDir = new Vector3(0,0,-1);
        iTween.MoveBy(this.gameObject,
            transform.InverseTransformDirection(vDir) * backDistance + Vector3.up * jumpHeight,
            0.3f);

        if (jumpHeight > 0.1)
        {
            StartCoroutine(DropDown());
        }

        //出血特效实例化
        GameObject.Instantiate(DataManager.Instance.OnGetSkillEffect("BloodSplatEffect"), transform.position + Vector3.forward, Quaternion.identity);
        if (m_nCurHp <= 0)
        {
            Dead();
        }
    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        Dead();
    //    }
    //}

    IEnumerator DropDown()
    {
        yield return new WaitForSeconds(0.3f);
        RaycastHit hit;
        bool collider = Physics.Raycast(transform.position, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));
        if (collider)
        {
            float fDis = transform.position.y - hit.point.y;
            fDis = fDis > 0 ? fDis : -fDis;
            iTween.MoveBy(this.gameObject, Vector3.down * fDis, 0.2f);
        }
    }

    //当死亡的时候调用这个方法
    void Dead()
    {
        m_anm.AnmType = ActionType.die;
        
        //TranscriptManager._instance.enemyList.Remove(this.gameObject);
        //this.GetComponent<CharacterController>().enabled = false;
        //Destroy(hpBarGameObject);
        //Destroy(hudTextGameObject);
        ////第一种死亡方式是播放死亡动画
        ////第二种死亡方式是使用破碎效果
        //int random = Random.Range(0, 10);
        //if (random <= 7)
        //{
        //    GetComponent<Animation>().Play("die");
        //}
        //else
        //{
        //    this.GetComponentInChildren<MeshExploder>().Explode();
        //    this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        //}
    }
}
