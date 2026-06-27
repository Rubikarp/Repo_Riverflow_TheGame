using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Sirenix.OdinInspector;

[System.Serializable]
public class STextColorInfo
{
    public Color mainColor = Color.white;
    public Color subColor = Color.white;
    public Ease ease = Ease.Linear;
    public float duration = 0.2f;
}

public  class ButtonInteraction : Selectable, IPointerClickHandler, IPointerUpHandler
{
    [Header("AutoStates")]
    [Tooltip("Select the value each state will trigger, if not in the dictionary, the state will not trigger any value")]
    [SerializeField] protected SerializedDictionary<SelectionState, int> statesSignals = new()
    {
        { SelectionState.Normal, 0 },
        { SelectionState.Highlighted, 1 },
        { SelectionState.Selected, 1 },
        { SelectionState.Pressed, 2 },
        { SelectionState.Disabled, 3 }
    };

    [Header("Events")] 
    public UnityEvent onClick = new();
    [HideInInspector] public UnityEvent<int> onStateChanged = new();
    
    public bool Interactable
    {
        get => IsInteractable();
        set => interactable = value;
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        //Trigger only if in dictionary
        if (statesSignals.ContainsKey(state))
        {
            onStateChanged?.Invoke(statesSignals[state]);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Interactable) return;

        onClick.Invoke();
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }
}

public abstract class ButtonHandler<T> : Selectable, IPointerClickHandler where T : Enum
{
    [field: Header("Valeur")]
    [field: SerializeField] public string MainText { get; protected set; }
    [field: SerializeField] public string SubText { get; protected set; }
    
    [Header("AutoStates")]
    [SerializeField] protected SerializedDictionary<SelectionState, T> _autoStates = new()
    {
        { SelectionState.Normal, default },
        { SelectionState.Highlighted, default },
        { SelectionState.Pressed, default },
        { SelectionState.Disabled, default }
    };

    [FoldoutGroup("Debug")]
    [field: SerializeField, ReadOnly]
    public T State { get; protected set; } = default;

    [Header("Events")] 
    public UnityEvent onClick = new();
    [HideInInspector] public UnityEvent<T> onStateChanged = new();
    public UnityEvent<string> onMainTextChange = new();
    public UnityEvent<string> onSubTextChange = new();

    public bool Interactable
    {
        get => IsInteractable();
        set => interactable = value;
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        if (_autoStates.ContainsKey(state))
        {
            SetState(_autoStates[state]);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Interactable) return;

        onClick.Invoke();
    }

    public virtual void EditMainText(string value)
    {
        MainText = value;
        onMainTextChange.Invoke(MainText);
    }

    public virtual void EditSubText(string value)
    {
        SubText = value;
        onSubTextChange.Invoke(SubText);
    }

    public virtual void SetState(T value)
    {
        if (EqualityComparer<T>.Default.Equals(State, value)) return;

        State = value;
        onStateChanged?.Invoke(State);
    }
}