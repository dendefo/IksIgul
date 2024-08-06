using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayButton : MonoBehaviour
{
    public static event Action<GameplayButton> OnClickEvent;
    private FieldState state;
    [SerializeField] TMP_Text Visual;
    [SerializeField] Button Button;
    private void Awake()
    {
        state = new EmptyFieldState();
        Visual.text = state.GetVisual();
    }
    public void OnClick()
    {
        if (state.CanBePressed())
        {
            OnClickEvent?.Invoke(this);
        }
    }
    public void ChangeState(FieldState newState)
    {
        state = newState;
        Visual.text = state.GetVisual();
        Button.interactable = state.CanBePressed();
    }
    public FieldState GetState() => state;
}
[Serializable]
abstract public class FieldState
{
    public static FieldState Empty => new EmptyFieldState();
    abstract public string GetVisual();
    virtual public bool CanBePressed() => false;
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        return Equals(GetVisual(), (obj as FieldState)?.GetVisual());
    }
}
[Serializable]
public class EmptyFieldState : FieldState
{
    static string visual = " ";
    public override string GetVisual()
    {
        return visual;
    }
    public override bool CanBePressed() => true;
}
[Serializable]
public class FilledState : FieldState
{
    [SerializeField] string visual;
    public FilledState(string visual)
    {
        this.visual = visual;
    }
    public override string GetVisual()
    {
        return visual;
    }
}