using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    [System.Serializable]
    public class LoopScrollPrefabSource 
    {
        public string prefabName;
        public int poolSize = 5;

        private bool inited = false;
        public virtual GameObject GetObject()
        {
            if(!inited)
            {
                SG.ResourcePool.Instance.InitPool(prefabName, poolSize);
                inited = true;
            }
            return SG.ResourcePool.Instance.GetObjectFromPool(prefabName);
        }
    }
}
