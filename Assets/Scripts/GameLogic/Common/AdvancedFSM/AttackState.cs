using UnityEngine;
using System.Collections;

public class AttackState : BaseFSMState {

    public AttackState(Transform[] wp)
    {
        waypoints = wp;
        stateType = FSMStateType.Attacking;
        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        float dist = Vector3.Distance(npc.position, player.position);

        if (dist >= 200.0f && dist < 300.0f)
        {
            npc.GetComponent<NPCTankController>().SetTransition(FSMAction.SawPlayer);
        }
        else if (dist >= 300.0f)
        {
            npc.GetComponent<NPCTankController>().SetTransition(FSMAction.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        destPos = player.position;

        Transform turret = npc.GetComponent<NPCTankController>().turret;

        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);

        npc.GetComponent<NPCTankController>().ShootBullet();
    }
}
