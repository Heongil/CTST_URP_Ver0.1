using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using Mirror.Examples.Additive;
using System;
using UnityEngine.UI;
using TMPro;
using Mirror.Examples.NetworkRoom;

[Serializable]
public class MyEvent : UnityEvent { }


public class MainGameProjectManager : NetworkBehaviour
{

    private static MainGameProjectManager instance;
    public static MainGameProjectManager GetInstance()
    {
        if (!instance)
        {
            instance = (MainGameProjectManager)GameObject.FindObjectOfType(typeof(MainGameProjectManager));
            if (!instance)
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
        }

        return instance;
    }


    [Header("===================================================")]
    [Header("stop")]
    [Tooltip("stp")]
    public MyEvent[] StopLine;

    [Header("횡단보도 시작 이벤트")]
    [Tooltip("횡단보도 이벤트를 시작할 때 실행 될 이벤트를 추가한다.")]
    public MyEvent[] CrosswalkGreenLight_CallBack;
    [Header("횡단보도 끝 이벤트")]
    [Tooltip("횡단보도 이벤트가 끝날 때 실행 될 이벤트를 추가한다.")]
    public MyEvent[] CrosswalkCrossed_CallBack;


    [Header("킥보드 시작 이벤트")]
    [Tooltip("킥보드 이벤트가 끝날 때 실행 될 이벤트를 추가한다.")]
    [Space(20)]
    public MyEvent[] kickboardStart;
    [Header("킥보드 중간 안전구역 이벤트")]
    [Tooltip("킥보드 이벤트 중간 안전구역 이벤트.")]
    [Space(20)]
    public MyEvent[] kickboardMiddle;
    [Header("킥보드 끝 이벤트")]
    [Tooltip("킥보드 이벤트가 끝날 때 실행 될 이벤트를 추가한다. 구급차 이벤트 시작한다.")]
    public MyEvent[] kickboardFinish;


    [Header("구급차 이벤트 끝 훈련1 이벤트 시작")]
    [Tooltip("훈련")]
    [Space(20)]
    public MyEvent[] trainingStep1StartEvent;
    [Header("훈련1 이벤트 끝")]
    [Tooltip("훈련1 이벤트가 끝날 때 실행 될 이벤트.")]
    public MyEvent[] trainingStep1FinishEvent;
    [Header("훈련1 이벤트 끝 훈련2 이벤트 시작")]
    [Tooltip("훈련")]
    [Space(20)]
    public MyEvent[] trainingStep2StartEvent;
    [Header("훈련2 이벤트 끝")]
    [Tooltip("훈련2 이벤트가 끝날 때 실행 될 이벤트.")]
    public MyEvent[] trainingStep2FinishEvent;
    [Header("훈련2 이벤트 끝 훈련3 이벤트 시작")]
    [Tooltip("훈련")]
    [Space(20)]
    public MyEvent[] trainingStep3StartEvent;
    [Header("훈련3 이벤트 끝")]
    [Tooltip("훈련3 이벤트가 끝날 때 실행 될 이벤트.")]
    public MyEvent[] trainingStep3FinishEvent;
    [Header("===================================================")]
    [Space(20)]
    [Header("===================================================")]
    [Header("stop")]
    [Tooltip("stp")]
    public MyEvent[] Server_StopLine;
    [Header("stop")]
    [Tooltip("stp")]
    public MyEvent[] Server_CrosswalkCrossed_CallBack;
    [Header("stop")]
    [Tooltip("stp")]
    public MyEvent[] Server_kickboardStart_CallBack;
    [Header("stop")]
    [Tooltip("stp")]
    public MyEvent[] Server_kickboardEnd_CallBack;


    [Header("훈련1 이벤트 끝")]
    [Tooltip("훈련1 이벤트가 끝날 때 실행 될 이벤트.")]
    public MyEvent[] Server_trainingStep1FinishEvent;
    [Header("훈련1 이벤트 끝 훈련2 이벤트 시작")]
    [Tooltip("훈련")]
    [Space(20)]
    public MyEvent[] Server_trainingStep2StartEvent;
    [Header("훈련2 이벤트 끝")]
    [Tooltip("훈련2 이벤트가 끝날 때 실행 될 이벤트.")]
    public MyEvent[] Server_trainingStep2FinishEvent;



    [Header("훈련2 이벤트 끝")]
    [Tooltip("훈련2 이벤트가 끝날 때 실행 될 이벤트.")]
    public MyEvent[] Server_trainingStep2FinishEvent_CallBack;
    [Header("===================================================")]
    [Space(20)]
    [Header("===================================================")]
    public MissionInfo localPlayer;

    [SerializeField]
    [SyncVar(hook = nameof(SetMission))]
    public MissionState mission;


