using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static Vector2Int FieldSize;
    public List<List<GameplayButton>> GameGrid;
    [SerializeField] private GameplayButton FieldPrefab;
    [SerializeField] private RectTransform FieldParent;
    const int MULTIPLIER = 128;
    private void Awake()
    {
        FieldParent.sizeDelta = new Vector2(FieldSize.x * MULTIPLIER, FieldSize.y * MULTIPLIER);
        GameGrid = new List<List<GameplayButton>>();
        for (int i = 0; i < FieldSize.x; i++)
        {
            GameGrid.Add(new List<GameplayButton>());
            for (int j = 0; j < FieldSize.y; j++)
            {
                var field = Instantiate(FieldPrefab, FieldParent);
                GameGrid[i].Add(field);
            }
        }
    }
    private void OnEnable()
    {
        GameCommand.OnCommandExecuted += OnCommandExecuted;
    }

    private void OnCommandExecuted()
    {
        if (HorizontalWinCondition() || VerticalWinCondition() || DiagonalWinCondition())
        {
            Debug.Log("Win");
        }
    }
    private bool HorizontalWinCondition()
    {
        for (int i = 0; i < FieldSize.x; i++)
        {
            FieldState horizontalState = GameGrid[i][0].GetState();
            int counter = 0;
            for (int j = 0; j < FieldSize.y; j++)
            {
                var tempNextState = GameGrid[i][j].GetState();
                if (horizontalState == tempNextState) counter++;
                else
                {
                    counter = 1;
                    horizontalState = tempNextState;
                }
                if (counter == 3) return true;

            }
        }
        return false;
    }
    private bool VerticalWinCondition()
    {
        for (int i = 0; i < FieldSize.y; i++)
        {
            FieldState verticalState = GameGrid[0][i].GetState();
            int counter = 0;
            for (int j = 0; j < FieldSize.x; j++)
            {
                var tempNextState = GameGrid[j][i].GetState();
                if (verticalState == tempNextState) counter++;
                else
                {
                    counter = 1;
                    verticalState = tempNextState;
                }
                if (counter == 3) return true;
            }
        }
        return false;
    }
    private bool DiagonalWinCondition()
    {
        // Check diagonal win condition from top-left to bottom-right
        for (int i = 0; i <= FieldSize.x - 3; i++)
        {
            for (int j = 0; j <= FieldSize.y - 3; j++)
            {
                FieldState topLeftState = GameGrid[i][j].GetState();
                if (topLeftState == FieldState.Empty)
                    continue;

                bool win = true;
                for (int k = 1; k < 3; k++)
                {
                    FieldState nextState = GameGrid[i + k][j + k].GetState();
                    if (nextState != topLeftState)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                    return true;
            }
        }

        // Check diagonal win condition from top-right to bottom-left
        for (int i = 0; i <= FieldSize.x - 3; i++)
        {
            for (int j = FieldSize.y - 1; j >= 2; j--)
            {
                FieldState topRightState = GameGrid[i][j].GetState();
                if (topRightState == FieldState.Empty)
                    continue;

                bool win = true;
                for (int k = 1; k < 3; k++)
                {
                    FieldState nextState = GameGrid[i + k][j - k].GetState();
                    if (nextState != topRightState)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                    return true;
            }
        }

        return false;
    }
    private void OnDisable()
    {
        GameCommand.OnCommandExecuted -= OnCommandExecuted;
    }
    private void OnDestroy()
    {
        Player.ClearPlayers();
    }
}
