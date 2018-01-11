using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using UnityEngine.UI;

public class UIPackagePanel : BasePanel
{
    #region UI
    /**|Role Knapsack|**/
    [HideInInspector,AutoUGUI]
    public Image Spr_Helm;
    [HideInInspector, AutoUGUI]
    public Image Spr_Cloth;
    [HideInInspector, AutoUGUI]
    public Image Spr_Weapon;
    [HideInInspector, AutoUGUI]
    public Image Spr_Shoes;
    [HideInInspector, AutoUGUI]
    public Image Spr_Necklace;
    [HideInInspector, AutoUGUI]
    public Image Spr_Bracelet;
    [HideInInspector, AutoUGUI]
    public Image Spr_Ring;
    [HideInInspector, AutoUGUI]
    public Image Spr_Wing;
    [HideInInspector, AutoUGUI]
    public Text Txt_Hp;
    [HideInInspector, AutoUGUI]
    public Text Txt_Power;
    [HideInInspector, AutoUGUI]
    public Slider Slider_Exp;

    /**|Package|**/
    [HideInInspector,AutoUGUI]
    public Transform ItemGrid;
    [HideInInspector, AutoUGUI]
    public Transform UIGoodsTips;
    [HideInInspector, AutoUGUI]
    public Text Txt_Index;
    [HideInInspector, AutoUGUI]
    public Text Txt_Value;
    [HideInInspector, AutoUGUI]
    public Button Btn_Left;
    [HideInInspector, AutoUGUI]
    public Button Btn_Right;
    [HideInInspector, AutoUGUI]
    public Button Btn_ClosePackage;
    [HideInInspector, AutoUGUI]
    public Button Btn_CloseGoodsTips;
    [HideInInspector, AutoUGUI]
    public Button Btn_Use;
    [HideInInspector, AutoUGUI]
    public Button Btn_MultUse;
    [HideInInspector, AutoUGUI]
    public Text Txt_GoodsShowName;
    [HideInInspector, AutoUGUI]
    public Text Txt_GoodsDes;
    [HideInInspector,AutoUGUI]
    public Image Spr_GoodsInfoIcon;

    /**|Equip Info Panel|**/
    [HideInInspector, AutoUGUI]
    public Transform UIEquipTips;
    [HideInInspector, AutoUGUI]
    public Button Btn_CloseTips;
    [HideInInspector, AutoUGUI]
    public Button Btn_Equip;
    [HideInInspector, AutoUGUI]
    public Button Btn_Upgrade;
    [HideInInspector, AutoUGUI]
    public Text Txt_Equiptitle;
    [HideInInspector, AutoUGUI]
    public Text Txt_Quality;
    [HideInInspector, AutoUGUI]
    public Text Txt_Level;
    [HideInInspector, AutoUGUI]
    public Text Txt_AddDamage;
    [HideInInspector, AutoUGUI]
    public Text Txt_AddPower;
    [HideInInspector, AutoUGUI]
    public Text Txt_AddHP;
    [HideInInspector, AutoUGUI]
    public Text Txt_Des;
    [HideInInspector,AutoUGUI]
    public Image Spr_EupIcon;

    [HideInInspector, AutoUGUI]
    public Transform SelectedEffect;
    #endregion

    #region Container
    private List<Transform> m_pPackageItems;
    private List<Transform> m_pEquipItem;
    private List<InventoryItem> m_pInventoryItemList;
    private List<InventoryItem> m_pUnDressInventoryItemList;
    private List<InventoryItem> m_pDressInventoryItemList;
    private int m_nPages;
    private int m_nCurIndex;
    private int m_nCurPage;
    private string m_strGoodsIconBg;
    private Sprite m_sprGoodsIconBg;

    private int m_nTotalHP;
    private int m_nTotalPower;
    #endregion
    public override UIType GetUIType()
    {
        return UIType.SubPackage;
    }

