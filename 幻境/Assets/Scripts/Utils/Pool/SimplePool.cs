///
/// Simple pooling for Unity.
///   Author: Martin "quill18" Glaude (quill18@quill18.com)
///   Latest Version: https://gist.github.com/quill18/5a7cfffae68892621267
///   License: CC0 (http://creativecommons.org/publicdomain/zero/1.0/)
///   UPDATES:
/// 	2015-04-16: Changed Pool to use a Stack generic.
/// 
/// Usage:
/// 
///   There's no need to do any special setup of any kind.
/// 
///   Instead of call Instantiate(), use this:
///       SimplePool.Spawn(somePrefab, somePosition, someRotation);
/// 
///   Instead of destroying an object, use this:
///       SimplePool.Despawn(myGameObject);
/// 
///   If desired, you can preload the pool with a number of instances:
///       SimplePool.Preload(somePrefab, 20);
/// 
/// Remember that Awake and Start will only ever be called on the first instantiation
/// and that member variables won't be reset automatically.  You should reset your
/// object yourself after calling Spawn().  (i.e. You'll have to do things like set
/// the object's HPs to max, reset animation states, etc...)
/// 
/// 
/// 


using UnityEngine;
using System.Collections.Generic;

public static class SimplePool
{

    // You can avoid resizing of the Stack's internal array by
    // setting this to a number equal to or greater to what you
    // expect most of your pool sizes to be.
    // Note, you can also use Preload() to set the initial size
    // of a pool -- this can be handy if only some of your pools
    // are going to be exceptionally large (for example, your bullets.)
    //你可以避免重新调整Stack的内部数组
    //将此值设置为等于或大于您的数字
    //期望你的大多数游泳池大小。
    //注意，您也可以使用Preload（）来设置初始大小
    //一个游泳池 - 如果只是你的一些游泳池，这可能很方便
    //会非常大（例如，你的子弹）。
    const int DEFAULT_POOL_SIZE = 3;

    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// Pool类表示特定预制件的池。
    /// </summary>
    class Pool
    {
        // We append an id to the name of anything we instantiate.
        // This is purely cosmetic.
        //我们将id添加到我们实例化的任何名称。
        //这纯粹是装饰性的。
        int nextId = 1;

        // The structure containing our inactive objects.
        // Why a Stack and not a List? Because we'll never need to
        // pluck an object from the start or middle of the array.
        // We'll always just grab the last one, which eliminates
        // any need to shuffle the objects around in memory.
        //包含我们的非活动对象的结构。
        //为什么是堆栈而不是列表？ 因为我们永远不需要
        //从数组的开头或中间拔出一个对象。
        //我们总是抓住最后一个，这就消除了
        //任何需要在内存中随机播放对象
        Stack<GameObject> inactive;

        // The prefab that we are pooling
        //我们正在使用的预制件
        GameObject prefab;

        // Constructor
        //构造函数
        public Pool(GameObject prefab, int initialQty)
        {
            this.prefab = prefab;

            // If Stack uses a linked list internally, then this
            // whole initialQty thing is a placebo that we could
            // strip out for more minimal code.
            //如果Stack在内部使用链表，那么这个
            //整个initialQty是我们可以安慰剂
            //剥离出更多的代码。
            inactive = new Stack<GameObject>(initialQty);
        }

        // Spawn an object from our pool
        //从我们的池中生成一个对象
        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject obj;
            if (inactive.Count == 0)
            {
                // We don't have an object in our pool, so we
                // instantiate a whole new object.
                //我们的池中没有对象，所以我们
                //实例化一个全新的对象。
                obj = (GameObject)GameObject.Instantiate(prefab, pos, rot);
                obj.name = prefab.name + " (" + (nextId++) + ")";

                // Add a PoolMember component so we know what pool
                // we belong to.
                //添加一个PoolMember组件，以便我们知道哪个池
                //我们属于。
                obj.AddComponent<PoolMember>().myPool = this;
            }
            else
            {
                // Grab the last object in the inactive array
                //抓取非活动数组中的最后一个对象
                obj = inactive.Pop();

                if (obj == null)
                {
                    // The inactive object we expected to find no longer exists.
                    // The most likely causes are:
                    //   - Someone calling Destroy() on our object
                    //   - A scene change (which will destroy all our objects).
                    //     NOTE: This could be prevented with a DontDestroyOnLoad
                    //	   if you really don't want this.
                    // No worries -- we'll just try the next one in our sequence.
                    //我们期望找到的非活动对象不再存在。
                    //最可能的原因是：
                    //  - 有人在我们的对象上调用Destroy（）
                    //  - 场景变化（会破坏我们所有的物体）。
                    //注意：使用DontDestroyOnLoad可以防止这种情况
                    //如果你真的不想要这个
                    //不用担心 - 我们只是按顺序尝试下一个。
                    return Spawn(pos, rot);
                }
            }
            if (pos != Vector3.zero)
            {
                obj.transform.position = pos;
            }
            obj.transform.rotation = rot;
            obj.SetActive(true);
            return obj;

        }

