using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 状态基类
/// </summary>
public abstract class BaseFSMState{

    protected Dictionary<FSMAction, FSMStateType> map = new Dictionary<FSMAction, FSMStateType>();

    protected FSMStateType stateType;
    public FSMStateType StateType
    {
        get
        {
            return stateType;
        }
    }

    protected Vector3 destPos;
    protected Transform[] waypoints;
    protected float curSpeed;
    protected float curRotSpeed;

    public abstract void Reason(Transform player, Transform npc);
    public abstract void Act(Transform player, Transform npc);

    public void AddTransition(FSMAction transition, FSMStateType id)
    {
        if (transition == FSMAction.None || id == FSMStateType.None)
        {
            Debug.LogWarning("FSMState ERROR: Null transition not allowed");
            return;
        }

        if (map.ContainsKey(transition))
        {
            Debug.LogWarning("FSMState ERROR: transition is already inside the map");
            return;
        }
        map.Add(transition, id);
        Debug.Log("Added: " + transition + " with ID: " + id);
    }

    public void DeleteTransition(FSMAction trans)
    {
        if (trans == FSMAction.None)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition passed was not on this state List");
    }

    public FSMStateType GetOutputState(FSMAction trans)
    {
        if (trans == FSMAction.None)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return FSMStateType.None;
        }

        if (map.ContainsKey(trans))
        {
            return map[trans];
        }

        Debug.LogError("FSMState ERROR: " + trans + " Transition passed to the state was not on the list");
        return FSMStateType.None;

    }

    public void FindNextPoint()
    {
        int rndIndex = Random.Range(0, waypoints.Length);
        destPos = waypoints[rndIndex].transform.position;
    }

    protected bool IsInCurrentRange(Transform trans, Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - trans.position.x);
        float zPos = Mathf.Abs(pos.z - trans.position.z);

        if (xPos <= 50 && zPos <= 50)
            return true;
        else
            return false;
    }
}
