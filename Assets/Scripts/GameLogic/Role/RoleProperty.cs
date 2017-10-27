using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
public class RoleProperty : BaseActor
{
    private int m_nGold;
    private int m_nGem;
    private int m_nLevel;
    private int m_nPL;
    private int m_nHp;
    private int m_nAttack;
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

    protected override void OnPropertyChanged(int id, object oldValue, object newValue)
    {
        base.OnPropertyChanged(id, oldValue, newValue);
        RefreshUI();
    }

    protected override void InitContainer()
    {
        m_nExp = 10000;
        m_nGold = 1000;
        m_nGem = 100;
        m_nHp = 90;
        m_nPL = 60;
        m_nLevel = m_nExp / 100;
        m_nAttack = 999;
        m_strRoleName = "马云";

        AddProperty(PropertyType.Attack, m_nAttack);
        AddProperty(PropertyType.HP, m_nHp);
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

        RefreshUI();
    }

    protected override void Register()
    {
        base.Register();
        RegisterMsg(MsgType.Role_GetRoleInfo, GetRoleInfo);
    }

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

        if (m_nHp < 100)
        {
            fHpTimer += Time.deltaTime;
            if (fHpTimer > 60)
            {
                m_nHp += 1;
                m_proHP.Content = m_nHp;
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
        msg["hp"] = m_nHp;
        msg["pl"] = m_nPL;
        msg["gold"] = m_nGold;
        msg["gem"] = m_nGem;
        msg["lv"] = m_nLevel;
        msg["name"] = m_strRoleName;
        msg["attack"] = m_nAttack;
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
    
}
