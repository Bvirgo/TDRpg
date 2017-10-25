
using UnityEngine;
using System.Collections;

namespace ZFrameWork
{

    #region Global delegate 委托
    // UI状态改变
    public delegate void StateChangedEvent(object sender, EnumObjectState newState, EnumObjectState oldState);

    public delegate void MessageEvent(Message message);

    public delegate void OnTouchEventHandle(GameObject _listener, object _args, params object[] _params);

    public delegate void PropertyChangedHandle(BaseActor actor, int id, object oldValue, object newValue);
    #endregion

    #region Global enum 枚举
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResType
    {
        /// <summary>
        /// The Texture
        /// </summary>
        Texture,
        /// <summary>
        /// The Fbx
        /// </summary>
        Fbx,
        /// <summary>
        /// The AseetBundle
        /// </summary>
        AssetBundle,
        /// <summary>
        /// The Byts
        /// </summary>
        Raw
    }

    /// <summary>
    /// 对象当前状态 
    /// </summary>
    public enum EnumObjectState
    {
        /// <summary>
        /// The none.
        /// </summary>
        None,
        /// <summary>
        /// The initial.
        /// </summary>
        Initial,
        /// <summary>
        /// The loading.
        /// </summary>
        Loading,
        /// <summary>
        /// The ready.
        /// </summary>
        Ready,
        /// <summary>
        /// The disabled.
        /// </summary>
        Disabled,
        /// <summary>
        /// The closing.
        /// </summary>
        Closing
    }

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
        /**|Register Panle|**/
        SubRegister,
        /**|CreateRole Panle|**/
        SubCreateRole,
        /**|Login Panle|**/
        SubLogin,
        /**|Package Panle|**/
        SubPackage,
        /**|Skills Tree|**/
        SubSkills,
        /**|Role Property Panel|**/
        SubRoleProperty,
        /**|Room Maps Panle|**/
        SubRoomsMaps
    }

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

    public enum PropertyType : int
    {
        RoleName = 1, 
        Sex,    
        RoleID, 
        Gold,   
        Coin,  
        Level,   
        Exp,    

        AttackSpeed,
        HP,     
        HPMax, 
        Attack, 
        Water, 
        Fire, 
    }

    /// <summary>
    /// 角色类型
    /// </summary>
    public enum EnumActorType
    {
        None = 0,
        Role,
        Monster,
        NPC,
    }

    /// <summary>
    /// 场景类型
    /// </summary>
    public enum ScnType
    {
        None = 0,
        StartGame,
        LoadingScene,
        LoginScene,
        MainScene,
        CopyScene,
        PVPScene,
        PVEScene,
        /////////////
        Login,
        Village,
        Battle,
    }

    #endregion

    #region Defines static class & cosnt

    /// <summary>
    /// 路径定义。
    /// </summary>
    public static class UIPathDefines
    {
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
                case UIType.SubCreateRole:
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
                default:
                    Debug.Log("Not Find EnumUIType! type: " + _uiType.ToString());
                    break;
            }
            return _scriptType;
        }
        #endregion
    }

    #endregion

    #region Global Const String
    public static class Defines
    {
        /**WaitingView**/
        public const string WaitingType_Clock = "clock";
        public const string WaitingType_Percent = "percent";

        /**Server**/
        public const string ServerAddress = "139.198.2.58:8000";

        /**AlertWindow**/
        public const string AlertType_Single = "Alert_Single";
        public const string AlertType_List = "Alert_List";

        /**Test Scene**/
        public const string PlayerModel = "SantaMale/Prefabs/SantaMale";
        public const string MainGroundPath = "Maps/Ground";
        public const string WhiteHousePath = "Building/WhiteHouse";
        public const string MapsLayerName = "Ground";
        public const string TestShopRoute = "03941001001";
    }
    #endregion
}
