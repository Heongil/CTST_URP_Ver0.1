using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComebackNPC : MonoBehaviour 
{
    //가이드 Particle
    public GameObject guideFlowObj;

    public Transform from;

    public Transform giudeTargetFrom;
    public Transform giudeTargetTo;

    // Use this for initialization


    PlaySceneManager playSceneManager;

    private void Start()
    {
        playSceneManager = (PlaySceneManager)NetworkRoomManagerExt.singleton.sceneManager;

    }

    public void SetOnGuideFlow()
    {
        guideFlowObj.SetActive(true);
        giudeTargetTo.parent = playSceneManager.localPlayer.transform.GetComponent<MainPlayer>().body.transform;
        giudeTargetTo.localPosition = new Vector3(0, 5f, 0);
        giudeTargetTo.localRotation = Quaternion.identity;

        giudeTargetFrom.parent = from;
        giudeTargetFrom.localPosition = new Vector3(0, 0.5f, 0);
        giudeTargetFrom.localRotation = Quaternion.identity;
    }

    public void OffGuideFlow()
    {
        guideFlowObj.SetActive(false);
    }
}
