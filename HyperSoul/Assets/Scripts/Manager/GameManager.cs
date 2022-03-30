using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _spawnPosBase;
    [SerializeField]
    private InputField _chatMsg;
    [SerializeField]
    private Text[] _chatList;

    private int _chatMaxLine = 10;
    private Queue<string> _chatQueue;

    private bool[] _isSpawned = new bool[4];

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
        int index = Random.Range(0, 4);
        while (_isSpawned[index])
        {
            index = Random.Range(0, 4);
        }
        PhotonNetwork.Instantiate("BazookaPlayer", spawnPoint[index].position, Quaternion.identity);

        photonView.RPC("SpawnPosMarking", RpcTarget.AllBuffered, index);
    }

    [PunRPC]
    private void SpawnPosMarking(int spawnPosNum)
    {
        _isSpawned[spawnPosNum] = true;

        // µð¹ö±ë¿ë
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

        for(int i = _chatList.Length - 1; i >= 0; --i)
        {
            if(_chatList[i].text == "")
            {
                _input = true;
                _chatList[i].text = msg;
                break;
            }
        }
        if(_input == false)
        {
            for (int i = 1; i < _chatList.Length; ++i)
            {
                _chatList[i - 1].text = _chatList[i].text;
                _chatList[_chatList.Length].text = msg;
            }
        }

        
    }

    

}
