using System;

using System.Collections;

using System.Collections.Generic;

using System.Runtime.InteropServices.WindowsRuntime;

using UnityEngine;



//设置自动回收的 字典id

public struct PoolID

{

    public int id;

    public float time;

    public PoolID(int id_, float time_)

    {

        id = id_;

        time = time_;

    }

}



public class ObjPool : MonoBehaviour

{



    public static Dictionary<int, Stack<Component>> pool_go = new Dictionary<int, Stack<Component>>();

    public static List<Component> returntf = new List<Component>();

    public static List<PoolID> returntime = new List<PoolID>();



    // Use this for initialization

    void Awake()
    {

        DontDestroyOnLoad(this);

    }



    private void Update()
    {//需要自动回收的在这里回收
        if (returntf.Count > 0)
        {

            int c = returntf.Count - 1;//ֱ倒序删除list

            for (int i = c; i > -1; i--)

            {

                var pd = returntime[i];

                if (pd.time < Time.time)

                {

                    Component C = returntf[i];

                    pool_go[pd.id].Push(C);

                    C.gameObject.SetActive(false);

                    returntf.RemoveAt(i);

                    returntime.RemoveAt(i);

                }



            }

        }



    }

    //根据需要初始化 数组的大小

    public static void SetPoolStartSize<T>(T key, int count) where T : Component
    {
        Stack<Component> temp;

        int name = key.GetHashCode();

        if (!pool_go.TryGetValue(name, out temp))
        {

            temp = new Stack<Component>(count);

            pool_go.Add(name, temp);

        }



        for (int i = 0; i < count; i++)
        {

            T go;

            //   go = GameObject.Instantiate<T>(prefab).GetComponent<T>();

            go = Instantiate<T>(key, Vector3.one * 99999, Quaternion.identity);

            go.hideFlags = HideFlags.HideInHierarchy;

            temp.Push(go);

            go.gameObject.SetActive(false);

        }



    }

    public static T GetComponent<T>(T key, Vector3 pos, Quaternion rot) where T : Component
    {
        Stack<Component> qc;

        T go;

        int name = key.GetHashCode();

        if (pool_go.TryGetValue(name, out qc))

        {

            if (qc.Count > 0)

            {

                go = (T)qc.Pop();

                go.transform.SetPositionAndRotation(pos, rot);

            }

            else

                go = Instantiate<T>(key, pos, rot);



        }

        else

        {

            qc = new Stack<Component>();

            pool_go.Add(name, qc);

            go = Instantiate<T>(key, pos, rot);

            // go.hideFlags = HideFlags.HideInHierarchy;

        }

        go.gameObject.SetActive(true);

        return go;

    }



    //可以自动回收  设置自动回收的时间

    public static T GetComponent<T>(T key, Vector3 pos, Quaternion rot, float t) where T : Component
    {
        Stack<Component> qc;

        T go;

        int name = key.GetHashCode();

        if (pool_go.TryGetValue(name, out qc))

        {

            if (qc.Count > 0)

            {

                go = (T)qc.Pop();

                go.transform.SetPositionAndRotation(pos, rot);

            }

            else

                go = Instantiate<T>(key, pos, rot);

        }

        else

        {

            qc = new Stack<Component>();

            pool_go.Add(name, qc);

            go = Instantiate<T>(key, pos, rot);

            // go.hideFlags = HideFlags.HideInHierarchy;


        }

        returntf.Add(go);

        returntime.Add(new PoolID(name, Time.time + t));

        go.gameObject.SetActive(true);

        return go;

    }

    // 使用示例

    // Transform tf=ObjPool.GetComponent<Transform>(keyTf);

    public static T GetComponent<T>(T key) where T : Component

    {

        Stack<Component> qc;

        T go;

        int name = key.GetHashCode();

        if (pool_go.TryGetValue(name, out qc))

        {

            if (qc.Count > 0)

                go = (T)qc.Pop();

            else

                go = Instantiate<T>(key);

        }

        else

        {

            qc = new Stack<Component>();

            pool_go.Add(name, qc);

            go = Instantiate<T>(key);

            // go.hideFlags = HideFlags.HideInHierarchy;
        }

        go.gameObject.SetActive(true);

        return go;

    }



    public static void ReturnGO<T>(T go, T key, Action act = null) where T : Component

    {

        if (act != null)

            act();

        int name = key.GetHashCode();

        if (!pool_go.ContainsKey(name))

        {

            pool_go.Add(name, new Stack<Component>());

        }

        pool_go[name].Push(go);

        go.gameObject.SetActive(false);

    }



}