    protected override void OnReady()
    {
        base.OnReady();
    }

    public override void OnShow()
    {
        base.OnShow();

        GetPackageGoodsList();
    }

    protected override void Register()
    {
        base.Register();
        RegisterMsg(MsgType.Role_RefreshRoleInventory, RefreshPackageList);
        RegisterMsg(MsgType.Role_RefreshRoleInfo,RefreshRolePropertyInfo);
    }

    private void GetPackageGoodsList()
    {
        Message msg = new Message(MsgType.Role_GetRoleGoodsList,this);
        msg.Send();
    }

    protected override void InitUI()
    {
        base.InitUI();
        string strPrefix = "InventoryItem";
        for (int i = 1; i <= 16; i++)
        {
            Transform tfItem = ItemGrid.Find(strPrefix + i);
            m_pPackageItems.Add(tfItem);
        }

        m_pEquipItem.Add(Spr_Bracelet.transform);
        m_pEquipItem.Add(Spr_Cloth.transform);
        m_pEquipItem.Add(Spr_Helm.transform);
        m_pEquipItem.Add(Spr_Necklace.transform);
        m_pEquipItem.Add(Spr_Ring.transform);
        m_pEquipItem.Add(Spr_Shoes.transform);
        m_pEquipItem.Add(Spr_Weapon.transform);
        m_pEquipItem.Add(Spr_Wing.transform);

        m_sprGoodsIconBg = m_pPackageItems[0].Find("Spr_Icon").GetComponent<Image>().sprite;

        Btn_ClosePackage.onClick.AddListener(()=> 
        {
            UIManager.Instance.HideSubPanel(UIType.SubPackage);
        });

        Btn_Left.onClick.AddListener(()=> 
        {
            m_nCurPage++;
            m_nCurPage = m_nCurPage > m_nPages ? 1 : m_nCurPage;
            PackagePageChange();
        });

        Btn_Right.onClick.AddListener(() =>
        {
            m_nCurPage--;
            m_nCurPage = m_nCurPage < 1 ? m_nPages : m_nCurPage;
            PackagePageChange();
        });
    }

    protected override void InitContainer()
    {
        base.InitContainer();

        m_nPages = 1;
        m_nCurPage = 1;
        m_nCurIndex = 0;
        m_strGoodsIconBg = "bg_道具.png";
        m_nTotalHP = 0;
        m_nTotalPower = 0;
        m_pPackageItems = new List<Transform>();
        m_pEquipItem = new List<Transform>();
        m_pInventoryItemList = new List<InventoryItem>();
        m_pUnDressInventoryItemList = new List<InventoryItem>();
        m_pDressInventoryItemList = new List<InventoryItem>();
    }

    private void RefreshPackageList(Message _msg)
    {
        m_pInventoryItemList = _msg["data"] as List<InventoryItem>;
        ResetUIState();
        CreateGoodsItem();
    }

    private void RefreshRolePropertyInfo(Message _msg)
    {
        m_nTotalPower = (int)_msg["power"];
        m_nTotalHP = (int)_msg["hp"];

        Txt_Power.text = m_nTotalPower.ToString();
        Txt_Hp.text = m_nTotalHP.ToString();
    }

    private void ResetUIState()
    {
        UIEquipTips.gameObject.SetActive(false);
        UIGoodsTips.gameObject.SetActive(false);
        SelectedEffect.gameObject.SetActive(false);

        for (int i = 0; i < m_pEquipItem.Count; i++)
        {
            Transform tf = m_pEquipItem[i];
            Button btn = tf.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();

            Image img = tf.GetComponent<Image>();
            img.sprite = m_sprGoodsIconBg;
        }
    }
    
