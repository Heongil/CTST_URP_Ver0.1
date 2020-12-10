using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BhapticsOculusPointing : MonoBehaviour
{
    public Transform shootPoint;
    public LayerMask rayLayerMask;



    [HideInInspector] public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;
    [HideInInspector] public Material laserMaterial;
    private LineRenderer laser;
    private Vector3 destinationPoint;
    private float rayDistance = 10f;
    private bool hitSomething;


    void Start()
    {
        laser = gameObject.AddComponent<LineRenderer>();
        laser.startWidth = laser.endWidth = 0.015f;
        laser.SetPositions(new Vector3[] { transform.position, transform.position });
        laser.material = laserMaterial;
        laser.enabled = true;
    }

    

    void Update()
    {
        CheckForTrigger();
        DrawLine();
    }

    private void CheckForTrigger()
    {
        RaycastHit raycastHit;
        hitSomething = Physics.Raycast(shootPoint.position, shootPoint.forward, out raycastHit, rayDistance, rayLayerMask);

        if (hitSomething)
        {
            destinationPoint = raycastHit.point;
            if (OVRInput.GetDown(button))
            {
                var uiButton = raycastHit.collider.GetComponent<Button>();
                uiButton?.onClick.Invoke();
            }
        }
        else
        {
            destinationPoint = shootPoint.position + shootPoint.forward * rayDistance;
        }
    }

    private void DrawLine()
    {
        if (shootPoint == null)
        {
            laser.enabled = false;
            return;
        }
        laser.material.color = hitSomething ? Color.green : Color.red;
        laser.enabled = true;
        laser.SetPosition(0, shootPoint.position);
        laser.SetPosition(1, destinationPoint);
    }
}
