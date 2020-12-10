//### Bezier Curve Point Inspector
//Provides a Unity Editor UI for `BezierCurvePointInformer`
//
//License information: [ASSET STORE TERMS OF SERVICE AND EULA](https://unity3d.com/legal/as_terms)
//
//Developed by [MathArtCode](https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:8738) team, 2016

using Assets.Scripts.BezierCurvedParticlesFlow.Utilities;
using UnityEditor;

#if !UNITY_3_5
namespace Assets.Editor.BezierCurvedParticlesFlow.Utilities {
#endif

[CustomEditor(typeof (BezierCurvePointInformer))]
class BezierCurvePointInspector : UnityEditor.Editor {
    private CurveDrawingHelper _drawer;
    private BezierCurvePointInformer _informer;

    private BezierCurvePointInformer informer {
        get {
            if (_informer == null) {
                _informer = target as BezierCurvePointInformer;
            }
            return _informer;
        }
    }

    private CurveDrawingHelper Drawer {
        get {
            if (_drawer == null) {
                _drawer = new CurveDrawingHelper(this, informer.Controller);
                _drawer.OnChangeInitiated += OnChangeStarted;
                _drawer.OnChangeCommit += OnChangeCommit;
            }
            return _drawer;
        }
    }

    private void OnSceneGUI() {
        Drawer.OnSceneGUI();
    }

    private void OnChangeStarted(string label) {
#if UNITY_3_5
        Undo.RegisterUndo(informer.Controller, label);
#else
            Undo.RecordObject( informer.Controller, label);
#endif
    }

    // Registers commits new checkpoint
    private void OnChangeCommit() {
        EditorUtility.SetDirty(informer.Controller);
    }

    public override void OnInspectorGUI() {
        if(Drawer.SelectedIndex != informer.Index) {
            Drawer.SelectedIndex = informer.Index;
        }
        Drawer.OnInspectorGUI();
    }
}

#if !UNITY_3_5
}
#endif