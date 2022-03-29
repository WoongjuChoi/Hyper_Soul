using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Queue<GameObject> _objQueue = new Queue<GameObject>();
    
    private GameObject _obj = null;

    public void Init(GameObject prefab, GameObject type)
    {
        _obj = prefab;
        _objQueue.Enqueue(CreateObj(type));
    }

    private GameObject CreateObj(GameObject type)
    {
        GameObject newObject = GameObject.Instantiate(_obj);

        return newObject;
    }

    public GameObject GetObj()
    {
        GameObject newObj = _objQueue.Dequeue();

        return newObj;
    }

    public void ReturnObj(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        _objQueue.Enqueue(obj);
    }
}
