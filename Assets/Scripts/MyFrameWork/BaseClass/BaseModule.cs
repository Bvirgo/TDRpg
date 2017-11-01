
using System;
using System.Collections.Generic;

namespace ZFrameWork
{
	public class BaseModule
	{
        public BaseModule()
        {
            InitContainer();
        }

		public enum EnumRegisterMode
		{
			NotRegister,
			AutoRegister,
			AlreadyRegister,
		}

		private ObjectState state = ObjectState.Initial;

		public ObjectState State
		{
			get
			{
				return state;
			}
			set
			{
				if (state == value) return;

				ObjectState oldState = state;
				state = value;

				if (null != StateChanged)
				{
					StateChanged(this, state, oldState);
				}
				OnStateChanged(state, oldState);
			}
		}

		public event StateChangedEvent StateChanged;

        protected virtual void OnStateChanged(ObjectState newState, ObjectState oldState) { }

		private EnumRegisterMode registerMode = EnumRegisterMode.AutoRegister;

		public bool AutoRegister
		{
			get
			{
				return registerMode == EnumRegisterMode.NotRegister ? false : true;
			}
			set
			{
				if (registerMode == EnumRegisterMode.NotRegister || registerMode == EnumRegisterMode.AutoRegister)
					registerMode = value ? EnumRegisterMode.AutoRegister : EnumRegisterMode.NotRegister;
			}
		}

		public bool HasRegistered
		{
			get
			{
				return registerMode == EnumRegisterMode.AlreadyRegister;
			}
		}

		public void Load()
		{
			if (State != ObjectState.Initial) return;

			State = ObjectState.Loading;

			//...
			if (registerMode == EnumRegisterMode.AutoRegister)
			{
				registerMode = EnumRegisterMode.AlreadyRegister; 
			}
			OnReady();
			State = ObjectState.Ready;
		}

		protected virtual void OnReady()
		{
            Register();
		}

        protected virtual void Register(){}

        protected virtual void InitContainer()
        {
            event_action = new Dictionary<string, List<MessageEvent>>();
            m_pRegisterModule = new List<BaseModule>();
        }

        public void Release()
		{
			if (State != ObjectState.Disabled)
			{
				State = ObjectState.Disabled;

				// ...
				if (registerMode == EnumRegisterMode.AlreadyRegister)
				{
					//unregister
					ModuleManager.Instance.UnRegister(this);
					registerMode = EnumRegisterMode.AutoRegister;
				}

				OnRelease();

                UnRegisterAllMsg();

                UnRegisterAllModule();
            }
		}

        protected virtual void OnRelease() { }

        #region Event
        public Dictionary<string, List<MessageEvent>> event_action;
        public List<BaseModule> m_pRegisterModule;
        /// <summary>
        /// Clean Events
        /// </summary>
        protected virtual void UnRegisterAllMsg()
        {
            foreach (var item in event_action)
            {
                var pEvents = item.Value;
                for (int i = 0; i < pEvents.Count; i++)
                {
                    MessageCenter.Instance.RemoveListener(item.Key, pEvents[i]);
                }
            }
        }

        protected virtual void RegisterMsg(string _strMsg, MessageEvent _cbMsg)
        {
            MessageCenter.Instance.AddListener(_strMsg, _cbMsg);
            if (event_action.ContainsKey(_strMsg))
            {
                var pList = event_action[_strMsg];
                pList.Add(_cbMsg);
                event_action[_strMsg] = pList;
            }
            else
            {
                List<MessageEvent> pEvent = new List<MessageEvent>();
                pEvent.Add(_cbMsg);
                event_action[_strMsg] = pEvent;
            }
        }

        protected virtual void RegisterModule(Type _moduleType)
        {
            m_pRegisterModule.Add(ModuleManager.Instance.Register(_moduleType));
        }

        protected virtual void UnRegisterAllModule()
        {
            for (int i = 0; i < m_pRegisterModule.Count; i++)
            {
                ModuleManager.Instance.UnRegister(m_pRegisterModule[i]);
            }
        }
        #endregion

    }
}

