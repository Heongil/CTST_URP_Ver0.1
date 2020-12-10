using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class EventObj : MonoBehaviour
{
    public string targetTag;
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerStayEvent;
    public UnityEvent OnTriggerExitEvent;
    public UnityEvent OnButtonControllerClick;

    void Start()
    {

        Rigidbody rid = GetComponent<Rigidbody>();
        rid.useGravity = false;
        rid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null) box.isTrigger = true;
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (OnTriggerEnterEvent != null) OnTriggerEnterEvent.Invoke();

            EventScore score = GetComponent<EventScore>();
            if (score != null)
            {
                score.UpdateEventScore(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (OnTriggerStayEvent != null) OnTriggerStayEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (OnTriggerExitEvent != null) OnTriggerExitEvent.Invoke();

           //EventScore score = GetComponent<EventScore>();
           //if (score != null)
           //{
           //    score.UpdateEventScore(false);
           //}
        }
    }
}
