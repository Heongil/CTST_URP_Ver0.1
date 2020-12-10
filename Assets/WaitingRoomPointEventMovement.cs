using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomPointEventMovement : MonoBehaviour
{
    public GameObject turnGuide;
    public SightCtrl sightCtrl;
    public void OnSetTurnGuide(Transform target)
    {
        turnGuide.transform.parent = target;
        turnGuide.transform.localPosition = Vector3.zero;
        turnGuide.transform.localRotation = Quaternion.identity;
        sightCtrl._transform = NetworkRoomManagerExt.singleton.roomPlayer.camera.transform;
        turnGuide.SetActive(true);
    }
}
