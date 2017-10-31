using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using UnityEngine.UI;
public class BattleView : BaseUI {
    #region UI
    [HideInInspector, AutoUGUI]
    public Button Btn_Base;
    [HideInInspector, AutoUGUI]
    public Button Btn_Skill_1;
    [HideInInspector, AutoUGUI]
    public Button Btn_Skill_2;
    [HideInInspector, AutoUGUI]
    public Button Btn_Skill_3;
    [HideInInspector, AutoUGUI]
    public Slider Bar_HeadHp;
    [HideInInspector, AutoUGUI]
    public Image Spr_Head;
    [HideInInspector, AutoUGUI]
    public Text Txt_ComboCount;
    [HideInInspector, AutoUGUI]
    public Transform Combo;
    [HideInInspector, AutoUGUI]
    public Transform UIGameOver;
    [HideInInspector, AutoUGUI]
    public Button Btn_Over;
    #endregion

    #region Containers

    #endregion

    #region Init & Reigster
    public override UIType GetUIType()
    {
        return UIType.Battle;
    }

    protected override void OnReady()
    {
        base.OnReady();
    }

    protected override void Register()
    {
        base.Register();
    }

    protected override void InitContainer()
    {
        base.InitContainer();
    }

    protected override void InitUI()
    {
        base.InitUI();
    }
    #endregion

}
