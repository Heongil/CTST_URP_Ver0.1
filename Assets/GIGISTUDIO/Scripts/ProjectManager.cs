using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectManager : NetworkBehaviour
{

    public bool IsServer;
    public NetworkRoomManagerExt networkmanager;

    public string localDeviceName;
    public Color localDeviceColor;
    //DeviceName
    public string[] deviceNames;
    //PlayerIconColors
    public Color[] playerIconColors;

    public int playerCount;


    private void Start()
    {
        if (IsServer) networkmanager.StartServer();
        else ConnectToServer();
    }

    public void ConnectToServer()
    {
        Debug.Log("connectionToServer");
        if (!IsServer) networkmanager.StartClient();
    }

   
}
