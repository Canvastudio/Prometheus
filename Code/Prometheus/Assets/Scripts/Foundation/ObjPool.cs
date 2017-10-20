using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Obj<T>
{
    public T obj;

    public int id;

    public Obj(T o, int _id = int.MaxValue)
    {
        obj = o;
        id = _id;
    }
}

public static class ObjExtend
{
    public static void Push<T>(this List<Obj<T>> l, Obj<T> o)
    {
        l.Add(o);
    }

    public static Obj<T> Kick<T>(this List<Obj<T>> l, int id)
    {
        var res = l[id];

        l.RemoveAt(id);

        return res;
    }

    public static Obj<T> Pop<T>(this List<Obj<T>> l)
    {
        if (l.Count == 0) return null;

        var res = l[l.Count - 1];

        l.RemoveAt(l.Count - 1);

        return res;
    }

    public static Obj<T> Peek<T>(this List<Obj<T>> l)
    {
        if (l.Count == 0) return null;

        return l[l.Count - 1];
    }
}


class UNode<T>
{
    public bool instantiate = true;
    public T source;
    public List<Obj<T>> u = new List<Obj<T>>();
    public List<Obj<T>> uu = new List<Obj<T>>();
    public int capacity;
}

public class ObjPool<T> : SingleObject<ObjPool<T>> where T : Component {

    public Transform transform;

    protected override void Init()
    {
        base.Init();

        transform = new GameObject("ObjPoolRoot: type: " + typeof(T).Name).transform;
    }

    int _id = 1000;

    private Dictionary<string, UNode<T>> Data = new Dictionary<string, UNode<T>>();

    //如果对象池存在就回收所有的对象，如果不存在则创建
    public void InitOrRecyclePool(string name, T source, int count = 10, bool Instantiate = true)
    {
        if (Data.ContainsKey(name) == false)
        {
            float t1 = Time.realtimeSinceStartup;

            CoroCore.Instance.StartCoro(InitData(name, source, count, Instantiate));

            Debug.Log("Qx: Ini Pool: " + name + ", cast: " + (Time.realtimeSinceStartup - t1).ToString());
        }
        else
        {
            RecyclePool(name);
        }
    }

    IEnumerator InitData(string name, T source, int count = 10, bool Instantiate = true)
    {
        UNode<T> n = new UNode<T>();

        n.instantiate = Instantiate;
        n.capacity = count;

        if (Data.ContainsKey(name))
        {
            Debug.LogError("Qing Xin: Use same key name.");
        }

        T o;

        Data[name] = n;

        n.source = source;

        int m = 0;

        for (int i = 0; i < count; i = Data[name].u.Count + Data[name].uu.Count)
        {
            if (Instantiate)
            {
                o = GameObject.Instantiate<T>(source);

                o.name = o.name + i.ToString();

                (o.gameObject).SetActive(false);
                (o.gameObject).transform.SetParent(transform);
            }
            else
            {
                o = source;
            }

            n.uu.Push(new Obj<T>(o, _id++));

            ++m;

            //if (m > 5)
            //{
            //    yield return 0;
            //}
        }

        n.capacity = Data[name].u.Count + Data[name].uu.Count;

        yield return 0;
    }



    public T GetObjFromPoolWithID(out int id,string name, T source = null, Component componet = null, bool autoCreatePool = true)
    {
        T res;

        id = 0;

        if (Data.ContainsKey(name) == false)
        {
            Debug.LogError("Qing Xin: Try to get unexcept obj from pool.");

            if (autoCreatePool)
            {
                CoroCore.Instance.StartCoro(InitData(name, source));
            }
            else
            {
                return null;
            }
        }

        if (Data[name].uu.Count > 0)
        {
            var o = Data[name].uu.Pop();

            o.id = _id++;

            res = o.obj;

            if (res == null)
            {
                if (Data[name].instantiate)
                {
                    res = GameObject.Instantiate(Data[name].source);
                    Data[name].u.Push(new Obj<T>(res, _id++));
                }
                else
                {
                    res = Data[name].source;
                    Data[name].u.Push(new Obj<T>(res, _id++));
                }
            }

            Data[name].u.Push(o);
        }
        else
        {
           	//Debug.Log("Qx -------- UI obj pool is poor :" + name);

            if (Data[name].instantiate)
            {
                res = GameObject.Instantiate(Data[name].source);
                Data[name].u.Push(new Obj<T>(res, _id++));
            }
            else
            {
                res = Data[name].source;
                Data[name].u.Push(new Obj<T>(res, _id++));
            }

            Data[name].capacity = Data[name].u.Count + Data[name].uu.Count;
        }

        id = Data[name].u.Peek().id;

        return res;
    }


