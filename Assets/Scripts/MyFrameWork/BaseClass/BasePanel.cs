using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZFrameWork
{
    /// <summary>
    /// Sub UI Panel
    /// </summary>
    public abstract class BasePanel : BaseUI
    {
        /**|Parent UI|**/
        public Transform parent;
        public virtual void OnShow() { }

        public virtual void OnHide() { }

        public void OnSetParent(Transform _tf)
        {
            if (_tf != null)
            {
                parent = _tf;
            }
        }

        protected override void OnReady()
        {
            base.OnReady();
            OnShow();
        }
    }
}

