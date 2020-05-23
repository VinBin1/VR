using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;



public class Networkmanager : MonoBehaviour
{
    const string Version = "v0.0.1";
    public string roomName = "myGame";
    public string playerprfab = "player";
    public string playerprfab2 = "player";
    public string playerprfab3 = "player";
    public Transform spawnpoint;
    public Transform spawnpoint2;
    public Transform spawnpoint3;
    private bool isEditor;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(Version);
        isEditor = Application.isEditor;

        if (!isEditor)
        { Screen.sleepTimeout = SleepTimeout.NeverSleep; }



    }

    void OnJoinedLobby()
    {
        RoomOptions RoomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom(roomName, RoomOptions, TypedLobby.Default);
        print("joinedlobbyplayer");
    }

    void OnJoinedRoom()
    {
        if (PhotonNetwork.playerList.Length == 2)
        {

            print("addingplayer");
            PhotonNetwork.Instantiate(playerprfab, spawnpoint.position, spawnpoint.rotation, 0);
        }
        if (PhotonNetwork.playerList.Length == 3)
        {
            //player2 add
            print("addingplayer");
            PhotonNetwork.Instantiate(playerprfab2, spawnpoint2.position, spawnpoint.rotation, 0);
        }
        if (PhotonNetwork.playerList.Length == 1)
        {
            //player2 add
            print("addingplayer");
            PhotonNetwork.Instantiate(playerprfab3, spawnpoint3.position, spawnpoint.rotation, 0);
        

        }



    }
}