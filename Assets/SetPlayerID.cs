using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerID : EventObjCircle
{
    public string ID;
    public bool isFinish;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            NetworkRoomPlayerExt networkRoomPlayerExt = other.GetComponent<Body>().root.GetComponent<NetworkRoomPlayerExt>();
            if (networkRoomPlayerExt.deviceName == "" && networkRoomPlayerExt.presentSetIDobj=="")
            {
                Debug.Log("SetPlayerDeviceName");
                transform.root.gameObject.SetActive(false);
                networkRoomPlayerExt.presentSetIDobj = name;
                networkRoomPlayerExt.SetPlayerDeviceName(ID);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            NetworkRoomPlayerExt networkRoomPlayerExt = other.GetComponent<Body>().root.GetComponent<NetworkRoomPlayerExt>();
            if (networkRoomPlayerExt.presentSetIDobj == name)
            {
                Debug.Log("SetPlayerDeviceName");

                networkRoomPlayerExt.presentSetIDobj = "";
                networkRoomPlayerExt.SetPlayerDeviceName(ID);
            }
        }
    }
}
