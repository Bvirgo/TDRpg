using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**行为动作**/
public enum FSMAction
{
    None = 0,
    SawPlayer,// 看到---追逐
    ReachPlayer,// 抵达攻击范围---攻击
    LostPlayer,// 丢失目标---巡逻
    NoHealth,// dead
}

public enum FSMStateType
{
    None = 0,
    Patrolling,// 巡逻
    Chasing,// 追逐
    Attacking,// 攻击
    Dead,
}

public class AdvancedFSM : FSMMono {
    
    private List<BaseFSMState> fsmStates;

    private FSMStateType currentStateID;
    public FSMStateType CurrentStateID
    {
        get
        {
            return currentStateID;
        }
    }

    private BaseFSMState currentState;
    public BaseFSMState CurrentState
    {
        get
        {
            return currentState;
        }
    }

    public AdvancedFSM()
    {
        fsmStates = new List<BaseFSMState>();
    }

    public void AddFSMState(BaseFSMState fsmState)
    {
        if (fsmState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        if (fsmStates.Count == 0)
        {
            fsmStates.Add(fsmState);
            currentState = fsmState;
            currentStateID = fsmState.StateType;
        }
        else
        {
            foreach (BaseFSMState state in fsmStates)
            {
                if (state.StateType == fsmState.StateType)
                {
                    Debug.LogError("FSM ERROR: Trying to add a state was already inside the list");
                    return;
                }
            }

            fsmStates.Add(fsmState);
        }
    }

    public void DeleteState(FSMStateType fsmState)
    {
        if (fsmState == FSMStateType.None)
        {
            Debug.LogError("FSM ERROR: bull id is not allowed");
            return;
        }

        foreach (BaseFSMState state in fsmStates)
        {
            if(state.StateType == fsmState)
            {
                fsmStates.Remove(state);
                return;
            }
        }
    }

    /// <summary>
    /// 根据Transition 切换FSM状态--->多态决定哪个FSMClass Excute
    /// </summary>
    /// <param name="trans"></param>
    public void PerformTransition(FSMAction trans)
    {
        if (trans == FSMAction.None)
        {
            Debug.LogError("FSM ERROR: Null transition is not allowed");
            return;
        }

        // 行为 获取 对应FSMType
        FSMStateType id = currentState.GetOutputState(trans);
        if (id == FSMStateType.None)
        {
            Debug.LogError("FSM ERROR: Current state does not have a target state for this transition");
            return;
        }

        currentStateID = id;
        // 所有注册FSM类中，查找类型匹配对象
        foreach (BaseFSMState state in fsmStates)
        {
            if (state.StateType == currentStateID)
            {
                currentState = state;
                break;
            }
        }
    }
}
