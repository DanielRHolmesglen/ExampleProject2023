using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InLevelUIManager : MonoBehaviour
{
    public bool isOnline = false;

    public TMP_Text centreText;

    public CanvasGroup resultGroup;

    public TMP_Text resultTitle;

    public float fadeRate;

    public void UpdateUI()
    {
        if (!isOnline)
        {
            if (LevelManager.instance.currentState == LevelManager.GameStates.Prepping)
            {
                centreText.text = "Next Wave In: \n" + LevelManager.instance.timer.displayTime;
            }
            else if (LevelManager.instance.currentState == LevelManager.GameStates.InWave)
            {
                WaveManager currentWave = LevelManager.instance.waves[LevelManager.instance.currentWave];
                int EnemiesLeft = currentWave.maxNumberOverAll - currentWave.killed;

                centreText.text = "SURVIVE! \n" + EnemiesLeft.ToString() + " / " + currentWave.maxNumberOverAll;
            }
        }
        else
        {
            if (OnlineLevelManager.instance.currentState == OnlineLevelManager.GameStates.Prepping)
            {
                centreText.text = "Next Wave In: \n" + OnlineLevelManager.instance.timer.displayTime;
            }
            else if (OnlineLevelManager.instance.currentState == OnlineLevelManager.GameStates.InWave)
            {
                WaveManager currentWave = OnlineLevelManager.instance.waves[OnlineLevelManager.instance.currentWave];
                int EnemiesLeft = currentWave.maxNumberOverAll - currentWave.killed;

                centreText.text = "SURVIVE! \n" + EnemiesLeft.ToString() + " / " + currentWave.maxNumberOverAll;
            }
        }
       
    }

    public void EndGameUI()
    {
        StartCoroutine(DisplayCanvas(fadeRate));
    }

    IEnumerator DisplayCanvas(float rate)
    {
        if (!isOnline)
        {
            if (LevelManager.instance.currentState == LevelManager.GameStates.Lost)
            {
                resultTitle.text = "You have perished";
            }
            else if (LevelManager.instance.currentState == LevelManager.GameStates.Won)
            {
                resultTitle.text = "Level Cleared";
            }
        }
        else
        {
            if (OnlineLevelManager.instance.currentState == OnlineLevelManager.GameStates.Lost)
            {
                resultTitle.text = "You have perished";
            }
            else if (OnlineLevelManager.instance.currentState == OnlineLevelManager.GameStates.Won)
            {
                resultTitle.text = "Level Cleared";
            }
        }
        

        while(resultGroup.alpha < 0.9f)
        {
            resultGroup.alpha = Mathf.Lerp(resultGroup.alpha, 1, rate);
            yield return new WaitForEndOfFrame();
        }

        resultGroup.alpha = 1f;
        yield return null;
    }
}
