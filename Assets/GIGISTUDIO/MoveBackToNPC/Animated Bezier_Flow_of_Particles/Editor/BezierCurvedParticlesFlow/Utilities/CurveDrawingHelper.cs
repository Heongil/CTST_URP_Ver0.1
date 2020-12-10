//### Curve Drawing Helper
//This is an utilety class for all major calculations related to vectorized bezier curve scene UI representation.
//
//License information: [ASSET STORE TERMS OF SERVICE AND EULA](https://unity3d.com/legal/as_terms)
//
//Developed by [MathArtCode](https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:8738) team, 2016

using Assets.Scripts.BezierCurvedParticlesFlow;

using UnityEditor;
using UnityEngine;

namespace Assets.Editor.BezierCurvedParticlesFlow.Utilities {
    public class CurveDrawingHelper {
        public delegate void ChangeChangeStart(string label);

        //### Provides events for handling interactions
        public delegate void ChangeCommit();

        //### UI style settings
        private const float HandleSize = 0.04f;
        private const float PickSize = 0.06f;

        private readonly ParticlesAnimatedBezierFlowController _flowController;
        private readonly Transform _handleTransform;

        //### Currently selected handle
        private Quaternion _handleRotation;

        public int SelectedIndex = -1;

        public event ChangeChangeStart OnChangeInitiated;
        public event ChangeCommit OnChangeCommit;

        //### Functions

        //Draws a dinamically movable UnityEditor point
        private Vector3 ShowPoint(int index) {
            var point = _handleTransform.TransformPoint(_flowController.BezierLogic.GetControlPoint(index));
            var size = HandleUtility.GetHandleSize(point);
            if (index == 0) {
                size *= 2f;
            }
            Handles.color = Color.cyan;

            if (Handles.Button(point, _handleRotation, size*HandleSize, size*PickSize, Handles.DotCap)) {
                SelectedIndex = index;
                var informer = _flowController.GetInformer(SelectedIndex);
                if (informer != null) {
                    Selection.activeGameObject = informer.gameObject;
                } else {
                    Debug.LogError(string.Format(
                        "Particles Animated Bezier Flow Controller Informers are currupt! {0}", SelectedIndex));
                }
                //   _client.Repaint();
            }
            if (SelectedIndex == index) {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, _handleRotation);
                if (EditorGUI.EndChangeCheck()) {
                    if (OnChangeInitiated != null) {
                        OnChangeInitiated("Move Point");
                    }
                    _flowController.BezierLogic.SetControlPoint(index, _handleTransform.InverseTransformPoint(point));
                    if (OnChangeCommit != null) {
                        OnChangeCommit();
                    }
                }
            }
            return point;
        }

        //Draws currently selected curve point Editor Inspector UI
        private void DrawSelectedPointInspector() {
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();
            var bezierLogic = _flowController.BezierLogic;

            var point = EditorGUILayout.Vector3Field("Position", bezierLogic.GetControlPoint(SelectedIndex));
            if (EditorGUI.EndChangeCheck()) {
                OnChangeInitiated("Move Point");
                bezierLogic.SetControlPoint(SelectedIndex, point);
                OnChangeCommit();
            }
            EditorGUI.BeginChangeCheck();
        }

        // Draws a bezier curve in Scene space
        // ReSharper disable once InconsistentNaming
        public void OnSceneGUI() {
            if(Tools.pivotMode != PivotMode.Pivot) {
                Tools.pivotMode = PivotMode.Pivot;
            }

            _handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? _handleTransform.rotation
                : Quaternion.identity;

            var p0 = ShowPoint(0);
            for(var i = 1; i < _flowController.BezierLogic.ControlPointCount; i += 3) {
                var p1 = ShowPoint(i);
                var p2 = ShowPoint(i + 1);
                var p3 = ShowPoint(i + 2);

                Handles.color = Color.gray;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);

                Handles.color = Color.yellow;
                Handles.ArrowCap(0, p0, Quaternion.LookRotation(p3 - p0), 0.25f);
#if !UNITY_3_5
                Handles.DrawBezier( p0, p3, p1, p2, Color.white, null, 1f );
#endif
                p0 = p3;
            }

#if UNITY_3_5
            var rasterize = 250;
            Handles.color = Color.gray;
            for(var i = 0; i < rasterize - 1; i++) {
                var pn = _handleTransform.TransformPoint(_flowController.BezierLogic.GetPoint(i / (float)rasterize));
                var pk = _handleTransform.TransformPoint(_flowController.BezierLogic.GetPoint((i + 1) / (float)rasterize));
                Handles.DrawLine(pn, pk);
            }
#endif

            var e = Event.current;
            switch(e.type) {
                case EventType.KeyDown:
                    {
                        if(Event.current.keyCode == (KeyCode.Delete) || Event.current.keyCode == (KeyCode.Backspace)) {
                            if(SelectedIndex % 3 == 0 && SelectedIndex > 3 &&
                                SelectedIndex < _flowController.BezierLogic.ControlPointCount) {
                                e.Use();
                                if(OnChangeInitiated != null) {
                                    OnChangeInitiated("Remove Point");
                                }
                                _flowController.BezierLogic.RemovePoint(SelectedIndex, true);
                                if(OnChangeCommit != null) {
                                    OnChangeCommit();
                                }
                            }
                        }

                        break;
                    }
            }
        }

        public void OnInspectorGUI() {
            if(SelectedIndex >= 0 && SelectedIndex < _flowController.BezierLogic.ControlPointCount) {
                DrawSelectedPointInspector();
            }

            if(GUILayout.Button("Add Point")) {
                OnChangeInitiated("Add Point");
                _flowController.BezierLogic.AddPoint();
                OnChangeCommit();
            }

            var usePositions = GUILayout.Toggle(_flowController.UsePositions, "Use Positions");
            if(usePositions != _flowController.UsePositions) {
                OnChangeInitiated("Use Positions changed");
                _flowController.UsePositions = usePositions;
                OnChangeCommit();
            }

            var useVelocities = GUILayout.Toggle(_flowController.UseVelocities, "Use Velocities");
            if(useVelocities != _flowController.UseVelocities) {
                OnChangeInitiated("Use Velocities changed");
                _flowController.UseVelocities = useVelocities;
                OnChangeCommit();
            }
        }

        public CurveDrawingHelper(UnityEditor.Editor uiClient, ParticlesAnimatedBezierFlowController flowController) {
            _flowController = flowController;
            _handleTransform = _flowController.transform;
        }
    }
}