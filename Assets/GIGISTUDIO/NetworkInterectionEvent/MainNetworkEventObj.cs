using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections;
using Mirror;
using System.Collections.Generic;
using Mirror.Examples.Additive;


[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class MainNetworkEventObj : NetworkBehaviour
{
    public bool OnStartActive;
    public string targetTag;
    public MyEvent OnTriggerEvent;


    List<GameObject> targets = new List<GameObject>();


    public int showing;
    public int netcount;

    public MissionState missionState;
    // Start is called before the first frame update

    private void OnEnable()
    {
        showing = 0;
        netcount = 0;
    }
    void Start()
    {

        Rigidbody rid =  GetComponent<Rigidbody>();
        rid.useGravity = false;
        rid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        GetComponent<BoxCollider>().isTrigger = true;
        StartCoroutine(Set());
    }


    IEnumerator Set()
    {
        yield return null;
        gameObject.SetActive(OnStartActive);
    }

  


    // Update is called once per frame
   //void Update()
   //{
   //
   //    netcount = AdditiveNetworkManager.singleton.numPlayers;
   //}


    private void OnTriggerEnter(Collider other)
    {

         if (other.CompareTag(targetTag))
         {
             Debug.Log(other.transform.name);
            if(!targets.Contains(other.gameObject))
            {
               
                targets.Add(other.gameObject);
                showing = targets.Count;

                if (targets.Count >= AdditiveNetworkManager.singleton.numPlayers)
                {
                    Debug.Log("OnTriggerEvent");
                    if (OnTriggerEvent != null) OnTriggerEvent.Invoke();
                   // MainGameProjectManager.GetInstance().SetState(missionState);
                }
            }

        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            targets.Remove(other.gameObject);
            showing = targets.Count;
        }
    }


}
