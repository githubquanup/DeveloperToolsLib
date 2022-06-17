using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace DeveloperToolsLib {
    public class UIManager {

        /// 
        /// 单例模式的核心
        /// 1，定义一个静态的对象 在外界访问 在内部构造
        /// 2，构造方法私有化

        private static UIManager _instance;

        public static UIManager Instance {
            get {
                if (_instance == null) {
                    _instance = new UIManager();
                }
                return _instance;
            }
        }

        private Transform canvasTransform;
        private Dictionary<UIPanelType, string> panelPathDict;//存储所有面板Prefab的路径
        private Dictionary<UIPanelType, BasePanel> panelDict = new Dictionary<UIPanelType, BasePanel>();//保存所有实例化面板的游戏物体身上的BasePanel组件

        private Stack<BasePanel> panelStack = new Stack<BasePanel>();
        public Transform Canvas { get => canvasTransform; }
        private UIManager() {
            ParseUIPanelTypeJson();
        }
        public void Init() {

            canvasTransform = GameObject.FindWithTag("UIRoot").transform;

            //清除对象池
            ClearPanelPool();
        }
        /// <summary>
        /// 把某个页面入栈，  把某个页面显示在界面上
        /// </summary>
        public void PushPanel(UIPanelType panelType) {
            //判断一下栈里面是否有页面
            if (panelStack.Count > 0) {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }

            BasePanel panel = GetPanel(panelType);

            panel.OnEnter();

            panelStack.Push(panel);
        }
        /// <summary>
        /// 把某个页面入栈，  把某个页面显示在界面上
        /// 传递自定义信息类
        /// </summary>
        public void PushPanel(BaseContext context) {
            if (context.prefab != null) {
                BasePanel go = GetContextPanel(context);
                return;
            }

            //判断一下栈里面是否有页面
            if (panelStack.Count > 0) {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }

            BasePanel panel = GetContextPanel(context);

            panel.OnEnter(context);

            panelStack.Push(panel);
        }

        /// <summary>不使用堆栈，弹出界面</summary>
        public void PushPanelWithoutStack(BaseContext context) {
            // 存在的界面不弹出
            if (panelDict.ContainsKey(context.PanelType)) return;

            if (context.prefab != null) {
                BasePanel go = GetContextPanel(context);
                go.OnEnter();

                panelDict.Add(context.PanelType, go);
                return;
            }

            BasePanel panel = GetContextPanel(context);
            if (panel == null) {
                Debug.Log("目标界面路径为空：" + context.PanelType + "，请检查");
                return;
            }
            panel.OnEnter(context);

            panelDict.Add(context.PanelType, panel);
        }
        /// <summary>不使用堆栈，退出界面</summary>
        public void PopPanelWithoutStack(UIPanelType _type) {
            if (panelDict.ContainsKey(_type)) {

                BasePanel panel = panelDict.TryGet(_type);
                panel.OnExit();

                panelDict.Remove(_type);
            } else {
                Debug.LogError("字典中不包含目标Panel：" + System.Enum.GetName(typeof(UIPanelType), _type) + " 请检查");
            }

        }
        /// <summary>
        /// 出栈 ，把页面从界面上移除
        /// </summary>
        public void PopPanel() {
            if (panelStack.Count <= 0) return;

            //关闭栈顶页面的显示
            BasePanel topPanel = panelStack.Pop();
            topPanel.OnExit();

            if (panelStack.Count <= 0) return;
            BasePanel topPanel2 = panelStack.Peek();
            topPanel2.OnResume();

        }

        private BasePanel GetContextPanel(BaseContext context) {
            //如果找不到，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
            string path = panelPathDict.TryGet(context.PanelType);
            GameObject prefab = (UnityEngine.GameObject)(context.prefab == null ? Resources.Load(path) : context.prefab);
            GameObject instPanel = GameObject.Instantiate(prefab) as GameObject;
            instPanel.transform.SetParent(canvasTransform, false);
            instPanel.GetComponent<BasePanel>().InitOnce(context);

            return instPanel.GetComponent<BasePanel>();
        }
        /// <summary>
        /// 根据面板类型 得到实例化的面板
        /// </summary>
        /// <returns></returns>
        private BasePanel GetPanel(UIPanelType panelType) {
            BasePanel panel = panelDict.TryGet(panelType);

            if (panel == null) {
                //如果找不到，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
                string path = panelPathDict.TryGet(panelType);
                GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
                instPanel.transform.SetParent(canvasTransform, false);
                instPanel.GetComponent<BasePanel>().InitOnce();
                if (!panelDict.ContainsKey(panelType))
                    panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
                return instPanel.GetComponent<BasePanel>();
            } else {
                panel.transform.SetAsLastSibling();
                return panel;
            }
        }

        /// <summary>返回栈顶的界面</summary>
        public BasePanel PeekOrNull() {
            if (panelStack.Count != 0) {
                return panelStack.Peek();
            }
            return null;
        }
        public BasePanel GetTargetPanel(UIPanelType panelType) {
            BasePanel panel = panelDict.TryGet(panelType);
            return panel;
        }
        public void ClearPanelPool() {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
            panelStack = new Stack<BasePanel>();
        }
        [Serializable]
        class UIPanelTypeJson {
            public List<UIPanelInfo> infoList;
        }
        private void ParseUIPanelTypeJson() {
            panelPathDict = new Dictionary<UIPanelType, string>();

            TextAsset ta = Resources.Load<TextAsset>("UIPanelType");

            UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

            foreach (UIPanelInfo info in jsonObject.infoList) {
                //Debug.Log(info.panelType);
                panelPathDict.Add(info.panelType, info.path);
            }
        }


        #region 系统窗口
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}
                                        // const int SW_SHOWMAXIMIZED = 3;//最大化
                                        // const int SW_SHOWRESTORE = 1;//还原
        private bool isRestore = false;
        public void OnClickMinimize() { //最小化 
            ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
        }

        public void OnClickMaximize() {
            //最大化
            if (isRestore) {
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                // ShowWindow(GetForegroundWindow(), SW_SHOWMAXIMIZED);
            } else
                Screen.SetResolution(960, 540, FullScreenMode.Windowed);
            // ShowWindow(GetForegroundWindow(), SW_SHOWRESTORE);

            isRestore = !isRestore;
            // Screen.fullScreen = !isRestore;
        }
        public void OnClickQuit() {
            Application.Quit();
        }
        #endregion
    }

}