using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField NicknameInput;

    [SerializeField]
    private Button _joinButton;
    [SerializeField]
    private Text _connetInfoText;

    private const string GAME_VERSION = "0.01";


    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        _joinButton.interactable = true;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }
    public override void OnJoinedRoom()
    {
        _connetInfoText.text = "Joined room";
        //PhotonNetwork.LoadLevel("Bajooka Sample Scene");
    }
    public void Connect()
    {
        _joinButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
        _connetInfoText.text = "Connecting to Master Server";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _joinButton.interactable = true;
        _connetInfoText.text = "Offline : Disconnected to Master Server \n Press connect button to reconnect";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        _connetInfoText.text = "No room, create new room";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
}
