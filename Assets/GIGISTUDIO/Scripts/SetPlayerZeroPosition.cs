using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerZeroPosition : MonoBehaviour
{

    public string targetTag;
    public bool isIn;
    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            isIn = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(targetTag))
        {
            isIn = false;
        }
    }

    
}
