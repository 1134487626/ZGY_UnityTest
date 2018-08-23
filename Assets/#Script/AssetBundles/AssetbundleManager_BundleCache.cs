using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anole
{
    public partial class AssetbundleManager
    {
        /// <summary>
        /// 得到bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        private AssetBundle GetBundle(string bundleName)
        {
            //    //判断配置表是否有这个AB文件
            //    if (abMainfest.GetAllAssetBundles().HasValue(bundleName))
            //    {
            //        //去缓存里面找
            //        AssetBundle ab = null;
            //        bundleCache.TryGetValue(bundleName, out ab);

            //        //如果没有就去获取
            //        if (ab == null)
            //        {
            //            ab = AssetBundle.LoadFromFile(PathManager.GetAssetBundlePath() + bundleName);

            //            //并且加入到缓存里面
            //            bundleCache.Add(bundleName, ab);
            //        }

            //        return ab;
            //    }
            //    else
            //    {
            //        ShowLoadError(bundleName);
            //    }

            return null;
        }

        /// <summary>
        /// 清除所有的bundle
        /// </summary>
        public void ClearBundle()
        {
            foreach (var item in bundleCache)
            {
                item.Value.Unload(false);
            }
            bundleCache.Clear();
        }
    }
}