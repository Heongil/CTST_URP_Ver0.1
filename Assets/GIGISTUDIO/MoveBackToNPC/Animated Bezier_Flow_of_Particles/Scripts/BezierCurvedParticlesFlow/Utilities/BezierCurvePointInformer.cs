//### Bezier Curve Point Informer
//This bahaviour is an informer that tells `ParticlesAnimatedBezierFlowController` mounted on some parent about bezier curve controll point position
//
//License information: [ASSET STORE TERMS OF SERVICE AND EULA](https://unity3d.com/legal/as_terms)
//
//Developed by [MathArtCode](https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:8738) team, 2016

using System;
using UnityEngine;

#if UNITY_EDITOR
#endif

#if !UNITY_3_5
namespace Assets.Scripts.BezierCurvedParticlesFlow.Utilities {
#endif
//A Control Point proxy object that is animatable inside unity editor animator
//Must be on a child of a GameObject containing `ParticlesAnimatedBezierFlowController`
[ExecuteInEditMode]
public sealed class BezierCurvePointInformer : MonoBehaviour {
    private Vector3 _lastPosition;

    [SerializeField]
    public ParticlesAnimatedBezierFlowController Controller;

    public bool? HasToBeDestroyed;

    [SerializeField]
    public int Index;

    // Correlates positioning of current element with hast `ParticlesAnimatedBezierFlowController`
    private void UpdatePosition() {
        var position = Controller.transform.InverseTransformPoint(transform.position);
        try {
            if (Controller.BezierLogic.GetControlPoint(Index) != position) {
                Controller.BezierLogic.SetControlPoint(Index, position, false);
            }
        } catch (IndexOutOfRangeException) {}

        _lastPosition = transform.localPosition;
    }

    private void InitializeIfNeeded() {
        if(Controller != null) {
            var parent = transform.parent;
            while(parent != null) {
                var component = parent.GetComponent<ParticlesAnimatedBezierFlowController>();
                if(component != null) {
                    Controller = component;
                    break;
                }
                parent = parent.transform.parent;
            }
            if(Controller == null) {
                Debug.LogError("Could not find a ParticlesAnimatedBezierFlowController in object parents");
            } else {
                Controller.RegisterInformer(Index, this);
                UpdatePosition();
            }
        }
    }

    // ReSharper disable once UnusedMember.Local
    private void Start() {
        Awake();
    }

    // Registers Object in ParticlesAnimatedBezierFlowController
    private void Awake() {
        InitializeIfNeeded();
        _lastPosition = transform.localPosition;
        HasToBeDestroyed = null;
    }

    // ReSharper disable once UnusedMember.Local
    private void Update() {
        InitializeIfNeeded();
        if (transform.localPosition != _lastPosition) {
            UpdatePosition();
        }
    }

    // ReSharper disable once UnusedMember.Local
    private void OnDestroy() {
        InitializeIfNeeded();
        if (HasToBeDestroyed == null) {
            HasToBeDestroyed = false;
        }
#if UNITY_EDITOR
        if ((Event.current != null) && (!Event.current.isMouse) && (Event.current.commandName == "SoftDelete"))
            // Project Open Scene == false, Game Object delete key == true 
        {
#endif
            if (!HasToBeDestroyed.Value) {
                if (Index == 0 || Index == Controller.BezierLogic.ControlPointCount - 1) {
                    Debug.LogError("One shall not remove Bezier curve End Points! Please Undo!", Controller);
                } else if (Index%3 != 0) {
                    Debug.LogError("One shall not remove Bezier curve Angle Points! Please Undo!", Controller);
                }
            }

            if (Index%3 == 0 && !HasToBeDestroyed.Value) {
                Controller.BezierLogic.RemovePoint(Index, true);
            }
#if UNITY_EDITOR
        }
#endif
    }

}

#if !UNITY_3_5
}
#endif