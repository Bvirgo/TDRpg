
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZFrameWork
{
    /// <summary>
    /// 所有场景对象基类：NPC,Role,Monster
    /// </summary>
	public class BaseActor : MonoBehaviour,IDynamicProperty
    {
        #region Property
        // 对象属性字典
        protected Dictionary<int, PropertyItem> dicProperty = null;

        public event PropertyChangedHandle PropertyChanged;

        public ActorType ActorType { set; get; }

        public string guid { set; get; }

        private BaseScene currentScene;

        public BaseScene CurrentScene
        {
            set
            {
                currentScene = value;
            }
            get
            {
                return currentScene;
            }
        }

        void Awake()
        {
            guid = Guid.NewGuid().ToString();
            OnAwake();
        }

        void Start()
        {
            OnReady();
        }

        protected virtual void OnAwake()
        {
            InitContainer();
        }

        protected virtual void OnReady()
        {
            Register();
        }

        protected virtual void Register() { }

        protected virtual void InitContainer()
        {
            event_action = new Dictionary<string, List<MessageEvent>>();
        }

        void OnDestory()
        {
            OnRelease();
        }

        protected virtual void OnRelease()
        {
            UnRegisterAllMsg();
        }
        #endregion

        #region Event
        public Dictionary<string, List<MessageEvent>> event_action;
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

        #endregion

        #region Property Register & UnRegister
        public virtual void AddProperty(PropertyType propertyType, object content)
        {
            AddProperty((int)propertyType, content);
        }

        public virtual void AddProperty(int id, object content)
        {
            PropertyItem property = new PropertyItem(id, content);
            AddProperty(property);
        }

        public virtual void AddProperty(PropertyItem property)
        {
            if (null == dicProperty)
            {
                dicProperty = new Dictionary<int, PropertyItem>();
            }
            if (dicProperty.ContainsKey(property.ID))
            {
                //remove same property
            }
            dicProperty.Add(property.ID, property);
            property.Owner = this;
        }

        public void RemoveProperty(PropertyType propertyType)
        {
            RemoveProperty((int)propertyType);
        }

        public void RemoveProperty(int id)
        {
            if (null != dicProperty && dicProperty.ContainsKey(id))
                dicProperty.Remove(id);
        }

        public void ClearProperty()
        {
            if (null != dicProperty)
            {
                dicProperty.Clear();
                dicProperty = null;
            }
        }

        #endregion

        #region Property Update
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public virtual PropertyItem GetProperty(PropertyType propertyType)
        {
            return GetProperty((int)propertyType);
        }

        /// <summary>
        /// 属性变更处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
		protected virtual void OnPropertyChanged(int id, object oldValue, object newValue)
        {
            //add update ....
            //			if (id == (int)EnumPropertyType.HP)
            //			{
            //
            //			}
        }

        /// <summary>
        /// 属性变更回调
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
		public void DoChangeProperty(int id, object oldValue, object newValue)
        {
            OnPropertyChanged(id, oldValue, newValue);
            if (null != PropertyChanged)
                PropertyChanged(this, id, oldValue, newValue);
        }

        public PropertyItem GetProperty(int id)
        {
            if (null == dicProperty)
                return null;
            if (dicProperty.ContainsKey(id))
                return dicProperty[id];
            Debug.LogWarning("Actor dicProperty non Property ID: " + id);
            return null;
        }
        #endregion

	}
}

