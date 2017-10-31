
using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ZFrameWork
{
    /// <summary>
    /// View Base Class
    /// </summary>
    public abstract class BaseUI : MonoBehaviour
    {
        #region Cache gameObject & transfrom

        /**|Sub Panel Parent|**/
        public Transform root;

        public object[] uiParams;

        private Transform _CachedTransform;
        /// <summary>
        /// Gets the cached transform.
        /// </summary>
        /// <value>The cached transform.</value>
        public Transform cachedTransform
        {
            get
            {
                if (!_CachedTransform)
                {
                    _CachedTransform = this.transform;
                }
                return _CachedTransform;
            }
        }

        private GameObject _CachedGameObject;
        /// <summary>
        /// Gets the cached game object.
        /// </summary>
        /// <value>The cached game object.</value>
        public GameObject cachedGameObject
        {
            get
            {
                if (!_CachedGameObject)
                {
                    _CachedGameObject = this.gameObject;
                }
                return _CachedGameObject;
            }
        }

        #endregion

        #region UIType & EnumObjectState
        /// <summary>
        /// The state.
        /// </summary>
        protected ObjectState state = ObjectState.None;

        /// <summary>
        /// Occurs when state changed.
        /// </summary>
        public event StateChangedEvent StateChanged;

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public ObjectState State
        {
            protected set
            {
                if (value != state)
                {
                    ObjectState oldState = state;
                    state = value;
                    if (null != StateChanged)
                    {
                        StateChanged(this, state, oldState);
                    }
                }
            }
            get { return this.state; }
        }

        /// <summary>
        /// Gets the type of the user interface.
        /// </summary>
        /// <returns>The user interface type.</returns>
        public abstract UIType GetUIType();

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

        #region When Open
        /**|1|**/
        void Awake()
        {
            this.State = ObjectState.Initial;
            root = gameObject.transform;
            event_action = new Dictionary<string, List<MessageEvent>>();
            OnAwake();
        }

        /**|2|**/
        protected virtual void OnAwake()
        {
            this.State = ObjectState.Loading;

            // Play When Open
            this.OnPlayOpenUIAudio();
        }

        /**|3|**/
        /// <summary>
        /// Play Music When Open
        /// </summary>
        protected virtual void OnPlayOpenUIAudio()
        {

        }

        /**|4|**/
        /// <summary>
        /// Set Params & Async Load Data When Open
        /// </summary>
        /// <param name="uiParams"></param>
        public void SetUIWhenOpening(params object[] uiParams)
        {
            SetUI(uiParams);

            MonoHelper.Instance.StartCoroutine(AsyncOnLoadData());
        }

        /**|5|**/
        protected virtual void SetUI(params object[] uiParams)
        {
            this.State = ObjectState.Loading;
            this.uiParams = uiParams;
        }

        /**|6|**/
        private IEnumerator AsyncOnLoadData()
        {
            yield return new WaitForSeconds(0);
            if (this.State == ObjectState.Loading)
            {
                this.OnLoadData();
                this.State = ObjectState.Ready;
            }
        }

        /**|7ing|**/
        /// <summary>
        /// Load Data When Open
        /// </summary>
        protected virtual void OnLoadData() { }

        /**|8|**/
        void Start()
        {
            // Initial Containers
            InitContainer();

            InitUI();

            Register();

            OnReady();
        }

        /**|9|**/
        /// <summary>
        /// Initialize Containers 
        /// </summary>
        protected virtual void InitContainer() { }

        /**|10|**/
        /// <summary>
        /// Initialize UI 
        /// </summary>
        protected virtual void InitUI() { }

        /**|11|**/
        protected virtual void Register() { }

        /**|12|**/
        protected virtual void OnReady() { }

        #endregion

        #region When Close

        /// <summary>
        /// Release this instance.
        /// </summary>
        public void Release()
        {
            this.State = ObjectState.Closing;
            GameObject.Destroy(cachedGameObject);
            OnRelease();
        }


        protected virtual void OnRelease()
        {
            this.OnPlayCloseUIAudio();
            UnRegisterAllMsg();
        }

        /// <summary>
        /// Play Music When Close
        /// </summary>
        protected virtual void OnPlayCloseUIAudio()
        {

        }
        #endregion

        #region Update
        /// <summary>
        /// UI Top
        /// </summary>
        protected virtual void SetDepthToTop()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (ObjectState.Ready == this.state)
            {
                OnUpdate(Time.deltaTime);
            }
        }

        protected virtual void OnUpdate(float deltaTime)
        {

        }
        #endregion
    }
}

