using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class PlaySceneEventManager : NetworkBehaviour
{
    public WatchingTrafficLightRaycastBoard watchingTrafficLightRaycastBoard;
    public void OnFinishEventCreen()
    {
        if (!isServerOnly) return;
        Debug.Log(isServerOnly);
        Dictionary<string, C_TracfficLight> tracfficLights = watchingTrafficLightRaycastBoard.dicTrafficLight;
        Dictionary<GameObject, MissionInfo> players =  MainGameProjectManager.GetInstance().players;
        foreach (var element in players)
        {
            Debug.Log(element.Value.deviceName);
           if(!tracfficLights[element.Value.deviceName].isFinish)
            {
                element.Value.AddScoreOnServer(-10);
            }
        }
    }

    public float waitTime = 0;

    public UnityEvent[] waitEvents;

    public GameObject scoreUI;
    public void SetWaitTime(float time)
    {
        waitTime = time;
    }
    public void StartEventAfterWait(int index)
    {
        StartCoroutine(StartEventAfterWaitCourutine(waitTime, index));
    }

    IEnumerator StartEventAfterWaitCourutine(float time, int index)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("StartEventAfterWaitCourutine");
        waitEvents[index].Invoke();
    }


    public void AddSocre(int score)
    {
        MainGameProjectManager.GetInstance().localPlayer.AddScore(score);
    }




    public NetPlayerScore[] playerScores;


    public void SetPlayerScores()
    {
      

        StartCoroutine(cWaitTimeCallBack(0.1f,()=>
        {
            Dictionary<GameObject, MissionInfo> players = MainGameProjectManager.GetInstance().players;
            int index = 0;
            foreach (var element in players)
            {
                playerScores[index].SetPlayerScore(element.Value.deviceColor, element.Value.score);
                index++;
            }

            scoreUI.SetActive(true);

        }));
    }


    IEnumerator cWaitTimeCallBack(float time,UnityAction callBack)
    {
        yield return new WaitForSeconds(time);

        callBack();
    }


   
    
}
