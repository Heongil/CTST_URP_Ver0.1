using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OfflineSceneManager : MonoBehaviour
{
    public TextMeshProUGUI display;
    // Start is called before the first frame update



    public GameObject testWorldRoot;
    public GameObject tartget;
    public GameObject testball;
    public GameObject box;
    public GameObject player;
    private void Start()
    {
        NetworkRoomManagerExt.singleton.sceneManager = this;
       // NetworkRoomManagerExt.singleton.networkAddress = PlayerPrefs.GetString("IP");
       // NetworkRoomManagerExt.singleton.networkAddress = "192.168.0.2";
        Setupdisplay();


      
    }
    public void Setupdisplay()
    {
        display.text = "Offline\nNetwork IP = " + NetworkRoomManagerExt.singleton.networkAddress;
    }

  
    public void OuterBoundary()
    {
        Vector3[] points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.OuterBoundary);
        Debug.Log(OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.OuterBoundary));
        for (int i = 0; i < points.Length; i++)
        {

            Instantiate(testball, new Vector3(points[i].x, 0, points[i].z), Quaternion.identity);
        }
     
        display.text = points.Length.ToString();
    }

    public Transform[] targets;

   // private void Update()
   // {
   //     targets[0].LookAt(targets[1].position);
   //     targets[0].localRotation = Quaternion.Euler(0, targets[0].localEulerAngles.y + 90, 0);
   //    //float Dot = Vector3.Dot(targets[0].position, targets[1].position);
   //    //
   //    //float Angle = Mathf.Acos(Dot);
   //    //
   //    //
   //    //Debug.Log("Dot : " + Dot);
   //    //Debug.Log("Angle : " + Angle);
   // }

    public void OuterBoundary2()
    {
       if (OVRManager.boundary.GetConfigured())
        {
            Vector3[] points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.OuterBoundary);
            Instantiate(testball, new Vector3(points[0].x, 0, points[0].z), Quaternion.identity);

            display.text = new Vector3(points[0].x, 0, points[0].z).ToString();
            player.transform.position = new Vector3(points[0].x, player.transform.position.y, points[0].z);
            player.transform.LookAt(new Vector3(points[1].x, player.transform.position.y, points[1].z));
            player.transform.localRotation = Quaternion.Euler(0, player.transform.localEulerAngles.y + 90f, 0);
        }
      
    }

    public void PlayArea()
    {
        Vector3[] points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        Debug.Log(OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea));

        for (int i = 0; i < points.Length; i++)
        {

            Instantiate(testball,new Vector3(points[i].x,0, points[i].z), Quaternion.identity);
        }

        testWorldRoot.transform.position = new Vector3(points[0].x, testWorldRoot.transform.position.y, points[0].z);
       // testWorldRoot.transform.LookAt(new Vector3(points[3].x, 0, points[3].z));

        display.text = points.Length.ToString();
    }
    public void visiable()
    {
        OVRManager.boundary.SetVisible(true);
        display.text = OVRManager.boundary.GetVisible().ToString();
    }

    public void BoundaryTestResult()
    {
        OVRBoundary.BoundaryTestResult test = OVRManager.boundary.TestPoint(tartget.transform.position, OVRBoundary.BoundaryType.OuterBoundary);
        display.text = test.ClosestPoint + "," + test.ClosestDistance;
        
             Instantiate(box, new Vector3(test.ClosestPoint.x, 0, test.ClosestPoint.z) , Quaternion.identity);
    }

}
