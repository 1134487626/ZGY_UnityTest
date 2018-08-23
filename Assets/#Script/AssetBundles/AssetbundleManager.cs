using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anole
{
    public partial class AssetbundleManager : Sington<AssetbundleManager>
    {
        /// <summary>
        /// 对象缓存
        /// </summary>
        public Dictionary<string, Object> objectCache;

        /// <summary>
        /// bundle的缓存
        /// </summary>
        public Dictionary<string, AssetBundle> bundleCache;

        /// <summary>
        /// 得到bundle配置
        /// </summary>
        private AssetBundleManifest abMainfest;

        private string mainBundleName = "Assetbundle";

        public AssetbundleManager()
        {
            objectCache = new Dictionary<string, Object>();

            bundleCache = new Dictionary<string, AssetBundle>();

            InitMainfest();
        }

        private void InitMainfest()
        {
            ////初始化bundle配置
            //AssetBundle ab = AssetBundle.LoadFromFile(PathManager.GetAssetBundlePath() + mainBundleName);
            //abMainfest = ab.LoadAsset<AssetBundleManifest>("AssetbundleManifest");
            //ab.Unload(false);
        }


        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public T LoadSynch<T>(string bundleName, string assetName) where T : Object
        {
            //一开始都要先从缓存里面找，如果找到了直接跳出
            T mObject = null;
            mObject = (T)FindObject(bundleName, assetName);

            //缓存有值，直接返回
            if (mObject != null)
                return mObject;
            else
            {
                //缓存没值，直接去bundle里面找
                AssetBundle ab = LoadAssetBundle(bundleName);

                mObject = ab.LoadAsset<T>(assetName);

                //添加缓存
                AddObject(bundleName, assetName, mObject);

                return mObject;
            }
        }

        /// <summary>
        /// 加载bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        private AssetBundle LoadAssetBundle(string bundleName)
        {
            //先找到这个bundle里面的依赖
            string[] depends = abMainfest.GetAllDependencies(bundleName);

            //递归加载所有的依赖
            foreach (var dependName in depends)
            {
                LoadAssetBundle(dependName);
            }
            AssetBundle ab = GetBundle(bundleName);

            return ab;
        }

        //加载不到资源，输出信息
        private void ShowLoadError(string filename)
        {
            if (filename.Length > 0) Debug.LogError("加载资源不存在！filename=" + filename);
        }
    }
}