        /// <summary>
        /// Return an object to the inactive pool.
        /// 将对象返回到非活动池。
        /// </summary>
        /// <param name="obj"></param>
        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);

            // Since Stack doesn't have a Capacity member, we can't control
            // the growth factor if it does have to expand an internal array.
            // On the other hand, it might simply be using a linked list 
            // internally.  But then, why does it allow us to specificy a size
            // in the constructor? Stack is weird.
            //由于Stack没有Capacity成员，我们无法控制
            //增长因子，如果它必须扩展内部数组。
            //另一方面，它可能只是使用链表
            //内部 但是，为什么它允许我们具体规模
            //在构造函数中？ 堆栈很奇怪。
            inactive.Push(obj);
        }

    }


    /// <summary>
    /// Added to freshly instantiated objects, so we can link back
    /// to the correct pool on despawn.
    /// 添加到新近实例化的对象，以便我们可以链接回来
    ///在despawn上正确的池。
    /// </summary>
    class PoolMember : MonoBehaviour
    {
        public Pool myPool;
    }

    // All of our pools
    //我们所有的对象池
    static Dictionary<GameObject, Pool> pools;

    /// <summary>
    /// Init our dictionary.
    /// 我们的字典。
    /// </summary>
    static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
    {
        if (pools == null)
        {
            pools = new Dictionary<GameObject, Pool>();
        }
        if (prefab != null && pools.ContainsKey(prefab) == false)
        {
            pools[prefab] = new Pool(prefab, qty);
        }
    }

    /// <summary>
    /// If you want to preload a few copies of an object at the start
    /// of a scene, you can use this. Really not needed unless you're
    /// going to go from zero instances to 10+ very quickly.
    /// Could technically be optimized more, but in practice the
    /// Spawn/Despawn sequence is going to be pretty darn quick and
    /// this avoids code duplication.
    ///如果要在开始时预加载一些对象的副本
    ///场景，你可以使用它。 真的不需要，除非你是
    ///很快就会从零实例变为10+。
    ///技术上可以更多地优化，但在实践中
    /// Spawn / Despawn序列非常快速
    ///这可以避免代码重复。
    /// </summary>
    static public void Preload(GameObject prefab, int qty = 1)
    {
        Init(prefab, qty);

        // Make an array to grab the objects we're about to pre-spawn.
        //创建一个数组来存储我们即将产生的对象。
        GameObject[] obs = new GameObject[qty];
        for (int i = 0; i < qty; i++)
        {
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        // Now despawn them all.
        //现在把它们全部消灭了。
        for (int i = 0; i < qty; i++)
        {
            Despawn(obs[i]);
        }
    }

    /// <summary>
    /// Spawns a copy of the specified prefab (instantiating one if required).
    /// NOTE: Remember that Awake() or Start() will only run on the very first
    /// spawn and that member variables won't get reset.  OnEnable will run
    /// after spawning -- but remember that toggling IsActive will also
    /// call that function.
    ///生成指定预制件的副本（如果需要，实例化一个）。
    ///注意：请记住，Awake（）或Start（）只会在第一个上运行
    /// spawn并且成员变量不会被重置。 OnEnable将运行
    ///生成 - 但请记住切换IsActive也会
    ///调用该函数。 
    /// </summary>
    static public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        Init(prefab);

        return pools[prefab].Spawn(pos, rot);
    }
    static public GameObject Spawn(GameObject prefab)
    {
        Init(prefab);

        return pools[prefab].Spawn(Vector3.zero, Quaternion.identity);
    }
    static public GameObject Spawn(GameObject prefab, Vector3 pos)
    {
        Init(prefab);

        return pools[prefab].Spawn(pos, Quaternion.identity);
    }
    /// <summary>
    /// Despawn the specified gameobject back into its pool.
    /// 将指定的游戏对象回收到池中。
    /// </summary>
    static public void Despawn(GameObject obj)
    {
        PoolMember pm = obj.GetComponent<PoolMember>();
        if (pm == null)
        {
            Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
            GameObject.Destroy(obj);
        }
        else
        {
            pm.myPool.Despawn(obj);
        }
    }

}