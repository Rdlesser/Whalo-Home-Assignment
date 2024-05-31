using System;
using System.Threading.Tasks;
using DG.DOTweenEditor;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[ExecuteInEditMode]
public class AToBTool : MonoBehaviour {

    [SerializeField] private bool _playOnEnable;
    [SerializeField] private float _animationTime = 2f;
    [SerializeField] private float _delay = 0f;

    [SerializeField] private bool _isPositionAnimated;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private AnimationSource _positionMultiplicativeSource = AnimationSource.None;
    [SerializeField] private Ease _xPositionEase;
    [SerializeField] private Ease _yPositionEase;
    [SerializeField] private AnimationCurve _xPositionCurve;
    [SerializeField] private AnimationCurve _yPositionCurve;
    [SerializeField] private bool _useAdditive;
    [SerializeField] private AnimationCurve _additiveXAnimation = AnimationCurve.Linear(0, 0, 0, 0);
    [SerializeField] private AnimationCurve _additiveYAnimation = AnimationCurve.Linear(0, 0, 0, 0);

    [SerializeField] private bool _isScaleAnimated;
    [SerializeField] private AnimationSource _scaleAnimationSource = AnimationSource.None;
    [SerializeField] private Vector2 _scaleStartValue;
    [SerializeField] private Vector2 _scaleEndValue;
    [SerializeField] private Ease _scaleEase;
    [SerializeField] private AnimationCurve _scaleCurve;

    [SerializeField] private bool _isRotationAnimated;
    [SerializeField] private AnimationSource _rotationAnimationSource = AnimationSource.None;
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

