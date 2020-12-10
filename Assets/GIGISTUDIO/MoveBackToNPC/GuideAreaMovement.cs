using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideAreaMovement : MonoBehaviour {


	public Transform target;

	public void SetAreaSize(float area)
    {
		target.localScale = new Vector3(area,0.1f,area);
    }
	
}
