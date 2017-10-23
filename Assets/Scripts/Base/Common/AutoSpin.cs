using UnityEngine;
using System.Collections;

public class AutoSpin : MonoBehaviour {

    public int m_nSpeed = 500;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(new Vector3(0,0,-1),m_nSpeed * Time.deltaTime);
	}
}
