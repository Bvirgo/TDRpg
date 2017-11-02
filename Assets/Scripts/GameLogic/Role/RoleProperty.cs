using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
public class RoleProperty : BaseActor
{
    #region Container
    private int m_nGold;
    private int m_nGem;
    private int m_nLevel;
    private int m_nPL;
    private int m_nBaseHp;
    private int m_nBaseDamage;
    private int m_nBasePower;
    private int m_nTotalHp;
    private int m_nTotalDamage;
    private int m_nTotalPower;
    private int m_nExp;
    private string m_strRoleName;

    private float fPLTimer = 0;
    private float fHpTimer = 0;

    private PropertyItem m_proGold;
    private PropertyItem m_proGem;
    private PropertyItem m_proLv;
    private PropertyItem m_proPL;
    private PropertyItem m_proHP;
    private PropertyItem m_proName;
    private PropertyItem m_proAttack;

    private List<SkillItem> m_pSkills;
    #endregion

    #region Init & Register
    protected override void OnPropertyChanged(int id, object oldValue, object newValue)
    {
        base.OnPropertyChanged(id, oldValue, newValue);

        RefreshUI();
    }

    protected override void InitContainer()
    {
        base.InitContainer();

        /// **************************
        ///	Base Info 
        /// **************************
        m_strRoleName = "马云";
        m_nExp = 1000;
        m_nGold = 1000;
        m_nGem = 100;
        m_nPL = 60;
        m_nLevel = m_nExp / 100;

        m_nBaseHp += m_nLevel * 50;
        m_nBaseDamage = m_nLevel * 30;
        m_nBasePower = m_nBaseHp + m_nBaseDamage;

        m_nTotalHp = m_nBaseHp;
        m_nTotalDamage = m_nBaseDamage;
        m_nTotalPower = m_nBasePower;

        AddProperty(PropertyType.Attack, m_nTotalPower);
        AddProperty(PropertyType.HP, m_nTotalHp);
        AddProperty(PropertyType.PL, m_nPL);
        AddProperty(PropertyType.Level, m_nLevel);
        AddProperty(PropertyType.Gold, m_nGold);
        AddProperty(PropertyType.Coin, m_nGem);
        AddProperty(PropertyType.RoleName, m_strRoleName);
        AddProperty(PropertyType.RoleID, guid);

        m_proAttack = GetProperty(PropertyType.Attack);
        m_proHP = GetProperty(PropertyType.HP);
        m_proPL = GetProperty(PropertyType.PL);
        m_proLv = GetProperty(PropertyType.Level);
        m_proGold = GetProperty(PropertyType.Gold);
        m_proGem = GetProperty(PropertyType.Coin);
        m_proName = GetProperty(PropertyType.RoleName);

        m_pSkills = new List<SkillItem>();     
    }

    protected override void Register()
    {
        base.Register();
        RegisterMsg(MsgType.Role_GetRoleInfo, GetRoleInfo);
        RegisterMsg(MsgType.Role_RefreshRoleProperty,GetInventory);
        RegisterMsg(MsgType.Role_GetSkillProperty,RefreshSkillInfo);
    }

    #endregion

    #region Inventory

    private void GetInventory(Message _msg)
    {
        int nSumHp = 0;
        int nSumDamage = 0;
        int nSumPower = 0;
        nSumPower = (int)_msg["power"];
        nSumHp = (int)_msg["hp"];
        nSumDamage = (int)_msg["damage"];
        m_nTotalHp = m_nBaseHp + nSumHp;
        m_nTotalDamage = m_nBaseDamage + nSumDamage;
        m_nTotalPower = m_nBasePower + nSumPower;

        RefreshUI();
    }
    #endregion

    #region Refresh UI
    void Update()
    {
        // Add HP & PL By Time
        if (this.m_nPL < 50)
        {
            fPLTimer += Time.deltaTime;
            if (fPLTimer > 60)
            {
                m_nPL += 1;
                m_proPL.Content = m_nPL;
                fPLTimer -= 60;
            }
        }
        else
        {
            this.fPLTimer = 0;
        }

        if (m_nBaseHp < 100)
        {
            fHpTimer += Time.deltaTime;
            if (fHpTimer > 60)
            {
                m_nBaseHp += 1;
                m_proHP.Content = m_nBaseHp;
                fHpTimer -= 60;

            }
        }
        else
        {
            fHpTimer = 0;
        }

        RefreshTimer();
    }

    private void RefreshUI()
    {
        Message msg = new Message(MsgType.Role_RefreshRoleInfo, this);
        msg["hp"] = m_nTotalHp;
        msg["pl"] = m_nPL;
        msg["gold"] = m_nGold;
        msg["gem"] = m_nGem;
        msg["lv"] = m_nLevel;
        msg["name"] = m_strRoleName;
        msg["attack"] = m_nTotalDamage;
        msg["power"] = m_nTotalPower;
        msg["exp"] = m_nExp;
        msg["plTimer"] = (int)fPLTimer;
        msg["hpTimer"] = (int)fHpTimer;
        msg.Send();
    }

    private void RefreshTimer()
    {
        Message msg = new Message(MsgType.Role_RefreshTimer, this);
        msg["plTimer"] = (int)fPLTimer;
        msg["hpTimer"] = (int)fHpTimer;
        msg.Send();
    }

    private void GetRoleInfo(Message _msg)
    {
        RefreshUI();
    }
    #endregion

    #region Skill
    private void RefreshSkillInfo(Message _msg)
    {
        Debug.Log("同步技能信息到角色");
        m_pSkills = _msg["skills"] as List<SkillItem>;
    }

    #endregion
}
