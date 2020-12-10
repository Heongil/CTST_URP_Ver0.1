using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]


public class CheckPlayers : MonoBehaviour
{
    [Header("PositiveTag")]
    public string[] targetTags;
    public MyEvent[] OnTriggerEvents;


    List<Body> lists = new List<Body>();
    int counts;
    [SerializeField]
    bool isEvent=false;
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

        for (int i = 0; i < targetTags.Length; i++)
        {
            if (other.CompareTag(targetTags[i]))
            {
                Body body = other.GetComponent<Body>();
                if (!lists.Contains(body))
                {
                    lists.Add(body);
                }
                Debug.Log(other.transform.name);

                
                if (lists.Count != NetworkRoomManagerExt.singleton.GetComponent<ProjectManager>().playerCount || isEvent) return;

                isEvent = true;

                for (int j = 0; j < OnTriggerEvents.Length; j++)
                {
                    OnTriggerEvents[j].Invoke();
                }
            }
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < targetTags.Length; i++)
        {
            if (other.CompareTag(targetTags[i]))
            {
                Body body = other.GetComponent<Body>();
                if (lists.Contains(body))
                {
                    lists.Remove(body);
                }
            }
        }
    }

}
