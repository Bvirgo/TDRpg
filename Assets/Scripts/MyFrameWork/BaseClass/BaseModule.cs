
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
				// register 
				ModuleManager.Instance.Register(this);
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
            event_action = new Dictionary<string, MessageEvent>();
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
            }
		}

        protected virtual void OnRelease() { }

        #region Event
        public Dictionary<string, MessageEvent> event_action;
        /// <summary>
        /// Clean Events
        /// </summary>
        protected virtual void UnRegisterAllMsg()
        {
            foreach (var item in event_action)
            {
                MessageCenter.Instance.RemoveListener(item.Key, item.Value);
            }
        }

        protected virtual void RegisterMsg(string _strMsg, MessageEvent _cbMsg)
        {
            MessageCenter.Instance.AddListener(_strMsg, _cbMsg);

            event_action.AddOrReplace(_strMsg, _cbMsg);
        }

        protected virtual void UnRegisterMsg(string _strMsg)
        {
            if (event_action.ContainsKey(_strMsg))
            {
                MessageCenter.Instance.RemoveListener(_strMsg, event_action[_strMsg]);
                event_action.Remove(_strMsg);
            }
        }
        #endregion

    }
}

