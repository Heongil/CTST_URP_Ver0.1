using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class test : MonoBehaviour
{
    public Transform[] targets;
    int index=0;
    // Start is called before the first frame update
    void Start()
    {
       if(!GetComponent<NetworkIdentity>().isServer)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].gameObject.SetActive(false);
            }
        }

        transform.LookAt(targets[index].position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * 10 * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("TESTOBJ"))
        {
            if(index==0)
            {
                index = 1;
            }
            else
            {
                index = 0;
            }

            transform.LookAt(targets[index].position);
        }
    }
}
