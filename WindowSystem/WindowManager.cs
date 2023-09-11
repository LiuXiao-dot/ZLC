using UnityEngine;
using UnityEngine.SceneManagement;
using XWEngine.UGUI;
using ZLC.Application;
using ZLC.Common;
using ZLC.ConfigSystem;
using ZLC.EventSystem;
using ZLC.ResSystem;
using ZLC.SceneSystem;
using ZLC.UISystem;
using Assembly = System.Reflection.Assembly;
namespace ZLC.WindowSystem
{
    /// <summary>
    /// 窗口管理器
    /// </summary>
    public class WindowManager : IWindowManager, IManager, ILoader, ISubscriber<SceneMessage>
    {
        private RectTransform _root;
        private RectTransform[] _layerRoots;
        private Stack<IWindowCtl>[] _ctlStacks;
        private Stack<AWindowView>[] _viewStacks;
        private IResLoader _resLoader;
        private WindowConfigsSO _config;
        private WindowLayer _lastLayer;
        
        /// <summary>
        /// 窗口配置
        /// </summary>
        private IWindowConfig _windowConfig;

        public void Init()
        {
            var windowConfig = Assembly.Load("AutoGenerate").GetType("XWEngine.UGUI.WindowConfig");
            _windowConfig = (IWindowConfig)Activator.CreateInstance(windowConfig);
            SceneMessageQueue.Instance.Subscribe(this, SceneMessage.OnSceneOpen);
        }

        ~WindowManager()
        {
            Dispose();
        }
        public void Dispose()
        {
        }

        public void Load(Action<int, int> onProgress)
        {
            _resLoader = IAppLauncher.Get<IResLoader>();
            _resLoader.LoadAsset<WindowConfigsSO>("WindowConfigsSO.asset", (success, so) =>
            {
                if (!success) {
                    Debug.LogError("窗口配置文件加载失败");
                    return;
                }
                _config = so;

                var layers = _config.layers;
                var count = layers.Count;
                _viewStacks = new Stack<AWindowView>[count];
                _ctlStacks = new Stack<IWindowCtl>[count];
                _layerRoots = new RectTransform[count];
                var index = 0;
                foreach (var layer in layers) {
                    _ctlStacks[index] = new Stack<IWindowCtl>();
                    _viewStacks[index] = new Stack<AWindowView>();
                    var layerGo = new GameObject(layer.Key.ToString());
                    var rectTransform = layerGo.AddComponent<RectTransform>();
                    rectTransform.SetParent(_root);
                    rectTransform.localScale = Vector3.one;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.offsetMin = Vector2.zero;
                    rectTransform.offsetMax = Vector2.zero;
                    _layerRoots[index] = rectTransform;
                    index++;
                    _lastLayer = layer.Key;
                }
                onProgress.Invoke(1, 1);
            });
        }

