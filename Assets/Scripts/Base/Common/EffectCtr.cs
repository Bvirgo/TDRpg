using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class EffectCtr : MonoBehaviour 
{

    private Renderer[] m_pRender;
    private NcCurveAnimation[] m_pNcAni;
    private Transform m_effect;
	// Use this for initialization
    void Start()
    {
        m_pNcAni = transform.GetComponentsInChildren<NcCurveAnimation>();
        m_pRender = transform.GetComponentsInChildren<Renderer>();
        m_effect = transform.Find("EffectOffset");
        gameObject.SetActive(false);
    }

    public void Show()
    {
        // 寒冰特效，它下面挂的粒子系统是PlayOnAwake,改变它父的Active属性的时候就会自动出发Awake（）方法，播放粒子系统
        gameObject.SetActive(true);
        if (m_effect != null)
        {
            m_effect.gameObject.SetActive(false);
            m_effect.gameObject.SetActive(true);
            return;
        }
        // FXMater制作的动画播放
        if (m_pNcAni == null || m_pRender == null) 
        {
            return;
        }
        //性能考虑，预先把Render都给禁用了
        for (int i = 0; i < m_pRender.Length; ++i)
        {
            m_pRender[i].enabled = true;
        }

        //FXMater软件制作的动画，动画播放
        for (int j = 0; j < m_pNcAni.Length;++j )
        {
            m_pNcAni[j].ResetAnimation();
        }
        Invoke("Hide",0.8f);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
