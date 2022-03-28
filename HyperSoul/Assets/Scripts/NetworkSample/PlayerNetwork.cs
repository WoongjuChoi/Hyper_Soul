using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNetwork : MonoBehaviourPun, IPunObservable
{
    // 테스트용 값
    public int value;

    [SerializeField]
    NetworkManager _networkNetwok;

    private PhotonView _photonView;

    void Start()
    {
        _photonView = photonView;
        _networkNetwok = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    [PunRPC]
    void TestRPC(string value1, string value2, PhotonMessageInfo info)
    {
        print(info.Sender.NickName + ", " + info.photonView.Owner.NickName + ", " + info.SentServerTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(value);
        }
        else
        {
            value = (int)stream.ReceiveNext();
        }
    }

    void Update()
    {
        if (!_photonView.IsMine) return;

        // 플레이어 움직임
    }
}
