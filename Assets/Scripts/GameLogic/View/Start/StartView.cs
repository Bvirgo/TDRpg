using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using UnityEngine.UI;
public class StartView : BaseUI
{
    public override UIType GetUIType()
    {
        return UIType.Start;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        UIManager.Instance.OpenSubPanle(UIType.SubIndex, root, true);
    }

    protected override void OnRelease()
    {
        base.OnRelease();
    }

}
