using JeffGrawAssets.FlexibleUI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

[System.Serializable]
public enum EButtonState
{
    Normal,
    Hovered,
    Pressed,
    Disabled
}

[System.Serializable]
public class SBasicButtonColorBlock
{
    public STextColorInfo normalColor;
    public STextColorInfo hoveredColor;
    public STextColorInfo pressedColor;
    public STextColorInfo disabledColor;
}

public class BasicButtonHandler : ButtonHandler<EButtonState>
{
    [Header("References")]
    public TextMeshProUGUI mainTextSlot;
    public TextMeshProUGUI subTextSlot;
    
    [Header("TextColors")]
    public SBasicButtonColorBlock textColorBlock;
    
    protected override void Awake()
    {
        base.Awake();
    }
    public override void EditMainText(string value)
    {
        MainText = value;
        mainTextSlot.text = MainText;
        
        onMainTextChange.Invoke(MainText);
    }
    public override void EditSubText(string value)
    {
        SubText = value;
        if (subTextSlot == null)
        {
            Debug.LogError("Try to update sub text, but the slot is not assigned.", this);
            return;
        }
        subTextSlot.text = SubText;
        
        onSubTextChange.Invoke(SubText);
    }
    public override void SetState(EButtonState value)
    {
        if (EqualityComparer<EButtonState>.Default.Equals(State, value)) return;
 
        State = value;
        EditTextColor(State);
        
        onStateChanged?.Invoke(State);
    }

    private Tween EditTextColor(EButtonState state)
    {
        var seq = DOTween.Sequence();
        switch (state)
        {
            case EButtonState.Normal:
                seq.Append(mainTextSlot.DOColor(textColorBlock.normalColor.mainColor, 
                        textColorBlock.normalColor.duration)
                    .SetEase(textColorBlock.normalColor.ease)
                    .SetTarget(this)
                );
                if (subTextSlot != null)
                    seq.Append(subTextSlot.DOColor(textColorBlock.normalColor.subColor, 
                            textColorBlock.normalColor.duration)
                    .SetEase(textColorBlock.normalColor.ease)
                    .SetTarget(this)
                );
                break;
            case EButtonState.Hovered:
                seq.Append(mainTextSlot.DOColor(textColorBlock.hoveredColor.mainColor, 
                        textColorBlock.hoveredColor.duration)
                    .SetEase(textColorBlock.hoveredColor.ease)
                    .SetTarget(this)
                );
                if (subTextSlot != null)
                    seq.Append(subTextSlot.DOColor(textColorBlock.hoveredColor.subColor, 
                            textColorBlock.hoveredColor.duration)
                    .SetEase(textColorBlock.hoveredColor.ease)
                    .SetTarget(this)
                );
                break;
            case EButtonState.Pressed:
                seq.Append(mainTextSlot.DOColor(textColorBlock.pressedColor.mainColor, 
                        textColorBlock.pressedColor.duration)
                    .SetEase(textColorBlock.pressedColor.ease)
                    .SetTarget(this)
                );
                if (subTextSlot != null)
                    seq.Append(subTextSlot.DOColor(textColorBlock.pressedColor.subColor, 
                            textColorBlock.pressedColor.duration)
                    .SetEase(textColorBlock.pressedColor.ease)
                    .SetTarget(this)
                );
                break;
            default:
                break;
        }
        return seq;
    }
}