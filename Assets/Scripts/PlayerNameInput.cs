using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    TMP_InputField input;
    private void Start()
    {
        input = GetComponent<TMP_InputField>();
    }
    public void InputName(int playerNum)
    {
        GameManager.instance.currentPlayers[playerNum - 1].playerName = input.text;
    }
}
