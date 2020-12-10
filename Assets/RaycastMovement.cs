using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMovement : MonoBehaviour
{
    public string targetTag;
    RaycastHit hitInfo;
    public Transform target;
    public MissionInfo missionInfo;
    // Update is called once per frame
    void Update()
    {
        RaycastHit();
    }

    void RaycastHit()
    {
        Debug.DrawRay(target.position, target.forward * 5, Color.green, 1f);
        if(Physics.Raycast(target.position, target.forward,out hitInfo,5f))
        {
            if(hitInfo.transform.tag == targetTag)
            {
                switch (hitInfo.transform.name)
                {
                    case "JudgTarget":
                            hitInfo.transform.GetComponent<WatchingTrafficLightRaycastBoard>().UpdateGage(missionInfo.deviceName);
                            break;
                        case "JudgTargetTraining":
                            hitInfo.transform.GetComponent<WatchingTrafficLightRaycastBoard>().UpdateGageTraining(missionInfo.deviceName);
                            break;
                    case "JudgTargetHand":
                        hitInfo.transform.GetComponent<WatchingTrafficLightRaycastBoard>().UpdateGageHand(missionInfo.deviceName);
                        break;
                }

                return;
            }
        }
    }
}
