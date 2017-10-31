
using UnityEngine;
using System.Collections;

namespace ZFrameWork
{
    #region Global delegate 委托
    // UI状态改变
    public delegate void StateChangedEvent(object sender, ObjectState newState, ObjectState oldState);

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
    public enum ObjectState
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
    #endregion

    #region Role Enum

    public enum PropertyType : int
    {
        RoleName = 1,
        Sex,
        RoleHeadIcon,
        RoleID,
        Gold,
        Coin,
        Level,
        Exp,
        PL,

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
    public enum ActorType
    {
        None = 0,
        Role,
        Monster,
        NPC,
    }

    #endregion

    #region Scene Enum
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
        VillageScene,
        BattleScene,
        CopyScene,
        PVPScene,
        PVEScene,
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

        /**|Role Define|**/
        public const string VillageMan = "Prefabs/Player/Boy_SimpleMove";
        public const string BattleMan = "Prefabs/Player/Boy_Room";
    }
    #endregion
}
