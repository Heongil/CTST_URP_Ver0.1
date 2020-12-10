using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointingToHand : MonoBehaviour {

    public GameObject leftHand, rightHand;

    public BhapticsOculusPointing pointingHandPrefab;

    public Material laserMaterial;


    void Awake()
    {
        leftHand = NetworkRoomManagerExt.singleton.roomPlayer.hands[0].gameObject;
        rightHand = NetworkRoomManagerExt.singleton.roomPlayer.hands[1].gameObject;
        if (leftHand != null)
        {
            var obj = Instantiate(pointingHandPrefab, leftHand.transform);
            var pointingLeft = obj.GetComponent<BhapticsOculusPointing>();
            pointingLeft.laserMaterial = laserMaterial;
            pointingLeft.button = OVRInput.Button.PrimaryIndexTrigger;
        }

        if (rightHand != null)
        {
            var obj = Instantiate(pointingHandPrefab, rightHand.transform);
            var pointingRight = obj.GetComponent<BhapticsOculusPointing>();
            pointingRight.laserMaterial = laserMaterial;
            pointingRight.button = OVRInput.Button.SecondaryIndexTrigger;
        }
    }
}
