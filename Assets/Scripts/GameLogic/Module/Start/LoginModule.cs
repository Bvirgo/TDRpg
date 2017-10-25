using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using LitJson;

public class LoginModule : BaseModule {

    #region Base
    public LoginModule()
    {
        this.AutoRegister = true;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        MessageCenter.Instance.AddListener(MsgType.Start_ShowLogin, OnLogin);
    }

    protected override void OnRelease()
    {
        base.OnRelease();

        MessageCenter.Instance.RemoveListener(MsgType.Start_ShowLogin, OnLogin);
    }
    #endregion

    #region Actions
    private void OnLogin(Message _msg)
    {
        string strUser = _msg["user"].ToString();
        string strPsw = _msg["psw"].ToString();

        //Message msg;
        LogicUtils.Instance.OnShowWaiting(1, "Login...",true);

    }
    #endregion

}