    public T GetObjFromPool(string name, T source = null, Component componet = null, bool autoCreatePool = true)
    {
        T res;

        if (Data.ContainsKey(name) == false)
        {
            Debug.LogError("Qing Xin: Try to get unexcept obj from pool.");

            if (autoCreatePool)
            {
                CoroCore.Instance.StartCoro(InitData(name, source));
            }
            else
            {
                return null;
            }
        }

        if (Data[name].uu.Count > 0)
        {
            var o = Data[name].uu.Pop();

            res = o.obj;

            if (res == null)
            {
                if (Data[name].instantiate)
                {
                    if (Data[name].source != null)
                        res = GameObject.Instantiate(Data[name].source);
                    Data[name].u.Push(new Obj<T>(res, _id++));
                }
                else
                {
                    res = Data[name].source;
                    Data[name].u.Push(new Obj<T>(res, _id++));
                }
            }

            Data[name].u.Push(o);
        }
        else
        {
            if (Data[name].instantiate)
            {
                res = GameObject.Instantiate(Data[name].source);
                Data[name].u.Push(new Obj<T>(res, _id++));
            }
            else
            {
                res = Data[name].source;
                Data[name].u.Push(new Obj<T>(res, _id++));
            }
     
            Data[name].capacity = Data[name].u.Count + Data[name].uu.Count;
        }

        

        return res;
    }

    public Object RecycleObj(string poolName, int id)
    {

        if (Data.ContainsKey(poolName))
        {
            for (int i = 0; i < Data[poolName].u.Count; ++i)
            {
                if (Data[poolName].u[i].id == id)
                {
                    var d = Data[poolName].u.Kick(i);

                    Data[poolName].uu.Push(d);

                    if (Data[poolName].instantiate)
                    {
                        //((GameObject)d.obj).SetActive(false);
                        if (d.obj != null)
						    d.obj.gameObject.SetActive(false);
                    }

                    d.id = int.MaxValue;

                    d.obj.gameObject.transform.SetParent(transform, true);

                    return d.obj;
                }
            }
        }

        return null;
    }

    public void RecyclePool(string name)
    {
        if (Data.ContainsKey(name) == false)
        {
            return;
        }

        while (Data[name].u.Count > 0)
        {
            var o = Data[name].u.Pop();
            var obj = (o).obj;

            if (obj != null)
            {
                Data[name].uu.Push(o);

                if (Data[name].instantiate)
                {
                    obj.gameObject.SetActive(false);
                }
                //obj.transform.SetParent(transform);
            }
        }

        if (Data[name].uu.Count < Data[name].capacity)
        {
            T o;

            for (int i = 0; i < Data[name].capacity; i = Data[name].uu.Count)
            {
                if (Data[name].instantiate)
                {
                    o = GameObject.Instantiate<T>(Data[name].source);

                    o.name = o.name + i.ToString();

                    (o).gameObject.SetActive(false);
                    (o).gameObject.transform.SetParent(transform);
                }
                else
                {
                    o = Data[name].source;
                }

                Data[name].uu.Push(new Obj<T>(o, _id++));
            }
        }
    }


    public void DiscardPool(string name)
    {
        if (Data.ContainsKey(name) == false)
        {
            return;
        }

        while(Data[name].u.Count > 0)
        {
            GameObject.Destroy(Data[name].u.Pop().obj);
        }

        Data[name].u = null;

        while (Data[name].uu.Count > 0)
        {
            GameObject.Destroy(Data[name].uu.Pop().obj);
        }

        Data[name].uu = null;
        Data[name].source = null;
        Data.Remove(name);
    }

    public void DiscardAll()
    {
        List<string> names = new List<string>(Data.Keys);

        for (int i = 0; i < names.Count; ++i)
        {
            DiscardPool(names[i]);
        }

        _id = int.MinValue;
    }

    public override void ResetData()
    {
        base.ResetData();

        DiscardAll();
    }
}
