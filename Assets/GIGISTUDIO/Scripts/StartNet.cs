using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNet : MonoBehaviour
{
    public bool isServer;
    // Start is called before the first frame update
    public NetworkRoomManagerExt network;
    void Start()
    {
        if(!isServer) network.StartClient();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
