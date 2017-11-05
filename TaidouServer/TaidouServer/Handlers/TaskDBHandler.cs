﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using TaidouCommon;
using TaidouCommon.Model;
using TaidouCommon.Tools;
using TaidouServer.DB.Manager;

namespace TaidouServer.Handlers {
    public class TaskDBHandler  :HandlerBase
    {

        private TaskDBManager taskDBManager;

        public TaskDBHandler()
        {
            taskDBManager = new TaskDBManager();
        }
        public override void OnHandlerMessage(Photon.SocketServer.OperationRequest request,OperationResponse response, ClientPeer peer,SendParameters sendParameters)
        {
            SubCode subCode = ParameterTool.GetParameter<SubCode>(request.Parameters, ParameterCode.SubCode, false);
            response.Parameters.Add((byte) ParameterCode.SubCode, subCode);
            switch (subCode)
            {
                    case SubCode.AddTaskDB:
                    TaskDB taskDB = ParameterTool.GetParameter<TaskDB>(request.Parameters, ParameterCode.TaskDB);
                    taskDB.Role = peer.LoginRole;
                    taskDBManager.AddTaskDB(taskDB);
                    taskDB.Role = null;
                    ParameterTool.AddParameter(response.Parameters,ParameterCode.TaskDB, taskDB);
                    response.ReturnCode = (short) ReturnCode.Success;
                    break;
                    case SubCode.GetTaskDB:
                    List<TaskDB> list = taskDBManager.GetTaskDBList(peer.LoginRole);
                    foreach (var taskDb in list)
                    {
                        taskDb.Role = null;
                    }
                    ParameterTool.AddParameter(response.Parameters,ParameterCode.TaskDBList, list);
                    response.ReturnCode = (short) ReturnCode.Success;

                    break;
                    case SubCode.UpdateTaskDB:
                    TaskDB taskDB2 = ParameterTool.GetParameter<TaskDB>(request.Parameters, ParameterCode.TaskDB);
                    taskDB2.Role = peer.LoginRole;
                    taskDBManager.UpdateTaskDB(taskDB2);
                    response.ReturnCode = (short) ReturnCode.Success;
                    break;
            }

        }

        public override TaidouCommon.OperationCode OpCode {
            get { return OperationCode.TaskDB;
            }
        }
    }
}