    public Dictionary<GameObject, MissionInfo> players = new Dictionary<GameObject, MissionInfo>();
    public Dictionary<string, MissionInfo> playersOnlyOnServer = new Dictionary<string, MissionInfo>();
    int index = 0;

    public MainEventMiddle[] mainEventMiddles;
    public Dictionary<MissionState, MainEventMiddle> dicMainEventMiddle = new Dictionary<MissionState, MainEventMiddle>();


    [Header("ScoreUI")]
    public GameObject UIBoard;
    public GameObject scoreUI;

    public PlaySceneManager playSceneManager;

    //네트워트 매니저
    private NetworkRoomManagerExt networkRoomManagerExt;
    //프로젝트 매니저
    private ProjectManager projectManager;
    private HapticMovement hapticMovement;
    private void Start()
    {
        InitSet();
    }

    void InitSet()
    {
        networkRoomManagerExt = NetworkRoomManagerExt.singleton;
        projectManager = networkRoomManagerExt.GetComponent<ProjectManager>();
        hapticMovement = projectManager.GetComponent<HapticMovement>();
        for (int i = 0; i < mainEventMiddles.Length; i++)
        {
            mainEventMiddles[i].Init();
            dicMainEventMiddle.Add(mainEventMiddles[i].missionState, mainEventMiddles[i]);
        }
        StartCoroutine(ICheckPlayerSettingToStart());
        
    }

    [ClientRpc]
    public void RpcOnArriveEvent()
    {
        Debug.Log(mission);
        Debug.Log(mission + 1);
        dicMainEventMiddle[mission+1].OnArriveEvent.Invoke();
    }

   

    public void SetPlayerScore()
    {
        StartCoroutine(SetPlayerScoreWait());
    }

