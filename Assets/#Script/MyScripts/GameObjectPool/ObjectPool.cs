using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anole
{
    /// <summary>
    /// 局部对象池
    /// 支持面向对象扩展功能
    /// </summary>
    public sealed partial class ObjectPool : Sington<ObjectPool>
    {
        //保存Resources同一路径下的所有实例化对象等等数据
        Dictionary<string, ObjectPoolResources> m_AllPoolItem;

        //某一类对象的父对象，方便开发界面查看
        List<GameObject> m_ItemsParent;

        public Transform Form { get { return transform; } }

        private void Awake()
        {
            gameObject.SetActive(false);
            m_AllPoolItem = new Dictionary<string, ObjectPoolResources>();
            m_ItemsParent = new List<GameObject>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r_path"></param>
        /// <returns></returns>
        public ObjectPoolItem Clone(string r_path)
        {
            if (TryExists(r_path))
            {
                ObjectPoolResources tempPool = m_AllPoolItem[r_path];
                ObjectPoolItem poolItem = tempPool.FindIdle;

                if (poolItem == null)
                {
                    GameObject tempGame = Instantiate(tempPool.Obj);

                    if (tempGame != null)
                    {
                        tempGame.name = tempPool.Name;
                        poolItem = new ObjectPoolItem(tempGame, tempPool.Parent, r_path);
                        tempPool.AddPoolItem(poolItem);
                    }
                    else
                    {
                        DeBug.LogError("注意：要实例化对象为空！");
                    }
                }
                poolItem.Idle = false;
                return poolItem;
            }

            DeBug.LogError("注意：调用对象池的路径出错！");
            return null;
        }

        /// <summary>
        /// 将某个对象池的对象设置为休闲状态
        /// </summary>
        /// <param name="r_item"></param>
        public void Idle(ObjectPoolItem r_item)
        {
            if (r_item != null)
            {
                r_item.Idle = true;
                r_item.SetTheIdle();
            }
        }

        /// <summary>
        /// 将某个相同路径下的所有对象设置为休闲状态
        /// </summary>
        /// <param name="r_item"></param>
        public void IdleKeys(string r_path)
        {
            if (m_AllPoolItem.ContainsKey(r_path))
            {
                m_AllPoolItem[r_path]?.IdleAllItem();
            }
        }

        /// <summary>
        /// 将某个相同路径下的所有对象设置为休闲状态
        /// </summary>
        /// <param name="r_item"></param>
        public void IdleKeys(params string[] r_paths)
        {
            if (r_paths?.Length > 0)
            {
                for (int i = 0; i < r_paths.Length; i++)
                {
                    IdleKeys(r_paths[i]);
                }
            }
        }

        /// <summary>
        /// 查找某个资源集合中是否有任意一个正在使用中
        /// </summary>
        public bool FindSomeIsIdle(string r_path)
        {
            if (m_AllPoolItem.ContainsKey(r_path))
            {
                return m_AllPoolItem[r_path].FindItemPoolIdle;
            }
            return false;
        }

        /// <summary>
        /// 查询是否有该对象的基础类
        /// </summary>
        /// <param name="r_path"></param>
        /// <returns></returns>
        private bool TryExists(string r_path)
        {
            if (string.IsNullOrEmpty(r_path))
            {
                Debug.LogError("注意：传入Resources路径为空！");
                return false;
            }

            if (m_AllPoolItem.ContainsKey(r_path)) return true;

            GameObject game = Resources.Load<GameObject>(r_path);

            if (game == null)
            {
                Debug.LogError($"注意：Resources.加载出错！{r_path}");
                return false;
            }
            else
            {
                string name = "All" + game.name;
                GameObject parent = new GameObject(name);
                parent.transform.parent = transform;
                parent.SetActive(false);
                m_ItemsParent.Add(parent);

                //保存基础对象池对象类
                m_AllPoolItem.Add(r_path, new ObjectPoolResources(game, parent.transform));
                return true;
            }
        }

        /// <summary>
        /// 销毁某个对象池中的对象
        /// </summary>
        /// <param name="r_item"></param>
        public void DestroyItem(ObjectPoolItem r_item, bool r_Immediate = false)
        {
            if (r_item != null)
            {
                string strPath = r_item.Path;

                if (m_AllPoolItem.ContainsKey(strPath))
                {
                    m_AllPoolItem[strPath].DestroyItem(r_item, r_Immediate);
                }
            }
        }

        /// <summary>
        /// 销毁同一路径下的所有实例化对象
        /// 注意时间段以及是否要先查询有无使用的对象
        /// </summary>
        /// <param name="r_path"></param>
        public void DestroyItems(string r_path, bool r_Immediate = false)
        {
            if (m_AllPoolItem.ContainsKey(r_path))
            {
                GameObject temp = m_AllPoolItem[r_path].Parent.gameObject;
                m_AllPoolItem[r_path].DestroyAll(r_Immediate);
                m_AllPoolItem.Remove(r_path);

                //并删除默认的父级对象
                if (temp != null && m_ItemsParent.Contains(temp))
                {
                    m_ItemsParent.Remove(temp);
                    Destroy(temp);
                }
            }
        }

        /// <summary>
        /// 销毁所有对象池中的对象
        /// 注意时间段以及是否要先查询有无使用的对象
        /// </summary>
        public void DestroyAllItem(bool r_Immediate = false)
        {
            for (int i = m_ItemsParent.Count - 1; i >= 0; i--)
            {
                GameObject game = m_ItemsParent[i];

                if (r_Immediate)
                {
                    DestroyImmediate(game);
                }
                else
                {
                    Destroy(game);
                }
            }

            m_ItemsParent.Clear();
            m_AllPoolItem.Clear();
        }

    }
}
