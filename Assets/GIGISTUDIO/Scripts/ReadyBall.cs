using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyBall : MonoBehaviour
{
    public bool isServer;
    public bool isGame;


  
    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Hand"))
       {
            if(isGame)
            {
                NetworkRoomManagerExt.singleton.ServerChangeScene(NetworkRoomManagerExt.singleton.RoomScene);
            }
            else
            {
                if (isServer)
                {
                    NetworkRoomManagerExt.singleton.StartGame();
                }
                else
                {
                    NetworkRoomManagerExt.singleton.roomPlayer.ReadyTEST();
                }
            }
            

            gameObject.SetActive(false);
       }
    }
}
