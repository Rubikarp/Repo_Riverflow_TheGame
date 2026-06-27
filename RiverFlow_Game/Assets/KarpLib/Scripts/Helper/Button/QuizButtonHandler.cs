using System.Collections.Generic;
using DG.Tweening;
using JeffGrawAssets.FlexibleUI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public enum EQuizButtonState
{
    Neutral,
    Correct,
    Wrong,
    Disabled
}

[System.Serializable]
public class SQuizButtonColorBlock
{
    public STextColorInfo normalColor;
    public STextColorInfo correctColor;
    public STextColorInfo wrongColor;
    public STextColorInfo disabledColor;
}

public class QuizButtonHandler : ButtonHandler<EQuizButtonState>
{
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI _mainTextSlot;
    [SerializeField] private TextMeshProUGUI _subTextSlot;
    [SerializeField] private FlexibleImage[] _images;
    [field:SerializeField] public bool IsCorrect { get; private set; }
 
    [FoldoutGroup("Text Colors")]
    [SerializeField] private SQuizButtonColorBlock _mainTextColorBlock;
    [FoldoutGroup("Text Colors")]
    [SerializeField] private SQuizButtonColorBlock _subTextColorBlock;
 
    [FoldoutGroup("Events")]
    public UnityEvent<bool> OnIsCorrectChanged;
    
    protected override void Awake()
    {
        base.Awake();
    }
    public void SetIsCorrect(bool isCorrect)
    {
        IsCorrect = isCorrect;
        OnIsCorrectChanged?.Invoke(IsCorrect);
    }
 
    public override void EditMainText(string value)
    {
        MainText = value;
        _mainTextSlot.text = MainText;
        onMainTextChange.Invoke(MainText);
    }
 
    public override void EditSubText(string value)
    {
        SubText = value;
        if (_subTextSlot == null)
        {
            Debug.LogError("Tentative de mise à jour du sub text, mais le slot n'est pas assigné.", this);
            return;
        }
        _subTextSlot.text = SubText;
        onSubTextChange.Invoke(SubText);
    }
 
    public override void SetState(EQuizButtonState value)
    {
        if (EqualityComparer<EQuizButtonState>.Default.Equals(State, value)) return;
 
        State = value;
        AnimateTextColor(State);
        foreach (var img in _images)
        {
            img.animationMode = FlexibleImage.AnimationStateDrivenBy.Script;
            img.scriptDrivenAnimationState = (int)value;
        }
 
        onStateChanged?.Invoke(State);
    }
 
    private Tween AnimateTextColor(EQuizButtonState state)
    {
        STextColorInfo mainInfo = state switch
        {
            EQuizButtonState.Neutral  => _mainTextColorBlock.normalColor,
            EQuizButtonState.Correct  => _mainTextColorBlock.correctColor,
            EQuizButtonState.Wrong    => _mainTextColorBlock.wrongColor,
            EQuizButtonState.Disabled => _mainTextColorBlock.disabledColor,
            _                         => _mainTextColorBlock.normalColor
        };
        STextColorInfo subInfo = state switch
        {
            EQuizButtonState.Neutral  => _subTextColorBlock.normalColor,
            EQuizButtonState.Correct  => _subTextColorBlock.correctColor,
            EQuizButtonState.Wrong    => _subTextColorBlock.wrongColor,
            EQuizButtonState.Disabled => _subTextColorBlock.disabledColor,
            _                         => _subTextColorBlock.normalColor
        };
 
        var seq = DOTween.Sequence();
 
        seq.Append(
            _mainTextSlot
                .DOColor(mainInfo.mainColor, mainInfo.duration)
                .SetEase(mainInfo.ease)
                .SetTarget(this)
        );
 
        if (_subTextSlot != null)
        {
            seq.Append(
                _subTextSlot
                    .DOColor(subInfo.mainColor, subInfo.duration)
                    .SetEase(subInfo.ease)
                    .SetTarget(this)
            );
        }
 
        return seq;
    }
}