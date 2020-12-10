using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffAftherTime : MonoBehaviour
{

    public float onOffTime;

    private void OnEnable()
    {
        StartCoroutine(wiatforTimeToOnOff());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator wiatforTimeToOnOff()
    {
        yield return new WaitForSeconds(onOffTime);
        gameObject.SetActive(false);
    }
}
