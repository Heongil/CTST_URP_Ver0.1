//### Particles Animated Bezier Flow Controller
//This is a component that controlls behaviour of particle system mounted to the same game object it is.
//
//It controlls only particles path, it does not controll any other aspect ot their behaviour or loock.
//
//License information: [ASSET STORE TERMS OF SERVICE AND EULA](https://unity3d.com/legal/as_terms)
//
//Developed by [MathArtCode](https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:8738) team, 2016

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.BezierCurvedParticlesFlow.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

#if !UNITY_3_5
namespace Assets.Scripts.BezierCurvedParticlesFlow {
#endif

[AddComponentMenu("Effects/MathArtCode/Animated Bezier Flow of Particles")]
[ExecuteInEditMode]
[RequireComponent(typeof (ParticleSystem))]
#if UNITY_3_5
[RequireComponent(typeof (Animation))]
#endif
public sealed class ParticlesAnimatedBezierFlowController : MonoBehaviour {
#if !UNITY_3_5
    [SerializeField]
    private bool _useLegacyAnimation = true;
#endif

    [SerializeField]
    private BezierLogic _bezierLogicLogic;

    private ParticleSystem.Particle[] _particles;

    private Dictionary<int, BezierCurvePointInformer> _informers = new Dictionary<int, BezierCurvePointInformer>();

    private ParticleSystem _system;

    [SerializeField]
    private bool _usePositions;

    [SerializeField]
    private bool _useVelocities;

#if UNITY_3_5
    [SerializeField]
    private int _maxParticles;

    //Unity3.5 ParticleSystem.MaxParticles
    public int MaxParticles {
        get { return _maxParticles; }
        set {
            _maxParticles = value;
            _particles = new ParticleSystem.Particle[_maxParticles];
        }
    }
#endif



        //Defines if pixel perfect positions as they are defined in bezier curve shall be used (which makes all particles follow one curve).
        public bool UsePositions {
        get { return _usePositions; }
        set { _usePositions = value; }
    }

    //Defines if each particle shall set its velocity relative to its life time (allowing you to use particle emmiter shape as flow form).
    public bool UseVelocities {
        get { return _useVelocities; }
        set { _useVelocities = value; }
    }

    //Hosts main curve related logic
    public BezierLogic BezierLogic {
        get {
            if (_bezierLogicLogic == null) {
                _bezierLogicLogic = new BezierLogic();
            }

            if (!_bezierLogicLogic.HasPointEventHandlers()) {
                _bezierLogicLogic.OnAdded += idx => {
                    var pointName = "";
                    var pointId = idx%3;
                    var isAngle = true;
                    var angleHost = idx;
                    if (pointId == 0) {
                        pointName = "control_point_" + angleHost/3;
                        isAngle = false;
                    } else if (pointId == 1) {
                        angleHost--;
                        pointName = "angle_out_" + angleHost/3;
                    } else if (pointId == 2) {
                        angleHost++;
                        pointName = "angle_in_" + angleHost/3;
                    }
                    var o = new GameObject {name = pointName};
#if UNITY_EDITOR
                    Undo.RegisterCreatedObjectUndo(o, pointName);
#endif
                    if (!isAngle) {
                        o.transform.position = transform.TransformPoint(BezierLogic.GetControlPoint(idx));
                        o.transform.parent = transform;
                    } else {
                        o.transform.position = transform.TransformPoint(BezierLogic.GetControlPoint(idx));
                        o.transform.parent = GetInformer(angleHost).transform;
                    }
#if UNITY_EDITOR
#if UNITY_3_5
                    Undo.RegisterSetTransformParentUndo(o.transform, o.transform.parent.transform, o.name);
#else
                    Undo.SetTransformParent(o.transform, o.transform.parent.transform, o.name);
#endif
#endif

                    var informer = o.AddComponent<BezierCurvePointInformer>();
                    informer.Controller = this;
                    informer.Index = idx;
                    _informers.Add(idx, informer);
#if UNITY_EDITOR
                    EditorUtility.SetDirty(informer.Controller);
#endif
                };

                _bezierLogicLogic.OnRemoved += idx => {
                    var bezierCurvePointInformer = GetInformer(idx);
                    if ((bezierCurvePointInformer != null) && (bezierCurvePointInformer.HasToBeDestroyed == null)) {
                        bezierCurvePointInformer.HasToBeDestroyed = true;
#if UNITY_EDITOR
                        DestroyImmediate(bezierCurvePointInformer.gameObject);
#else
                        Destroy(bezierCurvePointInformer.gameObject);
#endif
                    }
                };

                _bezierLogicLogic.OnMoved +=
                    idx => {
                        GetInformer(idx).transform.position = transform.TransformPoint(BezierLogic.GetControlPoint(idx));
                    };

                _bezierLogicLogic.OnRemoveCompleted += (start, count) => {
                    var range = Enumerable.Range(start, count);
                    var fixedInformers = _informers
                        .Where(pair => pair.Key > start && !range.Contains(pair.Key))
                        .ToDictionary(pair => pair.Key - count, pair => {
                            if (pair.Value != null) {
                                pair.Value.Index -= count;
                                var originalName = pair.Value.gameObject.name;

                                var id = Regex.Match(originalName, "(\\d+)(?!.*\\d)");
                                if (id.Success) {
                                    pair.Value.gameObject.name = Regex.Replace(originalName, "(\\d+)(?!.*\\d)",
                                        (int.Parse(id.Value) - 1).ToString());
                                }
                            }
                            return pair.Value;
                        });

                    _informers = _informers.Where(pair => pair.Key < start)
                        .Concat(fixedInformers)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
                };
            }
            return _bezierLogicLogic;
        }
    }

