using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPun
{
    [SerializeField]
    private InputField _chatMsg;
    [SerializeField]
    private Text[] _chatList;


    private void Update()
    {
        if (Keyboard.current[Key.Tab].wasPressedThisFrame && _chatMsg.isFocused == false)
        {
            _chatMsg.ActivateInputField();
        }
        if (Keyboard.current[Key.Enter].wasPressedThisFrame && _chatMsg.text != "")
        {
            SendChatMessage();
            _chatMsg.text = "";
        }
    }


    private void SendChatMessage()
    {
        string msg = $"[{PhotonNetwork.LocalPlayer.NickName}] \n {_chatMsg.text}";
        photonView.RPC("Chat", RpcTarget.OthersBuffered, msg);
        Chat(msg);
    }
    public void SendDieMessage(LivingEntity attacker, LivingEntity victim)
    {
        string msg = "";
        PlayerInfo attackerInfo = attacker.GetComponent<PlayerInfo>();
        PlayerInfo victimPlayerInfo = victim.GetComponent<PlayerInfo>();
        // MonsterInfo victimMonsterInfo =  victim.GetComponent<MonsterInfo>(); 만들기
        if (victimPlayerInfo != null)
        {
            msg = $"{attackerInfo.NickName}이(가) {victimPlayerInfo.NickName}을(를) 처치";
        }
        //else if(victimMonsterInfo != null)
        //{
        //    msg = $"{attackerInfo.NickName}이(가) {victimMonsterInfo.NickName}을(를) 처치";
        //}

        photonView.RPC("Chat", RpcTarget.OthersBuffered, msg);
        Chat(msg);
    }

    [PunRPC]
    private void Chat(string msg)
    {
        bool _input = false;

        for (int i = 0; i < _chatList.Length; ++i)
        {
            if (_chatList[i].text == "")
            {
                if (i != 0)
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
        if (_input == false)
        {
            for (int i = _chatList.Length - 1; i >= 1; --i)
            {
                _chatList[i].text = _chatList[i - 1].text;
            }
            _chatList[0].text = msg;
        }
    }
}
