using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPun, IPunObservable
{
    public int value;
    NetworkManager NM;
    PhotonView PV;



    void Start()
    {
        PV = photonView;
        NM = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
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
        if (!PV.IsMine) return;

        // 플레이어 움직임
    }
}
