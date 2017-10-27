
using System;
namespace ZFrameWork
{
	public class MsgType 
	{
        #region Test
        public static string Net_MessageTestOne = "Net_MessageTestOne";
        public static string Net_MessageTestTwo = "Net_MessageTestTwo";
        #endregion

        #region Common

        // ÊôÐÔ×÷±×Òì³£
        public const string Com_PropertyException = "PropertyItemDataException";
        public const string Com_AnyKeyHeld = "Com_AnyKeyHeld";
        public const string Com_AnyKeyDown = "Com_AnyKeyDown";
        public const string Com_MouseEvent = "Com_MouseEvent";

        public const string Com_CloseUI = "Com_CloseUI";
        public const string Com_OpenUI = "Com_OpenUI";

        #endregion

        #region WaitingView
        public const string WV_ShowWaiting = "Com_ShowWaiting";
        public const string WV_NewWaiting = "Com_NewWating";
        public const string WV_HideWaiting = "Com_HideWaiting";
        public const string WV_UpdateWaiting = "Com_UpdateWaiting";
        public const string WV_PushWaiting = "Com_PushWaiting";
        public const string WV_PopWaiting = "Com_PopWaiting";
        #endregion

        #region AlertWindow
        public const string Win_Show = "Win_ShowWindow";
        public const string Win_ItemClick = "Win_ItemClick";
        public const string Win_Affirm = "Win_Affirm";
        public const string Win_Refresh = "Win_RefreshWindow";
        public const string Win_Finish = "Win_Finish";
        #endregion

        #region Start
        public const string  Start_ShowLogin= "Start_ShowLogin";

        #endregion

        #region Role 
        public const string Role_RefreshRoleInfo = "Role_RefreshRoleInfo";
        public const string Role_GetRoleInfo = "Role_GetRoleInfo";
        public const string Role_RefreshTimer = "Role_RefreshTimer";

        public const string Role_RefreshPackage = "Role_RefreshPackage";
        public const string Role_GetRoleGoodsList = "Role_GetRoleGoodsList";
        #endregion
    }
}

