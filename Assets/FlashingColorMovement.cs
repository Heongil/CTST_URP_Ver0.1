using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlashingColorMovement : MonoBehaviour
{
    public int flashingCount=1;
    public float flashingCapTime;
    public Color color;
    public UnityEvent OnFinishFlashingEvent;
    public GameObject targetObj;

    public void StartEvent()
    {
        gameObject.SetActive(true);
        targetObj.SetActive(true);
        targetObj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
        StartCoroutine(EventCourutine(flashingCount, flashingCapTime));
    }

    IEnumerator EventCourutine(int count,float cooltime)
    {
        Debug.Log("Start Event");
        while (true)
        {

            yield return new WaitForSeconds(cooltime);
            targetObj.SetActive(false);
            Debug.Log(count + "/" + targetObj.activeSelf);
            count--;
            yield return new WaitForSeconds(cooltime);
            targetObj.SetActive(true);
            Debug.Log(count + "/" + targetObj.activeSelf);
            if (count <= 0)
            {
                Debug.Log("Finish Event");
              if(OnFinishFlashingEvent!=null)OnFinishFlashingEvent.Invoke();
                break;
            }
        }
    }
  
}
