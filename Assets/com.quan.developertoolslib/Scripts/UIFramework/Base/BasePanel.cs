using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace DeveloperToolsLib {
    public class BaseContext {
        public UIPanelType PanelType { get; private set; }
        public GameObject prefab { get; private set; }

        public BaseContext(UIPanelType type) {
            this.PanelType = type;
        }
        public BaseContext(UIPanelType type, GameObject _prefab) : this(type) {
            prefab = _prefab;
        }
    }
    public class BasePanel : MonoBehaviour {

        public virtual void InitOnce() {

        }

        public virtual void InitOnce(BaseContext context) {

        }
        /// <summary>
        /// 界面被显示出来
        /// </summary>
        public virtual void OnEnter() {

        }
        /// <summary>
        /// 界面被显示出来 携带消息
        /// </summary>
        public virtual void OnEnter(BaseContext context) {

        }
        /// <summary>
        /// 界面暂停
        /// </summary>
        public virtual void OnPause() {

        }

        /// <summary>
        /// 界面继续
        /// </summary>
        public virtual void OnResume() {

        }

        /// <summary>
        /// 界面不显示,退出这个界面，界面被关系
        /// </summary>
        public virtual void OnExit() {

        }
    }


}
