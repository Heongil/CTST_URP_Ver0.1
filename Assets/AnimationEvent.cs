using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public Animator animator;

    public UnityEvent OnFinishEvent;
    private void Start()
    {

    }

    public void SetAnimation(string aniName)
    {
        animator.SetTrigger(aniName);
    }

    public void ActionEvent()
    {
        if (OnFinishEvent != null)
        {
            Debug.Log("ActionEvent OnFinishEvent");
            OnFinishEvent.Invoke();
        }
    }

    public void AddEvent(UnityEvent callBack)
    {
        OnFinishEvent.RemoveAllListeners();
        OnFinishEvent.AddListener(() => callBack.Invoke());
    }
}
