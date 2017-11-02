using UnityEngine;
using System.Collections;

public class NPCTankController : AdvancedFSM {

    public GameObject Bullet;
    private int health;

    protected override void Initialize()
    {
        health = 100;

        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        turret = gameObject.transform.GetChild(0).transform;
        bulletSpawnPoint = turret.GetChild(0).transform;

        shootRate = 2.0f;
        elapsedTime = 0.0f;

        ConstructFSM();
    }

    private void ConstructFSM()
    {
        pointList = GameObject.FindGameObjectsWithTag("WandarPoint");

        Transform[] waypoints = new Transform[pointList.Length];
        int i = 0;
        foreach (GameObject obj in pointList)
        {
            waypoints[i] = obj.transform;
            i++;
        }

        PatrolState patrol = new PatrolState(waypoints);
        patrol.AddTransition(FSMAction.SawPlayer, FSMStateType.Chasing);
        patrol.AddTransition(FSMAction.ReachPlayer, FSMStateType.Attacking);
        patrol.AddTransition(FSMAction.NoHealth, FSMStateType.Dead);
        
        ChaseState chase = new ChaseState(waypoints);
        chase.AddTransition(FSMAction.LostPlayer, FSMStateType.Patrolling);
        chase.AddTransition(FSMAction.ReachPlayer, FSMStateType.Attacking);
        chase.AddTransition(FSMAction.NoHealth, FSMStateType.Dead);

        AttackState attack = new AttackState(waypoints);
        attack.AddTransition(FSMAction.LostPlayer, FSMStateType.Patrolling);
        attack.AddTransition(FSMAction.SawPlayer, FSMStateType.Chasing);
        attack.AddTransition(FSMAction.NoHealth, FSMStateType.Dead);

        DeadState dead = new DeadState();
        
        AddFSMState(patrol);
        AddFSMState(chase);
        AddFSMState(attack);
        AddFSMState(dead);

    }

    protected override void FSMUpdate()
    {
        elapsedTime += Time.deltaTime;
    }
    protected override void FSMFixedUpdate()
    {
        // 行为判断
        CurrentState.Reason(playerTransform, transform);

        // 行为 决定 FSM
        CurrentState.Act(playerTransform, transform);
    }

    public void SetTransition(FSMAction t)
    {
        PerformTransition(t);
    }

    public void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            elapsedTime = 0.0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag == "Bullet")
        {
            health -= 50;
        }

        if (health <= 0)
        {
            SetTransition(FSMAction.NoHealth);
            Explode();
        }
    }

    private void Explode()
    {
        float rndX = Random.Range(10.0f, 30.0f);
        float rndZ = Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++)
        {
            GetComponent<Rigidbody>().AddExplosionForce(600.0f, transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }
        Destroy(gameObject, 1.5f);        
    }
}
