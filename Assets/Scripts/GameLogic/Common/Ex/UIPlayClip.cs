using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIPlayClip : MonoBehaviour
{

    #region Containers
    [Header("Clip Volum:")]
    public float volum;
    [Header("Music Clip:")]
    public AudioClip clip;
    #endregion

    void Awake()
    {
        volum = 1;
    }
    void Start()
    {
        Button btn = transform.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(()=> 
            {
                if (clip != null)
                {
                    SoundManager.Instance.PlaySound(clip, transform.position,volum);
                }
            });
        }
    }
}
