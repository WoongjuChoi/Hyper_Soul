using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviourPun
{
    private Dictionary<string, Queue<GameObject>> _objPoolDictionary = new Dictionary<string, Queue<GameObject>>();


    public void Init(string prefabName, int amount)
    {
        Queue<GameObject> objQueue = new Queue<GameObject>();
        for (int i = 0; i < amount; ++i)
        {
            objQueue.Enqueue(CreateObj(prefabName));
        }
        _objPoolDictionary.Add(prefabName, objQueue);
    }

    private GameObject CreateObj(string prefabName)
    {
        GameObject newObject = null;

        newObject = PhotonNetwork.InstantiateRoomObject(prefabName, Vector3.zero, Quaternion.identity);
        newObject.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.All, false);
        //newObject.SetActive(false);

        return newObject;
    }


    public GameObject GetObj(string prefabName)
    {
        GameObject newObj = (_objPoolDictionary[prefabName].Count > 0) ? _objPoolDictionary[prefabName].Dequeue() : CreateObj(prefabName);

        return newObj;
    }

    public void ReturnObj(GameObject obj)
    {
        obj.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.All, false);
        if(false == _objPoolDictionary.ContainsKey(obj.name))
        {
            Debug.Log("리턴시 키값이 맞지않음");
        }
        _objPoolDictionary[obj.name].Enqueue(obj);
        _objPoolDictionary.Add(obj.name, _objPoolDictionary[obj.name]);
    }
}
