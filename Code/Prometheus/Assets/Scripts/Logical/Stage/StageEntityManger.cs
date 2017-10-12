using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETag : IEqualityComparer<ETag>
{
    private static Dictionary<string, ETag> dictionary = new Dictionary<string, ETag>();

    private static ulong uid = 1;

    public static ETag CreateETag(string name)
    {
        ETag eTag = new ETag(name, uid);
        dictionary[name] = eTag;
         uid *= 2;

        return eTag;
    }

    public ETag(string name, ulong uid)
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

public abstract class EntitysTag<E> where E : ITagable
{
    Map<ETag, List<E>> curTagMap = new Map<ETag, List<E>>();

    public void AddEntity(E entity, ETag[] primaryTag)
    {
        foreach (var tag in primaryTag)
        {
            if (!curTagMap.ContainsKey(tag))
            {
                curTagMap.Add(tag, new List<E>());
            }

            curTagMap[tag].Add(entity);
            entity.Etag.Add(tag);
        }
    }

    public void RemoveEntity(E entity)
    {
        foreach(var tag in entity.Etag)
        {
            curTagMap[tag].Remove(entity);
        }
    }

    public void RemoveEntity(E entity, ETag tag)
    {
        curTagMap[tag].Remove(entity);
    }
}


