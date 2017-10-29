using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
public class DataManager : Singleton<DataManager>
{
    #region Containers
    /// **************************
    /// All Equip Information
    /// **************************
    private Dictionary<int, Inventory> inVentorysID_inventoryDict;
    private string m_strInventoryConfig;
    #endregion

    #region

    public override void Init()
    {
        base.Init();
        inVentorysID_inventoryDict = new Dictionary<int, Inventory>();
        m_strInventoryConfig = Application.streamingAssetsPath + "//TxtInfo//InventoryListinfo.txt";
    }

    /// <summary>
    /// Load Inventory Config
    /// </summary>
    /// <returns></returns>
    public void  ReadInventoryInfo()
    {
        string str = Utils.ReadTextFile(m_strInventoryConfig);

        string[] itemStrArray = str.Split('\n');
        foreach (string itemStr in itemStrArray)
        {

            /// **************************
            ///	ID 名称 图标 类型（Equip，Drug） 装备类型 售价 星级 品质 伤害 生命 战斗力 作用类型 作用值 描述
            ///	EquipType:(Helm,Cloth,Weapon,Shoes,Necklace,Bracelet,Ring,Wing)
            /// **************************
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

            /// **************************
            ///	售价 星级 品质 伤害 生命 战斗力 作用类型 作用值 描述 
            /// **************************
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

    public Inventory OnGetInventory(int _nID)
    {
        if (inVentorysID_inventoryDict.ContainsKey(_nID))
        {
            return inVentorysID_inventoryDict[_nID];
        }
        return null;
    }
    #endregion
}
