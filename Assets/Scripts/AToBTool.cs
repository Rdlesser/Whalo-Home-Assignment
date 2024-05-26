using System;
using System.Threading.Tasks;
#if UNITY_EDITOR
using Cysharp.Threading.Tasks;
// using DG.DOTweenEditor;
#endif
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts {
    
    [ExecuteInEditMode]
    public class AToBTool : MonoBehaviour {
        
        [Header("Base Attributes")]
        [SerializeField] private bool _playOnEnable;
        [SerializeField] private float _animationTime = 2f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Transform _startPosition;
        [SerializeField] private Transform _endPosition;
        
        [Header("Position Attributes")]
        [SerializeField] private bool _isPositionAnimated;
        [SerializeField] private AnimationSource _positionAnimationSource = AnimationSource.Ease;
        [SerializeField] private Ease _xPositionEase;
        [SerializeField] private Ease _yPositionEase;
        [SerializeField] private AnimationCurve _xPositionCurve;
        [SerializeField] private AnimationCurve _yPositionCurve;

        [Header("Scale Attributes")]
        [SerializeField] private bool _isScaleAnimated;
        [SerializeField] private AnimationSource _scaleAnimationSource;
        [SerializeField] private Vector2 _scaleStartValue;
        [SerializeField] private Vector2 _scaleEndValue;
        [SerializeField] private Ease _scaleEase;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Rotation Attributes")]
        [SerializeField] private bool _isRotationAnimated;
        [SerializeField] private AnimationSource _rotationAnimationSource;
        [SerializeField] private Vector3 _rotationStartValue;
        [SerializeField] private Vector3 _rotationEndValue;
        [SerializeField] private Ease _rotationEase;
        [SerializeField] private AnimationCurve _rotationCurve;
        
        [Space(10)]
        [SerializeField] private UnityEvent _onAnimationComplete;
        
        public Action OnMoveStarted;
        public Action OnMoveComplete;

        private Sequence _tweenSequence;
        
        private Vector3? _originalPosition;
        private Vector3? _originalScale;
        private Quaternion? _originalRotation;

        private void OnEnable() {
            
            if (_playOnEnable) {
                
                Play();
            }
        }
        
        [ContextMenu("Play")]
        public async Task Play() {

            CacheOriginalValues();
            HandleSequenceReset();
            _tweenSequence = DOTween.Sequence().SetTarget(transform);

            if (!IsPreconditionAvailable()) {
                
                Debug.LogError("A to B script must have a start and end position");
                return;
            }
            
            var targetPosition = _endPosition.position;

            AddMoveSequence(targetPosition, ref _tweenSequence);
            AddScaleSequence(ref _tweenSequence);
            AddRotationSequence(ref _tweenSequence);
            
            _tweenSequence.PrependInterval(_delay);
            _tweenSequence.OnStart(MoveStarted);
            _tweenSequence.OnComplete(MoveComplete);
            _tweenSequence.Play();
            
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                // DOTweenEditorPreview.PrepareTweenForPreview(_tweenSequence);
                // DOTweenEditorPreview.Start();
                var duration = _tweenSequence.Duration();
                // await UniTask.Delay((1.1f * duration).SecondsToMilliseconds());
                Reset();
            }
#endif
        }

        private void CacheOriginalValues() {

            var originalTransform = transform;
            _originalPosition = originalTransform.position;
            _originalScale = originalTransform.localScale;
            _originalRotation = originalTransform.rotation;
        }

        private bool IsPreconditionAvailable() {

            return _startPosition != null && _endPosition != null;
        }

        private void HandleSequenceReset() {
            
            _tweenSequence?.Kill(true);
            _tweenSequence = null;
        }

        private void AddMoveSequence(Vector3 targetPosition, ref Sequence moveSequence) {

            if (!_isPositionAnimated) {
                return;
            }
            
            var startingPosition = _startPosition.position;

            var moveXSequence = transform.DOMoveX(targetPosition.x, _animationTime).
                From(startingPosition.x);
            var moveYSequence = transform.DOMoveY(targetPosition.y, _animationTime).
                From(startingPosition.y);

            switch (_positionAnimationSource) {

                case AnimationSource.Ease:

                    moveXSequence.SetEase(_xPositionEase);
                    moveYSequence.SetEase(_yPositionEase);

                    break;

                case AnimationSource.Curve:

                    moveXSequence.SetEase(_xPositionCurve);
                    moveYSequence.SetEase(_yPositionCurve);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            moveSequence.Insert(0, moveXSequence);
            moveSequence.Insert(0, moveYSequence);
        }

        private void AddScaleSequence(ref Sequence tweenSequence) {

            if (!_isScaleAnimated) {
                return;
            }
            
            var scaleSequence = transform.DOScale(_scaleEndValue, _animationTime).
                From(_scaleStartValue);

            switch (_scaleAnimationSource) {

                case AnimationSource.Ease:
                    scaleSequence.SetEase(_scaleEase);

                    break;

                case AnimationSource.Curve:
                    scaleSequence.SetEase(_scaleCurve);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            tweenSequence.Insert(0, scaleSequence);
        }

        private void AddRotationSequence(ref Sequence tweenSequence) {

            if (!_isRotationAnimated) {
                return;
            }

            _originalRotation = transform.rotation;
            var rotationSequence = transform.DORotate(_rotationEndValue, _animationTime).
                From(_rotationStartValue);

            switch (_rotationAnimationSource) {

                case AnimationSource.Ease:
                    rotationSequence.SetEase(_rotationEase);

                    break;

                case AnimationSource.Curve:
                    rotationSequence.SetEase(_rotationCurve);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            tweenSequence.Insert(0, rotationSequence);
        }

        private void MoveStarted() {
            
            OnMoveStarted?.Invoke();
        }

        private void MoveComplete() {
            
            OnMoveComplete?.Invoke();
            _onAnimationComplete?.Invoke();
        }

        [ContextMenu("Reset")]
        public void Reset() {

#if UNITY_EDITOR
            // DOTweenEditorPreview.Stop(true);
#endif
            ResetPosition();
            ResetScale();
            ResetRotation();
        }

        private void ResetPosition() {

            if (_originalPosition == null) {
                
                return;
            }

            transform.position = (Vector3) _originalPosition;
        }

        private void ResetScale() {

            if (_originalScale == null) {
                
                return;
            }

            transform.localScale = (Vector3) _originalScale;
        }

        private void ResetRotation() {

            if (_originalRotation == null) {
                
                return;
            }

            transform.rotation = (Quaternion) _originalRotation;
        }

        private void OnDisable() {
            
            _tweenSequence?.Kill();
        }

    }
}

[Serializable]
public enum AnimationSource {

    Ease,
    Curve

}

#if UNITY_EDITOR
[CanEditMultipleObjects]
// [CustomEditor(typeof(AToBTool))]
public class AToBToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Preview", GUILayout.Width(100)))
        {
            // var tar = target as AToBTool;
            // tar.Play();
        }

        if (GUILayout.Button("Reset", GUILayout.Width(100))) {

            // var tar = target as AToBTool;
            // tar.Reset();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
#endif