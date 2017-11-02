using UnityEngine;
using System.Collections;

public class ChaseState : BaseFSMState {

    public ChaseState(Transform[] wp)
    {
        waypoints = wp;
        stateType = FSMStateType.Chasing;

        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        destPos = player.position;
        float dist = Vector3.Distance(npc.position, destPos);
        if (dist < 200.0f)
        {
            npc.GetComponent<NPCTankController>().SetTransition(FSMAction.ReachPlayer);
        }
        else if (dist >= 300.0f)
        {
            npc.GetComponent<NPCTankController>().SetTransition(FSMAction.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        destPos = player.position;
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

}
