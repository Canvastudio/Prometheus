using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Obj
{
    public Object obj;

	public GameObject m_gameobject;

    public int id;

    public Obj(Object o, int _id = int.MaxValue)
    {
        obj = o;
		m_gameobject = (GameObject)obj;
        id = _id;
    }
}

public static class ObjExtend
{
    public static void Push(this List<Obj> l, Obj o)
    {
        l.Add(o);
    }

    public static Obj Kick(this List<Obj> l, int id)
    {
        var res = l[id];

        l.RemoveAt(id);

        return res;
    }

    public static Obj Pop(this List<Obj> l)
    {
        if (l.Count == 0) return null;

        var res = l[l.Count - 1];

        l.RemoveAt(l.Count - 1);

        return res;
    }

    public static Obj Peek(this List<Obj> l)
    {
        if (l.Count == 0) return null;

        return l[l.Count - 1];
    }
}


class UNode
{
    public bool instantiate = true;
    public Component component;
    public Object source;
    public List<Obj> u = new List<Obj>();
    public List<Obj> uu = new List<Obj>();
    public int capacity;
}

public class ObjPool : SingleGameObject<ObjPool> {

    int _id = int.MinValue;

    private Dictionary<string, UNode> Data = new Dictionary<string, UNode>();


    public void InitNewPool(string name, Object source, Component componet = null, int count = 10, bool Instantiate = true)
    {
        StartCoroutine(InitData(name, source, componet, count, Instantiate));
    }

    //如果对象池存在就回收所有的对象，如果不存在则创建
    public void InitOrRecyclePool(string name, Object source, Component componet = null, int count = 10, bool Instantiate = true)
    {
        if (Data.ContainsKey(name) == false)
        {
            float t1 = Time.realtimeSinceStartup;

            StartCoroutine(InitData(name, source, componet, count, Instantiate));

            Debug.Log("Qx: Ini Pool: " + name + ", cast: " + (Time.realtimeSinceStartup - t1).ToString());
        }
        else
        {
            RecyclePool(name);
        }
    }

    IEnumerator InitData(string name, Object source, Component componet = null, int count = 10, bool Instantiate = true)
    {
        UNode n = new UNode();
        n.instantiate = Instantiate;
        n.component = componet;
        n.capacity = count;

        if (Data.ContainsKey(name))
        {
            Debug.LogError("Qing Xin: Use same key name.");
        }

        Object o;

        Data[name] = n;

        n.source = source;
        n.component = componet;


        int m = 0;

        for (int i = 0; i < count; i = Data[name].u.Count + Data[name].uu.Count)
        {
            if (Instantiate)
            {
                o = GameObject.Instantiate(source) as GameObject;

                o.name = o.name + i.ToString();

                ((GameObject)o).SetActive(false);
                ((GameObject)o).transform.SetParent(transform);
            }
            else
            {
                o = source;
            }

            n.uu.Push(new Obj(o, _id++));

            ++m;

            //if (m > 5)
            //{
            //    yield return 0;
            //}
        }

        n.capacity = Data[name].u.Count + Data[name].uu.Count;

        yield return 0;
    }



    public Object GetObjFromPoolWithID(out int id,string name, GameObject source = null, Component componet = null, bool autoCreatePool = true)
    {
        Object res;

        id = 0;

        if (Data.ContainsKey(name) == false)
        {
            Debug.LogError("Qing Xin: Try to get unexcept obj from pool.");

            if (autoCreatePool)
            {
                StartCoroutine(InitData(name, source, componet));
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
                    Data[name].u.Push(new Obj(res, _id++));
                }
                else
                {
                    res = Data[name].source;
                    Data[name].u.Push(new Obj(res, _id++));
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
                Data[name].u.Push(new Obj(res, _id++));
            }
            else
            {
                res = Data[name].source;
                Data[name].u.Push(new Obj(res, _id++));
            }

            Data[name].capacity = Data[name].u.Count + Data[name].uu.Count;
        }

        id = Data[name].u.Peek().id;

        if (id > 0)
            Debug.LogError("Qx: id error");

        return res;
    }


    public Object GetObjFromPool(string name, GameObject source = null, Component componet = null, bool autoCreatePool = true)
    {
        Object res;

        if (Data.ContainsKey(name) == false)
        {
            Debug.LogError("Qing Xin: Try to get unexcept obj from pool.");

            if (autoCreatePool)
            {
                StartCoroutine(InitData(name, source, componet));
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
                    Data[name].u.Push(new Obj(res, _id++));
                }
                else
                {
                    res = Data[name].source;
                    Data[name].u.Push(new Obj(res, _id++));
                }
            }

            Data[name].u.Push(o);
        }
        else
        {
            if (Data[name].instantiate)
            {
                res = GameObject.Instantiate(Data[name].source);
                Data[name].u.Push(new Obj(res, _id++));
            }
            else
            {
                res = Data[name].source;
                Data[name].u.Push(new Obj(res, _id++));
            }
     
            Data[name].capacity = Data[name].u.Count + Data[name].uu.Count;
        }

        

        return res;
    }

    public Object RecycleObj(string poolName, int id)
    {
        //Debug.Log("Qx: recycle: " + id);

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
                        if (d.m_gameobject != null)
						    d.m_gameobject.SetActive(false);
                    }

                    d.id = int.MaxValue;

                    d.m_gameobject.transform.SetParent(transform, true);

                    return d.obj;
                }
            }
        }

        Debug.LogError("Qx ---------- RecycleObj faild: " + poolName + " id: " + id);

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
            var obj = ((GameObject)(o).obj);

            if (obj != null)
            {
                Data[name].uu.Push(o);

                if (Data[name].instantiate)
                {
                    obj.SetActive(false);
                }
                //obj.transform.SetParent(transform);
            }
        }

        if (Data[name].uu.Count < Data[name].capacity)
        {
            Object o;

            for (int i = 0; i < Data[name].capacity; i = Data[name].uu.Count)
            {
                if (Data[name].instantiate)
                {
                    o = GameObject.Instantiate(Data[name].source) as GameObject;

                    o.name = o.name + i.ToString();

                    ((GameObject)o).SetActive(false);
                    ((GameObject)o).transform.SetParent(transform);
                }
                else
                {
                    o = Data[name].source;
                }

                Data[name].uu.Push(new Obj(o, _id++));
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
        Data[name].component = null;
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
