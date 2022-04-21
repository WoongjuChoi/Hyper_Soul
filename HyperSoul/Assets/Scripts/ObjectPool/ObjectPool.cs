using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Dictionary<string, Queue<GameObject>> _objPoolDictionary = new Dictionary<string, Queue<GameObject>>();

    public GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = PhotonNetwork.Instantiate(prefabName, position, rotation);
        newObject.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.AllBuffered, false);

        return newObject;
    }
    public void Init(string prefabName, int amount)
    {
        Queue<GameObject> objQueue = new Queue<GameObject>();

        for (int i = 0; i < amount; ++i)
        {
            objQueue.Enqueue(Instantiate(prefabName, Vector3.zero, Quaternion.identity));
        }
        _objPoolDictionary.Add(prefabName, objQueue);
    }


    public GameObject GetObj(string prefabName)
    {
        if(false == _objPoolDictionary.ContainsKey(prefabName))
        {
            Debug.LogError($"{prefabName} 키값없음");

            foreach(var elem in _objPoolDictionary.Keys)
            {
                Debug.Log($"{elem}\n");
            }
            return null;
        }

        GameObject newObj = (_objPoolDictionary[prefabName].Count > 0) ? _objPoolDictionary[prefabName].Dequeue() : Instantiate(prefabName, Vector3.zero, Quaternion.identity);

        return newObj;
    }

    public void Destroy(GameObject gameObject)
    {
        string[] prefabName = gameObject.name.Split('(');

        gameObject.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.All, false);

        _objPoolDictionary[prefabName[0]].Enqueue(gameObject);
    }
}

