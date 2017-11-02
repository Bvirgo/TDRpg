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

        int nDamage = damageArray[0];
        float fBackDis = float.Parse(proArray[3]);
        float fJumpDis = float.Parse(proArray[4]);
        Message msg = new Message(MsgType.EmptyMsg, this);
        msg["damage"] = nDamage;
        msg["bDis"] = fBackDis;
        msg["jDis"] = fJumpDis;

        Vector3 vDir = transform.forward;
        msg["dir"] = vDir;

        if (posType == "2001" || posType =="1001")
        {
            List<GameObject> array = GetEnemyInAttackRange(AttackRange.Forward);
            
            //Calls the method named methodName on every MonoBehaviour in this game object.
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", msg);
            }
        }
        else
        {
            SkillAttack(args);
        }
    }

    /// <summary>
    /// Play Sound
    /// </summary>
    /// <param name="soundName"></param>
    void PlaySound(string soundName)
    {
        var clip = ResManager.Instance.Load<AudioClip>(Defines.ResMusicPath + soundName);
        SoundManager.Instance.PlaySound(clip,transform.position,0.5f);
    }

    //0 normal skill1 skill2 skill3
    //1 move forward
    //2 jump height
    void SkillAttack(string args)
    {
        string[] proArray = args.Split(',');
        string posType = proArray[0];

        int nDamage = damageArray[0];
        float fBackDis = float.Parse(proArray[3]);
        float fJumpDis = float.Parse(proArray[4]);
        Message msg = new Message(MsgType.EmptyMsg, this);
        msg["damage"] = nDamage;
        msg["bDis"] = fBackDis;
        msg["jDis"] = fJumpDis;
        Vector3 vDir = transform.forward;
        msg["dir"] = vDir;

        if (posType == "2002")
        {
            List<GameObject> array = GetEnemyInAttackRange(AttackRange.Around);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", msg);
            }
        }
        else if (posType == "2003")
        {
            List<GameObject> array = GetEnemyInAttackRange(AttackRange.Around);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", msg);
            }
        }
        else if (posType == "2004")
        {
            List<GameObject> array = GetEnemyInAttackRange(AttackRange.Forward);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", msg);
            }
        }
    }

    void ShowEffect(string args)
    {
        if (args.Equals("DevilHandMobile"))
        {
            ShowEffectDevilHand();
        }
    }
    
    void ShowEffectCtr(string effectName)
    {
        EffectCtr pe;
        if (effectDict.TryGetValue(effectName, out pe))
        {
            pe.Show();
        }
    }

    /// <summary>
    /// Show Devi Hand
    /// </summary>
    void ShowEffectDevilHand()
    {
        List<GameObject> array = GetEnemyInAttackRange(AttackRange.Forward);
        foreach (GameObject go in array)
        {
            RaycastHit hit;
            bool collider = Physics.Raycast(go.transform.position + Vector3.up, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));
            if (collider)
            {
                GameObject.Instantiate(DataManager.Instance.OnGetSkillEffect("DevilHandMobile"), hit.point, Quaternion.identity);
            }
        }
    }

    /// <summary>
    ///  Fire Bird
    /// </summary>
    /// <param name="effectName"></param>
    void ShowEffectInPlayer(string effectName)
    {
        List<GameObject> array = GetEnemyInAttackRange(AttackRange.Around);
        foreach (GameObject go in array)
        {
            GameObject goEffect = GameObject.Instantiate(DataManager.Instance.OnGetSkillEffect("FirePhoenixMobile"));
            goEffect.transform.position = transform.position + Vector3.up;
            goEffect.GetComponent<EffectSettings>().Target = go;
        }
    }

    /// <summary>
    /// MassFire
    /// </summary>
    /// <param name="effectName"></param>
    void ShowEffectAround(string effectName)
    {
        List<GameObject> array = GetEnemyInAttackRange(AttackRange.Around);
        foreach (GameObject go in array)
        {
            RaycastHit hit;
            bool collider = Physics.Raycast(go.transform.position + Vector3.up, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));
            if (collider)
            {
                GameObject goEffect = GameObject.Instantiate(DataManager.Instance.OnGetSkillEffect("HolyFireStrike"));
                goEffect.transform.position = hit.point;
            }
        }
    }

    /// <summary>
    /// Get Enemys In Attack Range
    /// </summary>
    /// <param name="attackRange"></param>
    /// <returns></returns>
    List<GameObject> GetEnemyInAttackRange(AttackRange attackRange)
    {
        List<GameObject> arrayList = new List<GameObject>();
        if (attackRange == AttackRange.Forward)
        {
            var pEnemy = Utils.GetAroundGameObject(transform.position, distanceAttackForward, "Monster");
            foreach (GameObject go in pEnemy)
            {
                Vector3 pos = transform.InverseTransformPoint(go.transform.position);
                if (pos.z > -0.5f)
                {
                   arrayList.Add(go);
                }
            }
        }
        else
        {
            var pEnemy = Utils.GetAroundGameObject(transform.position, distanceAttackAround, "Monster");
            arrayList.AddRange(pEnemy);
        }
        Debug.LogWarning(string.Format("Get Enemy Count:{0}",arrayList.Count));
        if (arrayList.Count != 0)
        {
            transform.LookAt(arrayList[0].transform);
        }
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