        if(_playOnEnable) {
            Play();
        }
    }

    public void ConfigureStartAndEndPosition(Transform start, Transform end) {

        _startPosition = start;
        _endPosition = end;
    }

    [ContextMenu("Play")]
    public async Task Play() {

        CacheOriginalValues();
        HandleSequenceReset();
        _tweenSequence = DOTween.Sequence().SetTarget(transform);

        var targetPosition = _endPosition.position;

        AddMoveSequence(targetPosition, ref _tweenSequence);
        AddScaleSequence(ref _tweenSequence);
        AddRotationSequence(ref _tweenSequence);

        _tweenSequence.PrependInterval(_delay);
        _tweenSequence.OnStart(MoveStarted);
        _tweenSequence.OnComplete(MoveComplete);
        _tweenSequence.Play();

#if UNITY_EDITOR
        if(!Application.isPlaying) {
            DOTweenEditorPreview.PrepareTweenForPreview(_tweenSequence);
            DOTweenEditorPreview.Start();
            var duration = _tweenSequence.Duration();
            await Task.Delay((int)(1.1f * duration * 1000));
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

        if(!_isPositionAnimated) {
            return;
        }

        var startingPosition = _startPosition.position;
        var endPosition = _endPosition.position;

        Tweener moveXSequence;
        Tweener moveYSequence;

        DOSetter<float> xSetter;
        DOSetter<float> ySetter;

        float xt = 0;
        float yt = 0;

        if(_useAdditive) {
            xSetter = (v) => transform.position = new Vector3(v + _additiveXAnimation.Evaluate(xt), transform.position.y, transform.position.z);
            ySetter = (v) => transform.position = new Vector3(transform.position.x, v + _additiveYAnimation.Evaluate(yt), transform.position.z);
        } else {
            xSetter = (v) => transform.position = new Vector3(v, transform.position.y, transform.position.z);
            ySetter = (v) => transform.position = new Vector3(transform.position.x, v, transform.position.z);
        }

        moveXSequence = DOTween.To(() => transform.position.x, xSetter, endPosition.x, _animationTime).From(startingPosition.x);
        moveYSequence = DOTween.To(() => transform.position.y, ySetter, endPosition.y, _animationTime).From(startingPosition.y);

        if(_useAdditive) {
            moveXSequence.OnUpdate(() => xt = moveXSequence.position / _animationTime);
            moveYSequence.OnUpdate(() => yt = moveYSequence.position / _animationTime);
        }

        switch(_positionMultiplicativeSource) {

            case AnimationSource.Ease:
                moveXSequence.SetEase(_xPositionEase);
                moveYSequence.SetEase(_yPositionEase);
                break;

            case AnimationSource.Curve:
                moveXSequence.SetEase(_xPositionCurve);
                moveYSequence.SetEase(_yPositionCurve);
                break;

            default:
                moveXSequence.SetEase(Ease.Linear);
                break;
        }

        moveSequence.Insert(0, moveXSequence);
        moveSequence.Insert(0, moveYSequence);

    }

    private void AddScaleSequence(ref Sequence tweenSequence) {

        if(!_isScaleAnimated) {
            return;
        }

        var scaleSequence = transform.DOScale(_scaleEndValue, _animationTime).
            From(_scaleStartValue);

        switch(_scaleAnimationSource) {

            case AnimationSource.Ease:
                scaleSequence.SetEase(_scaleEase);
                break;

            case AnimationSource.Curve:
                scaleSequence.SetEase(_scaleCurve);
                break;

            default:
                scaleSequence.SetEase(Ease.Linear);
                break;
        }

        tweenSequence.Insert(0, scaleSequence);
    }

    private void AddRotationSequence(ref Sequence tweenSequence) {

        if(!_isRotationAnimated) {
            return;
        }

        _originalRotation = transform.rotation;
        var rotationSequence = transform.DORotate(_rotationEndValue, _animationTime).
            From(_rotationStartValue);

        switch(_rotationAnimationSource) {

            case AnimationSource.Ease:
                rotationSequence.SetEase(_rotationEase);
                break;

            case AnimationSource.Curve:
                rotationSequence.SetEase(_rotationCurve);
                break;

            default:
                rotationSequence.SetEase(Ease.Linear);
                break;
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
        DOTweenEditorPreview.Stop(true);
#endif
        ResetPosition();
        ResetScale();
        ResetRotation();
    }

    private void ResetPosition() {

        if(_originalPosition == null) {

            return;
        }

        transform.position = (Vector3)_originalPosition;
    }

    private void ResetScale() {

        if(_originalScale == null) {

            return;
        }

        transform.localScale = (Vector3)_originalScale;
    }

    private void ResetRotation() {

        if(_originalRotation == null) {

            return;
        }

        transform.rotation = (Quaternion)_originalRotation;
    }

    private void OnDisable() {

        _tweenSequence?.Kill();
    }

    [Serializable]
    public enum AnimationSource {
        None,
        Ease,
        Curve

    }

#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AToBTool))]
    public class AToBToolEditor : Editor {
        public override void OnInspectorGUI() {
            // base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_playOnEnable)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_animationTime)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_delay)));

            var testedProp = serializedObject.FindProperty(nameof(_isPositionAnimated));
            EditorGUILayout.PropertyField(testedProp);
            if(testedProp.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_startPosition)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_endPosition)));

                testedProp = serializedObject.FindProperty(nameof(_positionMultiplicativeSource));
                EditorGUILayout.PropertyField(testedProp);
                switch((AnimationSource)testedProp.intValue) {
                    case AnimationSource.Ease:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_xPositionEase)));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_yPositionEase)));
                        break;
                    case AnimationSource.Curve:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_xPositionCurve)));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_yPositionCurve)));
                        break;
                }

                var blendedProp = serializedObject.FindProperty(nameof(_useAdditive));
                EditorGUILayout.PropertyField(blendedProp);

                if(blendedProp.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_additiveXAnimation)));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_additiveYAnimation)));
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }

            testedProp = serializedObject.FindProperty(nameof(_isScaleAnimated));
            EditorGUILayout.PropertyField(testedProp);
            if(testedProp.boolValue) {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_scaleStartValue)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_scaleEndValue)));

                testedProp = serializedObject.FindProperty(nameof(_scaleAnimationSource));
                EditorGUILayout.PropertyField(testedProp);
                switch((AnimationSource)testedProp.intValue) {
                    case AnimationSource.Ease:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_scaleEase)));
                        break;
                    case AnimationSource.Curve:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_scaleCurve)));
                        break;
                }
                EditorGUI.indentLevel--;
            }

            testedProp = serializedObject.FindProperty(nameof(_isRotationAnimated));
            EditorGUILayout.PropertyField(testedProp);
            if(testedProp.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_rotationStartValue)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_rotationEndValue)));

                testedProp = serializedObject.FindProperty(nameof(_rotationAnimationSource));
                EditorGUILayout.PropertyField(testedProp);
                switch((AnimationSource)testedProp.intValue) {
                    case AnimationSource.Ease:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_rotationEase)));
                        break;
                    case AnimationSource.Curve:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_rotationCurve)));
                        break;
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_onAnimationComplete)));

            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(20f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if(GUILayout.Button("Preview", GUILayout.Width(100))) {
                var tar = target as AToBTool;
                tar.Play();
            }

            if(GUILayout.Button("Reset", GUILayout.Width(100))) {

                var tar = target as AToBTool;
                tar.Reset();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
#endif

}