using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 로그인
    [SerializeField]
    private GameObject _loginPanel;
    [SerializeField]
    private InputField _nickNameInput;
    [SerializeField]
    private Button _joinButton;

    // 로비
    [SerializeField]
    private GameObject _roomPanel;
    [SerializeField]
    private InputField _roomNameInput;
    [SerializeField]
    private Button _createRoomButton;
    [SerializeField]
    private Button _randomJoinRoomButton;
    [SerializeField]
    private GameObject _roomInfo;
    [SerializeField]
    private Transform _roomPos;
    [SerializeField]
    private Text _connetInfoText;

    private Dictionary<string, GameObject> _roomList = new Dictionary<string, GameObject>();

    private GameObject _curPanel;

    private byte _maxPlayer = 4;

    private const string GAME_VERSION = "0.01";

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.AutomaticallySyncScene = true;    // 마스터클라이언트와 씬 동기화
        PhotonNetwork.GameVersion = GAME_VERSION;

        _curPanel = _loginPanel;
        _joinButton.interactable = false;
        _createRoomButton.interactable = false;
        _randomJoinRoomButton.interactable = false;
        _connetInfoText.text = "데이터 로딩 중입니다. 잠시 기다려주세요";

        DataManager.Instance.DataReady -= DataIsReady;
        DataManager.Instance.DataReady += DataIsReady;
    }
    private void DataIsReady()
    {
        _joinButton.interactable = true;
        _connetInfoText.text = "";
    }
    private void Start()
    {
        // 입력 없을 시 랜덤 생성
        _nickNameInput.text = PlayerPrefs.GetString("USER_NICKNAME", "USER_" + Random.Range(1, 1000));
        _roomNameInput.text = PlayerPrefs.GetString("ROOM_NAME", "ROOM_" + Random.Range(1, 1000));
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = _nickNameInput.text;

        PhotonNetwork.JoinLobby();
        _connetInfoText.text = "로비 접속 중입니다.";
    }

    public override void OnJoinedLobby()
    {
        StartCoroutine(LobbyText());
    }

    private IEnumerator LobbyText()
    {
        _connetInfoText.text = "로비에 접속했습니다.";
        _createRoomButton.interactable = true;
        _randomJoinRoomButton.interactable = true;
        yield return new WaitForSeconds(1.0f);

        _connetInfoText.text = "";
    }
    public void Connect()
    {
        Debug.Assert(true == DataManager.Instance.IsDataReady);

        _joinButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
        _connetInfoText.text = "마스터 서버에 접속 중입니다.";

        if (PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.NickName = PhotonNetwork.LocalPlayer.NickName;

            ChangePanel(_roomPanel);
        }
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(_roomNameInput.text, new RoomOptions { MaxPlayers = _maxPlayer }, null);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"{PhotonNetwork.CurrentRoom.Name} Created Room");
    }

    public override void OnRoomListUpdate(List<Photon.Realtime.RoomInfo> roomList)
    {
        GameObject _newRoom = null;
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList == true)
            {
                _roomList.TryGetValue(room.Name, out _newRoom);
                Destroy(_newRoom);
                _roomList.Remove(room.Name);
            }
            else
            {
                if (_roomList.ContainsKey(room.Name) == false)
                {
                    _newRoom = Instantiate(_roomInfo, _roomPos);
                    Room _room = _newRoom.GetComponent<Room>();
                    _room.RoomName = room.Name;
                    _room.CurPlayer = room.PlayerCount;
                    _room.MaxPlayer = room.MaxPlayers;
                    _room.UpdateInfo();
                    _room.GetComponent<Button>().onClick.AddListener(delegate { OnClickRoom(_room.RoomName); });
                    _roomList.Add(_room.RoomName, _newRoom);
                }
                else
                {
                    _roomList.TryGetValue(room.Name, out _newRoom);
                    Room _room = _newRoom.GetComponent<Room>();
                    _room.RoomName = room.Name;
                    _room.CurPlayer = room.PlayerCount;
                    _room.MaxPlayer = room.MaxPlayers;
                    _room.UpdateInfo();
                }
            }
        }
    }

    public void OnClickRoom(string roomName)
    {
        PlayerPrefs.SetString("USER_NICKNAME", PhotonNetwork.NickName);
        PhotonNetwork.JoinRoom(roomName, null);
    }
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        _connetInfoText.text = "방에 입장하였습니다.";
        PhotonNetwork.IsMessageQueueRunning = false;        // 통신 일시정지, 플레이어 스폰 후 다시 연결 시켜준다

        if (true == PhotonNetwork.AutomaticallySyncScene)
        {
            PhotonNetwork.LoadLevel("RoomScene");
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        _joinButton.interactable = true;
        _connetInfoText.text = "Offline : Disconnected to Master Server \n Press connect button to reconnect";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        _connetInfoText.text = "목록에 방이 없습니다. 방을 생성합니다.";
        PhotonNetwork.CreateRoom(_roomNameInput.text, new RoomOptions { MaxPlayers = 4 });
    }

    private void ChangePanel(GameObject panel)
    {
        _curPanel.SetActive(false);
        panel.SetActive(true);
        _curPanel = panel;
    }
}