        public void OnMessage(Event inGameEvent)
        {
            switch ((SceneMessage)inGameEvent.operate) {
                case SceneMessage.OnSceneOpen:
                    // 判断是否加载了UI场景，并在UI场景中初始化WindowManager
                    if (inGameEvent.data.Equals(AppConfigSO.Instance.uiSceneName)) {
                        SceneMessageQueue.Instance.Unsubscribe(this, SceneMessage.OnSceneOpen);
                        var uiScene = SceneManager.GetSceneByName(Path.GetFileNameWithoutExtension(AppConfigSO.Instance.uiSceneName));
                        var uiScenePrefab = uiScene.GetRootGameObjects();
                        _root = (RectTransform)uiScenePrefab[0].transform.Find("WindowManager");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            SceneMessageQueue.Instance.Callback(inGameEvent);
        }

        /// <summary>
        /// 同步加载的方式打开界面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IWindowCtl OpenWindow(int id, object args = null)
        {
            _resLoader.LoadAssetSync<GameObject>(GetWindowPath(id), out var windowPrefab);
            if (windowPrefab == null) {
                Debug.LogError($"窗口{id}没有对应的prefab");
                return default;
            }
            var view = windowPrefab.GetComponent<AWindowView>();
            var layer = view.windowLayer;
            var layerIndex = (int)layer;
            switch (layer) {

                case WindowLayer.MAIN:
                case WindowLayer.CHILD:
                    // 高于layer的窗口会被关闭（Loading层自己控制开关不处理）
                    for (var i = WindowLayer.POPUP; i > layer; i--) {
                        ClearWindows(i);
                    }
                    // 上一级窗口的暂停判断(弹窗和面板不会使子窗口暂停)
                    for (var i = layer; i >= 0; i--) {
                        var index = (int)i;
                        if (_ctlStacks[index].Count > 0) {
                            var pauseCtl = _ctlStacks[index].Peek();
                            WindowMessageQueue.Instance.SendEvent(WindowMessage.BeforeWindowPause, id);
                            pauseCtl.Pause();
                            if (layer == i) pauseCtl.GetView().gameObject.SetActive(false);
                            WindowMessageQueue.Instance.SendEvent(WindowMessage.AfterWindowPause, id);
                            break;
                        }
                    }
                    break;
                case WindowLayer.PANEL:
                    break;
                case WindowLayer.POPUP:
                    break;
                case WindowLayer.LOADING:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var ctlStack = _ctlStacks[layerIndex];
            if (ctlStack.TryPeek(out var tempCtl) && tempCtl.GetWindowID() == id) {
                // 已开启的界面，再刷新一次
                tempCtl.SetValue(args);
                tempCtl.ReOpen();
                return default;
            }

            var ctl = CreateWindowCtl(id);
            ctlStack.Push(ctl);
            ctl.SetValue(args);
            ctl.SetView(OpenView(view));
            WindowMessageQueue.Instance.SendEvent(WindowMessage.AfterWindowLoaded, id);
            WindowMessageQueue.Instance.SendEvent(WindowMessage.BeforeWindowEnter, id);
            ctl.Open();
            WindowMessageQueue.Instance.SendEvent(WindowMessage.AfterWindowEnter, id);
            return ctl;
        }

        protected virtual void OnOpenView(GameObject windowGo)
        {
            
        }

        private void ClearWindows(WindowLayer layer)
        {
            var layerIndex = (int)layer;
            var count = _ctlStacks[layerIndex].Count;
            for (int i = 0; i < count; i++) {
                CloseWindow(_ctlStacks[layerIndex].Peek().GetWindowID());
            }
        }

        public void OpenMainWindow()
        {
            
        }
        public void OpenLoadingWindow(params object[] objects)
        {
            
        }
        /// <summary>
        /// todo:修改成pushTo
        /// </summary>
        /// <param name="id"></param>
        public void CloseWindow(int id)
        {
            if (!_resLoader.CheckAsset<GameObject>(GetWindowPath(id), out var windowPrefab)) {
                return;
            }
            var prefabView = windowPrefab.GetComponent<AWindowView>();
            var layer = prefabView.windowLayer;
            var layerIndex = (int)layer;
            if(_ctlStacks[layerIndex].Count == 0) return;
            if (_ctlStacks[layerIndex].Peek().GetWindowID() != id) return;
            if (_ctlStacks[layerIndex].Peek() is IBeforeWindowClose beforeWindowClose && beforeWindowClose.BeforeWindowClose()) {
                return;
            }
            var ctl = _ctlStacks[layerIndex].Pop();

            
            WindowMessageQueue.Instance.SendEvent(WindowMessage.BeforeWindowExit, id);
            ctl.Close();
            ctl = null;

            var view = _viewStacks[layerIndex].Peek();
            CloseView(view);

            WindowMessageQueue.Instance.SendEvent(WindowMessage.AfterWindowExit, id);

            if (layer <= WindowLayer.CHILD) {
                // 上一级窗口的恢复判断(弹窗和面板不会使子窗口恢复)
                for (var i = WindowLayer.CHILD; i >= 0; i--) {
                    if (_ctlStacks[(int)i].Count > 0) {
                        var pauseCtl = _ctlStacks[(int)i].Peek();
                        WindowMessageQueue.Instance.SendEvent(WindowMessage.BeforeWindowResume, pauseCtl.GetWindowID());
                        if (layer == i) pauseCtl.GetView().gameObject.SetActive(true);
                        pauseCtl.Resume();
                        WindowMessageQueue.Instance.SendEvent(WindowMessage.AfterWindowResume, pauseCtl.GetWindowID());
                        break;
                    }
                }
            }
        }

        #region View
        private string GetWindowPath(int id)
        {
            return $"{id.ToString()}.prefab";
        }

        private AWindowView OpenView(AWindowView prefabView)
        {
            var layerIndex = (int)prefabView.windowLayer;
            var newWindow = GameObject.Instantiate(prefabView.gameObject, _layerRoots[layerIndex]);
            OnOpenView(newWindow);
            var newWindowView = newWindow.GetComponent<AWindowView>();
            _viewStacks[layerIndex].Push(newWindowView);
            RefreshSortingOrder();
            return newWindowView;
        }

        /// <summary>
        /// 刷新Canvas的sorting order
        /// </summary>
        private void RefreshSortingOrder()
        {
            var layer = WindowLayer.MAIN;
            var sortingOrder = _config.layers[layer] + _viewStacks[(int)layer].Count - 1;
            foreach (var tempWindows in _viewStacks) {
                foreach (var tempWindow in tempWindows) {
                    var canvas = tempWindow.GetComponent<Canvas>();
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = sortingOrder;
                    sortingOrder--;
                }

                if (layer == _lastLayer) return;
                layer++;
                sortingOrder = _config.layers[layer] + _viewStacks[(int)layer].Count - 1;
            }
        }

        private void CloseView(AWindowView windowView)
        {
            var layerIndex = windowView.windowLayer;
            var poped = _viewStacks[(int)layerIndex].Pop();
            _resLoader.ReleaseGameObject(poped.gameObject);
        }
        #endregion

        private IWindowCtl CreateWindowCtl(int id)
        {
            return _windowConfig.CreateWindowCtl(id);
        }
    }
}