using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Queue<GameObject> _objQueue = new Queue<GameObject>();
    
    private GameObject _obj = null;

    public void Init(GameObject prefab)
    {
        _obj = prefab;
        _objQueue.Enqueue(CreateObj());
    }

    private GameObject CreateObj()
    {
        GameObject newObject = GameObject.Instantiate(_obj);
        newObject.SetActive(false);

        return newObject;
    }

    public GameObject GetObj()
    {
        GameObject newObj = (_objQueue.Count > 0) ? newObj = _objQueue.Dequeue() : newObj = CreateObj();

        return newObj;
    }

    public void ReturnObj(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        _objQueue.Enqueue(obj);
    }
}
