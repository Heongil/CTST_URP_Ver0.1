using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SightCtrl : MonoBehaviour
{



    public float ViewAngle;    //시야각
    public float ViewDistance; //시야거리

    public LayerMask TargetMask;    //Target 레이어마스크 지정을 위한 변수
    public LayerMask ObstacleMask;  //Obstacle 레이어마스크 지정 위한 변수

  
    public bool isOn;

    public float driverWatchTime;


    public Transform _transform = null;

    public LineRenderer left;
    public LineRenderer right;
    public LineRenderer headtarget;
    public TextMeshProUGUI info;

    public UnityEvent OnTarget;
    public UnityEvent OffTarget;

    void Awake()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(CheckTurnback());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    public void OnLine()
    {
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        headtarget.gameObject.SetActive(true);
    }

    public void OffLine()
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        headtarget.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_transform == null) return;
        DrawView();             //Scene뷰에 시야범위 그리기
        FindVisibleTargets();   //Enemy인지 Obstacle인지 판별
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        //좌우 회전값 갱신
        angleInDegrees += _transform.eulerAngles.y;
        //경계 벡터값 반환
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void DrawView()
    {
        Vector3 leftBoundary = DirFromAngle(-ViewAngle / 2);
        Vector3 rightBoundary = DirFromAngle(ViewAngle / 2);
        Debug.DrawLine(_transform.position, _transform.position + leftBoundary * ViewDistance, Color.blue);
        Debug.DrawLine(_transform.position, _transform.position + rightBoundary * ViewDistance, Color.blue);

        left.SetPosition(0, _transform.position);
        left.SetPosition(1, _transform.position + leftBoundary * ViewDistance);

        right.SetPosition(0, _transform.position);
        right.SetPosition(1, _transform.position + rightBoundary * ViewDistance);
    }

    public void FindVisibleTargets()
    {
        //시야거리 내에 존재하는 모든 컬라이더 받아오기
        Collider[] targets = Physics.OverlapSphere(_transform.position, ViewDistance, TargetMask);

        for (int i = 0; i < targets.Length; i++)
        {
            Transform target = targets[i].transform;

            //타겟까지의 단위벡터
            Vector3 dirToTarget = (target.position - _transform.position).normalized;

            //_transform.forward와 dirToTarget은 모두 단위벡터이므로 내적값은 두 벡터가 이루는 각의 Cos값과 같다.
            //내적값이 시야각/2의 Cos값보다 크면 시야에 들어온 것이다.
            if (Vector3.Dot(_transform.forward, dirToTarget) > Mathf.Cos((ViewAngle / 2) * Mathf.Deg2Rad))
            {
                float distToTarget = Vector3.Distance(_transform.position, target.position);

                if (!Physics.Raycast(_transform.position, dirToTarget, distToTarget, ObstacleMask))
                {
                    if(!isOn)
                    {
                        
                        if(OnTarget != null)
                        {
                            OnTarget.Invoke();
                        }
                        isOn = true;
                    }
                    Debug.DrawLine(_transform.position, target.position, Color.red);

                    headtarget.SetPosition(0, _transform.position + (_transform.forward * 0.5f));
                    headtarget.SetPosition(1, target.position);

                    driverWatchTime += Time.deltaTime;
                   // info.text += "/ Watch";
                }
            }
            else
            {
                headtarget.SetPosition(0, _transform.position);
                headtarget.SetPosition(1, _transform.position + (_transform.forward * 0.5f) * ViewDistance);
                driverWatchTime -= Time.deltaTime;

                if (isOn)
                {

                    if (OffTarget != null)
                    {
                        OffTarget.Invoke();
                    }
                    isOn = false;
                }
                // info.text += "/ Watch not";
            }
        }
    }

    public GameObject[] particles;
    IEnumerator CheckTurnback()
    {
        yield return new WaitForSeconds(3f);
        if (!isOn)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].SetActive(true);
            }
        }
    }

}

