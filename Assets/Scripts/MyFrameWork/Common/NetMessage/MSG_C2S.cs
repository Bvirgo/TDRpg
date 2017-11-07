using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ZFrameWork
{
    // Client To Server 
    public abstract class MSG_C2S
    {

    }

    /// <summary>
    /// Team Msg
    /// </summary>
    public class MSG_TEAM : MSG_C2S
    {
        public const string GetTeamList = "GetRoomList";
        public const string EnterRoom = "EnterRoom";
        public const string CreateRoom = "CreateRoom";
        public const string GetRoomInfo = "GetRoomInfo";
        public const string LeaveRoom = "LeaveRoom";
        public const string StartFight = "StartFight";
        public const string GetAchieve = "GetAchieve";
        public const string Fight = "Fight";
    }

}