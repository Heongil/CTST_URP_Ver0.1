using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBoxToHands : MonoBehaviour
{
    public GameObject leftHand, rightHand;
    public GameObject boxPrefab;


    void Awake()
    {
        leftHand = NetworkRoomManagerExt.singleton.roomPlayer.hands[0].gameObject;
        rightHand = NetworkRoomManagerExt.singleton.roomPlayer.hands[1].gameObject;
        if (boxPrefab != null)
        {
            if (leftHand != null)
            {
                Instantiate(boxPrefab, leftHand.transform);
            }

            if (rightHand != null)
            {
                Instantiate(boxPrefab, rightHand.transform);
            }
        }
    }
}
