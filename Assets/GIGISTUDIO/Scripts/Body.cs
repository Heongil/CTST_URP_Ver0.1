using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Transform headTarget;
    public Transform root;
    public Transform height;
    float fHeight;
    // Start is called before the first frame update
    void Start()
    {
        fHeight = height.localScale.y * 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(headTarget.position.x, fHeight, headTarget.position.z-0.25f);
    }
}
