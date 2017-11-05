using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork; 

public class SkillModule : BaseModule
{
    #region Containers
    private List<SkillItem> m_pSkills;
    #endregion

    #region Init & Reigster
    protected override void OnReady()
    {
        base.OnReady();

        DataManager.Instance.ReadSkillConfig();

        RandomSkills();

        MonoHelper.Instance.UpdateRegister(Update);
    }

    protected override void InitContainer()
    {
        base.InitContainer();
        m_pSkills = new List<SkillItem>();
    }

    protected override void Register()
    {
        base.Register();
        RegisterMsg(MsgType.Role_PressSkillBtn, PressSkill);
    }

    private void RandomSkills()
    {
        for (int i = 1001; i < 1005; i++)
        {
            var sk = DataManager.Instance.OnGetSkillByID(i);
            if (sk != null)
            {
                SkillItem si = new SkillItem(sk);
                m_pSkills.Add(si);
            }
        }

        AsyncSkillInformation();
    }
    #endregion

    #region Skill UI 
    private void PressSkill(Message _msg)
    {
        int nPos = (int)_msg["pos"];
        if (0 == nPos)
        {
            // Attack
            BroadCastSkillPressed(m_pSkills[0]);
        }
        else if (nPos > -1 && nPos < m_pSkills.Count)
        {
            var sk = m_pSkills[nPos];
            if (!sk.m_bCD)
            {
                sk.m_bCD = true;
                // Animatior & Effect
                BroadCastSkillPressed(sk);
            }
        }

        AsyncSkillInformation();
    }

    private void BroadCastSkillPressed(SkillItem _sk)
    {
        Message msg = new Message(MsgType.Role_Fire, this);
        msg["pos"] = _sk.m_sk.m_nPos;
        msg.Send();
    }

    private void AsyncSkillInformation()
    {
        // 同步技能信息到角色身上
        Message msg = new Message(MsgType.Role_GetSkillProperty, this);
        msg["skills"] = m_pSkills;
        msg.Send();
    }

    private void Update()
    {
        for (int i = 0; i < m_pSkills.Count; i++)
        {
            var sk = m_pSkills[i];

            if (sk.m_bCD )
            {
                if (sk.m_fCDTimer > 0)
                {
                    sk.m_fCDTimer -= Time.deltaTime;
                }
                else
                {
                    sk.m_bCD = false;
                    sk.m_fCDTimer = sk.m_sk.m_nCD;
                }

                UpdateSkillCDUI(sk);
            }
        }
    }

    private void UpdateSkillCDUI(SkillItem _sk)
    {
        Message msg = new Message(MsgType.Role_UpdateSkillCD,this);
        float fPro = _sk.m_fCDTimer / _sk.m_sk.m_nCD;
        if (!_sk.m_bCD)
        {
            fPro = 0;
        }
        msg["progress"] = fPro;
        msg["pos"] = _sk.m_sk.m_nPos;
        msg.Send(); 
    }
    #endregion

}
