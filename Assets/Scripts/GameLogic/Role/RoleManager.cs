using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class RoleManager : DDOLSingleton<RoleManager>
{
    #region Container
    private Transform m_tfPlayerRoot;
    private List<BaseActor> m_pRoleList;
    #endregion

    void Awake()
    {
        m_pRoleList = new List<BaseActor>();
    }

    /// <summary>
    /// Get Main Player
    /// </summary>
    /// <returns></returns>
    public RoleProperty OnNewMainPlayer(Vector3 _vSpawn,Quaternion _q, bool _bVillage = true)
    {
        string strRolePrefab = _bVillage ? Defines.VillageMan : Defines.BattleMan;
        GameObject objPrefab = Resources.Load(strRolePrefab) as GameObject;
        objPrefab = GameObject.Instantiate(objPrefab);
        objPrefab.name = "MainRole";
        if (m_tfPlayerRoot == null)
        {
            var root = new GameObject();
            root.name = "Roles";
            m_tfPlayerRoot = root.transform;
            m_tfPlayerRoot.position = Vector3.zero;
            m_tfPlayerRoot.localScale = Vector3.one;
            m_tfPlayerRoot.rotation = Quaternion.identity;
        }
        if (objPrefab.GetComponent<PlayerMove>() == null)
        {
            objPrefab.AddComponent<PlayerMove>();
        }
        Transform tf = objPrefab.transform;
        tf.SetParent(m_tfPlayerRoot, true);
        tf.position = _vSpawn;
        tf.rotation = _q;

        RoleProperty rp = objPrefab.AddComponent<RoleProperty>();
        rp.ActorType = ActorType.MainRole;
        rp.guid = NetMgr.srvConn.m_strToken;
        m_pRoleList.Add(rp);
        return rp;
    }

    /// **************************
    ///	Get Main Player 
    /// **************************
    public BaseActor OnGetMainPlayer()
    {
        return m_pRoleList.Find((item )=> { return item.ActorType == ActorType.MainRole; });
    }

    /// <summary>
    /// Create Net Player
    /// </summary>
    /// <param name="_vSpawn"></param>
    /// <param name="_q"></param>
    /// <param name="_strId"></param>
    /// <returns></returns>
    public BaseActor OnNewNetPlayer(Vector3 _vSpawn, Quaternion _q, string _strId)
    {
        string strRolePrefab = Defines.BattleMan;
        GameObject objPrefab = Resources.Load(strRolePrefab) as GameObject;
        objPrefab = GameObject.Instantiate(objPrefab);
        objPrefab.name = _strId;
        if (m_tfPlayerRoot == null)
        {
            var root = new GameObject();
            root.name = "Roles";
            m_tfPlayerRoot = root.transform;
            m_tfPlayerRoot.position = Vector3.zero;
            m_tfPlayerRoot.localScale = Vector3.one;
            m_tfPlayerRoot.rotation = Quaternion.identity;
        }

        objPrefab.AddComponent<NetPlayerMove>();
        if (objPrefab.GetComponent<PlayerMove>() != null)
        {

        }

        Transform tf = objPrefab.transform;
        tf.SetParent(m_tfPlayerRoot, true);
        tf.position = _vSpawn;
        tf.rotation = _q;

        NetPlayer rp = objPrefab.AddComponent<NetPlayer>();
        rp.ActorType = ActorType.NetRole;
        rp.guid = _strId;
        m_pRoleList.Add(rp);
        return rp;
    }

    /// **************************
    ///	Get Role By GUID 
    /// **************************
    public BaseActor OnGetRole(string _strGUID)
    {
        return m_pRoleList.Find((item) => { return item.guid.Equals(_strGUID); });
    }


    /// **************************
    ///	Remove Role 
    /// **************************
    public void OnRemoveRole(string _strGUID)
    {
        var r = m_pRoleList.Find((item) => { return item.guid.Equals(_strGUID); });
        if (r != null)
        {
            GameObject.Destroy(r.gameObject);
            m_pRoleList.Remove(r);
        }
    }


    /// **************************
    ///	Remove All Roles 
    /// **************************
    public void OnRemoveAllRole()
    {
        for (int i = 0; i < m_pRoleList.Count; i++)
        {
            var r = m_pRoleList[i];
            if (r != null && r.gameObject != null)
            {
                GameObject.Destroy(r.gameObject);
                m_pRoleList.Remove(r);
            }
        }
    }
    
}