    /// <summary>
    /// Refresh Package By Msg
    /// </summary>
    private void CreateGoodsItem()
    {
        m_pDressInventoryItemList = m_pInventoryItemList.FindAll((item) => { return item.IsDressed; });
        m_pUnDressInventoryItemList = m_pInventoryItemList.FindAll((item) => { return !item.IsDressed; });
        int nPackageGoodsCount = m_pUnDressInventoryItemList.Count;

        m_nCurPage = 1;
        m_nPages = (nPackageGoodsCount + 15) / 16;
        m_nPages = m_nPages < 1 ? 1 : m_nPages;

        PackagePageChange();

        EquipRole();
    }

    /// <summary>
    /// Equip Role
    /// </summary>
    private void EquipRole()
    {
        for (int i = 0; i < m_pDressInventoryItemList.Count; i++)
        {
            var item = m_pDressInventoryItemList[i];
            UIIconDefines.GetGoodsIcon(item.Inventory.ICON, (spr) => 
            {
                Image img = null;
                switch (item.Inventory.EquipTYPE)
                {
                    case EquipType.Helm:
                        img = Spr_Helm;
                        break;
                    case EquipType.Cloth:
                        img = Spr_Cloth;
                        break;
                    case EquipType.Weapon:
                        img = Spr_Weapon;
                        break;
                    case EquipType.Shoes:
                        img = Spr_Shoes;
                        break;
                    case EquipType.Necklace:
                        img = Spr_Necklace;
                        break;
                    case EquipType.Bracelet:
                        img = Spr_Bracelet;
                        break;
                    case EquipType.Ring:
                        img = Spr_Ring;
                        break;
                    case EquipType.Wing:
                        img = Spr_Wing;
                        break;
                    default:
                        break;
                }
                if (img != null)
                {
                    img.sprite = spr;
                    Button btn = img.transform.GetComponent<Button>();
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => 
                    {
                        ShowEquipInfo(item,true);
                    });
                }
            });
          
        }
    }

    private void ShowSelectedEffect(Transform _tf)
    {
        SelectedEffect.gameObject.SetActive(true);
        SelectedEffect.position = new Vector3(_tf.position.x,_tf.position.y,-10);
    }

    /// <summary>
    /// Refresh Package By Page Index
    /// </summary>
    private void PackagePageChange()
    {
        int nPackageGoodsCount = m_pUnDressInventoryItemList.Count;
        int nStart = 0;
        int nEnd = 0;
        nStart = (m_nCurPage - 1) * 16;
        nEnd = nStart + 16;
        nEnd = nEnd >= nPackageGoodsCount ? nPackageGoodsCount : nEnd;

        Txt_Index.text = m_nCurPage + "/" + m_nPages;

        CleanPackgeItem();

        for (int i = nStart; i < nEnd; i++)
        {
            CreateNewGoodsItem(m_pPackageItems[i - nStart], m_pUnDressInventoryItemList[i]);
        }
    }

    /// <summary>
    /// Create New Goods Item
    /// </summary>
    /// <param name="_tf"></param>
    /// <param name="_item"></param>
    private void CreateNewGoodsItem(Transform _tf, InventoryItem _item)
    {
        Button btn = _tf.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(()=> 
        {
            Debug.Log(string.Format("Goods:{0},Is Choosed!",_item.Inventory.Name));
            ChooseGoodsItem(_item);
            ShowSelectedEffect(_tf);
        });
        Image spr_bg = _tf.Find("Spr_Icon").GetComponent<Image>();
        UIIconDefines.GetGoodsIcon(_item.Inventory.ICON, (spr) => 
        {
            spr_bg.sprite = spr;
        });
        Text txt_num = _tf.Find("Txt_Num").GetComponent<Text>();
        txt_num.text = _item.Count.ToString();
        txt_num.gameObject.SetActive(true);
    }

    /// <summary>
    /// Clean Package Item
    /// </summary>
    private void CleanPackgeItem()
    {
        for (int i = 0; i < m_pPackageItems.Count; i++)
        {
            Transform tf = m_pPackageItems[i];
            Image spr_bg = tf.Find("Spr_Icon").GetComponent<Image>();
            Button btn = tf.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            spr_bg.sprite = m_sprGoodsIconBg;
            Text txt_num = tf.Find("Txt_Num").GetComponent<Text>();
            txt_num.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Show Inventory Information
    /// </summary>
    /// <param name="_item"></param>
    private void ChooseGoodsItem(InventoryItem _item)
    {
        if (_item != null)
        {
            switch (_item.Inventory.InventoryTYPE)
            {
                case InventoryType.Equip:
                    ShowEquipInfo(_item);
                    break;
                case InventoryType.Drug:
                case InventoryType.Box:
                    ShowGoodsInfo(_item);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Show Equip Information
    /// </summary>
    /// <param name="_item"></param>
    private void ShowEquipInfo(InventoryItem _item,bool _bIsDressed = false)
    {
        UIEquipTips.gameObject.SetActive(true);
        Txt_AddDamage.text = _item.Inventory.Damage.ToString();
        Txt_AddHP.text = _item.Inventory.HP.ToString();
        Txt_Des.text = _item.Inventory.Des;
        Txt_Quality.text = _item.Inventory.Quality.ToString();
        Txt_AddPower.text = _item.Inventory.Power.ToString();
        Txt_Equiptitle.text = _item.Inventory.Name;
        UIIconDefines.GetGoodsIcon(_item.Inventory.ICON, (spr) =>
        {
            Spr_EupIcon.sprite = spr;
        });

        string strBtnName = _bIsDressed ? "卸下" : "装备";
        Utils.SetBtnName(Btn_Equip, strBtnName);
        Btn_Equip.onClick.RemoveAllListeners();
        Btn_Equip.onClick.AddListener(() => 
        {
            string strMsgType = _bIsDressed ? MsgType.Role_Dequip : MsgType.Role_Equip;
            Message msg = new Message(strMsgType, this);
            msg["data"] = _item.guid;
            msg.Send();
            Debug.Log("Equip:"+ _item.Inventory.Name);
        });

        Btn_Upgrade.onClick.RemoveAllListeners();
        Btn_Upgrade.onClick.AddListener(() => 
        {
            Message msg = new Message(MsgType.Role_UpgradeEquip, this);
            msg["data"] = _item.guid;
            msg.Send();
            Debug.Log("Equip:" + _item.Inventory.Name);
            Debug.Log("Upgrade:" + _item.Inventory.Name);
        });

        Btn_CloseTips.onClick.RemoveAllListeners();
        Btn_CloseTips.onClick.AddListener(() => 
        {
            UIEquipTips.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// Show Goods Inforamtion
    /// </summary>
    /// <param name="_item"></param>
    private void ShowGoodsInfo(InventoryItem _item)
    {
        ResetUIState();

        UIGoodsTips.gameObject.SetActive(true);

        Txt_GoodsDes.text = _item.Inventory.Des;
        Txt_GoodsShowName.text = _item.Inventory.Name;
        UIIconDefines.GetGoodsIcon(_item.Inventory.ICON, (spr) => 
        {
            Spr_GoodsInfoIcon.sprite = spr;
        });

        Btn_CloseGoodsTips.onClick.RemoveAllListeners();
        Btn_CloseGoodsTips.onClick.AddListener(()=>
        {
            UIGoodsTips.gameObject.SetActive(false);
        });

        Btn_Use.onClick.RemoveAllListeners();
        Btn_Use.onClick.AddListener(() => 
        {
            Message msg = new Message(MsgType.Role_UseGoods, this);
            msg["data"] = _item.guid;
            msg.Send();
        });

        Btn_MultUse.onClick.RemoveAllListeners();
        Btn_MultUse.onClick.AddListener(() =>
        {
            Message msg = new Message(MsgType.Role_MultUse, this);
            msg["data"] = _item.guid;
            msg.Send();
        });
    }
}
