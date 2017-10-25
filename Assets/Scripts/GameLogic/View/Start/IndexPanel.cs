using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ZFrameWork;

public class IndexPanel : BasePanel {
    #region UI
    [HideInInspector, AutoUGUI]
    public Button Btn_User;
    [HideInInspector, AutoUGUI]
    public Button Btn_Server;
    [HideInInspector, AutoUGUI]
    public Button Btn_Enter;
    #endregion
    public override UIType GetUIType()
    {
        return UIType.SubIndex;
    }

    public override void OnShow()
    {
        base.OnShow();

        InitUI();
    }

    private void InitUI()
    {
        Btn_User.onClick.AddListener(()=> 
        {
            UIManager.Instance.OpenSubPanle(UIType.SubLogin, parent, true);
        });
    }

    public override void OnHide()
    {
        base.OnHide();  
    }
}
