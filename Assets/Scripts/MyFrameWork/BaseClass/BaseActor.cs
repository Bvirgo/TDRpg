
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZFrameWork
{
    /// <summary>
    /// ���г���������ࣺNPC,Role,Monster
    /// </summary>
	public class BaseActor : MonoBehaviour,IDynamicProperty
    {
        #region Property
        // ���������ֵ�
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
            OnStart();
        }

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnStart()
        {

        }

        void OnDestory()
        {
            OnRelease();
        }

        protected virtual void OnRelease()
        { }
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
        /// ��ȡ����ֵ
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public virtual PropertyItem GetProperty(PropertyType propertyType)
        {
            return GetProperty((int)propertyType);
        }

        /// <summary>
        /// ���Ա������
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
        /// ���Ա���ص�
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

