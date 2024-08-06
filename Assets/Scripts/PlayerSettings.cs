using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField] TMP_InputField NameInput;
    [SerializeField] TMP_InputField SymbolInput;

    public Player GetPlayer()
    {
        var _name = NameInput.text;
        if (_name == "") _name = NameInput.placeholder.GetComponent<TMP_Text>().text;
        var symbol = SymbolInput.text;
        if (symbol == "") symbol = SymbolInput.placeholder.GetComponent<TMP_Text>().text;
        try { return new Player(name: _name, Symbol: symbol); }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }
}
