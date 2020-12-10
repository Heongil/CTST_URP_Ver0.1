using DG.Tweening;
using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GuideMovementEvent : MonoBehaviour
{

   
    public Transform[] moveTargets;
    public int moveIndex;
    public Transform[] rotateTargets;
    public int rotateIndex;




    public UnityEvent[] OnFinishEvent;
    public UnityEvent[] OnFinishMove;
    public UnityEvent[] OnFinishRotate;
    public int finishIndex=100;
    public int finishMoveIndex = 100;
    public int finishRotateIndex = 100;

    public Animator animator;
    private void Start()
    {

    }


    public void SetAnimation(string aniName)
    {
        Debug.Log("SetAnimation " + aniName);
        animator.SetTrigger(aniName);
    }

    public void ActionEvent()
    {
      
        if (OnFinishEvent[finishIndex] != null)
        {
            Debug.Log("ActionEvent OnFinishEvent");
            OnFinishEvent[finishIndex].Invoke();
        }
     
    }
    public void ActionMoveFinishEvent()
    {
      
        if (OnFinishMove[finishMoveIndex] != null)
        {
            Debug.Log("ActionEvent OnFinishEvent");
            OnFinishMove[finishMoveIndex].Invoke();
        }
      
    }
    public void ActionRotateFinishEvent(int eventIndex)
    {
     
        if (OnFinishRotate[eventIndex] != null)
        {
            Debug.Log("ActionEvent OnFinishEvent" + "/"+ eventIndex);
            OnFinishRotate[eventIndex].Invoke();
        }
     
    }
   


    public void MoveToTarget(int posIndex)
    {
        transform.DOLookAt(moveTargets[posIndex].position, 1f);
        transform.DOMove(moveTargets[posIndex].position, 3);
    }

    float movingTime = 1f;
    float rotateTime = 1f;

    public void SetRotatingTime(float time)
    {
        rotateTime = time;
    }
    public void SetMovingTime(float time)
    {
        movingTime = time;
    }
    public void SetFinishMoveIndex(int index)
    {
        finishMoveIndex = index;
    }
    public void SetFinishRotateIndex(int index)
    {
        Debug.Log(finishRotateIndex);
        finishRotateIndex = index;
        Debug.Log(finishRotateIndex);
    }
    public void RotateToTargetOnServer(int posIndex)
    {
        transform.DOLookAt(rotateTargets[posIndex].position, rotateTime).OnComplete(()=>ActionRotateFinishEvent(finishRotateIndex));
    }
    public void MoveToTargetOnServer(int posIndex)
    {
        transform.DOMove(moveTargets[posIndex].position, movingTime).OnComplete(ActionMoveFinishEvent);
    }

    public void TeleportToTargetServer(int posIndex)
    {
        transform.position = moveTargets[posIndex].position;
        transform.rotation = moveTargets[posIndex].rotation;
    }

}
