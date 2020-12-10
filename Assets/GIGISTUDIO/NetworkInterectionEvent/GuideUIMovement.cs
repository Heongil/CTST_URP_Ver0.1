using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class GuideUIMovement : NetworkBehaviour
{

    public VideoClip[] videos;
    public string[] expainText;
    public TextMeshProUGUI text;
    public VideoPlayer videoPlayer;
    public UnityEvent OnEndEvent;
    public UnityEvent OnEndEventNetwork;
    int index = -1;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(isServerOnly);
        videoPlayer.clip = videos[0];
        index = -1;
        videoPlayer.SetDirectAudioVolume(0,0);
    }

    public void PlayVideoEvent()
    {
        if (!isServerOnly)
        {
            return;
        }
        StartCoroutine(VideoPlayEventCourutine());
    }

    [ClientRpc]
    public void RpcOnEndEventNetwork()
    {
        Debug.Log("RpcOnEndEventNetwork");
        if (OnEndEventNetwork != null)
        {
            Debug.Log("OnEndEventNetwork.Invoke()");
            OnEndEventNetwork.Invoke();
        }
    }

    [ClientRpc]
    public void RpcOffWindow()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void OffWindow()
    {
        transform.parent.gameObject.SetActive(false);
    }
    [ClientRpc]
    public void RpcPlayVideo(int serverindex)
    {
        index = serverindex;
      
        StartCoroutine(waitingEndofPlay(index));
    }

    


    IEnumerator waitingEndofPlay(int index)
    {
        text.text = index + "Start Playing";
        bool isPlaying = false;
        bool isFinishVideo = false;
        while (enabled)
        {
            yield return new WaitForSeconds(0.1f);
            text.text = index + "Start Playing/" + videoPlayer.isPlaying;
            if (!videoPlayer.isPlaying && !isPlaying)
            {
                if(!isFinishVideo)
                {
                    videoPlayer.Stop();
                    videoPlayer.clip = videos[index];
                    videoPlayer.Play();
                    text.text = index + "Start";
                    isPlaying = true;
                    isFinishVideo = true;
                }
                else
                {
                    text.text = index + "Finish Playing";
                    MainGameProjectManager.GetInstance().localPlayer.CmdReSetFinishPlayerVideo(false);
                    break;
                }

              


            }
            else if (videoPlayer.isPlaying && isPlaying)
            {
                isPlaying = false;
            }
        }
    }



    IEnumerator VideoPlayEventCourutine()
    {
        bool isPlaying=false;

        Dictionary<GameObject, MissionInfo> players = MainGameProjectManager.GetInstance().players;
        while (enabled)
        {
            yield return null;
            Debug.Log(videoPlayer.isPlaying +"/"+ isPlaying);
            if (!videoPlayer.isPlaying && !isPlaying)
            {

                bool IsAllStop = true;

               foreach (var element in players)
               {
                  if (element.Value.IsPlayingVideoMission)
                   {
                       IsAllStop = false;
                       break;
                   }
               }

                if(IsAllStop)
                {
                    index++;

                    if (index < videos.Length)
                    {
                        Debug.Log(index + "Start");
                        text.text = index + "Start";
                        videoPlayer.Stop();
                        videoPlayer.clip = videos[index];
                        videoPlayer.Play();
                        isPlaying = true;
                        RpcPlayVideo(index);

                    }
                    else
                    {
                        yield return new WaitForSeconds(0.1f);
                        Debug.Log("FinishEvent");
                      if(OnEndEvent!=null)OnEndEvent.Invoke();
                        RpcOnEndEventNetwork();
                       //OffWindow();
                       //RpcOffWindow();
                        break;
                    }
                }
                
            }
            else if (videoPlayer.isPlaying && isPlaying)
            {
                Debug.Log(videoPlayer.isPlaying + "/" + isPlaying);
                isPlaying = false;
            }
          
        }
    }



    private void Update()
    {
         if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayVideoEvent();
        }
    }
}
