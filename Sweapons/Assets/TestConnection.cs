using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConnection : MonoBehaviourPunCallbacks
{
    void Start()
    {
        print("Connecting to server....");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = MasterManager.Instance.ActiveGameSettings.GameVersion;
        PhotonNetwork.NickName = MasterManager.Instance.ActiveGameSettings.NickName;
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        print(PhotonNetwork.LocalPlayer.NickName + " Connected to server");

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected: " + cause.ToString());
    }
}
