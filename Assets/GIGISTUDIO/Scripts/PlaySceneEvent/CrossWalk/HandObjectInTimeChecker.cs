using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObjectInTimeChecker : MonoBehaviour
{
    public bool isIn;
    public float inTime;
    List<Transform> hands = new List<Transform>();

    private void Update()
    {
       if (!isIn) return;
        Debug.Log(hands.Count);
       if(hands.Count > 0)
        {
            inTime += Time.deltaTime;
        }
       else
        {
            inTime -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.CompareTag("Hand"))
        {
            if(!hands.Contains(other.transform))
            {
                hands.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if (hands.Contains(other.transform))
            {
                hands.Remove(other.transform);
            }
        }
    }
}
