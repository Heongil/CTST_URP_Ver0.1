using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainEventMiddle : NetworkBehaviour
{

    public MissionState missionState;
    public Transform[] PlayerMovePosition;

    ProjectManager projectManager;

    public Dictionary<string, Transform> DicPositions = new Dictionary<string, Transform>();

    public GuideMovementEvent guideMovement;
    public UnityEvent[] callBack;
    public UnityEvent[] OnFinishMoveCallBack;
    public UnityEvent[] OnFinishRotateCallBack;

    public bool isOnStart=false;

    public GameObject turnGuide;
    public SightCtrl sightCtrl;
    public void Init()
    {
        projectManager = NetworkRoomManagerExt.singleton.GetComponent<ProjectManager>();
        for (int i = 0; i < projectManager.deviceNames.Length; i++)
        {
            PlayerMovePosition[i].name = projectManager.deviceNames[i];
            PlayerMovePosition[i].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", projectManager.playerIconColors[i]);
            DicPositions.Add(projectManager.deviceNames[i], PlayerMovePosition[i]);
        }
        gameObject.SetActive(isOnStart);
    }
    public void SetUpNameAndTag(string deviceName)
    {
        DicPositions[deviceName].GetComponent<PlayerMovePositionEventObj>().targetTag = "Player";
        DicPositions[deviceName].gameObject.SetActive(true);
    }

    public void OnSetTurnGuide(Transform target)
    {
        turnGuide.transform.parent = target;
        turnGuide.transform.localPosition = Vector3.zero;
        turnGuide.transform.localRotation = Quaternion.identity;
        sightCtrl._transform = MainGameProjectManager.GetInstance().localPlayer.GetComponent<MainPlayer>().camera.transform;
        turnGuide.SetActive(true);
    }


    public void IsInAllPlayerArrive()
    {
       
    }

    public UnityEvent OnArriveEvent;
}
