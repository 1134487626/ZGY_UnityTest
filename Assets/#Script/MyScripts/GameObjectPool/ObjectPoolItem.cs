using UnityEngine;

namespace Anole
{
    /// <summary>
    /// 局部对象池单个对象类
    /// 支持面向对象扩展功能
    /// </summary>
    public sealed partial class ObjectPoolItem
    {
        /// <summary> 返回在对象池中的自身 </summary>
        public GameObject Obj { get; private set; }

        /// <summary> 返回自身的Transform </summary>
        public Transform Form { get { return Obj.transform; } }

        private RectTransform rect;

        private Transform parent;//用以闲置时归类的同一父对象【方便Hierarchy查看】

        /// <summary> 返回自身的RectTransform </summary>
        public RectTransform Rect
        {
            get
            {
                if (rect == null)
                {
                    rect = Obj.GetComponent<RectTransform>();
                    if (rect == null)
                    {
                        DeBug.LogError("注意：该对象没有 RectTransform 组件！");
                    }
                }
                return rect;
            }
        }

        /// <summary> Resources 路径 </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 是否为闲置状态
        /// 根据此状态来获取实例化对象相对安全
        /// </summary>
        public bool Idle { get; set; }

        private ObjectPoolItem() { }

        /// <summary>
        /// 构造赋值对象池中的对象
        /// </summary>
        /// <param name="r_obj"></param>
        public ObjectPoolItem(GameObject r_obj, Transform r_parent, string r_path)
        {
            if (r_obj == null) DeBug.LogError("传入的对象为空!");
            else
            {
                Obj = r_obj;
                Path = r_path;
                parent = r_parent;
            }
        }

        /// <summary> 设置为休闲状态 </summary>
        public void SetTheIdle()
        {
            if (Idle)
            {
                Obj.SetActive(false);
                Form.parent = parent;
                TransformInit();
                DeBug.LogGolden($"   {Obj.name} 成功设置为闲置状态！");
            }
            else
            {
                Debug.Log("注意：该对象未设置为闲置状态！");
            }
        }

        /// <summary> 设置Transform组件初始默认参数 </summary>
        public void TransformInit()
        {
            Form.localScale = Vector3.one;
            Form.localEulerAngles = Vector3.zero;
            Form.localPosition = Vector3.zero;
        }

        /// <summary>  </summary>
        public void SetActive(bool r_bool) => Obj?.SetActive(r_bool);

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="r_parent">父对象</param>
        /// <param name="r_Actie">是否激活</param>
        /// <param name="worldPositionStays"> 同系统函数 </param>
        public void SetParent(Transform r_parent, bool worldPositionStays = false)
        {
            Form.SetParent(r_parent, worldPositionStays);
        }
    }
}
