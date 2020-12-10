using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.NetworkRoom;

public class ChildrenPlayerMovement : NetworkBehaviour
{
    [SyncVar]
    public int health = 100;
    public PlayerScore playerScore;
    // Start is called before the first frame update
    void Start()
    {
        if(!isLocalPlayer)
        {
            enabled = false;
            return;
        }
       
    }

    // Update is called once per frame
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            CmdShoot();
        }
    }

    [Command]
    void CmdAddScore()
    {
      
    }

    [Command]
    public void CmdShoot()
    {
        // Do your own shot validation here because this runs on the server
        health -= 5;
    }
}
