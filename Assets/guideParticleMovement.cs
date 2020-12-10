using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class guideParticleMovement : MonoBehaviour
{
    public ParticleSystem particle;
    public Transform target;
   public void TurnOnParticle()
   {
        gameObject.SetActive(true);

        StartCoroutine(cWaitCallback(0.1f,()=>
        {
            particle.Stop();
            particle.Play();
        }));

   }

  //  private void Update()
  //  {
  //      transform.LookAt(target.position);
  //  }


    IEnumerator cWaitCallback(float time,UnityAction callBack)
    {
        yield return new WaitForSeconds(time);
        callBack();
    }
}
