using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// create timers and format them in seconds and minutes in string form
/// </summary>
public class Timer : MonoBehaviour
{
    public float startTime;
    public float currentTime;

    public string displayTime;
    public bool isTiming = false;

    public UnityEvent TimesUp;
    
    // Update is called once per frame
    void Update()
    {
        if (isTiming)
        {
            currentTime -= Time.deltaTime;

            //format the time
            string minutes = Mathf.Floor(currentTime / 60).ToString("00");
            string seconds = (currentTime % 60).ToString("00");

            if(currentTime <= 0)
            {
                displayTime = "00:00";
                isTiming = false;
                TimesUp.Invoke();
            }
            else
            {
                displayTime = string.Format("{0}:{1}", minutes, seconds);
            }
        }
    }
    public void StartTimer(float length)
    {
        startTime = length;
        currentTime = startTime;
        isTiming = true;
    }
}
