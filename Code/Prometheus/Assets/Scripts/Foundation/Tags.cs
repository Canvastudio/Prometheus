using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETag : IEqualityComparer<ETag>
{
    private static Dictionary<string, ETag> dictionary = new Dictionary<string, ETag>();

    private static ulong uid = 1;

    public static ETag Tag(string name)
    {
        ETag eTag;

        if (!dictionary.TryGetValue(name, out eTag))
        {
            eTag = new ETag(name, uid);
            dictionary[name] = eTag;
            uid *= 2;
        }

        return eTag;
    }

    public static ETag[] GetETag(params string[] names)
    {
        ETag[] result = new ETag[names.Length];

        for(int i = 0; i < names.Length; ++i)
        {
            var name = names[i];
            ETag eTag;

            if (!dictionary.TryGetValue(name, out eTag))
            {
                eTag = new ETag(name, uid);
                dictionary[name] = eTag;
                uid *= 2;
            }

            result[i] = eTag;
        }

        return result;
    }

    private ETag(string name, ulong uid)
    {
        this.entityTag = uid;
        this.name = name;
    }


    public ulong entityTag = 0;
    public string name;

    public bool Equals(ETag x, ETag y)
    {
        if (x.entityTag == y.entityTag)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetHashCode(ETag obj)
    {
        return base.GetHashCode();
    }
}

public interface ITagable
{
    HashSet<ETag> Etag { get; set; }
}

public class EntitysTag<E> where E : ITagable
{
    Dictionary<ETag, List<E>> curTagMap = new Dictionary<ETag, List<E>>();

    public void AddEntity(E entity, params ETag[] primaryTag)
    {
        foreach (var tag in primaryTag)
        {
            List<E> entityList;

            if (!curTagMap.TryGetValue(tag, out entityList))
            {
                entityList = new List<E>();
                curTagMap.Add(tag, entityList);
            }

            entityList.Add(entity);

            if (entity.Etag == null)
            {
                entity.Etag = new HashSet<ETag>();
            }

            entity.Etag.Add(tag);
        }
    }

    public void RemoveEntity(E entity)
    {
        foreach (var tag in entity.Etag)
        {
            List<E> entitys;
            if (curTagMap.TryGetValue(tag, out entitys))
            {
                entitys.Remove(entity);
            }
        }

        entity.Etag.Clear();
    }

    public void RemoveEntityTag(E entity, ETag tag)
    {
        List<E> entitys;

        if (curTagMap.TryGetValue(tag, out entitys))
        {
            entitys.Remove(entity);
            entity.Etag.Remove(tag);
        }
    }

    public void SetEntityTag(E entity, ETag tag, bool has)
    {
        if (has)
        {
            AddEntity(entity, tag);
        }
        else
        {
            RemoveEntityTag(entity, tag);
        }
    }

    public List<E> GetEntity(params ETag[] tags)
    {
        List<E> entitys;
        List<E> res = new List<E>(3);

        for (int n = 0; n < tags.Length; ++n)
        {
            ETag tag = tags[n];

            if (curTagMap.TryGetValue(tag, out entitys))
            {
                for (int i = 0; i < entitys.Count; ++i)
                {
                    for (int m = n; m < tags.Length; ++m)
                    {
                        if (!entitys[i].Etag.Contains(tags[m]))
                        {
                            break;
                        }

                        if (m == tags.Length - 1)
                        {
                            res.Add(entitys[i]);
                        }
                    }

                }

                break;
            }
        }

        return res;
    }

    /// <summary>
    /// 换回指定类型的对象，而不是返回基本类型，需要调用时确保类型正确
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <param name="tags"></param>
    /// <returns></returns>
    public List<M> GetEntity<M>(params ETag[] tags) where M : E
    {
        List<E> entitys;
        List<M> res = new List<M>(3);

        for (int n = 0; n < tags.Length; ++n)
        {
            ETag tag = tags[n];

            if (curTagMap.TryGetValue(tag, out entitys))
            {
                for (int i = 0; i < entitys.Count; ++i)
                {
                    for (int m = n; m < tags.Length; ++m)
                    {
                        if (!entitys[i].Etag.Contains(tags[m]))
                        {
                            break;
                        }

                        if (m == tags.Length - 1)
                        {
                            res.Add((M)entitys[i]);
                        }
                    }

                }

                break;
            }
        }

        return res;
    }
}


