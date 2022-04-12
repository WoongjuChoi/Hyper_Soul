using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviourPun
{
    private Dictionary<string, Queue<GameObject>> _objPoolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        Init("BazookaMissile", 5);

    }

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

        newObject = PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity);
        newObject.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.AllBuffered, false);
        //newObject.SetActive(false);

        return newObject;
    }


    public GameObject GetObj(string prefabName)
    {
        GameObject newObj = (_objPoolDictionary[prefabName].Count > 0) ? _objPoolDictionary[prefabName].Dequeue() : CreateObj(prefabName);

        return newObj;
    }

    public void ReturnObj(GameObject obj, string prefabName)
    {
        obj.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.AllBuffered, false);

        _objPoolDictionary[prefabName].Enqueue(obj);
    }
}
