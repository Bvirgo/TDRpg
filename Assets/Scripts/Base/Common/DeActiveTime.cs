using UnityEngine;
using System.Collections;

public class DeActiveTime : MonoBehaviour {

    public int m_fShowTime = 1;
    private float m_timer = 0;
	// Use this for initialization
    void Update()
    {
        if (m_timer >= m_fShowTime)
        {
            gameObject.SetActive(false);
            m_timer = 0;
        }
        else
        {
            m_timer += Time.deltaTime;
        }
    }
}
