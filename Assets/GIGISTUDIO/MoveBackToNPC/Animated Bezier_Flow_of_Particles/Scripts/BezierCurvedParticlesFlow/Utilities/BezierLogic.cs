//### Bezier Logic
//This is an utilety class for all major calculations related to vectorized bezier curve.
//
//License information: [ASSET STORE TERMS OF SERVICE AND EULA](https://unity3d.com/legal/as_terms)
//
//Developed by [MathArtCode](https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:8738) team, 2016

using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BezierCurvedParticlesFlow.Utilities {
    [Serializable]
    public sealed class BezierLogic {
        public delegate void ControllPointsChange(int index);

        public delegate void OnRemoveComplete(int begin, int count);

        [SerializeField]
        private Vector3[] _points;

        //All points are handeled inside a single array 
        //- positioning (end points) have indexes 0, 3, 6, ... 
        //- angles (control points) have indexes 1,2, 4,5, ...
        private Vector3[] Points {
            get {
                if (_points == null) {
                    Reset();
                }
                return _points;
            }
        }

        // Provides ammount of all points (end points + control pouints in AI terminolagy)
        public int ControlPointCount {
            get { return Points.Length; }
        }

        // Provides ammount of all curved segments of Bezier curve
        private int CurveCount {
            get { return (Points.Length - 1)/3; }
        }

        // Called on each controll point creation, first is called for main CP, then for angle related controll points
        public event ControllPointsChange OnAdded;
        //Called on CP move
        //
        // Note: one event event is sent per controll point, meaning there are no events for angle points when main CP is moved
        public event ControllPointsChange OnMoved;
        // Called on each controll point removal, first is called for angles and then for main controll point
        public event ControllPointsChange OnRemoved;
        // Sends a complete range of removed items
        public event OnRemoveComplete OnRemoveCompleted;

        //### Mathematical core of Bezier Logic

        private static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            t = Mathf.Clamp01(t);
            var oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * oneMinusT * p0 +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * p3;
        }

        private static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            t = Mathf.Clamp01(t);
            var oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }

        //### Main Bezier Logic

        // Returns a coordinate point relative to curve 'time' where 0 is bezier curve beginning and 1 is end of the bezier curve (length vise)
        public Vector3 GetPoint(float t) {
            if (_points.Length < 4) {
                return Vector3.one;
            }
            int i;
            if (t >= 1f) {
                t = 1f;
                i = _points.Length - 4;
            } else {
                t = Mathf.Clamp01(t)*CurveCount;
                i = (int) t;
                t -= i;
                i *= 3;
            }
            return GetPoint(_points[i], _points[i + 1], _points[i + 2], _points[i + 3], t);
        }

        // Returns a velocity point relative to curve 'time' where 0 is bezier curve beginning and 1 is end of the bezier curve (length vise)
        public Vector3 GetVelocity(float t) {
            if (_points.Length < 4) {
                return Vector3.one;
            }
            int i;
            if (t >= 1f) {
                t = 1f;
                i = _points.Length - 4;
            } else {
                t = Mathf.Clamp01(t)*CurveCount;
                i = (int) t;
                t -= i;
                i *= 3;
            }
            return
                GetFirstDerivative(_points[i], _points[i + 1], _points[i + 2], _points[i + 3], t);
        }

        // Returns a normalized velocity point relative to curve 'time' where 0 is bezier curve beginning and 1 is end of the bezier curve (length vise)
        public Vector3 GetDirection(float t) {
            return GetVelocity(t).normalized;
        }

        public void CleanUp() {
            if (_points != null) {
                for (var i = 0; i < _points.Length; i += 3) {
                    RemovePoint(i, false);
                }
                if (OnRemoveCompleted != null) {
                    OnRemoveCompleted(0, _points.Length);
                }
            }
        }

        //Resets component to its default 2 points (forming a straight line)
        public void Reset() {
            CleanUp();

            _points = new[] {
                new Vector3(0f, 0f, 0f),
                new Vector3(2f, 0f, 0f),
                new Vector3(3f, 0f, 0f),
                new Vector3(4f, 0f, 0f)
            };

            if (OnAdded != null) {
                OnAdded(0);
                OnAdded(1);
                OnAdded(3);
                OnAdded(2);
            }
        }

        //Returns a point by its Index
        //- positioning (end points) have indexes 0, 3, 6, ... 
        //- angles (control points) have indexes 1,2, 4,5, ...
        public Vector3 GetControlPoint(int index) {
            if (Points.Length <= index) {
                throw new IndexOutOfRangeException();
            }

            return Points[index];
        }

        //Sets a point value by its Index
        //- positioning (end points) have indexes 0, 3, 6 ... 
        //- angles (control points) have indexes 1,2, 4,5, ...
        public void SetControlPoint(int index, Vector3 point, bool fireEvent = true) {
            var delta = point - Points[index];

            if (index%3 == 0) {
                if (index > 0) {
                    var cpIdxBack = index - 1;
                    _points[cpIdxBack] += delta;
                }
                var cpIdxFront = index + 1;
                if (cpIdxFront < _points.Length) {
                    _points[cpIdxFront] += delta;
                }
            }

            Points[index] = point;
            if (fireEvent && OnMoved != null) {
                OnMoved(index);
            }
        }

        //Unity Editor Undo clears event handlers
        public bool HasPointEventHandlers() {
            var result = OnAdded != null && OnMoved != null && OnRemoved != null && OnRemoveCompleted != null;
            return result;
        }

        // Adds a position point + 2 angle points
        public void AddPoint() {
            var length = Points.Length;
            var point = _points[length - 1];
            var velocity = GetDirection(1f);
            Array.Resize(ref _points, length + 3);
            length = Points.Length;

            point += velocity;
            _points[length - 3] = point;
            point += velocity;
            _points[length - 2] = point;
            point += velocity;
            _points[length - 1] = point;

            if (OnAdded != null) {
                OnAdded(length - 3);
                OnAdded(length - 1);
                OnAdded(length - 2);
            }
        }

        // Removes a position point + 2 angle points. gets an end point Index as an argument
        //- positioning (end points) have indexes 0, 3, 6... 
        public void RemovePoint(int selectedIndex, bool doRemoval) {
            var points = Points.ToList();
            var atEnd = false;
            var atStart = false;
            var removeRangeStart = selectedIndex;
            var removeRangeCount = 3;

            if (selectedIndex == 0) {
                atStart = true;
                removeRangeCount = 2;
            } else if (selectedIndex == _points.Length - 1) {
                atEnd = true;
                removeRangeStart -= 1;
                removeRangeCount = 2;
            } else {
                removeRangeStart = selectedIndex - 1;
            }

            if ((removeRangeStart < 0) || ((removeRangeStart + removeRangeCount) > _points.Length)) {
                return; // Undo specifics
            }

            if (doRemoval) {
                points.RemoveRange(removeRangeStart, removeRangeCount);
                _points = points.ToArray();
            }

            if (OnRemoved != null) {
                if (atStart) {
                    OnRemoved(selectedIndex + 1);
                } else if (atEnd) {
                    OnRemoved(selectedIndex - 1);
                } else {
                    OnRemoved(selectedIndex - 1);
                    OnRemoved(selectedIndex + 1);
                }
                OnRemoved(selectedIndex);
            }

            if ((OnRemoveCompleted != null) && doRemoval) {
                OnRemoveCompleted(removeRangeStart, removeRangeCount);
            }
        }
    }
}