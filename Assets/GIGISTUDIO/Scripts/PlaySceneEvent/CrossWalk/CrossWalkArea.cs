using System.ComponentModel;
using TMPro;
using UnityEngine;

public class CrossWalkArea : MonoBehaviour
{


    public SightCtrl sightCtrl;
    public HandObjectInTimeChecker handTimeChecker;
    public Transform handCheckerPostionTrn;
    private Transform heaTarget;


    private bool isIn;
    private float totalIntime;
    public TextMeshProUGUI showingText;
    public void SetCrossWalkEvent(Transform HeadTarget)
    {
        gameObject.SetActive(true);
        heaTarget = HeadTarget;
        handTimeChecker.inTime = 0;
        sightCtrl.driverWatchTime = 0;
    }



    public void SightEventEnter()
    {
        sightCtrl.enabled = true;
        handTimeChecker.enabled = true;
        sightCtrl._transform = heaTarget;
    }

    public void SightEventExit()
    {
        sightCtrl.enabled = false;
        handTimeChecker.enabled = false;
        sightCtrl._transform = null;
    }

    public void HandCheckEventEnter()
    {
        handCheckerPostionTrn.position = new Vector3(handCheckerPostionTrn.position.x, heaTarget.position.y, handCheckerPostionTrn.position.z);
        handTimeChecker.isIn = true;
    }

    public void HandCheckEventExit()
    {
        handTimeChecker.isIn = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = true;
            HandCheckEventEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = false;
            HandCheckEventExit();
        }
    }

    private void Update()
    {
        if(isIn)
        {
            totalIntime += Time.deltaTime;
        }
      // showingText.text =
      //     "WatchTime = " + sightCtrl.driverWatchTime.ToString("N0")+" \n " +
      //     "HandTime = "+handTimeChecker.inTime.ToString("N0") +"\n"+
      //     "totaltime = "+ totalIntime.ToString("N0");
    }
}
