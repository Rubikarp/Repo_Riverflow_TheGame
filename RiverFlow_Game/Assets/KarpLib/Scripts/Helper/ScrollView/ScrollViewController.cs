using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using ReadOnly = Sirenix.OdinInspector.ReadOnlyAttribute;

[RequireComponent(typeof(ScrollView))]
public class ScrollViewController : UIBehaviour
{
    [Header("References")] [SerializeField]
    private ScrollView scrollView;

    [Header("Position")] [SerializeField] public bool UseStep;

    [ShowNativeProperty, ReadOnly]
    public Vector2 CurrentPos
    {
        get => new Vector2(_posX, _posY);
        private set
        {
            _posX = Mathf.Clamp01(value.x);
            _posY = Mathf.Clamp01(value.y);
        }
    }

    [Sirenix.OdinInspector.HideIf("UseStep"), SerializeField, PropertyRange(0f, 1f)]
    private float _posX;

    [Sirenix.OdinInspector.HideIf("UseStep"), SerializeField, PropertyRange(0f, 1f)]
    private float _posY;

    private int _rangeStepXCount => StepXCount - 1;

    [Sirenix.OdinInspector.ShowIf("UseStep"), SerializeField, PropertyRange(0, "_rangeStepXCount")]
    private int _stepX;

    [Sirenix.OdinInspector.ShowIf("UseStep"), SerializeField, Min(1)]
    public int StepXCount = 2;

    private int _rangeStepYCount => StepYCount - 1;

    [Sirenix.OdinInspector.ShowIf("UseStep"), SerializeField, PropertyRange(0, "_rangeStepYCount")]
    private int _stepY;

    [Sirenix.OdinInspector.ShowIf("UseStep"), SerializeField, Min(1)]
    public int StepYCount = 2;

    [Header("Events")] public UnityEvent<Vector2> OnPositionChanged;

    protected override void Awake()
    {
        base.Awake();
        if (scrollView == null) scrollView = GetComponent<ScrollView>();
    }

    protected void OnValidate()
    {
        if (scrollView == null) scrollView = GetComponent<ScrollView>();
        if (UseStep)
            ApplyStep();
        else
            ApplyMove(CurrentPos);
    }

    // Step API
    public void MoveHorizontalToNextStep() => MoveToStep(_stepX + 1, _stepY);
    public void MoveHorizontalToPreviousStep() => MoveToStep(_stepX - 1, _stepY);
    public void MoveVerticalToNextStep() => MoveToStep(_stepX, _stepY + 1);
    public void MoveVerticalToPreviousStep() => MoveToStep(_stepX, _stepY - 1);

    public void MoveToStep(int x, int y)
    {
        _stepX = Mathf.Clamp(x, 0, Mathf.Max(0, StepXCount - 1));
        _stepY = Mathf.Clamp(y, 0, Mathf.Max(0, StepYCount - 1));
        ApplyStep();
    }

    private void ApplyStep()
    {
        float x = StepXCount > 1 ? (float)_stepX / (StepXCount - 1) : 0f;
        float y = StepYCount > 1 ? (float)_stepY / (StepYCount - 1) : 0f;
        MoveTo(new Vector2(x, y));
    }

    // Move API
    public void MoveHorizontallyTo(float x) => MoveTo(new Vector2(x, _posY));
    public void MoveVerticallyTo(float y) => MoveTo(new Vector2(_posX, y));

    public void MoveTo(Vector2 normalizedPos)
    {
        CurrentPos = normalizedPos;
        ApplyMove(CurrentPos);
    }

    private void ApplyMove(Vector2 target)
    {
        scrollView.NormalizedPosition = target;
        OnPositionChanged?.Invoke(CurrentPos);
    }

    // Tween API
    public void SimpleMove(Vector2 endValue) => DOScrollTo(endValue, 0.5f).SetEase(Ease.InOutSine);

    public TweenerCore<Vector2, Vector2, VectorOptions> DOScrollTo(Vector2 endValue, float duration)
    {
        CurrentPos = scrollView.NormalizedPosition;
        return DOTween.To(() => CurrentPos, x => MoveTo(x), endValue, duration).SetTarget(scrollView);
    }

    public TweenerCore<Vector2, Vector2, VectorOptions> DOScrollToStep(int stepX, int stepY, float duration)
    {
        CurrentPos = scrollView.NormalizedPosition;
        stepX = Mathf.Clamp(stepX, 0, Mathf.Max(0, StepXCount - 1));
        stepY = Mathf.Clamp(stepY, 0, Mathf.Max(0, StepYCount - 1));

        Vector2 endValue = new Vector2(
            StepXCount > 1 ? (float)stepX / (StepXCount - 1) : 0f,
            StepYCount > 1 ? (float)stepY / (StepYCount - 1) : 0f
        );
        return DOScrollTo(endValue, duration);
    }
}