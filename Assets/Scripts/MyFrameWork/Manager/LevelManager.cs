using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ZFrameWork
{
    /// <summary>
    /// Level Manager 
    /// </summary>
	public class LevelManager :DDOLSingleton<LevelManager>
	{
		#region SceneInfoData class

		public class SceneInfoData
		{
            // 关卡管理类
			public Type SceneType { get; private set; }

			public string SceneName { get; private set; }

			public object[] Params { get; private set; }

			public SceneInfoData(string _sceneName, Type _sceneType, params object[] _params)
			{
				this.SceneType = _sceneType;
				this.SceneName = _sceneName;
				this.Params = _params;
			}
		}
        #endregion

        #region Base Data
        private Dictionary<ScnType, SceneInfoData> dicSceneInfos = null;

        private BaseScene currentScene = new BaseScene();

        public ScnType LastSceneType { get; set; }

        public ScnType ChangeSceneType { get; private set; }

        private UIType sceneOpenUIType = UIType.None;
        private object[] sceneOpenUIParams = null;

        private Image m_loadingImg;
        private bool m_bLoadingImgFill = false;
        private float m_fTarget = 0;
        private float m_fProBarSpeed = 2;
        private float m_fDelayTime = 0.3f;

        public BaseScene CurrentScene
        {
            get { return currentScene; }
            set
            {
                currentScene = value;
                //				if (null != currentScene)
                //				{
                //					currentScene.Load();
                //				}
            }
        }

        private  void InitContainer()
        {
            dicSceneInfos = new Dictionary<ScnType, SceneInfoData>();
        }

        public void OnInit()
        {
            InitContainer();

            // Registe All Scene
            RegisterAllScene();
        }
        #endregion

        #region Scene Register & UnRegister
        /// <summary>
        /// 场景注册
        /// </summary>
        private void RegisterAllScene()
        {
            RegisterScene(ScnType.LoadingScene,"LoadingScene",typeof(EmptyScn));
            // Example
            RegisterScene(ScnType.StartGame, "Start", typeof(StartScn), null);

            RegisterScene(ScnType.VillageScene, "Village", typeof(VillageScn), null);

            //RegisterScene(ScnType.Battle, ScnType.Battle.ToString(), typeof(CompEditorScn), null);
        }

        /// <summary>
        /// Register Scene
        /// </summary>
        /// <param name="_sceneID">关卡ID</param>
        /// <param name="_sceneName">关卡名</param>
        /// <param name="_sceneType">关卡管理类</param>
        /// <param name="_params">参数</param>
        private void RegisterScene(ScnType _sceneID, string _sceneName, Type _sceneType, params object[] _params)
        {
            if (_sceneType == null || _sceneType.BaseType != typeof(BaseScene))
            {
                throw new Exception("Register scene type must not null and extends BaseScene");
            }
            if (!dicSceneInfos.ContainsKey(_sceneID))
            {
                SceneInfoData sceneInfo = new SceneInfoData(_sceneName, _sceneType, _params);
                dicSceneInfos.Add(_sceneID, sceneInfo);
            }
        }

        public void UnRegisterScene(ScnType _sceneID)
        {
            if (dicSceneInfos.ContainsKey(_sceneID))
            {
                dicSceneInfos.Remove(_sceneID);
            }
        }

        public bool IsRegisterScene(ScnType _sceneID)
        {
            return dicSceneInfos.ContainsKey(_sceneID);
        }

        internal BaseScene GetBaseScene(ScnType _sceneType)
        {
            Debug.Log(" GetBaseScene  sceneId = " + _sceneType.ToString());
            SceneInfoData sceneInfo = GetSceneInfo(_sceneType);
            if (sceneInfo == null || sceneInfo.SceneType == null)
            {
                return null;
            }
            BaseScene scene = System.Activator.CreateInstance(sceneInfo.SceneType) as BaseScene;
            return scene;
        }

        public SceneInfoData GetSceneInfo(ScnType _sceneID)
        {
            if (dicSceneInfos.ContainsKey(_sceneID))
            {
                return dicSceneInfos[_sceneID];
            }
            Debug.LogError("This Scene is not register! ID: " + _sceneID.ToString());
            return null;
        }

        public string GetSceneName(ScnType _sceneID)
        {
            if (dicSceneInfos.ContainsKey(_sceneID))
            {
                return dicSceneInfos[_sceneID].SceneName;
            }
            Debug.LogError("This Scene is not register! ID: " + _sceneID.ToString());
            return null;
        }

        public void ClearScene()
        {
            dicSceneInfos.Clear();
        }
        #endregion

        #region Change Scene Direction

        /// <summary>
        /// Change Scene Direct
        /// </summary>
        /// <param name="_sceneType"></param>
        public void ChangeSceneDirect(ScnType _sceneType)
		{
			UIManager.Instance.CloseUIAll();

			if (CurrentScene != null)
			{
				CurrentScene.Release();
				CurrentScene = null;
			}

			LastSceneType = ChangeSceneType;
			ChangeSceneType = _sceneType;
            SceneInfoData sid = GetSceneInfo(_sceneType);
			string sceneName = GetSceneName(_sceneType);
           
			//change scene
			MonoHelper.Instance.StartCoroutine(AsyncLoadScene(sceneName,()=> 
            {
                // 注册场景Module
                ModuleManager.Instance.RegisterModule(sid.SceneType);

                if (sceneOpenUIType != UIType.None)
                {
                    UIManager.Instance.OpenUICloseOthers(sceneOpenUIType, false, sceneOpenUIParams);
                    sceneOpenUIType = UIType.None;
                }
            }));
		}

        /// <summary>
        /// 场景切换
        /// </summary>
        /// <param name="_sceneType"></param>
        /// <param name="_uiType"></param>
        /// <param name="_params"></param>
		public void ChangeSceneDirect(ScnType _sceneType, UIType _uiType, params object[] _params)
		{
			sceneOpenUIType = _uiType;
			sceneOpenUIParams = _params;

            if (ChangeSceneType != _sceneType)
            {
                ChangeSceneDirect(_sceneType);
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
		private IEnumerator<AsyncOperation> AsyncLoadScene(string sceneName,Action _cbDone = null)
		{
            AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);

            yield return oper;
            // message send
            if (oper.isDone && _cbDone != null)
            {
                _cbDone();
            }
		}
        #endregion

        #region Change Scene By Loading
   
        void Update()
        {
            if (m_bLoadingImgFill)
            {
                m_loadingImg.fillAmount = Utils.Approach(m_loadingImg.fillAmount, m_fTarget, Time.deltaTime * m_fProBarSpeed);
            }
        }
        public void ChangeScene(ScnType _sceneType)
        {
            UIManager.Instance.CloseUIAll();

            if (CurrentScene != null)
            {
                CurrentScene.Release();
                CurrentScene = null;
            }
            LastSceneType = ChangeSceneType;
            ChangeSceneType = _sceneType;
            //change loading scene
            MonoHelper.Instance.StartCoroutine(AsyncLoadOtherScene());
        }

        /// <summary>
        /// 加载有过渡loading的界面切换
        /// </summary>
        /// <param name="_sceneType"></param>
        /// <param name="_uiType"></param>
        /// <param name="_params"></param>
        public void ChangeScene(ScnType _sceneType, UIType _uiType, params object[] _params)
        {
            sceneOpenUIType = _uiType;
            sceneOpenUIParams = _params;
            ChangeScene(_sceneType);
        }

        private IEnumerator AsyncLoadOtherScene()
        {
            string sceneName = GetSceneName(ScnType.LoadingScene);
            AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
            yield return oper;
            if (oper.isDone)
            {
                GameObject uiObj = GameObject.Find("Canvas");
                if (uiObj != null)
                {
                    m_bLoadingImgFill = true;

                    Transform LoadingProgressBar = uiObj.transform.Find("Bg/LoadingProgressBar");
                    m_loadingImg = LoadingProgressBar.GetComponent<Image>();
                    m_loadingImg.fillAmount = 0;

                    // we start loading the scene
                    SceneInfoData sid = GetSceneInfo(ChangeSceneType);
                    String strNextScnName = sid.SceneName;


                    AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync(strNextScnName, LoadSceneMode.Single);
                    _asyncOperation.allowSceneActivation = false;

                    // while the scene loads, we assign its progress to a target that we'll use to fill the progress bar smoothly
                    while (_asyncOperation.progress < 0.9f)
                    {
                        m_bLoadingImgFill = true;

                        m_fTarget = _asyncOperation.progress;
                        yield return null;
                    }
                    // when the load is close to the end (it'll never reach it), we set it to 100%
                    m_fTarget = 1f;

                    // we wait for the bar to be visually filled to continue
                    while (LoadingProgressBar.GetComponent<Image>().fillAmount != m_fTarget)
                    {
                        yield return null;
                    }
                    m_bLoadingImgFill = false;

                    // we switch to the new scene
                    _asyncOperation.allowSceneActivation = true;

                    yield return new WaitForSeconds(0.5f);
                    // the load is now complete, we replace the bar with the complete animation
                    if (_asyncOperation.isDone)
                    {
                        // the load is now complete, we replace the bar with the complete animation
                        SceneLoadCompleted(sid);
                    }

                }
            }
        }
        
        /// <summary>
        /// loading 完成
        /// </summary>
		void SceneLoadCompleted(SceneInfoData _scInfo)
        {          
            // Register Module
            ModuleManager.Instance.RegisterModule(_scInfo.SceneType);

            //Open UI
            if (sceneOpenUIType != UIType.None)
            {
                UIManager.Instance.OpenUI(sceneOpenUIType, false, sceneOpenUIParams);
                sceneOpenUIType = UIType.None;
            }
        }
        #endregion
    }
}