    IEnumerator SetPlayerScoreWait()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var element in players)
        {
            RpcSetScoreUI(element.Value.playerName, element.Value.score);
            SetScoreUI(element.Value.playerName, element.Value.score);
        }
    }

    [ClientRpc]
    public void RpcSetScoreUI(string name,int score)
    {
        SetScoreUI(name, score);
    }
    public void SetScoreUI(string name, int score)
    {
        GameObject Tempobj = Instantiate(scoreUI, UIBoard.transform);
        Tempobj.GetComponent<NetPlayerScore>().SetPlayerScore(name, "" + score);
        UIBoard.gameObject.SetActive(true);
    }

    public void AddPlayers(GameObject obj)
    {
        MissionInfo info = obj.GetComponent<MissionInfo>();
        players.Add(obj, info);
        index++;
    }

    public TextMeshProUGUI ShowingInfo;
  
    

    public void UpdateUI()
    {
       //ShowingInfo.text = "";
       //foreach (var element in players)
       //{
       //    ShowingInfo.text += element.Value.score + "/";
       //}
    }

    // Start is called before the first frame update

    private void Update()
    {
     // if(localPlayer != null && ShowingInfo !=null) ShowingInfo.text = ""+localPlayer.score;

      if(isServerOnly)
        {
            Debug.Log(players.Count);
        }
    }
    public void SetStateServerEvent()
    {
        switch (mission)
        {
            case MissionState.NONE:
                break;

            case MissionState.StopLine:

                break;

            case MissionState.CrosswalkGreenLight:

                break;
            case MissionState.CrosswalkCrossed:
                if (Server_CrosswalkCrossed_CallBack[0] != null) Server_CrosswalkCrossed_CallBack[0].Invoke();
                break;
            case MissionState.KickboardStart:
                if (Server_kickboardStart_CallBack[0] != null) Server_kickboardStart_CallBack[0].Invoke();
                
                break;
            case MissionState.KickboardEnd:
                if (Server_kickboardEnd_CallBack[0] != null) Server_kickboardEnd_CallBack[0].Invoke();
                break;

            case MissionState.trainingStep1Start:
              
                break;
            case MissionState.trainingStep1End:
                if (Server_trainingStep1FinishEvent[0] != null) Server_trainingStep1FinishEvent[0].Invoke();
                
                break;
            case MissionState.trainingStep2Start:
                if (Server_trainingStep2FinishEvent[0] != null) Server_trainingStep2FinishEvent[0].Invoke();
                break;
            case MissionState.trainingStep2End:
                if (Server_trainingStep2FinishEvent_CallBack[0] != null) Server_trainingStep2FinishEvent_CallBack[0].Invoke();
                
                break;
            case MissionState.trainingStep3Start:

                break;
            case MissionState.trainingStep3End:

                break;
        }
    }
    public void SetState(MissionState state)
    {
        if (!isServer) return;
       // mission = state;
        SetMission(mission, state);
        SetStateServerEvent();
    }

    public void SetMission(MissionState old,MissionState newstate)
    {
        mission = newstate;
        switch (mission)
        {
            case MissionState.NONE:
                break;
            case MissionState.StopLine:
                if (StopLine[0] != null) StopLine[0].Invoke();
                break;
            case MissionState.CrosswalkGreenLight:
                if (CrosswalkGreenLight_CallBack[0] != null) CrosswalkGreenLight_CallBack[0].Invoke();
                break;
            case MissionState.CrosswalkCrossed:
                if (CrosswalkCrossed_CallBack[0] != null) CrosswalkCrossed_CallBack[0].Invoke();
                break;
            case MissionState.KickboardStart:
               if (kickboardStart[0] != null) kickboardStart[0].Invoke();
                break;

            case MissionState.KickboardMiddle:
                if (kickboardMiddle[0] != null) kickboardMiddle[0].Invoke();
                break;
            case MissionState.KickboardEnd:
                if (kickboardFinish[0] != null) kickboardFinish[0].Invoke();
                break;
            case MissionState.trainingStep1Start:
                if (trainingStep1StartEvent[0] != null) trainingStep1StartEvent[0].Invoke();
                break;
            case MissionState.trainingStep1End:
                if (trainingStep1FinishEvent[0] != null) trainingStep1FinishEvent[0].Invoke();
                break;
            case MissionState.trainingStep2Start:
                if (trainingStep2StartEvent[0] != null) trainingStep2StartEvent[0].Invoke();
                break;
            case MissionState.trainingStep2End:
                if (trainingStep2FinishEvent[0] != null) trainingStep2FinishEvent[0].Invoke();
                break;
            case MissionState.trainingStep3Start:
                if (trainingStep3StartEvent[0] != null) trainingStep3StartEvent[0].Invoke();
                break;
            case MissionState.trainingStep3End:
                if (trainingStep3FinishEvent[0] != null) trainingStep3FinishEvent[0].Invoke();
                break;
        }
        Debug.Log(mission);
    }




    
    public void SetPlayerReady(bool isReady)
    {
        if (localPlayer == null) return;
        localPlayer.CmdSetReadyForGame(isReady);
    }
    public bool SetPlayerReadyOnServer(string key ,bool isReady)
    {
        playersOnlyOnServer[key].IsPlayReadyForGame = isReady;
        if (isReady)
        {
            return CheckAllInPlayerPosition();
        }
        return false;
    }

    public void PlusScoreAll()
    {
        foreach (var element in playersOnlyOnServer)
        {
            element.Value.AddScore(20);
        }
    }


    public void SetPlayerMovePosition(string deviceName)
    {
        Debug.Log("SetPlayerMovePosition");
        for (int i = 0; i < mainEventMiddles.Length; i++)
        {
            mainEventMiddles[i].SetUpNameAndTag(deviceName);
        }
    }

    public bool CheckAllInPlayerPosition()
    {
        if (players.Count != projectManager.playerCount) return false;

        foreach (var element in players)
        {
            if (element.Value.deviceName == "")
            {
                return false;
            }

            if (element.Value.deviceColor == Color.clear)
            {
                return false;
            }

            if (!element.Value.IsPlayReadyForGame)
            {
                return false;
            }
        }

            foreach (var element in players)
            {
                element.Value.IsPlayReadyForGame = false;
            }
        SetState(mission = (MissionState)mission + 1);
        return true;
    }


    private bool CheckPlayerSettingToStart()
    {
        if (players.Count != projectManager.playerCount) return false;

        foreach (var element in players)
        {
            if (element.Value.deviceName == "")
            {
                return false;
            }

            if (element.Value.deviceColor == Color.clear)
            {
                return false;
            }

        }
      
        return true;
    }
    IEnumerator ICheckPlayerSettingToStart()
    {
        while (true)
        {
            yield return null;
            if (CheckPlayerSettingToStart())
            {
                OnPlayrMoveposition();
                if(isServerOnly)
                {
                    foreach (var element in players)
                    {
                        element.Value.IsPlayReadyForGame = false;
                    }
                }
                break;
            }
        }
    }

  
    void OnPlayrMoveposition()
    {
        foreach (var elemnent in players)
        {
            dicMainEventMiddle[(MissionState)mission + 1].DicPositions[elemnent.Value.deviceName].gameObject.SetActive(true);
            Debug.Log(elemnent.Value.deviceName+"/" + elemnent.Value.deviceColor);
            elemnent.Value.SetPlayerColor();
        }
        if(!isServerOnly)
        {
            playSceneManager.SetOnGuideFlow();
            playSceneManager.OnOffFlow(true);
        }
    }

    public UnityEvent OnWatchAllEvent;
    [ClientRpc]
    public void RpcOnWatchAll()
    {
        OnWatchAllEvent.Invoke();
    }


    public void PlayerHeadVibration(int index)
    {
      //  hapticMovement.PlayHaptic(index);
    }
}
