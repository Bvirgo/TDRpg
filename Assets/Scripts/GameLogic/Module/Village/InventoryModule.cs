using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
public class InventoryModule : BaseModule
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

    private int nSumHp = 0;
    private int nSumDamage = 0;
    private int nSumPower = 0;
    #endregion

    #region Init & Register

    protected override void OnReady()
    {
        base.OnReady();
        RandomInventoryItemInfo();
    }

    protected override void Register()
    {
        base.Register();

        RegisterMsg(MsgType.Role_GetRoleGoodsList, RefreshRoleEquipList);

        RegisterMsg(MsgType.Role_Equip, EquipRole);

        RegisterMsg(MsgType.Role_Dequip, EquipRole);

        RegisterMsg(MsgType.Role_UseGoods, UseGoods);

        RegisterMsg(MsgType.Role_MultUse, MultUseGoods);
    }

    protected override void InitContainer()
    {
        base.InitContainer();
        m_pDressInventoryItemList = new List<InventoryItem>();
        m_pUnDressInventoryItemList = new List<InventoryItem>();
        m_pInventoryItemList = new List<InventoryItem>();
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
        nSumHp = 0;
        nSumDamage = 0;
        nSumPower = 0;

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

        RefreshRoleProperty();
    }

    private void RefreshInventoryPanel()
    {
        Message msg = new Message(MsgType.Role_RefreshRoleInventory, this);
        msg["data"] = m_pInventoryItemList;
        msg.Send();
    }

    private void RefreshRoleProperty()
    {
        Message msg = new Message(MsgType.Role_RefreshRoleProperty, this);
        msg["power"] = nSumPower;
        msg["hp"] = nSumHp;
        msg["damage"] = nSumDamage;
        msg.Send();
    }
    #endregion

    #region Package Event
    private void RefreshRoleEquipList(Message _msg)
    {
        RefreshInventoryPanel();
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

        RefreshInventoryPanel();
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
        RefreshInventoryPanel();
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

        RefreshInventoryPanel();
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
        RefreshInventoryPanel();
    }
    #endregion
}
