using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSceneManager : NetworkBehaviour
{
    //씬의 모들 플레이어를 담는 리스트
    public List<NetworkRoomPlayerExt> roomPlayers = new List<NetworkRoomPlayerExt>();
    
    //비디오 오브젝트 : 플레이어들이 보게될 튜토리얼 영상
    public GameObject videoObj;

    //Onlyserver에서 시작될 때 켜지는 오브젝트들
    public GameObject[] OnlyServerOnObjects;

    //PlayerIconPrefeb
    public GameObject playerIcon;
    //PlayerIconRoot
    public GameObject playerIconRoot;
    //PlayerSetPositionAreas
    public GameObject[] PlayerContentStartPositions;
    //PlayerIcons
    public GameObject[] playerIcons;
    //playerColorDic
    public Dictionary<string, Color> dicPlayerColors = new Dictionary<string, Color>();
    //playerStartPosionObjsDic
    public Dictionary<string, GameObject> dicPlayerStartPosionObjs = new Dictionary<string, GameObject>();
    //PlayerIconsList
    public Dictionary<string, GameObject> dicPlayerIcon = new Dictionary<string, GameObject>();
    //PlayerIconsList
    public Dictionary<string, SetPlayerID> dicPlayerIDs = new Dictionary<string, SetPlayerID>();
    //네트워트 매니저
    private NetworkRoomManagerExt networkRoomManagerExt;
    //프로젝트 매니저
    private ProjectManager projectManager;
    //시작 버튼 UI
    public GameObject startGameButton;
    //시작 비디오 버튼
    public GameObject startVideoButton;
    //가이드 Particle
    public GameObject guideFlowObj;
    public Transform giudeTargetFrom;
    public Transform giudeTargetTo;
    //SetPlayerID
    public GameObject setPlayerIDobjRoot;
    public SetPlayerID[] setPlayerIDobjs;
   

    void Awake()
    {
        NetworkRoomManagerExt.singleton.sceneManager = this;
        networkRoomManagerExt = NetworkRoomManagerExt.singleton;
        projectManager = networkRoomManagerExt.GetComponent<ProjectManager>();
        SetPlayerDic();
        QualitySettings.antiAliasing = 2;
        Debug.Log("The current MSAA level is " + QualitySettings.antiAliasing);
    }

    public void SetPlayerDic()
    {
        //등록된 디바이스와 플레이어 컬러가 다르다면
        if(projectManager.deviceNames.Length != projectManager.playerIconColors.Length)
        {
            Debug.LogError("등록된 디바이스와 컬러가 숫자가 같아야합니다."+ projectManager.deviceNames +"/"+ projectManager.playerIconColors);
        }
        //초기화
        dicPlayerColors.Clear();

        //초기 세팅
        for (int i = 0; i < projectManager.deviceNames.Length; i++)
        {
            dicPlayerColors.Add(projectManager.deviceNames[i], projectManager.playerIconColors[i]);
            dicPlayerStartPosionObjs.Add(projectManager.deviceNames[i], PlayerContentStartPositions[i]);
            setPlayerIDobjs[i].ID = projectManager.deviceNames[i];
            setPlayerIDobjs[i].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", projectManager.playerIconColors[i]);
            dicPlayerIDs.Add(setPlayerIDobjs[i].ID, setPlayerIDobjs[i]);
            playerIcons[i].GetComponent<Image>().color = projectManager.playerIconColors[i];
            dicPlayerIcon.Add(projectManager.deviceNames[i], playerIcons[i]);
        }
    }



    private void Start()
    {
        if(isServerOnly)
        {
            for (int i = 0; i < OnlyServerOnObjects.Length; i++)
            {
                OnlyServerOnObjects[i].SetActive(true);
            }
        }

        StartCoroutine(CheckPlayerID_Obj());
    }


    IEnumerator CheckPlayerID_Obj()
    {
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < roomPlayers.Count; i++)
        {

          if(roomPlayers[i].deviceName!="")dicPlayerIDs[roomPlayers[i].deviceName].isFinish = true;
        }

        foreach (var element in dicPlayerIDs)
        {
            if (!element.Value.isFinish) element.Value.gameObject.SetActive(true);
        }
    }
  

    //모든 플레이어가 시작될때 실행하는 함수, 룸플레이어를 보관한다.
    public void SetPlayer(NetworkRoomPlayerExt player)
    {
        roomPlayers.Add(player);
        projectManager.playerCount = roomPlayers.Count;
    }
    public void SetPlayerDeviceName()
    {
      
    }

    public void PlayerStartPosisionObj()
    {
        if (isServerOnly) return;
        networkRoomManagerExt.roomPlayer.CmdOnPlayerStartPosisionObj();
    }

    //영상이 끝났을때 실행될 준비 함수
    public void OnPlayerStartPosisionObjServer(string deviceName)
    {
        Debug.Log("OnPlayerStartPosisionObjServer");
        //서버에서 실행
        SetPlayerStartPositionOnject(deviceName);
        //클라이언트 실행
        RpcOnPlayerStartPosisionObj(deviceName);
    }
    
  

    [ClientRpc]
    public void RpcOnPlayerStartPosisionObj(string deviceName)
    {
        Debug.Log("RpcOnPlayerStartPosisionObj");
        SetPlayerStartPositionOnject(deviceName);
    }
    void SetPlayerStartPositionOnject(string deviceName)
    {
        Debug.Log("SetPlayerStartPositionOnject");
        GameObject obj = dicPlayerStartPosionObjs[deviceName];
        obj.SetActive(true);
        obj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", dicPlayerColors[deviceName]);
    }

 

    public void PlayerReady(bool isReady)
    {
        Debug.Log("PlayerReady");
        NetworkRoomManagerExt.singleton.roomPlayer.CmdChangeReadyState(isReady);

    }
    public void SetOnGuideFlow()
    {
        if (isServerOnly) return;
        giudeTargetTo.parent = dicPlayerStartPosionObjs[networkRoomManagerExt.roomPlayer.deviceName].transform;
        giudeTargetTo.localPosition = new Vector3(0, 5f, 0);
        giudeTargetTo.localRotation = Quaternion.identity;

        giudeTargetFrom.parent = networkRoomManagerExt.roomPlayer.body.transform;
        giudeTargetFrom.localPosition =new Vector3(0,0.5f,0);
        giudeTargetFrom.localRotation = Quaternion.identity;
        guideFlowObj.GetComponent<guideParticleMovement>().TurnOnParticle();
    }
    public void OnOffFlow(bool isOn)
    {
        if(isOn)
        {
            guideFlowObj.GetComponentInChildren<ParticleSystem>().Play();
        }
        else
        {
            guideFlowObj.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }
    public void OnSetPlayerName(NetworkRoomPlayerExt localPlayer)
    {
        dicPlayerStartPosionObjs[localPlayer.deviceName].GetComponent<PlayerMovePositionEventObj>().enabled = true;
        dicPlayerStartPosionObjs[localPlayer.deviceName].GetComponent<PlayerMovePositionEventObj>().targetTag = localPlayer.body.tag;
    }

    public void UpdatePlayerReadyState()
    {
        bool IsReadyState = true;
        for (int i = 0; i < roomPlayers.Count; i++)
        {
            if(dicPlayerIcon.ContainsKey(roomPlayers[i].deviceName))
            {
                dicPlayerIcon[roomPlayers[i].deviceName].GetComponentInChildren<Text>().text = roomPlayers[i].readyToBegin == true ? "Ready" : "Not\nReady";
                if(!roomPlayers[i].readyToBegin && IsReadyState)
                {
                    IsReadyState = false;
                }
            }
        }

        if (IsReadyState && roomPlayers.Count>0)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }


    [ClientRpc]
    public void RpcOffPlayerIDobj(string key)
    {
        dicPlayerIDs[key].gameObject.SetActive(false);
    }
    public void OffPlayerIDobj(string key)
    {
        dicPlayerIDs[key].gameObject.SetActive(false);
        RpcOffPlayerIDobj(key);
    }

    //서버에서 플에이어들의 접속 상태 등 온,오프라인 확인이 끝난 후 버튼(UI)이벤트로 실행될 함수
    [ClientRpc]
    public void RpcActiveVideo()
    {
        videoObj.SetActive(true);
    }

    public GameObject StartObj;
    [ClientRpc]
    public void RpcActiveNPC()
    {
        StartObj.SetActive(true);
    }

    //Player생성시 Icon생성
    public void UpdatePlayerIcon(string key,bool isOn)
    {
        dicPlayerIcon[key].SetActive(isOn);
        int onCount = 0;
        if (isOn)
        {
           
            foreach (var element in dicPlayerIcon)
            {
              if(element.Value.activeSelf)onCount++;
            }
            if (roomPlayers.Count == onCount)
            {
                startVideoButton.SetActive(isOn);
                setPlayerIDobjRoot.SetActive(false);
            }
        }
        else
        {
            foreach (var element in dicPlayerIcon)
            {
                if (!element.Value.activeSelf) onCount++;
            }
            if(roomPlayers.Count != onCount)
            {
                startVideoButton.SetActive(isOn);
            }
        }
       
       //int listcount = dicPlayerIcon.Count;
       //
       //foreach (var element in dicPlayerIcon)
       //{  
       //    //다끄기
       //    element.Value.SetActive(false);
       //}
       //
       //for (int i = 0; i < roomPlayers.Count; i++)
       //{
       //   
       //    //생성되어 있던 플레이어 아이콘이 현재 플레이어 숫자보다 많거거 같을 때 기존의 아이콘의 색상을 업데이트 한다.
       //    if (i < listcount)
       //    {
       //    
       //        dicPlayerIcon[roomPlayers[i].deviceName].GetComponent<Image>().color = dicPlayerColors[roomPlayers[i].deviceName];
       //    }
       //    //생성되어 있던 플레이어 아이콘이 현재 플레이어 숫자보다 적을 때 새로 생성하여 색상을 추가한다.
       //    else
       //    {
       //        if(roomPlayers[i].deviceName!="")
       //        {
       //            Debug.Log(roomPlayers[i].deviceName);
       //            GameObject playerIconObj = Instantiate(playerIcon);
       //            playerIconObj.transform.parent = playerIconRoot.transform;
       //            playerIconObj.GetComponent<Image>().color = dicPlayerColors[roomPlayers[i].deviceName];
       //            dicPlayerIcon.Add(roomPlayers[i].deviceName, playerIconObj);
       //        }
       //
       //    }
       //}

    }
    //리무브 플레이어
    public void RemovePlayer(NetworkRoomPlayerExt player)
    {
        if(!isServerOnly)
        {
            return;
        }
        roomPlayers.Remove(player);
        UpdatePlayerIcon(player.deviceName,false);
    }
   

    //콘텐츠 시작 함수
    public void StartContents()
    {
        networkRoomManagerExt.StartGame();
    }

    public void UpdatePlayerModelColor()
    {
        
    }

    public void testva()
    {
        networkRoomManagerExt.GetComponent<HapticMovement>().PlayHaptic(0);
    }
}
