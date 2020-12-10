//### Particles Animated Bezier Flow Inspector
//Provides a Unity Editor UI for `ParticlesAnimatedBezierFlowController`
//
//License information: [ASSET STORE TERMS OF SERVICE AND EULA](https://unity3d.com/legal/as_terms)
//
//Developed by [MathArtCode](https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:8738) team, 2016

using System.Linq;
using Assets.Editor.BezierCurvedParticlesFlow.Utilities;
using UnityEditor;
using UnityEngine;

#if !UNITY_3_5
using Assets.Scripts.BezierCurvedParticlesFlow;
#endif

#if !UNITY_3_5
namespace Assets.Editor.BezierCurvedParticlesFlow {
#endif
// ReSharper disable once UnusedMember.Global
    [CustomEditor(typeof (ParticlesAnimatedBezierFlowController))]
    public sealed class ParticlesAnimatedBezierFlowInspector : UnityEditor.Editor {
        private string _clipName = "Animated_Particles_Flow";
        private CurveDrawingHelper _curveDrawingHelperObject;
        private ParticlesAnimatedBezierFlowController _flowController;
        private bool _hasSelectedAnimation;
        private bool _useLagacyAnimations;

        private string _lastAnimationClipName;
        private int _videoLength = 2;
        // A safe ParticlesAnimatedBezierFlowController getter
        private ParticlesAnimatedBezierFlowController FlowController {
            get {
                if (_flowController == null) {
                    _flowController = target as ParticlesAnimatedBezierFlowController;
                }
                return _flowController;
            }
        }

        // A safe CurveDrawingHelper getter
        private CurveDrawingHelper CurveDrawingHelper {
            get {
                if (_curveDrawingHelperObject == null) {
                    _curveDrawingHelperObject =
                        new CurveDrawingHelper(this, FlowController);
                    CurveDrawingHelper.OnChangeInitiated += OnChangeStarted;
                    CurveDrawingHelper.OnChangeCommit += OnChangeCommit;
                }
                return _curveDrawingHelperObject;
            }
        }

        // Draws curve on in Scene window
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private void OnSceneGUI() {
            CurveDrawingHelper.OnSceneGUI();
        }

        // Registers editor action checkpoint
        private void OnChangeStarted(string label) {
#if UNITY_3_5
        Undo.RegisterUndo(FlowController, label);
#else
            Undo.RecordObject(FlowController, label);
#endif
        }

        // Registers commits new checkpoint
        private void OnChangeCommit() {
            EditorUtility.SetDirty(FlowController);
        }

        //Draws inspector interface
        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();
            CurveDrawingHelper.OnInspectorGUI();

#if UNITY_3_5
        EditorGUILayout.HelpBox(
            "You shall set this to the same value \"Particle System->Game Object->Max Particles\" is set. \nThis field is editable only for Unity 3.",
            MessageType.Warning);
        var maxParticles = EditorGUILayout.IntField("Max Particles", FlowController.MaxParticles);
        if(maxParticles != FlowController.MaxParticles) {
            OnChangeStarted("Max Particles count changed");
            FlowController.MaxParticles = maxParticles;
            OnChangeCommit();
        }
#endif
#if !UNITY_3_5
            _clipName = EditorGUILayout.TextField("Clip Name", _clipName);
            _useLagacyAnimations = GUILayout.Toggle(_useLagacyAnimations, "Use Legacy Animation");
#endif
            if (GUILayout.Button("Create Animation Clip")) {
#if !UNITY_3_5
                if (_useLagacyAnimations) {
#endif
                    UseLegacyAnimations();
#if !UNITY_3_5
                } else {
                    var clip = new AnimationClip();
                    FillAnimation(clip);
                    AssetDatabase.CreateAsset(clip, "Assets/" + _clipName + ".anim");
                    AssetDatabase.SaveAssets();
                }
#endif
            }
        }

        private void UseLegacyAnimations() {
            var anim = FlowController.GetComponent<Animation>();

            if (anim == null) {
                Debug.LogError("Legacy Animation Clip was not created/added because ther is no Animation component on " +
                               FlowController.gameObject.name);
                return;
            }


            _videoLength = EditorGUILayout.IntField("Animation Length", _videoLength);

            if (!_hasSelectedAnimation && anim != null && anim.clip != null) {
                _hasSelectedAnimation = true;
                _clipName = anim.clip.name;
            } else if (anim != null && anim.clip == null) {
                _hasSelectedAnimation = false;
            }

            _clipName = EditorGUILayout.TextField("Animation Clip Name", _clipName);

            if (GUILayout.Button("Prepare Animation Clip")) {
#if UNITY_3_5
            Undo.RegisterUndo(anim, "Create Animation Clip");
#else
                Undo.RecordObject(anim, "Create Animation Clip");
#endif
                AnimationClip clip = null;

                var updateAnimationClip = anim.clip != null && anim.clip.name == _clipName;
                clip = updateAnimationClip ? anim.clip : new AnimationClip();
                FillAnimation(clip);

                if (!updateAnimationClip) {
                    anim.AddClip(clip, _clipName);
                }

                EditorUtility.SetDirty(anim);
            }
        }

        private void FillAnimation(AnimationClip clip) {
#if !UNITY_3_5
            clip.legacy = _useLagacyAnimations;
#endif

            FlowController.GetInformers().ForEach( informer => {
                var localPosition = informer.transform.localPosition;
                var path = AnimationUtility.CalculateTransformPath( informer.transform, FlowController.transform );
                clip.SetCurve( path, typeof( Transform ), "localPosition.x",
                    AnimationCurve.Linear( 0, localPosition.x, _videoLength, localPosition.x ) );
                clip.SetCurve( path, typeof( Transform ), "localPosition.y",
                    AnimationCurve.Linear( 0, localPosition.y, _videoLength, localPosition.y ) );
                clip.SetCurve( path, typeof( Transform ), "localPosition.z",
                    AnimationCurve.Linear( 0, localPosition.z, _videoLength, localPosition.z ) );
            } );
        }
    }

#if !UNITY_3_5
}
#endif