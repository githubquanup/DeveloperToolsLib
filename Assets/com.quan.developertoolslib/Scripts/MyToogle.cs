using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DeveloperToolsLib{
    /// <summary>
    /// 自定义Toggle组件
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class MyToggle : Selectable, IEventSystemHandler, IPointerClickHandler, ISubmitHandler, ICanvasElement
    {

        /// <summary>
        /// toggle为true时切换显示的底图
        /// </summary>
        public Graphic m_ChangeImage;

        [SerializeField]
        [FormerlySerializedAs("m_IsActive")]
        private bool m_IsOn;

        public MyToggle.ToggleTransition toggleTransition = MyToggle.ToggleTransition.Fade;
        // public ToggleGroup group;

        public Toggle.ToggleEvent onValueChanged = new Toggle.ToggleEvent();

        /// <summary>
        /// 设置ison属性（自动调用一次方法）
        /// </summary>
        public bool isOn
        {
            get
            {
                return this.m_IsOn;
            }
            set
            {
                this.Set(value);
            }
        }

        /// <summary>
        /// 设置ison属性（不会自动调用方法）
        /// </summary>
        public bool IsOn
        {
            get
            {
                return this.m_IsOn;
            }
            set
            {
                this.Set(value, value, value);
            }
        }


        protected MyToggle()
        {
        }

        public virtual void Rebuild(CanvasUpdate executing)
        {
            if (executing != CanvasUpdate.Prelayout)
                return;
            this.onValueChanged.Invoke(this.m_IsOn);
        }


        public virtual void LayoutComplete()
        {
        }


        public virtual void GraphicUpdateComplete()
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.PlayEffect(true);
            this.IsOn = false;
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            this.IsOn = false;
        }


        private void Set(bool value)
        {
            this.Set(value, true);
        }

        private void Set(bool value, bool sendCallback)
        {
            if (this.m_IsOn == value)
                return;
            this.m_IsOn = value;
            this.PlayEffect(this.toggleTransition == MyToggle.ToggleTransition.None);

            if (!sendCallback)
                return;
            this.onValueChanged.Invoke(this.m_IsOn);
        }

        private void Set(bool value, bool sendCallback, bool call)
        {
            if (this.m_IsOn == value)
                return;
            this.m_IsOn = value;
            this.PlayEffect(this.toggleTransition == MyToggle.ToggleTransition.None);

            if (!sendCallback)
                return;
        }

        private void InternalToggle()
        {
            if (!this.IsActive() || !this.IsInteractable())
                return;
            this.isOn = !this.isOn;
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="eventData">Current event.</param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            this.InternalToggle();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            this.InternalToggle();
        }
        

        public enum ToggleTransition
        {
            None,
            Fade,
        }


        [Serializable]
        public class ToggleEvent : UnityEvent<bool>
        {
        }

        protected override void OnDidApplyAnimationProperties()
        {
            if ((UnityEngine.Object)this.m_ChangeImage != (UnityEngine.Object)null)
            {
                bool flag = !Mathf.Approximately(this.m_ChangeImage.canvasRenderer.GetColor().a, 0.0f);
                if (this.m_IsOn != flag)
                {
                    this.m_IsOn = flag;
                    this.Set(!flag);
                }
            }
            base.OnDidApplyAnimationProperties();
        }

        private void PlayEffect(bool instant)
        {
            if ((UnityEngine.Object)this.m_ChangeImage == (UnityEngine.Object)null)
                return;
            if (!Application.isPlaying)
                this.m_ChangeImage.canvasRenderer.SetAlpha(!this.m_IsOn ? 0.0f : 1f);
            else
                this.m_ChangeImage.CrossFadeAlpha(!this.m_IsOn ? 0.0f : 1f, !instant ? 0.1f : 0.0f, true);
        }
        protected override void Start()
        {
            this.PlayEffect(true);
        }
        // #if UNITY_EDITOR
        // protected override void OnValidate()
        // {
        //     base.OnValidate();
        //     this.Set(this.m_IsOn, false);
        //     this.PlayEffect(this.toggleTransition == MyToggle.ToggleTransition.None);
        //     if (PrefabUtility.GetPrefabAssetType((UnityEngine.Object)this) != PrefabAssetType.NotAPrefab || Application.isPlaying)
        //         return;
        //     CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild((ICanvasElement)this);
        // }
        // #endif // UNITY_EDITOR

    }
}