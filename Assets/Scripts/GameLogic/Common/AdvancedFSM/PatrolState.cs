using UnityEngine;
using System.Collections;

public class PatrolState : BaseFSMState {
    public PatrolState(Transform[] wp)
    {
        waypoints = wp;
        stateType = FSMStateType.Patrolling;

        curRotSpeed = 1.0f;
        curSpeed = 100.0f;
    }

    /// <summary>
    ///  行为判断
    /// </summary>
    /// <param name="player"></param>
    /// <param name="npc"></param>
    public override void Reason(Transform player, Transform npc)
    {
        float dist = Vector3.Distance(npc.position, player.position);
        if (dist < 200.0f)
        {
            npc.GetComponent<NPCTankController>().SetTransition(FSMAction.ReachPlayer);
        }
        else if ( dist < 300.0f)
        {
            Debug.Log("Switch to Chase State");
            npc.GetComponent<NPCTankController>().SetTransition(FSMAction.SawPlayer);
        }
        
    }

    /// <summary>
    /// 行为动作
    /// </summary>
    /// <param name="player"></param>
    /// <param name="npc"></param>
    public override void Act(Transform player, Transform npc)
    {
        if (Vector3.Distance(npc.position, destPos) <= 100.0f)
        {
            FindNextPoint();
        }

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }
}
