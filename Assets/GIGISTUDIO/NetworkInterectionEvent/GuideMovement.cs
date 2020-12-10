using DG.Tweening;
using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GuideMovement : NetworkBehaviour
{
    [SerializeField]
    int index = 0;
    public Transform[] moveTargets;

    Vector3 pastPos;
    // Start is called before the first frame update


    public Animator animator;

    public UnityEvent OnFinishEvent;
    public UnityEvent OnFinishMove;
    public UnityEvent OnFinishRotate;
    private void Start()
    {
        
    }

    public void MoveTotarget()
    {
        if (!isServerOnly) return;
        pastPos = transform.position;
        transform.DOMove(moveTargets[index].position,3f).onComplete =()=> { transform.DOLookAt(pastPos, 2f).onComplete = () => { Debug.Log("EndRotate"); }; } ;
        index++;
    }

   
    public void SetAnimation(string aniName)
    {
        Debug.Log("SetAnimation " + aniName);
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
    public void ActionMoveFinishEvent()
    {
        if (OnFinishMove != null)
        {
            Debug.Log("ActionEvent OnFinishEvent");
            OnFinishMove.Invoke();
        }
    }
    public void ActionRotateFinishEvent()
    {
        if (OnFinishRotate != null)
        {
            Debug.Log("ActionEvent OnFinishEvent");
            OnFinishRotate.Invoke();
        }
    }
    public void AddEvent(UnityEvent callBack)
    {
        Debug.Log("AddEvent " + callBack);
        if (OnFinishEvent != null)
        {
            OnFinishEvent.RemoveAllListeners();
            OnFinishEvent.AddListener(() => callBack.Invoke());
        }
    }
    public void AddOnFinishMoveEvent(UnityEvent callBack)
    {
        if (OnFinishMove != null)
        {
            Debug.Log("AddOnFinishMoveEvent " + callBack);
            OnFinishMove.RemoveAllListeners();
            OnFinishMove.AddListener(() => callBack.Invoke());
        }
    }
    public void AddOnFinishRotateEvent(UnityEvent callBack)
    {
        if (OnFinishRotate != null)
        {
            Debug.Log("AddOnFinishRotateEvent " + callBack);
            OnFinishRotate.RemoveAllListeners();
            OnFinishRotate.AddListener(() => callBack.Invoke());
        }
    }
    public Transform[] movePoints;
    public DOTweenPath doTweenPath;
   
    public void AddMovePoint(int index)
    {
        Debug.Log(doTweenPath.wps.Count);
        doTweenPath.wps.Add(movePoints[index].position);
        Debug.Log(doTweenPath.wps.Count);
    }

    public void PlayDotTween()
    {
        Debug.Log("PlayDotTween/"+ isServerOnly);
        if (isServerOnly)
        {
            doTweenPath.onComplete.RemoveAllListeners();
            doTweenPath.DOPlay();
            doTweenPath.onComplete.AddListener(() => doTweenPath.wps.Clear());
        }
    
    }

    public void MoveToTarget(int posIndex)
    {
        transform.DOLookAt(movePoints[posIndex].position, 1f);
        transform.DOMove(movePoints[posIndex].position, 3);
    }

    float movingTime=1f;
    float rotateTime = 1f;

    public void SetRotatingTime(float time)
    {
        rotateTime = time;
    }
    public void SetMovingTime(float time)
    {
        movingTime = time;
    }
    public void RotateToTargetOnServer(int posIndex)
    {
        transform.DOLookAt(movePoints[posIndex].position, rotateTime).OnComplete(ActionRotateFinishEvent);
    }
    public void MoveToTargetOnServer(int posIndex)
    {
        transform.DOMove(movePoints[posIndex].position, movingTime).OnComplete(ActionMoveFinishEvent);
    }

    public void TeleportToTargetServer(int posIndex)
    {
        if (!isServerOnly) return;
        transform.position = movePoints[posIndex].position;
        transform.rotation = movePoints[posIndex].rotation;
    }


    public UnityEvent[] ActionEventLater;
    public int eventPlayIndex;

    public void SetEventIndex(int index)
    {
        eventPlayIndex = index;
    }
    public void PlayerDoEventLater(float waitTime)
    {
        StartCoroutine(doEventLater(waitTime));
    }
    IEnumerator doEventLater(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (ActionEventLater != null) ActionEventLater[eventPlayIndex].Invoke();
    }
}
