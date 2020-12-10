using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrafficLightMovement : MonoBehaviour
{
    public enum TrafficLightState
    {
        None,
        Creen,
        Red,
        yellow
    }
    

   

    public int creenTime;
    public int redTime;
    [Header("이벤트 시작상태")]
    public TrafficLightState state;
    public TrafficLightState startState;
    // 0 = offall 1= creen 2= red
    [Header("0=offall/1=creen/2=red")]
    public Texture[] trafficLightTextures;

    
    public MeshRenderer[] Renderers;

    public UnityEvent OnFinishEvent;
    private void Start()
    {
        SetTexture(2);
    }

    public void SetTexture(int index)
    {
        for (int i = 0; i < Renderers.Length; i++)
        {
            Renderers[i].material.SetTexture("_EmissionMap", trafficLightTextures[index]);
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void StartEvent()
    {
        StartCoroutine(UpdatingLight());
    }

   

    public void StartYellowEvent()
    {
        StartCoroutine(YellowEvent());
    }

    IEnumerator YellowEvent()
    {

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (enabled) SetTexture(0);
            else break;
            yield return new WaitForSeconds(0.5f);
            if (enabled) SetTexture(3);
            else break;
        }
      
    }
    public void CreenEvent()
    {
        StartCoroutine(WaitEvent());
    }

    IEnumerator WaitEvent()
    {
        if (enabled) SetTexture(2);
        yield return new WaitForSeconds(5f);
        if (enabled)
        {
            SetTexture(1);
            if (OnFinishEvent != null) OnFinishEvent.Invoke();
        }

    }

    IEnumerator UpdatingLight()
    {

        switch (state)
        {
            case TrafficLightState.Creen:
                SetTexture(1);
                break;
            case TrafficLightState.Red:
                SetTexture(2);
                break;
            case TrafficLightState.yellow:
                SetTexture(3);
                break;
        }

        int creenBlinkTime = (int)(creenTime * 0.4f);
        int presentTime=0;
        bool IsFlashing=false;
        while (enabled)
        {
           yield return new WaitForSeconds(1f);
            presentTime++;

            switch (state)
            {
                case TrafficLightState.Creen:

                    if(presentTime >= creenBlinkTime)
                    {
                        if(IsFlashing)
                        {
                            SetTexture(1);
                            IsFlashing = false;
                        }
                        else
                        {
                            SetTexture(0);
                            IsFlashing = true;
                        }
                    }

                    if (presentTime >= creenTime)
                    {
                        SetTexture(2);
                        presentTime = 0;
                        state = TrafficLightState.Red;
                    }
                    break;
                case TrafficLightState.Red:
                    if (presentTime >= redTime)
                    {
                        presentTime = 0;
                        state = TrafficLightState.Red;
                    }
                    break;
                case TrafficLightState.yellow:
                    if (presentTime >= 1)
                    {
                        presentTime = 0;
                      
                    }
                    break;
            }

         
        }
    }
}
