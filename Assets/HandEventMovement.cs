using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandEventMovement : MonoBehaviour
{

    [SerializeField]
    private bool isFinishEvent;
    [SerializeField]
    private float inTime=0;
    [SerializeField]
    private float goalTime=0;
    public UnityEvent OnFinishGoalTime;


    private void Start()
    {
        SetHandEventObj();
    }

    public void SetHandEventObj()
    {
        if (MainGameProjectManager.GetInstance().localPlayer == null) return;
        Transform pos = MainGameProjectManager.GetInstance().localPlayer.GetComponent<MainPlayer>().camera.transform;
       transform.position = new Vector3(transform.position.x, pos.position.y + 0.3f, transform.position.z);
       gameObject.SetActive(true);
    }
    public void StartHandIn()
    {
        if (isFinishEvent) return;
        StartCoroutine(UpdatingTime());
    }

    public void ExitHand()
    {
        StopAllCoroutines();
    }


    IEnumerator UpdatingTime()
    {
        while (true)
        {
            inTime += Time.deltaTime;
            yield return null;
            if(inTime>=goalTime)
            {
                isFinishEvent = true;
                OnFinishGoalTime.Invoke();
                break;
            }
        }
    }
}
