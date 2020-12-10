using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections;
using Mirror;
using System.Collections.Generic;
using Mirror.Examples.Additive;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class MainEventObj : MonoBehaviour
{
    [Header("PositiveTag")]
    public string targetTag;
    public MyEvent OnTriggerEvent;
    public MyEvent OnTriggerExitEvent;

    // Start is called before the first frame update
    void Start()
    {

        Rigidbody rid = GetComponent<Rigidbody>();
        rid.useGravity = false;
        rid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        GetComponent<BoxCollider>().isTrigger = true;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(targetTag))
        {
            Debug.Log(other.transform.name);
            OnTriggerEvent.Invoke();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log(other.transform.name);
            OnTriggerExitEvent.Invoke();
        }
    }

}
