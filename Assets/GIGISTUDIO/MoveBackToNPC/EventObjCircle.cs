using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class EventObjCircle : MonoBehaviour
{
    public string targetTag;
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    void Start()
    {

        Rigidbody rid = GetComponent<Rigidbody>();
        rid.useGravity = false;
        rid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        SphereCollider box = GetComponent<SphereCollider>();
        if (box != null) box.isTrigger = true;
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetTag == "") return;
        if (other.CompareTag(targetTag))
        {
            Debug.Log(targetTag);
            if (OnTriggerEnterEvent != null) OnTriggerEnterEvent.Invoke();
        }
    }

  

    private void OnTriggerExit(Collider other)
    {
        if (targetTag == "") return;
        if (other.CompareTag(targetTag))
        {
            Debug.Log(targetTag);
            if (OnTriggerExitEvent != null) OnTriggerExitEvent.Invoke();
        }
    }
}
