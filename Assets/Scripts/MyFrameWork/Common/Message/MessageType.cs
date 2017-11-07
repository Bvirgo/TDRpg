
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

        public const string EmptyMsg = "Empty_Msg";

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
        public const string Start_EnterGame = "Start_EnterGame";

        #endregion

        #region Role 
        public const string Role_RefreshRoleInfo = "Role_RefreshRoleInfo";
        public const string Role_GetRoleInfo = "Role_GetRoleInfo";
        public const string Role_RefreshTimer = "Role_RefreshTimer";

        public const string Role_RefreshRoleInventory = "Role_RefreshRoleInventory";
        public const string Role_RefreshRoleProperty = "Role_RefreshRoleProperty";
        public const string Role_GetRoleGoodsList = "Role_GetRoleGoodsList";
        public const string Role_Equip = "Role_Equip";
        public const string Role_Dequip = "Role_Dequip";
        public const string Role_UseGoods = "Role_UseGoods";
        public const string Role_MultUse = "Role_MultUse";
        public const string Role_UpgradeEquip = "Role_UpgradeEquip";

        public const string Role_GetTaskInfo = "Role_GetTaskInfo";
        public const string Role_RefreshTaskUI = "Role_RefreshTaskUI";
        public const string Role_EnterNpcRange = "Role_EnterNpcRange";
        public const string Role_TaskStateChanged = "Role_TaskStateChanged";
        public const string Role_TaskItemClicked = "Role_TaskItemClicked";

        public const string Role_GoTargetPos = "Role_GoTargetPos";
        public const string Role_ArriveTargetPos = "Role_ArriveTargetPos";

        public const string Role_GetSkillProperty = "Role_GetSkillProperty";
        public const string Role_PressSkillBtn = "Role_PressSkillBtn";
        public const string Role_UpdateSkillCD = "Role_UpdateSkillCD";
        public const string Role_Fire = "Role_Fire";

        public const string Team_RefreshTeamList = "Team_RefreshTeamList";
        public const string Team_RefreshAchive = "Team_RefreshAchive";
        public const string Team_ShowTeamInfo = "Team_ShowTeamInfo";

        public const string Team_GetTeamInfo = "Team_GetTeamInfo";
        public const string Team_GetTeamList = "Team_GetTeamList";
        public const string Team_GetAchive = "Team_GetAchive";

        public const string Team_NewTeam = "Team_NewTeam";
        public const string Team_JoinTeam = "Team_JoinTeam";
        public const string Team_QuitTeam = "Team_QuitTeam";
        public const string Team_Fighting = "Team_Fighting";

        #endregion
    }
}

