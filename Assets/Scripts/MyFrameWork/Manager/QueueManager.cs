using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZFrameWork
{
    /// <summary>
    /// Queue Manager
    /// </summary>
    public  class QueueManager: Singleton<QueueManager>
    {
        /// <summary>
        /// Runing Count
        /// </summary>
        public int m_nMax;

        public Action m_actAllDone;

        List<AsyncTask> m_pRuningList;

        List<AsyncTask> m_pWaitingList;

        public QueueManager()
        {
            m_nMax = 1;
        }

        public void OnSetThreshold(int _nWindow)
        {
            m_nMax = _nWindow;
        }
        public override void Init()
        {
            base.Init();
            m_pWaitingList = new List<AsyncTask>();
            m_pRuningList = new List<AsyncTask>();
        }

        /// <summary>
        /// New Task
        /// </summary>
        /// <param name="_task"></param>
        /// <returns></returns>
        public AsyncTask Add(AsyncTask _task)
        {
            Insert(_task);

            TaskLoop();

            return _task;
        }

        /// <summary>
        /// New Task
        /// </summary>
        /// <param name="_taskAct">Task Act & Done Act</param>
        /// <param name="_nPriority">Priority</param>
        /// <param name="_strTips">Tips</param>
        /// <returns></returns>
        public AsyncTask Add(Action<Action> _taskAct, TaskPriority _tp = TaskPriority.Bronze, string _strTips = "")
        {
            AsyncTask _task = new AsyncTask(_taskAct, _tp, _strTips);
            return Add(_task);
        }

        /// <summary>
        /// Insert New Task
        /// </summary>
        /// <param name="_task"></param>
        public void Insert(AsyncTask _task)
        {
            if (!m_pRuningList.Contains(_task))
            {
                if (m_pWaitingList.Contains(_task))
                {
                    m_pWaitingList.Remove(_task);
                }
                m_pWaitingList.Add(_task);
                m_pWaitingList.Sort(SortTask);               
            }
        }

        /// <summary>
        /// Sort By Task Priority
        /// </summary>
        /// <param name="_q1"></param>
        /// <param name="_q2"></param>
        /// <returns></returns>
        private int SortTask(AsyncTask _q1,AsyncTask _q2)
        {
            return _q1.tp > _q2.tp ? -1 : 1;
        }

        /// <summary>
        /// Remove Waiting Task
        /// </summary>
        /// <param name="_task"></param>
        /// <returns></returns>
        public bool Cancel(AsyncTask _task)
        {
            if (m_pWaitingList.Contains(_task))
            {
                m_pWaitingList.Remove(_task);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void ClearWaitList()
        {
            m_pWaitingList.Clear();
            m_pRuningList.Clear();
        }

        /// <summary>
        /// Get Waiting List
        /// </summary>
        /// <returns></returns>
        public List<AsyncTask> GetWaitList()
        {
            return m_pWaitingList;
        }

        /// <summary>
        /// Get The Length of  Waiting List 
        /// </summary>
        /// <returns></returns>
        public int GetWaitLength()
        {
            return m_pWaitingList.Count + m_pRuningList.Count;
        }
        
        /// <summary>
        /// Go
        /// </summary>
        void TaskLoop()
        {
            if (m_pRuningList.Count >= m_nMax) return;

            if (m_pWaitingList.Count > 0)
            {
                int nCount = m_nMax - m_pRuningList.Count;

                nCount = nCount > m_pWaitingList.Count ? m_pWaitingList.Count : nCount;

                for (int i = 0; i < nCount; i++)
                {
                    AsyncTask qvo = m_pWaitingList[i];
                    m_pRuningList.Add(m_pWaitingList[i]);
                    m_pWaitingList.RemoveAt(i);
                    qvo._taskAct(() =>
                    {
                        TaskLoop();
                        m_pRuningList.Remove(qvo);

                        if (m_pRuningList.Count == 0 && m_pWaitingList.Count == 0)
                        {
                            if (m_actAllDone != null)
                            {
                                m_actAllDone();
                            }
                        }
                    });
                }
            }
        }
    }

    public class AsyncTask
    {
        public Action<Action> _taskAct;
        public string Label;
        public TaskPriority tp;

        public AsyncTask(Action<Action> loadFunc, TaskPriority _tp, string label)
        {
            _taskAct = loadFunc;
            tp = _tp;
            Label = label;
        }
    }

    public enum TaskPriority
    {
        Bronze = 1, // 青铜
        Silver,     // 白银
        Gold,       // 黄金
        Diamond,    // 钻石
        Obsidian,   // 星耀
        King        // 王者       
    }
}
