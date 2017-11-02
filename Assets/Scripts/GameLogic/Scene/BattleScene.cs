using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class BattleScene : BaseScene
{
    #region Containers
    private GameObject m_objMainPlayer;
    private GameObject m_Spawn;
    #endregion

    #region Init & Register
    protected override void OnReady()
    {
        base.OnReady();

        m_Spawn = GameObject.Find("Spawn");

        NewPlayer();
    }

    protected override void Register()
    {
        base.Register();

        RegisterModule(typeof(SkillModule));

        RegisterMsg(MsgType.Role_Fire,MainPlaySkill);

        // Play Background Music
        //PlayBackGroundMuisc();
    }

    private void PlayBackGroundMuisc()
    {
        string strClipPath = Defines.ResMusicPath + "bg-city";
        var mClip = ResManager.Instance.Load<AudioClip>(strClipPath);
        SoundManager.Instance.PlayBackgroundMusic(mClip);
    }

    private void NewPlayer()
    {
        Vector3 vPos = Vector3.zero;
        Quaternion qRotation = Quaternion.identity;
        if (m_Spawn != null)
        {
            vPos = m_Spawn.transform.position;
            qRotation = m_Spawn.transform.rotation;
        }
        RoleProperty rp = RoleManager.Instance.OnNewMainPlayer(vPos, qRotation, false);
        if (rp.gameObject.GetComponent<PlayerAttack>() == null)
        {
            rp.gameObject.AddComponent<PlayerAttack>();
        }
        rp.CurrentScene = this;
        AddActor(rp);
    }

    protected override void OnRelease()
    {
        base.OnRelease();

        RoleManager.Instance.OnRemoveAllRole();
    }
    #endregion

    #region Anatior & Effect
    /// **************************
    ///	Main Player Show Skill 
    /// **************************
    private void MainPlaySkill(Message _msg)
    {
        ActionType at = ActionType.attack;
        int nPos = (int)_msg["pos"];
        switch (nPos)
        {
            case 1:
                at = ActionType.sk_1;
                break;
            case 2:
                at = ActionType.sk_2;
                break;
            case 3:
                at = ActionType.sk_3;
                break;
            default:
                break;
        }

        var role = RoleManager.Instance.OnGetMainPlayer();
        PlaySkill(role.transform,at);
    }

    private void PlaySkill(Transform _tf, ActionType _at)
    {
        RoleAnimator ra = _tf.GetComponent<RoleAnimator>();
        if (ra != null)
        {
            ra.AnmType = _at;
        }
    }

    #endregion
}
