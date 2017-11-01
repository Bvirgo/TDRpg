using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ZFrameWork;

/// <summary>
/// 攻击检测 以及 攻击特效
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    private Dictionary<string, EffectCtr> effectDict = new Dictionary<string, EffectCtr>();
    public EffectCtr[] effectArray;
    public float distanceAttackForward = 2;
    public float distanceAttackAround = 3;
    public int[] damageArray = new int[] { 20, 30, 30, 30 };
    public enum AttackRange
    {
        Forward,
        Around
    }
    public int hp = 1000;
    private Animator anim;
    private RoleAnimator m_roleAnmCtr;

    private GameObject hudTextGameObject;
    private Transform damageShowPoint;

    void Awake()
    {
        EffectCtr[] peArray = this.GetComponentsInChildren<EffectCtr>();
        foreach (EffectCtr pe in peArray)
        {
            effectDict.Add(pe.gameObject.name, pe);
        }
        //foreach (EffectCtr pe in effectArray)
        //{
        //    effectDict.Add(pe.gameObject.name, pe);
        //}
        anim = this.GetComponent<Animator>();
        m_roleAnmCtr = this.GetComponent<RoleAnimator>();

        damageShowPoint = transform.Find("DamageShowPoint");
    }


    //0 normal skill1 skill2 skill3
    //1 effect name
    //2 sound name
    //3 move forward
    //4 jump height
    void Attack(string args)
    {
        Debug.Log(string.Format("Attack :{0}",args));
        string[] proArray = args.Split(',');
        //1 show effect
        string effectName = proArray[1];
        ShowEffectCtr(effectName);

        //2 play sound
        string soundName = proArray[2];
        PlaySound(soundName);

        //3 move forward  控制前冲的效果
        float moveForward = float.Parse(proArray[3]);
        if (moveForward > 0.1f)
        {
            iTween.MoveBy(this.gameObject, Vector3.forward * moveForward, 0.3f);
        }

        //4 damage
        string posType = proArray[0];
        if (posType == "2001" || posType =="1001")
        {
            ArrayList array = GetEnemyInAttackRange(AttackRange.Forward);

            //Calls the method named methodName on every MonoBehaviour in this game object.
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", damageArray[0] + "," + proArray[3] + "," + proArray[4]);
            }
        }
    }

    /// <summary>
    /// Play Sound
    /// </summary>
    /// <param name="soundName"></param>
    void PlaySound(string soundName)
    {
        //SoundManager.Instance.Play(soundName);
    }

    //0 normal skill1 skill2 skill3
    //1 move forward
    //2 jump height
    void SkillAttack(string args)
    {
        string[] proArray = args.Split(',');
        string posType = proArray[0];
        if (posType == "skill1")
        {
            ArrayList array = GetEnemyInAttackRange(AttackRange.Around);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", damageArray[1] + "," + proArray[1] + "," + proArray[2]);
            }
        }
        else if (posType == "skill2")
        {
            ArrayList array = GetEnemyInAttackRange(AttackRange.Around);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", damageArray[2] + "," + proArray[1] + "," + proArray[2]);
            }
        }
        else if (posType == "skill3")
        {
            ArrayList array = GetEnemyInAttackRange(AttackRange.Forward);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", damageArray[3] + "," + proArray[1] + "," + proArray[2]);
            }
        }
    }

    void ShowEffect(string args)
    {
        if (args.Equals("DeviHandMobile"))
        {
            ShowEffectDevilHand();
        }
    }

    void ShowEffectAround(string args)
    {

    }

    void ShowEffectCtr(string effectName)
    {
        EffectCtr pe;
        if (effectDict.TryGetValue(effectName, out pe))
        {
            pe.Show();
        }
    }

    void ShowEffectDevilHand()
    {
        string effectName = "DevilHandMobile";
        EffectCtr pe;
        effectDict.TryGetValue(effectName, out pe);
        ArrayList array = GetEnemyInAttackRange(AttackRange.Forward);
        foreach (GameObject go in array)
        {
            RaycastHit hit;
            bool collider = Physics.Raycast(go.transform.position + Vector3.up, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));
            if (collider)
            {
                GameObject.Instantiate(pe, hit.point, Quaternion.identity);
            }
        }
    }

    void ShowEffectSelfToTarget(string effectName)
    {
        //print("showeffect self to target");
        EffectCtr pe;
        effectDict.TryGetValue(effectName, out pe);
        ArrayList array = GetEnemyInAttackRange(AttackRange.Around);
        foreach (GameObject go in array)
        {
            GameObject goEffect = (GameObject.Instantiate(pe) as EffectCtr).gameObject;
            goEffect.transform.position = transform.position + Vector3.up;
            goEffect.GetComponent<EffectSettings>().Target = go;
        }
    }
    void ShowEffectToTarget(string effectName)
    {
        //print("showeffect self to target");
        EffectCtr pe;
        effectDict.TryGetValue(effectName, out pe);
        ArrayList array = GetEnemyInAttackRange(AttackRange.Around);
        foreach (GameObject go in array)
        {
            RaycastHit hit;
            bool collider = Physics.Raycast(go.transform.position + Vector3.up, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));
            if (collider)
            {
                GameObject goEffect = (GameObject.Instantiate(pe) as EffectCtr).gameObject;
                goEffect.transform.position = hit.point;
            }
        }
    }

    //得到在攻击范围之内的敌人
    ArrayList GetEnemyInAttackRange(AttackRange attackRange)
    {
        ArrayList arrayList = new ArrayList();
        //if (attackRange == AttackRange.Forward) {
        //    foreach (GameObject go in TranscriptManager._instance.enemyList) {
        //        Vector3 pos = transform.InverseTransformPoint(go.transform.position);
        //        if (pos.z > -0.5f) {
        //            float distance = Vector3.Distance(Vector3.zero, pos);
        //            if (distance < distanceAttackForward) {
        //                arrayList.Add(go);
        //            }
        //        }
        //    }
        //} else {
        //    foreach (GameObject go in TranscriptManager._instance.enemyList) {
        //        float distance = Vector3.Distance(transform.position, go.transform.position);
        //            if (distance < distanceAttackAround) {
        //                arrayList.Add(go);
        //            }
        //    }
        //}

        return arrayList;
    }

    void TakeDamage(int damage)
    {
        if (this.hp <= 0) return;
        this.hp -= damage;
        //播放受到攻击的动画
        int random = Random.Range(0, 100);
        if (random < damage)
        {
            //anim.SetTrigger("TakeDamage");
            m_roleAnmCtr.AnmType = ActionType.hurt;
        }
        ////显示血量的减少
        //hudText.Add("-" + damage, Color.red, 0.3f);
        ////屏幕上血红特效的显示
        //BloodScreen.Instance.Show();
    }

}
