using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZFrameWork;
using System;
using DG.Tweening;

public class LoginPanel : BasePanel {
    #region UI
    [HideInInspector, AutoUGUI]
    public Text txt_user;
    [HideInInspector, AutoUGUI]
    public Text txt_psw;
    [HideInInspector, AutoUGUI]
    public Button btn_login;
    [HideInInspector, AutoUGUI]
    public Button btn_register;
    [HideInInspector, AutoUGUI]
    public Button btn_close;
    [HideInInspector, AutoUGUI]
    public InputField ipt_user;
    [HideInInspector, AutoUGUI]
    public InputField ipt_pwd;
    [HideInInspector, AutoUGUI]
    public DOTweenAnimation Bg;
    #endregion
    public override UIType GetUIType()
    {
        return UIType.SubLogin;
    }

    void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        ipt_user.text = UserCache.GetUserName();
        ipt_pwd.text = UserCache.GetPassword();

        btn_login.onClick.AddListener(() => {
            Message msg = new Message(MsgType.Start_ShowLogin, this);
            msg["user"] = ipt_user.text;
            msg["psw"] = ipt_pwd.text;
            msg.Send();
        });

        btn_close.onClick.AddListener(()=> 
        {
            UIManager.Instance.OpenSubPanle(UIType.SubIndex, parent, true);
        });
    }

    public override void OnShow()
    {
        base.OnShow();
        Bg.DOPlayForward();
    }

    public override void OnHide()
    {
        base.OnHide();
        Bg.DOPlayBackwards();
    }
}
