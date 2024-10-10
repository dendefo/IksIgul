using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObserver : MonoBehaviour
{
    private CommandManager commandManager;
    private void OnEnable()
    {
        GameplayButton.OnClickEvent += OnClick;
        commandManager = new CommandManager();
    }
    public void Undo()
    {
        if (GamePlayManager.IsEnded) return;
        if (commandManager.Undo()) Player.PreviousActivePlayer();
    }
    public void Redo()
    {
        if (GamePlayManager.IsEnded) return;
        if (commandManager.Redo()) Player.NextActivePlayer();
    }
    private void OnClick(GameplayButton button)
    {
        if (GamePlayManager.IsEnded) return;
        var state = Player.ActivePlayer.GetFieldState();
        commandManager.AddCommand(new GameCommand(button, state));
        Player.NextActivePlayer();
    }
    private void OnDisable()
    {
        GameplayButton.OnClickEvent -= OnClick;
    }
}
public class CommandManager
{
    private List<GameCommand> gameCommands;
    private int currentIndex;
    public CommandManager()
    {
        gameCommands = new List<GameCommand>();
        currentIndex = -1;
    }
    public void AddCommand(GameCommand gameCommand)
    {
        //If player made Redo to some actions, clean the Queue from the current index to the end
        if (currentIndex < gameCommands.Count - 1)
        {
            gameCommands.RemoveRange(currentIndex + 1, gameCommands.Count - currentIndex - 1);
        }
        gameCommands.Add(gameCommand);
        gameCommand.Do();
        currentIndex++;
    }
    public bool Undo()
    {
        if (currentIndex >= 0)
        {
            gameCommands[currentIndex].Undo();
            currentIndex--;
            return true;
        }
        return false;
    }
    public bool Redo()
    {
        if (currentIndex < gameCommands.Count - 1)
        {
            currentIndex++;
            gameCommands[currentIndex].Do();
            return true;
        }
        return false;
    }
}
public class GameCommand
{
    public static event Action OnCommandExecuted;
    private FieldState stateBefore;
    private FieldState stateAfter;
    private GameplayButton GameplayButton;
    public GameCommand(GameplayButton gameplayButton, FieldState newState)
    {
        GameplayButton = gameplayButton;
        stateBefore = GameplayButton.GetState();
        stateAfter = newState;
    }
    public void Do()
    {
        GameplayButton.ChangeState(stateAfter);
        OnCommandExecuted?.Invoke();
    }
    public void Undo()
    {
        GameplayButton.ChangeState(stateBefore);
        OnCommandExecuted?.Invoke();
    }
}
