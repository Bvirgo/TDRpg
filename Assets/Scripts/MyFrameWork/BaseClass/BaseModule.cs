
using System;
namespace ZFrameWork
{
	public class BaseModule
	{
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

		protected virtual void OnStateChanged(ObjectState newState, ObjectState oldState)
		{

		}

		private EnumRegisterMode registerMode = EnumRegisterMode.NotRegister;

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

			OnLoad();
			State = ObjectState.Ready;
		}

		protected virtual void OnLoad()
		{
		
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
			}
		}

		protected virtual void OnRelease()
		{

		}

	}
}

