using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScoreMovement : MonoBehaviour
{
    public EventScore[] eventScores;
    public GameObject[] offObjs;
    public void AddScoreToLocalPlayer()
    {
        StartCoroutine(AddScoreToLocalPlayerCOU());
    }

    IEnumerator AddScoreToLocalPlayerCOU()
    {
        int result = 0;
        for (int i = 0; i < eventScores.Length; i++)
        {
            result += eventScores[i].GetScore();
            Debug.Log(eventScores[i].GetScore());
        }
     
        MainGameProjectManager.GetInstance().localPlayer.AddScore(result);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < offObjs.Length; i++)
        {
            offObjs[i].SetActive(false);
        }

    }
}
