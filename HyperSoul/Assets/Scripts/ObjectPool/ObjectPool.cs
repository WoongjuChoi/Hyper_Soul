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
            Debug.Log($"{prefabName} 키값없음");

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
        gameObject.SetActive(false);
        gameObject.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.All, false);

        _objPoolDictionary[prefabName[0]].Enqueue(gameObject);

        //Debug.Log($"{prefabName[0]} {gameObject.GetComponent<PhotonView>().ViewID} 리턴됨");
    }
}



//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObjectPool : MonoBehaviourPun, IPunPrefabPool
//{
//    private Dictionary<string, Queue<GameObject>> _objPoolDictionary = new Dictionary<string, Queue<GameObject>>();

//    //private Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

//    //[SerializeField]
//    //private GameObject _bazookaMisille;

//    public GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation)
//    {
//        GameObject newObject = PhotonNetwork.Instantiate(prefabName, position, rotation);
//        newObject.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.AllBuffered, false);
//        //newObject = GameObject.Instantiate(prefabId, position, rotation);
//        //newObject.SetActive(false);

//        return newObject;
//    }
//    public void Init(string prefabName, int playerId, int amount)
//    {
//        Queue<GameObject> objQueue = new Queue<GameObject>();
//        for (int i = 0; i < amount; ++i)
//        {
//            objQueue.Enqueue(Instantiate(prefabName, Vector3.zero, Quaternion.identity));
//        }
//        _objPoolDictionary.Add(prefabName + playerId.ToString(), objQueue);
//    }


//    public GameObject GetObj(string prefabName, int playerId)
//    {
//        if (false == _objPoolDictionary.ContainsKey(prefabName + playerId.ToString()))
//        {
//            Debug.Log($"{prefabName + playerId.ToString()} 키값없음");

//            foreach (var elem in _objPoolDictionary.Keys)
//            {
//                Debug.Log($"{elem}\n");
//            }
//            return null;
//        }
//        GameObject newObj = (_objPoolDictionary[prefabName + playerId.ToString()].Count > 0) ? _objPoolDictionary[prefabName + playerId.ToString()].Dequeue() : Instantiate(prefabName, Vector3.zero, Quaternion.identity);

//        return newObj;
//    }

//    public void Destroy(GameObject gameObject)
//    {
//        int playerId = gameObject.GetComponent<Projectile>().ProjectileOwnerID;
//        string[] prefabName = gameObject.name.Split('(');
//        gameObject.SetActive(false);
//        gameObject.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.AllBuffered, false);

//        _objPoolDictionary[prefabName[0] + playerId.ToString()].Enqueue(gameObject);

//        Debug.Log($"{prefabName[0] + playerId.ToString()} {gameObject.GetComponent<BazookaMissile>().GetComponent<PhotonView>().ViewID} 리턴됨");
//    }
//}
