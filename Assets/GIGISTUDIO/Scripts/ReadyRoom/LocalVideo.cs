using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class LocalVideo : MonoBehaviour
{
    public VideoClip[] videos;
    public string[] expainText;
    public UnityEvent[] AddedEvent;
    public int[] playingIndex;

    public TextMeshProUGUI text;
    public VideoPlayer videoPlayer;

    [SerializeField]
    int presentIndex = -1;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    public void PlayerViode(int index)
    {
        gameObject.SetActive(true);
        presentIndex = index;
        videoPlayer.Stop();
        videoPlayer.clip = videos[presentIndex];
        StartCoroutine(cCheckPlaying());
    }

    IEnumerator cCheckPlaying()
    {
        videoPlayer.Play();
        yield return null;
        while (true)
        {
            yield return null;
            if(!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
                playingIndex[presentIndex]++;
                break;
            }
        }
    }

    public void CheckPlayingAndShootEvent(int checkIndex)
    {
        StartCoroutine(cCheckPlayingAndShootEvent(checkIndex));
    }

    IEnumerator cCheckPlayingAndShootEvent(int checkIndex)
    {
     
        while (true)
        {
            yield return null;

                if (playingIndex[presentIndex] > 0)
                {
                    if (AddedEvent[presentIndex] != null) AddedEvent[presentIndex].Invoke();
                    break;
                }
        }
    }

    //public void PlayVideoEvent()
    //{
    //    StartCoroutine(VideoPlayEventCourutine());
    //}
    //
    //
    //
    //
    //
    //public void OffWindow()
    //{
    //    transform.parent.gameObject.SetActive(false);
    //}
    //
    //public void PlayVideo(int serverindex)
    //{
    //    index = serverindex;
    //    StartCoroutine(waitingEndofPlay(index));
    //}




    // IEnumerator waitingEndofPlay(int index)
    // {
    //  
    //     bool isPlaying = false;
    //     bool isFinishVideo = false;
    //     while (enabled)
    //     {
    //         yield return new WaitForSeconds(0.1f);
    //      
    //         if (!videoPlayer.isPlaying && !isPlaying)
    //         {
    //             if (!isFinishVideo)
    //             {
    //                 videoPlayer.Stop();
    //                 videoPlayer.clip = videos[index];
    //                 videoPlayer.Play();
    //              
    //                 isPlaying = true;
    //                 isFinishVideo = true;
    //             }
    //             else
    //             {
    //              
    //
    //                 break;
    //             }
    //
    //
    //
    //
    //         }
    //         else if (videoPlayer.isPlaying && isPlaying)
    //         {
    //             isPlaying = false;
    //         }
    //     }
    // }



    // IEnumerator VideoPlayEventCourutine()
    // {
    //     bool isPlaying = false;
    //
    //     while (enabled)
    //     {
    //         yield return null;
    //         Debug.Log(videoPlayer.isPlaying + "/" + isPlaying);
    //         if (!videoPlayer.isPlaying && !isPlaying)
    //         {
    //
    //             bool IsAllStop = true;
    //
    //             if (IsAllStop)
    //             {
    //                 index++;
    //
    //                 if (index < videos.Length)
    //                 {
    //                     Debug.Log(index + "Start");
    //                 
    //                     videoPlayer.Stop();
    //                     videoPlayer.clip = videos[index];
    //                     videoPlayer.Play();
    //                     isPlaying = true;
    //                 
    //
    //                 }
    //                 else
    //                 {
    //                     yield return new WaitForSeconds(0.1f);
    //                     Debug.Log("FinishEvent");
    //                     if (OnEndEvent != null) OnEndEvent.Invoke();
    //                     break;
    //                 }
    //             }
    //
    //         }
    //         else if (videoPlayer.isPlaying && isPlaying)
    //         {
    //            // Debug.Log(videoPlayer.isPlaying + "/" + isPlaying);
    //             isPlaying = false;
    //         }
    //
    //     }
    // }
}
