using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InLevelUIManager : MonoBehaviour
{
    public TMP_Text centreText;

    public void UpdateUI()
    {
        if(LevelManager.instance.currentState == LevelManager.GameState.Prepping)
        {
            centreText.text = "Next Wave in: \n" + LevelManager.instance.timer.displayTime;
        }
        else if (LevelManager.instance.currentState == LevelManager.GameState.InWave)
        {
            centreText.text = "SURVIVE!\n" + (LevelManager.instance.waves[LevelManager.instance.currentWave].maxNumberOverLifeTime - LevelManager.instance.waves[LevelManager.instance.currentWave].killed) + " zombies left";
        }
    }
}
