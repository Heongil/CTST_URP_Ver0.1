using Mirror.Examples.Tanks;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        static readonly ILogger logger = LogFactory.GetLogger(typeof(NetworkRoomPlayerExt));
        [SyncVar(hook = nameof(OnSetPlayerName))]
        public string deviceName="";
        RoomSceneManager roomSceneManager;

        public GameObject camera;
        public GameObject rig;
        public Transform[] hands;
        public GameObject body;

        public GameObject[] models;
        public GameObject BandObj;
        private PlaySceneManager playSceneManager;
        public OVRHeadsetEmulator ovrHeadsetEmulator;

        public GameObject rootPosition;

        public string presentSetIDobj;
        private void Start()
        {
            roomSceneManager = (RoomSceneManager)NetworkRoomManagerExt.singleton.sceneManager;
            roomSceneManager.SetPlayer(this);
            if (isLocalPlayer)
           {

                NetworkRoomManagerExt.singleton.roomPlayer = this;


                rig.AddComponent<OVRCameraRig>();
                OVRManager ovrmanager = rig.AddComponent<OVRManager>();
                ovrmanager.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;

                ovrHeadsetEmulator = rig.AddComponent<OVRHeadsetEmulator>();
                ovrHeadsetEmulator.opMode = OVRHeadsetEmulator.OpMode.AlwaysOn;

             
                Camera cameraComponent = camera.AddComponent<Camera>();
                camera.AddComponent<AudioListener>();
                camera.tag = "MainCamera";
                cameraComponent.nearClipPlane = 0.2f;
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

               //StartCoroutine(cDoWaitTime(1f,()=>
               //{
               //    gameObject.AddComponent<OVRPlayerController>();
               //    gameObject.AddComponent<OVRSceneSampleController>();
               //}));
            
                CmdSetPlayerZeroPos(PlayerPrefs.GetFloat("PlayerYZeroPos"));
           }
          
            rig.SetActive(true);
        }

        public void SetPlayerDeviceName(string _devicename)
        {
            Debug.Log("SetPlayerDeviceName");
            CmdSetPlayerDeviceName(_devicename);
            CmdOffPlayerIDobj(_devicename);
 
        }
        [Command]
        public void CmdOffPlayerIDobj(string key)
        {
            roomSceneManager.OffPlayerIDobj(key);
        }

        [Command]
        public void CmdSetPlayerDeviceName(string _devicename)
        {
            deviceName = _devicename;
            roomSceneManager.UpdatePlayerIcon(deviceName,true);
        }

        [Command]
        public void CmdSetPlayerColor()
        {
            for (int i = 0; i < models.Length; i++)
            {
                Material[] mats = models[i].GetComponent<SkinnedMeshRenderer>().materials;

                mats[1].SetColor("_BaseColor", roomSceneManager.dicPlayerColors[deviceName]);
            }
            BandObj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", roomSceneManager.dicPlayerColors[deviceName]);
            RpcSetPlayerColor();
        }

        [ClientRpc]
        public void RpcSetPlayerColor()
        {
            for (int i = 0; i < models.Length; i++)
            {
                Material[] mats = models[i].GetComponent<SkinnedMeshRenderer>().materials;
                mats[1].SetColor("_BaseColor", roomSceneManager.dicPlayerColors[deviceName]);
            }
            BandObj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", roomSceneManager.dicPlayerColors[deviceName]);
        }

        public void OnSetPlayerName(string oldname, string newname)
        {
            deviceName = newname;
            Debug.Log("OnSetPlayerName");
            if (!isLocalPlayer)
            {
                StartCoroutine(cDoWaitTime(0.1f, ()=>
                {

                    for (int i = 0; i < models.Length; i++)
                    {
                        Material[] mats = models[i].GetComponent<SkinnedMeshRenderer>().materials;

                        mats[1].SetColor("_BaseColor", roomSceneManager.dicPlayerColors[deviceName]);
                    }
                    BandObj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", roomSceneManager.dicPlayerColors[deviceName]);

                }));
          
                return;
            }

            Debug.Log("OnSetPlayerName");
            NetworkRoomManagerExt.singleton.GetComponent<ProjectManager>().localDeviceName = newname;
            NetworkRoomManagerExt.singleton.GetComponent<ProjectManager>().localDeviceColor = roomSceneManager.dicPlayerColors[deviceName];
            roomSceneManager.OnSetPlayerName(this);
            CmdSetPlayerColor();
        }

        public IEnumerator cDoWaitTime(float time,UnityAction callBack)
        {
            yield return new WaitForSeconds(time);
            callBack();
        }

        public override void OnStartClient()
        {
            if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnStartClient {0}", SceneManager.GetActiveScene().path);

            base.OnStartClient();
        }

        public override void OnClientEnterRoom()
        {
            if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnClientEnterRoom {0}", SceneManager.GetActiveScene().path);
        }

        public override void OnClientExitRoom()
        {
            if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnClientExitRoom {0}", SceneManager.GetActiveScene().path);
        }

        public override void ReadyStateChanged(bool _, bool newReadyState)
        {
            if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "ReadyStateChanged {0}", newReadyState);
        }

        public void ReadyTEST()
        {
            CmdChangeReadyState(true);
        }

        private void OnDestroy()
        {
            Debug.Log("OnDestroy");
            
            Debug.Log("RemovePlayer");
           if(roomSceneManager!=null) roomSceneManager.RemovePlayer(this);
            
        }

        [Command]
        public void CmdOnPlayerStartPosisionObj()
        {

            roomSceneManager.OnPlayerStartPosisionObjServer(deviceName);
            
        }

        [Command]
        public void CmdSetPlayerZeroPos(float y)
        {
            transform.localPosition += new Vector3(0, y, 0);
        }


      
        
        public  override void OnChangeReadyState()
        {
            roomSceneManager.UpdatePlayerReadyState();
        }
    }
}
