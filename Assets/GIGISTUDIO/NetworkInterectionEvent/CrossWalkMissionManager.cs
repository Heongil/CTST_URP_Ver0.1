using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossWalkMissionManager : MonoBehaviour
{
    public int Test = 0;

    private void Start()
    {
        Test = Random.Range(-100, 101);
    }
    public void FinishMission()
    {
        MainGameProjectManager.GetInstance().localPlayer.AddScore(Test);
    }
}
