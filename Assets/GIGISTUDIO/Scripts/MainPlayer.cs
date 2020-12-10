using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MainPlayer : NetworkBehaviour
{
    public GameObject camera;
    public GameObject rig;
    public Transform[] hands;
    public GameObject body;

    public GameObject[] models;
    private PlaySceneManager playSceneManager;
    public OVRHeadsetEmulator ovrHeadsetEmulator;

    public GameObject rootPosition;
    // Start is called before the first frame update
    void Start()
    {
        playSceneManager = (PlaySceneManager)NetworkRoomManagerExt.singleton.sceneManager;
        if (isLocalPlayer)
        {
            rig.AddComponent<OVRCameraRig>();
            OVRManager ovrmanager = rig.AddComponent<OVRManager>();
            ovrmanager.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;

            ovrHeadsetEmulator = rig.AddComponent<OVRHeadsetEmulator>();
            ovrHeadsetEmulator.opMode = OVRHeadsetEmulator.OpMode.AlwaysOn;


            Camera cameraComponent = camera.AddComponent<Camera>();
           // camera.AddComponent<AudioListener>();
            camera.tag = "MainCamera";
            camera.AddComponent<OVRScreenFade>();
           // cameraComponent.nearClipPlane = 0.45f;
            cameraComponent.cullingMask = ~(1 << 9);
            for (int i = 0; i < models.Length; i++)
            {
                models[i].layer = 9;
            }
            body.tag = "Player";
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].tag = "Hand";
            }
            playSceneManager.SetPlayer(this);


            CmdSetPlayerZeroPos(PlayerPrefs.GetFloat("PlayerYZeroPos"));

        }

        playSceneManager.players.Add(this);
        rig.SetActive(true);


      
    }
    [Command]
    public void CmdSetPlayerZeroPos(float y)
    {
        transform.localPosition += new Vector3(0, y, 0);
    }

   

    IEnumerator SetWorldPos()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(2f);
            if (OVRManager.instance != null && OVRManager.boundary != null)
            {
                if (OVRManager.boundary.GetConfigured())
                {
                    Vector3[] points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.OuterBoundary);
                    rootPosition = Instantiate(rootPosition, new Vector3(points[0].x, 0, points[0].z), Quaternion.identity);
                    playSceneManager.rootObj.transform.parent = rootPosition.transform;
                    playSceneManager.rootObj.transform.localPosition = Vector3.zero;
                    break;
                }

            }
            
        }
    }


    public void SetPlayerPos()
    {

        Vector3[] points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.OuterBoundary);
        Instantiate(rootPosition, new Vector3(0, 0, 0), Quaternion.identity);
        //localPlayer.rootPosition = worldrootpos;
        //rootObj.transform.parent = localPlayer.rootPosition.transform;
        //rootObj.transform.localPosition = Vector3.zero;
        transform.parent = playSceneManager.rootObj.transform;
        transform.localPosition = new Vector3(points[0].x * -1 +camera.transform.localPosition.x * -1, 0, points[0].z * -1 + camera.transform.localPosition.z * -1);
    }

  

}
