using UnityEngine;
using System.Collections;

public class FSMMono : MonoBehaviour {

    protected Transform playerTransform;

    protected Vector3 destPos;

    protected GameObject[] pointList;

    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }

    protected float shootRate;
    protected float elapsedTime;

    public Transform turret { get; set; }
    public Transform bulletSpawnPoint { get; set; }

	// Use this for initialization
	void Start () {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        FSMUpdate();
	}

    void FixedUpdate()
    {
        FSMFixedUpdate();
    }
}
