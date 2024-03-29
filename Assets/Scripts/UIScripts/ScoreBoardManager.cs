using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreBoardManager : MonoBehaviour
{
    [SerializeField] PlayerScoreCard[] scores = new PlayerScoreCard[10];

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        for (int i = 0; i < 10; i++)
        {
            scores[i].playerName.text = GameManager.instance.saveData.highScorePlayerNames[i];
            scores[i].kills.text = "Kills: " + GameManager.instance.saveData.highScorePlayerKills[i];
            scores[i].deaths.text = "Deaths: " + GameManager.instance.saveData.highScorePlayerDeaths[i];
            scores[i].waves.text = "Waves: " + GameManager.instance.saveData.highScorePlayerWaves[i];
        }
    }
}
