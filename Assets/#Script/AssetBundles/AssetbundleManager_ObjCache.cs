using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anole
{
    public partial class AssetbundleManager
    {
        /// <summary>
        /// 清缓存
        /// </summary>
        public void Clear()
        {
            objectCache.Clear();
        }


        /// <summary>
        /// 从缓存中查找是否有这个对象
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        public Object FindObject(string bundleName, string assetName)
        {
            string key = bundleName + assetName;
            Object obj = null;
            objectCache.TryGetValue(key, out obj);
            return obj;
        }

        /// <summary>
        /// 添加一个缓存
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        /// <param name="obj"></param>
        public void AddObject(string bundleName, string assetName, Object obj)
        {
            if (obj == null) return;
            string key = bundleName + assetName;
            objectCache[key] = obj;
        }

    }
}