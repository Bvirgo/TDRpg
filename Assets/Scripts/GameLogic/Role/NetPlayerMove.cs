using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
public class NetPlayerMove : MonoBehaviour {
    private RoleAnimator m_roleAnmCtr;
    //last 上次的位置信息
    Vector3 lPos;
    Vector3 lRot;
    //forecast 预测的位置信息
    Vector3 fPos;
    Vector3 fRot;
    //时间间隔
    float delta = 1;
    //上次接收的时间
    float lastRecvInfoTime = float.MinValue;

    void Start()
    {
        m_roleAnmCtr = GetComponent<RoleAnimator>(); 
        lPos = transform.position;
        lRot = transform.eulerAngles;
        fPos = transform.position;
        fRot = transform.eulerAngles;
    }

    //位置预测
    public void NetForecastInfo(Vector3 nPos, Vector3 nRot)
    {
        //预测的位置：位置同步时间间隔是0.1s，那么预测距离就是： lPos + (nPos - lPos) * 0.1f;
        // 如果出现同步卡顿严重，那么就调小同步间隔
        fPos = lPos + (nPos - lPos) * 0.1f;
        fRot = lRot + (nRot - lRot) * 0.1f;
        // 网络异常，更新延时，那就不能再按预测位置移动，而是直接过去吧
        if (Time.time - lastRecvInfoTime > 0.3f)
        {
            fPos = nPos;
            fRot = nRot;
        }
        //时间
        delta = Time.time - lastRecvInfoTime;
        //更新
        lPos = nPos;
        lRot = nRot;
        lastRecvInfoTime = Time.time;
    }

    /// <summary>
    /// 更新动画
    /// </summary>
    void UpdateAnm()
    {
        float z = transform.InverseTransformPoint(fPos).z;
        //判断坦克是否在移动
        if (Mathf.Abs(z) < 0.1f || delta <= 0.05f)
        {
            m_roleAnmCtr.AnmType = ActionType.idel;
        }
        else
        {
            m_roleAnmCtr.AnmType = ActionType.run;
        }
    }
    void FixedUpdate()
    {
        //当前位置
        Vector3 pos = transform.position;
        Vector3 rot = transform.eulerAngles;

        //更新位置
        if (delta > 0)
        {
            transform.position = Vector3.Lerp(pos, fPos, delta);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(rot),
                                              Quaternion.Euler(fRot), delta);
        }

        UpdateAnm();
    }
}
