using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] RectTransform PlayerContent;
    [SerializeField] PlayerSettings PlayerSettingsPrefab;

    public void AddPlayer()
    {
        Instantiate(PlayerSettingsPrefab, PlayerContent);
        RescaleContentHeight();
    }
    public void RemovePlayer()
    {
        if (PlayerContent.childCount > 2)
            Destroy(PlayerContent.GetChild(PlayerContent.childCount - 1).gameObject);
        RescaleContentHeight();
    }
    public void RescaleContentHeight()
    {
        var childAmount = PlayerContent.childCount;
        var height = PlayerContent.GetChild(0).GetComponent<RectTransform>().rect.height;
        PlayerContent.sizeDelta = new Vector2(PlayerContent.sizeDelta.x, height * childAmount);
    }
    public void StartGame()
    {
        int playerCount = 0;
        foreach (Transform child in PlayerContent)
        {
            var player = child.GetComponent<PlayerSettings>().GetPlayer();
            if (player == null)
            {
                continue;
            }
            playerCount++;
        }
        if (playerCount < 2) return;
        Player.NextActivePlayer();
        GamePlayManager.FieldSize = new Vector2Int(6, 6);
        SceneManager.LoadScene("Game Scene");
    }
}
