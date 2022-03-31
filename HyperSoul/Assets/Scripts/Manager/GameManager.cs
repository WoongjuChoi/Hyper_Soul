using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private Transform _spawnPosBase;
    [SerializeField]
    private InputField _chatMsg;
    [SerializeField]
    private Text[] _chatList;

    private bool[] _isSpawned = new bool[5];

    void Start()
    {
        SpawnPlayer();
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    void Update()
    {
        if(Keyboard.current[Key.Tab].wasPressedThisFrame && _chatMsg.isFocused == false)
        {
            _chatMsg.ActivateInputField();
        }
        if (Keyboard.current[Key.Enter].wasPressedThisFrame && _chatMsg.text != "")
        {
            SendChatMessage();
            _chatMsg.text = "";
        }

    }

    private void SpawnPlayer()
    {
        Transform[] spawnPoint = _spawnPosBase.GetComponentsInChildren<Transform>();
        int index = Random.Range(1, 5);
        while (_isSpawned[index])
        {
            index = Random.Range(1, 5);
        }
        Debug.Log($"스폰위치 {index}");
        PhotonNetwork.Instantiate("BazookaPlayer", spawnPoint[index].position, Quaternion.identity);

        photonView.RPC("SpawnPosMarking", RpcTarget.AllBuffered, index);
    }

    private void SpawnPosMarking(int spawnPosNum)
    {
        _isSpawned[spawnPosNum] = true;

        // 디버깅용
        for (int i = 0; i < _isSpawned.Length; ++i)
        {
            Debug.Log($"{i} : {_isSpawned[i]}");
        }
    }

    private void SendChatMessage()
    {
        string msg = $"[{PhotonNetwork.LocalPlayer.NickName}] \n {_chatMsg.text}";
        photonView.RPC("Chat", RpcTarget.OthersBuffered, msg);
        Chat(msg);
    }

    [PunRPC]
    private void Chat(string msg)
    {
        bool _input = false;

        for(int i = 0; i < _chatList.Length; ++i)
        {
            if(_chatList[i].text == "")
            {
                if(i != 0)
                {
                    for (int j = i; j >= 1; --j)
                    {
                        _chatList[j].text = _chatList[j - 1].text;
                    }
                }
                _input = true;
                _chatList[0].text = msg;
                break;
            }
        }
        if(_input == false)
        {
            for (int i = _chatList.Length - 1; i >= 1 ; --i)
            {
                _chatList[i].text = _chatList[i - 1].text;
            }
            _chatList[0].text = msg;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isSpawned);
        }
        else
        {
            _isSpawned = (bool[])stream.ReceiveNext();
        }
    }
}
