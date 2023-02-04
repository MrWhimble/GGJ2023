
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : PooledBehaviour
{
    private List<T> _objects;
    private T _prefab;

    public void Init(T prefab, int count)
    {
        _prefab = prefab;
        _objects = new List<T>();
        for (int i = 0; i < count; i++)
        {
            T obj = Object.Instantiate(_prefab);
            obj.name = $"{_prefab.GetType()} ({i})";
            obj.gameObject.SetActive(false);
            _objects.Add(obj);
            Debug.Log(i);
        }
    }

    public T Spawn()
    {
        int objCount = _objects.Count;
        for (int i = 0; i < objCount; i++)
        {
            if (_objects[i].gameObject.activeSelf)
                continue;

            _objects[i].Spawn();
            return _objects[i];
        }
        
        T obj = Object.Instantiate(_prefab);
        obj.name = $"{_prefab.GetType()} ({objCount})";
        _objects.Add(obj);
        obj.Spawn();
        return obj;
    }
}