using UnityEngine;
using System.Collections;

public class AutoDestory : MonoBehaviour 
{
    public bool bAutoDestory = true;
    public int delyTime = 3;

    void Start()
    {
        if (bAutoDestory)
        {
            if (delyTime < 0)
            {
                delyTime = 0;
            }
            Invoke("OnDestory", delyTime);
        }
    }

    void OnDestory()
    {
        GameObject.DestroyImmediate(gameObject);
    }
}
