using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZFrameWork;

public class VillageModule : BaseModule
{
    #region Container
    /// **************************
    /// All Equip Information
    /// **************************
    private Dictionary<int, Inventory> inVentorysID_inventoryDict;

    /// **************************
    /// Role Package Goods List
    /// **************************
    private List<InventoryItem> m_pInventoryItemList;

    private string m_strInventoryConfig;
    #endregion
    
    protected override void OnReady()
    {
        base.OnReady();

        ReadInventoryInfo();

        RandomInventoryItemInfo();
    }
    
    protected override void Register()
    {
        base.Register();

        RegisterMsg(MsgType.Role_GetRoleGoodsList, RefreshRoleEquipList);
    }

    protected override void InitContainer()
    {
        base.InitContainer();
        inVentorysID_inventoryDict = new Dictionary<int, Inventory>();
        m_pInventoryItemList = new List<InventoryItem>();
        m_strInventoryConfig = Application.streamingAssetsPath + "//TxtInfo//InventoryListinfo.txt";
    }

    void ReadInventoryInfo()
    {
        string str = Utils.ReadTextFile(m_strInventoryConfig);

        string[] itemStrArray = str.Split('\n');
        foreach (string itemStr in itemStrArray)
        {
            //ID 名称 图标 类型（Equip，Drug） 装备类型 售价 星级 品质 伤害 生命 战斗力 作用类型 作用值 描述
            // EquipType:(Helm,Cloth,Weapon,Shoes,Necklace,Bracelet,Ring,Wing)
            string[] proArray = itemStr.Split('|');
            Inventory inventory = new Inventory();
            inventory.ID = int.Parse(proArray[0]);
            inventory.Name = proArray[1];
            inventory.ICON = proArray[2];
            switch (proArray[3])
            {
                case "Equip":
                    inventory.InventoryTYPE = InventoryType.Equip;
                    break;
                case "Drug":
                    inventory.InventoryTYPE = InventoryType.Drug;
                    break;
                case "Box":
                    inventory.InventoryTYPE = InventoryType.Box;
                    break;
            }
            if (inventory.InventoryTYPE == InventoryType.Equip)
            {
                switch (proArray[4])
                {
                    case "Helm":
                        inventory.EquipTYPE = EquipType.Helm;
                        break;
                    case "Cloth":
                        inventory.EquipTYPE = EquipType.Cloth;
                        break;
                    case "Weapon":
                        inventory.EquipTYPE = EquipType.Weapon;
                        break;
                    case "Shoes":
                        inventory.EquipTYPE = EquipType.Shoes;
                        break;
                    case "Necklace":
                        inventory.EquipTYPE = EquipType.Necklace;
                        break;
                    case "Bracelet":
                        inventory.EquipTYPE = EquipType.Bracelet;
                        break;
                    case "Ring":
                        inventory.EquipTYPE = EquipType.Ring;
                        break;
                    case "Wing":
                        inventory.EquipTYPE = EquipType.Wing;
                        break;
                }

            }
            //print(itemStr);
            //售价 星级 品质 伤害 生命 战斗力 作用类型 作用值 描述
            inventory.Price = int.Parse(proArray[5]);
            if (inventory.InventoryTYPE == InventoryType.Equip)
            {
                inventory.StarLevel = int.Parse(proArray[6]);
                inventory.Quality = int.Parse(proArray[7]);
                inventory.Damage = int.Parse(proArray[8]);
                inventory.HP = int.Parse(proArray[9]);
                inventory.Power = int.Parse(proArray[10]);
            }
            if (inventory.InventoryTYPE == InventoryType.Drug)
            {
                inventory.ApplyValue = int.Parse(proArray[12]);
            }
            inventory.Des = proArray[13];
            inVentorysID_inventoryDict.Add(inventory.ID, inventory);
        }
    }

    /// <summary>
    /// Get Goods List For Role 
    /// </summary>
    void RandomInventoryItemInfo()
    {
        //TODO 需要链接服务器 取得当前角色拥有的物品信息
        //随机生成主角拥有的物品
        for (int j = 0; j < 20; j++)
        {
            int id = Random.Range(1001, 1020);
            Inventory i = null;
            inVentorysID_inventoryDict.TryGetValue(id, out i);

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
        //OnInventoryChange();
    }

    #region Inventory 
    private void RefreshRoleEquipList(Message _msg)
    {
        Message msg = new Message(MsgType.Role_RefreshPackage,this);
        msg["data"] = m_pInventoryItemList;
        msg.Send();
    }
    #endregion
}