    //Allows to get a position which shall be taken by particle relative to its life time.
    private Vector3 GetPoint(float t) {
        return BezierLogic.GetPoint(t);
    }

    //Allows to get a velocity which shall be taken by particle relative to its life time.
    private Vector3 GetVelocity(float t) {
        return BezierLogic.GetVelocity(t);
    }

    //Gets GameObject particle system
    private void InitializeIfNeeded() {
        if(_system == null) {
            _system = GetComponent<ParticleSystem>();
        }

        if(_particles == null) {
#if !UNITY_3_5
                _particles = new ParticleSystem.Particle[_system.maxParticles];
#else
            _particles = new ParticleSystem.Particle[_maxParticles];
#endif
            _system.GetParticles(_particles);
        }
    }

    //Resets component to its default values
    // ReSharper disable once UnusedMember.Local
    private void Reset() {
#if UNITY_3_5
        _maxParticles = 150;
#endif
        BezierLogic.Reset();

        _usePositions = true;
        _usePositions = true;
    }

    // ReSharper disable once UnusedMember.Local
    private void Start() {
        Awake();
    }

    private void Awake() {
        _informers = new Dictionary<int, BezierCurvePointInformer>();
    }

    //Updates `ParticleSystem` particles setting positions and velocities if needed
    // ReSharper disable once UnusedMember.Local
    private void LateUpdate() {
        InitializeIfNeeded();

        var numParticlesAlive = _system.GetParticles(_particles);

        for (var i = 0; i < numParticlesAlive; i++) {
            var p = _particles[i];
            var t = (p.startLifetime - p.remainingLifetime)/p.startLifetime;
            if (_useVelocities) {
                _particles[i].velocity = GetVelocity(t);
            }

            if (_usePositions) {
                _particles[i].position = GetPoint(t);
            }
        }

        _system.SetParticles(_particles, numParticlesAlive);
    }

    // ReSharper disable once UnusedMember.Local
    private void OnDestroy() {
        BezierLogic.CleanUp();
    }

    // Registers already existing controll point informer
    public void RegisterInformer(int idx, BezierCurvePointInformer informer) {
        if(_informers.ContainsKey(idx)) {
            _informers[idx] = informer; // All objects get recreated on GO Destruction Undo
        } else {
            _informers.Add(idx, informer);
        }
    }

    // Returns an BezierCurvePointInformer that correlates to given index, can be null.
    public BezierCurvePointInformer GetInformer(int idx) {
        BezierCurvePointInformer informer;
        _informers.TryGetValue(idx, out informer);
        return informer;
    }

    public List<BezierCurvePointInformer> GetInformers() {
        return _informers.Values.ToList();
    }

}

#if !UNITY_3_5
}
#endif