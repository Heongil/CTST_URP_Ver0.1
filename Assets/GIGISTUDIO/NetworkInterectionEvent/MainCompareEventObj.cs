using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainCompareEventObj : MainEventObj
{
    [Header("NativeTag")]
    public string compareTag;

    public UnityEvent OnTriggerEnterNativeEvent;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log(other.transform.name);
            OnTriggerEvent.Invoke();
        }
        else if (other.CompareTag(compareTag))
        {
            OnTriggerEnterNativeEvent.Invoke();
        }
    }
}
