using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player
{
    public static event Action<Player> PlayerChanged;
    public static Player ActivePlayer { get; private set; }
    private static List<Player> _players = new List<Player>();
    [SerializeField] string playerName;
    [SerializeField] FieldState FieldState;
    public string GetPlayerName() => playerName;
    public FieldState GetFieldState() => FieldState;

    public Player(string name, string Symbol)
    {
        //Error Handling
        if (_players.Select(x => x.GetFieldState().GetVisual()).Any(x => x.Equals(Symbol)))
            throw new Exception("Symbol already taken");
        if (name == "")
            throw new Exception("Name can't be empty");
        if (Symbol == "")
            throw new Exception("Symbol can't be empty");
        if (_players.Any(_players => _players.GetPlayerName().Equals(name)))
            throw new Exception("Name already taken");

        playerName = name;
        FieldState = new FilledState(Symbol);
        _players.Add(this);
    }
    public void Destroy()
    {
        _players.Remove(this);
    }
    public static void ResetPlayerOrder()
    {
        ActivePlayer = _players[0];
        PlayerChanged?.Invoke(ActivePlayer);
    }
    public static void NextActivePlayer()
    {
        if (ActivePlayer == null)
        {
            ActivePlayer = _players[0];
        }
        else
        {
            int currentIndex = _players.IndexOf(ActivePlayer);
            int nextIndex = (currentIndex + 1) % _players.Count;
            ActivePlayer = _players[nextIndex];
        }
        PlayerChanged?.Invoke(ActivePlayer);
    }
    public static void PreviousActivePlayer()
    {
        if (ActivePlayer == null)
        {
            ActivePlayer = _players[0];
        }
        else
        {

            int currentIndex = _players.IndexOf(ActivePlayer);
            int nextIndex = (currentIndex - 1) % _players.Count;
            if (nextIndex < 0)
            {
                nextIndex = _players.Count - 1;
            }
            ActivePlayer = _players[nextIndex];
        
        }
        PlayerChanged?.Invoke(ActivePlayer);
    }
    public static void ClearPlayers()
    {
        for (int i = _players.Count - 1; i >= 0; i--)
        {
            Player player = _players[i];
            player.Destroy();
        }
        _players.Clear();
        ActivePlayer = null;
    }

}
