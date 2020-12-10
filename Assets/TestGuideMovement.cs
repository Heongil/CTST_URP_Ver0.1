
using DG.Tweening;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;

public class TestGuideMovement : MonoBehaviour
{
    [SerializeField]
    int index = 0;
    public Transform[] moveTargets;

    Vector3 pastPos;
    // Start is called before the first frame update


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
        if (OnFinishEvent != null) OnFinishEvent.Invoke();
    }

    public Transform[] movePoints;
    public DOTweenPath doTweenPath;
    public void AddMovePoint(int index)
    {
        Debug.Log(doTweenPath.wps.Count);
        Vector3 pos = movePoints[index].position;
        Debug.Log(pos);
        doTweenPath.wps.Add(pos);
        Debug.Log(doTweenPath.wps.Count);
    }

    public PathType pathType = PathType.CatmullRom;
    public Vector3[] waypoints;
    public Transform target;
    public Tween t;
    Path path;
    public void PlayDotTween()
    {
        // AddMovePoint(0);

        List<Vector3> test = new List<Vector3>();

        test.Add(movePoints[0].position);
        transform.DOLookAt(movePoints[0].position,1f);
        transform.DOMove(movePoints[0].position, 3);
        // Then set the ease to Linear and use infinite loops



    }
 
}
