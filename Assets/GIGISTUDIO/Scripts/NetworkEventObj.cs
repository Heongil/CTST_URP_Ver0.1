using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkEventObj : NetworkBehaviour
{
    public string targetTag;
    [SerializeField]
    public UnityEvent OnTriggerEnterEvent;
    [SerializeField]
    public UnityEvent OnTriggerExitEvent;
    [SerializeField]
    public UnityEvent OnButtonControllerClick;


    private void OnTriggerEnter(Collider other)
    {
        if (OnTriggerEnterEvent == null) return;
        if (other.CompareTag(targetTag))
        {
            OnTriggerEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (OnTriggerExitEvent == null) return;
        if (other.CompareTag(targetTag))
        {
            OnTriggerExitEvent.Invoke();
        }
    }
}
