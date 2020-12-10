using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotPathMovement : MonoBehaviour
{
    DOTweenPath originePath;
    DOTweenPath pathDo;
    void Start()
    {
        originePath = GetComponent<DOTweenPath>();
        pathDo = originePath;
    }
    public void SetPathAndPlay(Transform trs)
    {
        pathDo = trs.GetComponent<DOTweenPath>();
        pathDo.DOPlay();
    }
   
}
