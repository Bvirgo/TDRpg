using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
public class RoleProperty : BaseActor
{
    #region Container
    /// **************************
    /// Role Package Goods List
    /// **************************
    private List<InventoryItem> m_pInventoryItemList;
    /// **************************
    ///	Dressed Equips 
    /// **************************
    private List<InventoryItem> m_pUnDressInventoryItemList;
    private List<InventoryItem> m_pDressInventoryItemList;

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

    /// **************************
    ///	Equip Guid
    /// **************************
    private string m_strHelm;
    private string m_strCloth;
    private string m_strWeapon;
    private string m_strShoes;
    private string m_strNecklace;
    private string m_strBracelet;
    private string m_strRing;
    private string m_strWing;

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
        m_nExp = 1000;
        m_nGold = 1000;
        m_nGem = 100;
        m_nPL = 60;
        m_nLevel = m_nExp / 100;

        m_nBaseHp += m_nLevel * 50;
        m_nBaseDamage = m_nLevel * 30;
        m_nBasePower = m_nBaseHp + m_nBaseDamage;
        m_strRoleName = "马云";

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

        m_pDressInventoryItemList = new List<InventoryItem>();
        m_pUnDressInventoryItemList = new List<InventoryItem>();
        m_pInventoryItemList = new List<InventoryItem>();

        // Random Test Inventory List
        RandomInventoryItemInfo();
    }

    protected override void Register()
    {
        base.Register();
        RegisterMsg(MsgType.Role_GetRoleInfo, GetRoleInfo);

        RegisterMsg(MsgType.Role_GetRoleGoodsList, RefreshRoleEquipList);

        RegisterMsg(MsgType.Role_Equip, EquipRole);

        RegisterMsg(MsgType.Role_Dequip, EquipRole);

        RegisterMsg(MsgType.Role_UseGoods, UseGoods);

        RegisterMsg(MsgType.Role_MultUse, MultUseGoods);
    }

    #endregion

    #region Inventory
    /// <summary>
    /// Get Goods List For Role 
    /// </summary>
    void RandomInventoryItemInfo()
    {
        /// **************************
        ///	TODO 需要链接服务器 取得当前角色拥有的物品信息 
        ///	随机生成主角拥有的物品
        /// **************************
        for (int j = 0; j < 20; j++)
        {
            int id = Random.Range(1001, 1020);
            Inventory i = DataManager.Instance.OnGetInventory(id);

            if (i.InventoryTYPE == InventoryType.Equip)
            {
                InventoryItem it = new InventoryItem();
                it.Inventory = i;
                it.Level = Random.Range(1, 10);
                it.Count = 1;
                m_pInventoryItemList.Add(it);
            }
            else
            {
                //先判断背包里面是否已经存在
                InventoryItem it = null;
                bool isExit = false;
                foreach (InventoryItem temp in m_pInventoryItemList)
                {
                    if (temp.Inventory.ID == id)
                    {
                        isExit = true;
                        it = temp;
                        break;
                    }
                }
                if (isExit)
                {
                    it.Count++;
                }
                else
                {
                    it = new InventoryItem();
                    it.Inventory = i;
                    it.Count = 1;
                    m_pInventoryItemList.Add(it);
                }
            }
        }

        Debug.Log("--------------Read Equip:" + m_pInventoryItemList.Count);

        DespDerss();
    }

    private void DespDerss()
    {
        m_pDressInventoryItemList = m_pInventoryItemList.FindAll((item) => { return item.IsDressed; });
        m_pUnDressInventoryItemList = m_pInventoryItemList.FindAll((item) => { return !item.IsDressed; });
        DressEquip();
    }

    private void DressEquip()
    {
        int nSumHp=0;
        int nSumDamage=0;
        int nSumPower = 0;

        for (int i = 0; i < m_pDressInventoryItemList.Count; i++)
        {
            var item = m_pDressInventoryItemList[i];
            nSumDamage += item.Level * 50 + item.Inventory.Damage;
            nSumHp += item.Level * 100 + item.Inventory.HP;
            nSumPower += nSumDamage + item.Inventory.Power + nSumHp;

            switch (item.Inventory.EquipTYPE)
            {
                case EquipType.Helm:
                    m_strHelm = item.guid;
                    break;
                case EquipType.Cloth:
                    m_strCloth = item.guid;
                    break;
                case EquipType.Weapon:
                    m_strWeapon = item.guid;
                    break;
                case EquipType.Shoes:
                    m_strShoes = item.guid;
                    break;
                case EquipType.Necklace:
                    m_strNecklace = item.guid;
                    break;
                case EquipType.Bracelet:
                    m_strBracelet = item.guid;
                    break;
                case EquipType.Ring:
                    m_strRing = item.guid;
                    break;
                case EquipType.Wing:
                    m_strWing = item.guid;
                    break;
                default:
                    break;
            }
        }

        m_nTotalHp = m_nBaseHp + nSumHp;
        m_nTotalDamage = m_nBaseDamage + nSumDamage;
        m_nTotalPower = m_nBasePower + nSumPower;

        RefreshUI();
    }
    #endregion
   
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

    #region Package Event
    private void RefreshRoleEquipList(Message _msg)
    {
        Message msg = new Message(MsgType.Role_RefreshRoleInventory, this);
        msg["data"] = m_pInventoryItemList;
        msg["power"] = m_nTotalPower;
        msg["hp"] = m_nTotalHp;
        msg.Send();
    }

    /// <summary>
    /// Equip Role
    /// </summary>
    /// <param name="_msg"></param>
    private void EquipRole(Message _msg)
    {
        string guid = _msg["data"].ToString();
        var iItem = m_pInventoryItemList.Find((item) => { return item.guid.Equals(guid); });
        if (_msg.Name.Equals(MsgType.Role_Dequip))
        {
            iItem.IsDressed = false;
        }
        else
        {
            var dressedEquip = m_pInventoryItemList.Find((item) =>
            {
                return (item.Inventory.EquipTYPE == iItem.Inventory.EquipTYPE) && (item.IsDressed);
            });

            if (dressedEquip != null)
            {
                dressedEquip.IsDressed = false;
            }
            iItem.IsDressed = true;
        }

        DespDerss();
        RefreshRoleEquipList(_msg);
    }

    /// <summary>
    /// Uprade Role
    /// </summary>
    /// <param name="_msg"></param>
    private void UpgradeRole(Message _msg)
    {
        string guid = _msg["data"].ToString();
        var iItem = m_pInventoryItemList.Find((item) => { return item.guid.Equals(guid); });
        iItem.Level++;

        RefreshRoleEquipList(_msg);
    }
    /// <summary>
    /// Use Goods
    /// </summary>
    /// <param name="_msg"></param>
    private void UseGoods(Message _msg)
    {
        string guid = _msg["data"].ToString();
        var ii = m_pInventoryItemList.Find((item) => { return item.guid.Equals(guid); });
        if (ii.Count > 1)
        {
            ii.Count--;
        }
        else
        {
            m_pInventoryItemList.Remove(ii);
        }

        RefreshRoleEquipList(_msg);
    }

    /// <summary>
    /// Mult Use Goods
    /// </summary>
    /// <param name="_msg"></param>
    private void MultUseGoods(Message _msg)
    {
        string guid = _msg["data"].ToString();
        var ii = m_pInventoryItemList.Find((item) => { return item.guid.Equals(guid); });
        m_pInventoryItemList.Remove(ii);
        RefreshRoleEquipList(_msg);
    }
    #endregion
}
