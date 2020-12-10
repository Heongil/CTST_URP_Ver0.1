using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlaySceneManager : NetworkBehaviour
{
    public GameObject[] serverOnlyObj;
    public MainPlayer localPlayer;

    public List<MainPlayer> players = new List<MainPlayer>();

    bool isSetup;

    
    public GameObject rootObj;
    public GameObject roottestobj;
    GameObject worldrootpos;
    public TextMeshProUGUI meshpro;

    public MainGameProjectManager mainGameProjectManager;
   // private 
    // Start is called before the first frame update
    void Awake()
    {
        NetworkRoomManagerExt.singleton.sceneManager = this;
    }

    private void Start()
    {
        if(isServerOnly)
        {
            for (int i = 0; i < serverOnlyObj.Length; i++)
            {
                serverOnlyObj[i].SetActive(true);
            }
          
        }
       // else
       // {
       //     enabled = false;
       // }


    }
   

    public void SetPlayer(MainPlayer player)
    {
        localPlayer = player;

    }

    string tt = "test";

    private void Update()
    {
        if (localPlayer != null)
        {
            meshpro.text = tt+" : " + localPlayer.transform.localPosition +'\n'+"Pos : " + pos;
        }


        if (isSetup) return;

      
        if (Input.GetKeyDown(KeyCode.Space))
        {
           for (int i = 0; i < players.Count; i++)
           {
               players[i].rig.transform.localPosition = new Vector3(players[i].camera.transform.localPosition.x * -1,0, players[i].camera.transform.localPosition.z * -1f);
           }

            
        }
    }

    Vector3 pos;
    public void SetPlayerPos()
    {
        if (isSetup) return;
        isSetup = true;
        tt = "SetPlayerPosStart";
        Vector3[] points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.OuterBoundary);
        pos = points[0];

        GameObject testpos = Instantiate(roottestobj, new Vector3(points[0].x, 0, points[0].z), Quaternion.identity);

        //localPlayer.rootPosition = worldrootpos;
        //rootObj.transform.parent = localPlayer.rootPosition.transform;
        //rootObj.transform.localPosition = Vector3.zero;


        int index = 10;

        localPlayer.transform.position = new Vector3(points[0].x * -1f, localPlayer.transform.position.y, points[0].z * -1f);

        testpos.transform.LookAt(new Vector3(points.Length - 1, 0, points[points.Length - index].z));

        testpos.transform.localRotation = Quaternion.Euler(0, testpos.transform.localEulerAngles.y - 90f, 0);

        rootObj.transform.localRotation = testpos.transform.localRotation;

        Instantiate(roottestobj, new Vector3(points[points.Length- index].x, 0, points[points.Length - index].z), Quaternion.identity).GetComponent<MeshRenderer>().material.color = Color.white;

        

        tt = "SetPlayerPosEnd";
    }
    public void MoveRightTest()
    {
        localPlayer.transform.localPosition += Vector3.right;
    }

    public void MoveLeftTest()
    {
        localPlayer.transform.localPosition += Vector3.left;
    }
    [Command]
    public void CmdSetPlayerPos()
    {
        Debug.Log("CmdSetPlayerPos");
    }



    //가이드 Particle
    public GameObject guideFlowObj;
    public Transform giudeTargetFrom;
    public Transform giudeTargetTo;
    public void SetOnGuideFlow()
    {
        if (isServerOnly) return;
       giudeTargetTo.parent = localPlayer.body.transform;
       giudeTargetTo.localPosition = new Vector3(0, 0.5f, 0);
       giudeTargetTo.localRotation = Quaternion.identity;
      
       giudeTargetFrom.parent = mainGameProjectManager.dicMainEventMiddle[(MissionState)mainGameProjectManager.mission + 1].DicPositions[NetworkRoomManagerExt.singleton.GetComponent<ProjectManager>().localDeviceName].transform;
       giudeTargetFrom.localPosition = new Vector3(0, 0.5f, 0);
       giudeTargetFrom.localRotation = Quaternion.identity;
        guideFlowObj.GetComponent<guideParticleMovement>().TurnOnParticle();
    }
   
    public void OnOffFlow(bool isOn)
    {
        guideFlowObj.SetActive(isOn);
    }
    //////////////////////////////
    /// <summary>
    /// 
    /// 
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// 

    float waitTime=1f;
    bool IsTargetOn = true;
    public void SetObjOnOff(bool isOn)
    {
        IsTargetOn = isOn;
    }
    public void SetObjOnOffWaitTime(float WaitTime)
    {
        waitTime = WaitTime;
    }
    public void TrunOnObj(GameObject target)
    {
        StartCoroutine(CTrunOnObj(target, waitTime, IsTargetOn));
    }

    IEnumerator CTrunOnObj(GameObject target,float WaitTime,bool isOn)
    {
        yield return new WaitForSeconds(WaitTime);
        target.gameObject.SetActive(isOn);
    }

    public void PlayerFade(string fadeInfo)
    {
        if (localPlayer != null)
        {
            localPlayer.camera.GetComponent<OVRScreenFade>().ScreenFade(fadeInfo);
        }
    }

    
}
