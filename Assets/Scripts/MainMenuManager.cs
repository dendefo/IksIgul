using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        new Player(name: "Player 1", Symbol:"X");
        new Player(name: "Player 2", Symbol:"O");
        new Player(name: "Player 3", Symbol:"L");
        new Player(name: "Player 4", Symbol:"T");
        new Player(name: "Player 5", Symbol:"P");
        new Player(name: "Player 6", Symbol:"Z");
        Player.NextActivePlayer();
        GamePlayManager.FieldSize = new Vector2Int(6, 6);
        SceneManager.LoadScene("Game Scene");
    }
}
