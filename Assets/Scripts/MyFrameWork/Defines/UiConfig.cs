using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public static class UiConfig{}


#region UI Enum
/// <summary>
/// Enum user interface type.
/// UI面板类型
/// </summary>
public enum UIType : int
{
    /// <summary>
    /// The none.
    /// </summary>
    None = -1,
    /// <summary>
    /// Alert Window
    /// </summary>
    AlertWindow,
    /// <summary>
    /// Waiting View
    /// </summary>
    Waiting,
    /// <summary>
    /// Login View
    /// </summary>
    Start,
    /// <summary>
    /// Main View
    /// </summary>
    Village,
    /// <summary>
    /// Shop View
    /// </summary>
    Battle,

    /**|Start Index|**/
    SubIndex,
    /**|Register Panel|**/
    SubRegister,
    /**|CreateRole Panel|**/
    SubCreateRole,
    /**|Login Panel|**/
    SubLogin,
    /**|Role Information|**/
    SubRoleInfo,
    /**|Package Panel|**/
    SubPackage,
    /**|Skills Tree|**/
    SubSkills,
    /**|Role Property Panel|**/
    SubRoleProperty,
    /**|Room Maps Panel|**/
    SubRoomsMaps,
}

/// <summary>
/// UI Event Type
/// </summary>
public enum EnumTouchEventType
{
    OnClick,
    OnDoubleClick,
    OnDown,
    OnUp,
    OnEnter,
    OnExit,
    OnSelect,
    OnUpdateSelect,
    OnDeSelect,
    OnDrag,
    OnDragEnd,
    OnDrop,
    OnScroll,
    OnMove,
}
#endregion

/// <summary>
/// UI Defines
/// </summary>
public static class UIPathDefines
{
    #region Path Prefix
    /// <summary>
    /// UI预设。
    /// </summary>
    public const string UI_PREFAB = "UIPrefabs/";
    /// <summary>
    /// ui子页面预设。
    /// </summary>
    public const string UI_SUBUI_PREFAB = "UIPrefabs/SubUI/";
    /// <summary>
    /// icon路径
    /// </summary>
    public const string UI_IOCN_PATH = "UI/Icon/";
    #endregion

    #region UI View
    /// <summary>
    /// Gets the type of the prefab path by.
    /// </summary>
    /// <returns>The prefab path by type.</returns>
    /// <param name="_uiType">_ui type.</param>
    public static string GetPrefabPathByType(UIType _uiType)
    {
        string _uiPrefab = string.Empty;
        switch (_uiType)
        {
            case UIType.Start:
                _uiPrefab = "StartView";
                break;
            case UIType.Waiting:
                _uiPrefab = "WaitingView";
                break;

            case UIType.AlertWindow:
                _uiPrefab = "WinView";
                break;

            case UIType.Village:
                _uiPrefab = "VillageView";
                break;

            case UIType.Battle:
                _uiPrefab = "BattleView";
                break;

            default:
                Debug.Log("Not Find EnumUIType! type: " + _uiType.ToString());
                break;
        }
        return UI_PREFAB + _uiPrefab;
    }

    /// <summary>
    /// Gets the type of the user interface script by.
    /// </summary>
    /// <returns>The user interface script by type.</returns>
    /// <param name="_uiType">_ui type.</param>
    public static System.Type GetUIScriptByType(UIType _uiType)
    {
        System.Type _scriptType = null;
        switch (_uiType)
        {
            case UIType.Start:
                _scriptType = typeof(StartView);
                break;

            case UIType.Waiting:
                _scriptType = typeof(WaitingView);
                break;

            case UIType.AlertWindow:
                _scriptType = typeof(AlertWindowView);
                break;

            case UIType.Village:
                _scriptType = typeof(VillageMainView);
                break;

            default:
                Debug.Log("Not Find EnumUIType! type: " + _uiType.ToString());
                break;
        }
        return _scriptType;
    }
    #endregion

    #region Sub UI 
    /// <summary>
    /// Gets the type of the prefab path by.
    /// </summary>
    /// <returns>The prefab path by type.</returns>
    /// <param name="_uiType">_ui type.</param>
    public static string GetSubUIPrefabPathByType(UIType _uiType)
    {
        string _uiPrefab = string.Empty;
        switch (_uiType)
        {
            case UIType.SubLogin:
                _uiPrefab = "LoginPanel";
                break;
            case UIType.SubIndex:
                _uiPrefab = "IndexPanel";
                break;

            case UIType.SubRoleInfo:
                _uiPrefab = "UIHeroInfo";
                break;

            case UIType.SubPackage:
                _uiPrefab = "UIPackage";
                break;

            default:
                Debug.Log("Not Find EnumUIType! type: " + _uiType.ToString());
                break;
        }
        return UI_SUBUI_PREFAB + _uiPrefab;
    }

    /// <summary>
    /// Gets the type of the user interface script by.
    /// </summary>
    /// <returns>The user interface script by type.</returns>
    /// <param name="_uiType">_ui type.</param>
    public static System.Type GetSubUIScriptByType(UIType _uiType)
    {
        System.Type _scriptType = null;
        switch (_uiType)
        {
            case UIType.SubLogin:
                _scriptType = typeof(LoginPanel);
                break;
            case UIType.SubIndex:
                _scriptType = typeof(IndexPanel);
                break;
            case UIType.SubRoleInfo:
                _scriptType = typeof(RoleInfoPanel);
                break;
            case UIType.SubPackage:
                _scriptType = typeof(UIPackagePanel);
                break;
            default:
                Debug.Log("Not Find EnumUIType! type: " + _uiType.ToString());
                break;
        }
        return _scriptType;
    }
    #endregion
}

public static class UIIconDefines
{
    public static void GetGoodsIcon(string _strIconName,Action<Sprite> _cbDone)
    {
        string GOODSICONPATH = Application.streamingAssetsPath + "//GoodsIcon//";
        string strIconPath = GOODSICONPATH + _strIconName + ".png";
        ResManager.Instance.OnLoadLocalTexture(strIconPath, (tx) =>
        {
            Sprite spr = Sprite.Create(tx,new Rect(0,0,tx.width,tx.height),Vector2.zero);
            _cbDone(spr);
        });
    }
}