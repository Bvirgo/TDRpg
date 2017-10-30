using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class RoleManager : DDOLSingleton<RoleManager>
{
    #region Container
    private Transform m_tfPlayerRoot;
    private GameObject objRole;
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
    public RoleProperty OnNewPlayer()
    {
        GameObject objPrefab = Resources.Load(Defines.Man) as GameObject;
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
        objRole = objPrefab;
        objPrefab.transform.SetParent(m_tfPlayerRoot, true);
        RoleProperty rp = objPrefab.AddComponent<RoleProperty>();
        rp.ActorType = ActorType.Role;
        m_pRoleList.Add(rp);
        return rp;
    }

    /// **************************
    ///	Get Main Player 
    /// **************************
    public BaseActor OnGetMainPlayer()
    {
        return m_pRoleList.Find((item )=> { return item.ActorType == ActorType.Role; });
    }

    public void OnRemoveRole(string _strGUID)
    {
        var r = m_pRoleList.Find((item) => { return item.guid.Equals(_strGUID); });
        if (r != null)
        {
            m_pRoleList.Remove(r);
        }
    }

    void OnDrawGizmos()
    {
        if (objRole != null)
        {
            RTDebug.DrawGizmosBounds(Utils.GetRendererBounds(objRole), Color.red);
        }
    }
}
