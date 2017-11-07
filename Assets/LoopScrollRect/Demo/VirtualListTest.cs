using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SG;
public class VirtualListTest : MonoBehaviour
{
    public Button btn_FindOne;
    public InputField ipt_index;
    private LoopVerticalScrollRect m_LoopVSr;
	// Use this for initialization
	void Start ()
    {
        // 子项的渲染在，子项预制体绑定的脚本上
        m_LoopVSr = GetComponent<LoopVerticalScrollRect>();
        m_LoopVSr.prefabSource.prefabName = "ScrollCell1";
        m_LoopVSr.totalCount = 2300;

        // 扩展了子项渲染，分离到外部，可以自由发挥了，子项预制件不需要强制绑定渲染脚本
        m_LoopVSr.renderItem = RenderItem;

        btn_FindOne.onClick.AddListener(()=> 
        {
            int nIndex = int.Parse(ipt_index.text.Trim());
            m_LoopVSr.RefillCells(nIndex);
        });

        // 简单预制件缓存池
        var go = ResourcePool.Instance.GetObjectFromPool("ScrollCell2",true,10);
        if (go != null)
        {
            Debug.LogWarning(string.Format("Load Prefabs:{0}, From Pool Is Success!",go.name));
        }

	}

    private void RenderItem(Transform _tf,int _nIndex)
    {
        _tf.Find("Text").GetComponent<Text>().text = "Render:"+_nIndex;
    }
	
}
