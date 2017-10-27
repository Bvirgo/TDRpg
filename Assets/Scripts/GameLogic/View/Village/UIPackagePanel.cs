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
    public Text Txt_goodsShowName;
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
    public Text Txt_title;
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
    #endregion

    #region Container
    private List<Transform> m_pPackageItems;
    private List<InventoryItem> m_pInventoryItemList;
    private List<InventoryItem> m_pUnDressInventoryItemList;
    private int m_nPages;
    private int m_nCurIndex;
    private int m_nCurPage;
    private string m_strGoodsIconBg;
    private Sprite m_sprGoodsIconBg;
    #endregion
    public override UIType GetUIType()
    {
        return UIType.SubPackage;
    }

    protected override void OnReady()
    {
        base.OnReady();

        GetPackageGoodsList();
    }

    public override void OnShow()
    {
        base.OnShow();

        GetPackageGoodsList();
    }

    protected override void Register()
    {
        base.Register();
        RegisterMsg(MsgType.Role_RefreshPackage, RefreshPackageList);
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

        m_sprGoodsIconBg = m_pPackageItems[0].Find("Spr_Icon").GetComponent<Image>().sprite;

        Btn_ClosePackage.onClick.AddListener(()=> 
        {
            UIManager.Instance.HideSubPanle(UIType.SubPackage);
        });
    }

    protected override void InitContainer()
    {
        base.InitContainer();

        m_nPages = 1;
        m_nCurPage = 1;
        m_nCurIndex = 0;
        m_strGoodsIconBg = "bg_道具.png";
        m_pPackageItems = new List<Transform>();
        m_pInventoryItemList = new List<InventoryItem>();
        m_pUnDressInventoryItemList = new List<InventoryItem>();
    }

    private void RefreshPackageList(Message _msg)
    {
        m_pInventoryItemList = _msg["data"] as List<InventoryItem>;

        CreateGoodsItem();
    }

    /// <summary>
    /// Refresh Package By Msg
    /// </summary>
    private void CreateGoodsItem()
    {
        m_pUnDressInventoryItemList = m_pInventoryItemList.FindAll((item) => { return !item.IsDressed; });
        int nPackageGoodsCount = m_pUnDressInventoryItemList.Count;

        m_nCurPage = 1;
        m_nPages = (nPackageGoodsCount + 15) / 16;
        m_nPages = m_nPages < 1 ? 1 : m_nPages;
        Txt_Index.text = m_nCurPage +"/"+m_nPages;

        PackagePageChange();
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

        CleanPackgeItem();

        for (int i = nStart; i < nEnd; i++)
        {
            CreateNewGoodsItem(m_pPackageItems[i - nStart], m_pUnDressInventoryItemList[i]);
        }
    }

    private void CreateNewGoodsItem(Transform _tf, InventoryItem _item)
    {
        Button btn = _tf.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(()=> 
        {
            Debug.LogWarning(string.Format("Goods:{0},Is Choosed!",_item.Inventory.Name));
        });
        Image spr_bg = _tf.Find("Spr_Icon").GetComponent<Image>();
        UIIconDefines.GetGoodsIcon(_item.Inventory.ICON, (tx) => 
        {
            Sprite spr = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height),Vector2.zero);
            spr_bg.sprite = spr;
        });
        Text txt_num = _tf.Find("Txt_Num").GetComponent<Text>();
        txt_num.text = _item.Count.ToString();
        txt_num.gameObject.SetActive(true);
    }

    private void CleanPackgeItem()
    {
        for (int i = 0; i < m_pPackageItems.Count; i++)
        {
            Transform tf = m_pPackageItems[i];
            Image spr_bg = tf.Find("Spr_Icon").GetComponent<Image>();
            spr_bg.sprite = m_sprGoodsIconBg;
            Text txt_num = tf.Find("Txt_Num").GetComponent<Text>();
            txt_num.gameObject.SetActive(false);
        }
    }
    
}
