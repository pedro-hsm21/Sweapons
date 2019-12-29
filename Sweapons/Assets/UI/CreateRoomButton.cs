using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomButton : MonoBehaviourPunCallbacks
{
    [SerializeField] Text _roomName;

    public void OnClick_CreateRoom ()
    {
        if (!PhotonNetwork.IsConnected || _roomName.text == "") return;

        RoomOptions _roomOptions = new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true };

        PhotonNetwork.JoinOrCreateRoom(_roomName.text, _roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        print("Room " + _roomName.text + " was created");
        RoomCanvasManager.Instance.EnterRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Room creation failed: " + message);
    }
}
