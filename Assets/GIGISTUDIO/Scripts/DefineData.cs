

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum MissionState
{
    NONE,
    StopLine,
    CrosswalkGreenLight,
    CrosswalkCrossed,
    KickboardStart,
    KickboardMiddle,
    KickboardEnd,
    trainingStep1Start,
    trainingStep1End,
    trainingStep2Start,
    trainingStep2End,
    trainingStep3Start,
    trainingStep3End,
    FINISH
}

public enum NPCRedyRoomState
{
    NONE,
    stet1,
    CrosswalkGreenLight,
    CrosswalkCrossed,
    KickboardStart,
    KickboardEnd,
    trainingStep1Start,
    trainingStep1End,
    trainingStep2Start,
    trainingStep2End,
    trainingStep3Start,
    trainingStep3End,
    FINISH
}

public enum PointState
{
    None,
    Crosswalk,
    StationaryVehicle,
    Kickboard,
    EmergencyVehicle
}

public struct MissionInfo_ST
{
    public MissionState state;
    public int point;

}
public class C_TracfficLight
{
    public Image image;
    public bool isFinish;
}

[Serializable]
public struct SubArray
{
    public string tip;
    [SerializeField]
    public MissionState state;
    [SerializeField]
    public SubElement[] explainArrayStep;

}

[Serializable]
public struct SubArrayReadyRoom
{
    public string tip;
    [SerializeField]
    public NPCRedyRoomState state;
    [SerializeField]
    public SubElement[] explainArrayStep;

}

[Serializable]
public struct SubElement
{
    public string tip;
    public float delayTime;
    public string[] explains;
    public UnityEvent onStartEvent;
    public UnityEvent onFinishEvent;
}

