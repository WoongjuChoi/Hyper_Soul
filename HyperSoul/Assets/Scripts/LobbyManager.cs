using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    private const string GAME_VERSION = "0.01";

    [SerializeField]
    private GameObject _loginPanel;
    [SerializeField]
    private GameObject _lobbyPanel;
    [SerializeField]
    private GameObject _roomPanel;

    GameObject _curPanel;

    [SerializeField]
    private InputField _nickNameInput;
    [SerializeField]
    private InputField _roomNameInput;

    [SerializeField]
    private Button _joinButton;
    [SerializeField]
    private Button _createRoomButton;
    [SerializeField]
    private Button _randomJoinRoomButton;
    [SerializeField]
    private Text _connetInfoText;

    private byte _maxPlayer = 4;

   

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.AutomaticallySyncScene = true; // ������Ŭ���̾�Ʈ�� ���� �� ����ȭ
        _joinButton.interactable = true;
        _curPanel = _loginPanel;

    }
    private void Start()
    {
        // �Է� ���� �� ���� ����
        _nickNameInput.text = PlayerPrefs.GetString("USER_NICKNAME", "USER_" + Random.Range(1, 1000));
        _roomNameInput.text = PlayerPrefs.GetString("ROOM_NAME", "ROOM_" + Random.Range(1, 1000));
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = _nickNameInput.text;
        _connetInfoText.text = "Connect to Master Server";
    }
    public void Connect()
    {
        _joinButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
        _connetInfoText.text = "Connecting to Master Server";
        ChangePanel(_roomPanel);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(_roomNameInput.text, new RoomOptions { MaxPlayers = _maxPlayer }, null);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(_roomNameInput.text, new RoomOptions { MaxPlayers = _maxPlayer }, null);
    }
    public override void OnJoinedRoom()
    {
        _connetInfoText.text = "Joined room";
        PhotonNetwork.IsMessageQueueRunning = false; // ��� �Ͻ�����, �÷��̾� ���� �� �ٽ� ���� �����ش�
        PhotonNetwork.LoadLevel("Bajooka Sample Scene");
    }
   

    public override void OnDisconnected(DisconnectCause cause)
    {
        _joinButton.interactable = true;
        _connetInfoText.text = "Offline : Disconnected to Master Server \n Press connect button to reconnect";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        _connetInfoText.text = "No room, create new room";
        PhotonNetwork.CreateRoom(_roomNameInput.text, new RoomOptions { MaxPlayers = 4 });
    }

    private void ChangePanel(GameObject panel)
    {
        _curPanel.SetActive(false);
        panel.SetActive(true);
        _curPanel = panel;
    }
}
