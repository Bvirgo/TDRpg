using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class RoleManager : DDOLSingleton<RoleManager>
{
    #region Container
    private Transform m_tfPlayerRoot;
    #endregion
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
        objPrefab.transform.SetParent(m_tfPlayerRoot, true);
        RoleProperty rp = objPrefab.AddComponent<RoleProperty>();
        rp.ActorType = ActorType.Role;
        return rp;
    }
}
