using System.Collections.Generic;
using UnityEngine;

namespace Anole
{
    /// <summary>
    /// 克隆每一个对象池对象的基础【缓存】
    /// </summary>
    public class ObjectPoolResources
    {
        //保存Resources同一路径下的所有实例化对象
        private List<ObjectPoolItem> AllItem = new List<ObjectPoolItem>();

        private ObjectPoolResources() { }

        public Transform Parent { get; private set; }

        public ObjectPoolResources(string r_path) { }

        /// <summary> 基础克隆对象 </summary>
        public GameObject Obj { get; private set; }

        /// <summary> 基础克隆对象名字 </summary>
        public string Name { get; private set; }

        public ObjectPoolResources(GameObject r_obj, Transform r_parent)
        {
            if (r_obj == null)
            {
                DeBug.LogError(">>>>>>  传入的对象为空!");
            }
            else
            {
                Obj = r_obj;
                Name = r_obj.name;
                Parent = r_parent;
            }
        }

        /// <summary>
        /// 查找有无空闲的对象
        /// </summary>
        public ObjectPoolItem FindIdle
        {
            get
            {
                for (int i = AllItem.Count - 1; i >= 0; i--)
                {
                    if (AllItem[i] != null)
                    {
                        if (AllItem[i].Idle) return AllItem[i];
                    }
                    else
                    {
                        AllItem.RemoveAt(i);
                        DeBug.LogRed("注意：查找是发现空对象！");
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 保存每次实例化的对象
        /// </summary>
        /// <param name="r_item"></param>
        public void AddPoolItem(ObjectPoolItem r_item)
        {
            if (r_item != null)
            {
                AllItem.Add(r_item);
            }
        }

        /// <summary>
        /// 将统一路径下的所有实例化的对象统一设置为休闲
        /// 自行考虑有任一有在使用过程的对象
        /// 是否一起设置为空闲状态
        /// </summary>
        public void IdleAllItem()
        {
            if (AllItem?.Count > 0)
            {
                for (int i = 0; i < AllItem.Count; i++)
                {
                    AllItem[i].Idle = true;
                    AllItem[i].SetTheIdle();
                }
            }
        }

        /// <summary>
        /// 查找某个资源集合中是否有任意一个正在使用中
        /// </summary>
        public bool FindItemPoolIdle
        {
            get
            {
                for (int i = 0; i < AllItem.Count; i++)
                {
                    if (AllItem[i]?.Idle == false) return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 销毁某一个对象
        /// </summary>
        /// <param name="r_item"></param>
        /// <param name="r_Immediate"></param>
        public void DestroyItem(ObjectPoolItem r_item, bool r_Immediate)
        {
            if (AllItem?.Count > 0)
            {
                if (AllItem.Contains(r_item))
                {
                    GameObject game = r_item.Obj;

                    if (r_Immediate)
                    {
                        Object.DestroyImmediate(game);
                    }
                    else
                    {
                        Object.Destroy(game);
                    }

                    AllItem.Remove(r_item);
                }
            }
        }

        /// <summary>
        /// 销毁所有保存的对象
        /// </summary>
        public void DestroyAll(bool r_Immediate)
        {
            for (int i = AllItem.Count - 1; i >= 0; i--)
            {
                if (AllItem[i] != null)
                {
                    GameObject game = AllItem[i].Obj;

                    if (r_Immediate)
                    {
                        Object.DestroyImmediate(game);
                    }
                    else
                    {
                        Object.Destroy(game);
                    }
                }

                AllItem[i] = null;
            }

            AllItem.Clear();
        }
    }
}
