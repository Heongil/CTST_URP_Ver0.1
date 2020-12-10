using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WatchingTrafficLightRaycastBoard : MonoBehaviour
{
 

    public Dictionary<string, C_TracfficLight> dicTrafficLight = new Dictionary<string, C_TracfficLight>();
   

    ProjectManager projectManager;
    public Image[] Images;

    public float fullTime;
    public bool IsFinishEvent;

    private void Start()
    {
        projectManager = NetworkRoomManagerExt.singleton.GetComponent<ProjectManager>();
        for (int i = 0; i < projectManager.deviceNames.Length; i++)
        {
            C_TracfficLight TracfficLight = new C_TracfficLight();
            TracfficLight.image = Images[i];
            TracfficLight.image.fillAmount = 0;
            TracfficLight.image.color = projectManager.playerIconColors[i];
            TracfficLight.isFinish = false;
            dicTrafficLight.Add(projectManager.deviceNames[i], TracfficLight);
        }
        IsFinishEvent = false;
    }


    public void UpdateGage(string targetName)
    {
        Debug.Log("UpdateGage");
        if(dicTrafficLight[targetName].image.fillAmount>=1f)
        {
            if (dicTrafficLight[targetName].isFinish) return;
            if (!MainGameProjectManager.GetInstance().isServerOnly) return;
            //끝 이벤트 넣을 자리(임시로 text)
            dicTrafficLight[targetName].image.GetComponentInChildren<Text>().text = "끝";
            if (MainGameProjectManager.GetInstance().isServerOnly)
            {
                Debug.Log(targetName +"true");
              if(MainGameProjectManager.GetInstance().SetPlayerReadyOnServer(targetName,true))
                {
                    //Finish Event
                    gameObject.SetActive(false);
                }
                Debug.Log(targetName+"/"+MainGameProjectManager.GetInstance().playersOnlyOnServer[targetName].IsPlayReadyForGame);
            }
            dicTrafficLight[targetName].isFinish = true;
            dicTrafficLight[targetName].image.fillAmount = 1f;
            IsFinishEvent = true;
            return;
        }
        dicTrafficLight[targetName].image.fillAmount += Time.deltaTime / fullTime;
    }


    public void UpdateGageTraining(string targetName)
    {
        Debug.Log("UpdateGageTraining");
        if (dicTrafficLight[targetName].image.fillAmount >= 1f)
        {
            if (dicTrafficLight[targetName].isFinish) return;
            if (!MainGameProjectManager.GetInstance().isServerOnly) return;



            MainGameProjectManager.GetInstance().playersOnlyOnServer[targetName].AddScoreOnServer(10);
            //끝 이벤트 넣을 자리(임시로 text)
            dicTrafficLight[targetName].image.GetComponentInChildren<Text>().text = "끝";  
            dicTrafficLight[targetName].isFinish = true;
            dicTrafficLight[targetName].image.fillAmount = 1f;
            IsFinishEvent = true;


            Dictionary<string, MissionInfo> players = MainGameProjectManager.GetInstance().playersOnlyOnServer;

            foreach (var element in players)
            {
               if(!dicTrafficLight[element.Key].isFinish) return;
            }

            MainGameProjectManager.GetInstance().OnWatchAllEvent.Invoke();
            MainGameProjectManager.GetInstance().RpcOnWatchAll();

            return;
        }
        dicTrafficLight[targetName].image.fillAmount += Time.deltaTime / fullTime;
    }


    public WatchingTrafficLightRaycastBoard partnerWatchingTrafficLightRaycastBoard;
    public void UpdateGageHand(string targetName)
    {
        Debug.Log("UpdateGageHand");
        if (dicTrafficLight[targetName].image.fillAmount >= 1f)
        {
            if (dicTrafficLight[targetName].isFinish) return;
            if (!MainGameProjectManager.GetInstance().isServerOnly) return;
            //끝 이벤트 넣을 자리(임시로 text)
            dicTrafficLight[targetName].image.GetComponentInChildren<Text>().text = "끝";
            dicTrafficLight[targetName].isFinish = true;
            dicTrafficLight[targetName].image.fillAmount = 1f;
            Debug.Log("끝");
            if(partnerWatchingTrafficLightRaycastBoard.dicTrafficLight[targetName].isFinish)
            {
                Debug.Log("isFinish");
                MainGameProjectManager.GetInstance().playersOnlyOnServer[targetName].AddScoreOnServer(10);
            }
         
            IsFinishEvent = true;
            return;
        }
        dicTrafficLight[targetName].image.fillAmount += Time.deltaTime / fullTime;
    }
}
