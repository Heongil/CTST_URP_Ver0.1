using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.NetworkRoom;

public class MissionInfo : NetworkBehaviour
{

    [SerializeField]
    [SyncVar]
    public int score=100;

    [SyncVar]
    public string playerName;
    [SyncVar]
    public bool IsPlayingVideoMission;


    public Transform target;
    [SyncVar]
    public string deviceName="";
    [SyncVar]
    public Color deviceColor = Color.clear;
    [SyncVar]
    public bool IsPlayReadyForGame;
    // Start is called before the first frame update
    void Start()
    {
        if(isLocalPlayer)
        {
            target.tag = "Player";
            MainGameProjectManager.GetInstance().localPlayer = this;
            string playerName = NetworkRoomManagerExt.singleton.GetComponent<ProjectManager>().localDeviceName;
            CmdSetPlayerDeviceName(playerName);
            SetPlayerMovePosition(playerName);
            CmdSetPlayerMovePosition(playerName);
            CmdSetPlayerColor(NetworkRoomManagerExt.singleton.GetComponent<ProjectManager>().localDeviceColor);
            Debug.Log(isLocalPlayer);
        }
        else
        {
            target.tag = "NetworkPlayer";
        }
        MainGameProjectManager.GetInstance().AddPlayers(this.gameObject);
        Debug.Log("MainGameProjectManager.GetInstance().players.Add");
    }

    [Command]
    public void CmdSetPlayerDeviceName(string _deviceName)
    {
        deviceName = _deviceName;
        MainGameProjectManager.GetInstance().playersOnlyOnServer.Add(deviceName,this);
    }
    [Command]
    public void CmdSetPlayerMovePosition(string newname)
    {
        Debug.Log("SetPlayerMovePosition" + newname);
        MainGameProjectManager.GetInstance().SetPlayerMovePosition(newname);

    }
    public void SetPlayerMovePosition(string newname)
    {
        Debug.Log("SetPlayerMovePosition" + newname);
        MainGameProjectManager.GetInstance().SetPlayerMovePosition(newname);

    }

    public GameObject BandObj;
    [Command]
    public void CmdSetPlayerColor(Color color)
    {
        deviceColor = color;
        Debug.Log(deviceColor);
        MainPlayer mainplay = GetComponent<MainPlayer>();
        for (int i = 0; i < mainplay.models.Length; i++)
        {
            mainplay.models[i].GetComponent<SkinnedMeshRenderer>().materials[1].SetColor("_BaseColor", deviceColor);
        }

        BandObj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", deviceColor);
    }
   
    public void SetPlayerColor()
    {
        MainPlayer mainplay = GetComponent<MainPlayer>();
        for (int i = 0; i < mainplay.models.Length; i++)
        {
            mainplay.models[i].GetComponent<SkinnedMeshRenderer>().materials[1].SetColor("_BaseColor", deviceColor);
        }
        BandObj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", deviceColor);
    }
  

    [Command]
    public void CmdSetPlayer(GameObject obj)
    {
        MainGameProjectManager.GetInstance().AddPlayers(obj);
    }
    
    public void AddScore(int addSocre)
    {
        CmdAddScore(addSocre);
    }

    
    [Command]
    public void CmdAddScore(int addSocre)
    {
        AddScoreOnServer(addSocre);
    }
    public void AddScoreOnServer(int addSocre)
    {
        score += addSocre;
        MainGameProjectManager.GetInstance().UpdateUI();
        Debug.Log(deviceColor + "/"+score);
    }

    [Command]
    public void CmdReSetFinishPlayerVideo(bool isMission)
    {
        IsPlayingVideoMission = isMission;
    }

    [Command]
    public void CmdSetReadyForGame(bool isReady)
    {
        IsPlayReadyForGame = isReady;
      if(IsPlayReadyForGame)MainGameProjectManager.GetInstance().CheckAllInPlayerPosition();
    }
}

  
