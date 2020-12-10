using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPosZeroController : MonoBehaviour
{
    public SetPlayerZeroPosition setPlayerZeroPosition;

    public void SetPlayerPos()
    {
        PlayerPrefs.SetFloat("PlayerYZeroPos", NetworkRoomManagerExt.singleton.roomPlayer.rig.transform.position.y);
    }

    public void GoUpUntillExit()
    {
        if(setPlayerZeroPosition.isIn)
        {
            NetworkRoomManagerExt.singleton.roomPlayer.rig.transform.position += Vector3.up * 0.5f * Time.deltaTime;
        }
    }
    public void GoDownUntillEnter()
    {
        if (!setPlayerZeroPosition.isIn)
        {
            NetworkRoomManagerExt.singleton.roomPlayer.rig.transform.position -= Vector3.up * 0.5f * Time.deltaTime;
        }
    }

    public void GoDown()
    {
        NetworkRoomManagerExt.singleton.roomPlayer.rig.transform.position -= Vector3.up * 0.5f * Time.deltaTime;
    }

    public void GoUp()
    {
        NetworkRoomManagerExt.singleton.roomPlayer.rig.transform.position += Vector3.up * 0.5f * Time.deltaTime;
    }
}
