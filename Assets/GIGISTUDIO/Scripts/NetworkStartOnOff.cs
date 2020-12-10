using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkStartOnOff : MonoBehaviour
{
    public bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(isOn);
    }

   
}
