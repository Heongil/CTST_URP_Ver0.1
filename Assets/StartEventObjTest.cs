using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class StartEventObjTest : NetworkBehaviour
{
    [Header("PositiveTag")]
    public string targetTag;
    public MyEvent OnTriggerEvent;
    public MyEvent OnTriggerExitEvent;

    // Start is called before the first frame update
    void Start()
    {
        if (isServerOnly) targetTag = "NetworkPlayer";
        else targetTag = "Player";
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
