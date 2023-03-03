using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// used to create timers, format timer strings ready for UI display, and trigger events on a timer ending.
/// </summary>
public class Timer : MonoBehaviour
{
    public float startTime;
    public float currentTime;

    public string displayTime;
    public bool isTiming = false;

    public UnityEvent OnTimerComplete;

    // Update is called once per frame
    void Update()
    {
        if (isTiming)
        {
            //reduce the time
            currentTime -= Time.deltaTime;

            //format strings in minutes and seconds
            string minutes = Mathf.Floor(currentTime / 60).ToString("00");
            string seconds = (currentTime % 60).ToString("00");

            //if timer is out, end time and display 00:00, or else display the current time.
            if(currentTime <= 0)
            {
                displayTime = "00:00";
                isTiming = false;
                OnTimerComplete.Invoke();
            }
            else
            {
                displayTime = string.Format("{0}:{1}", minutes, seconds);
            }  
        }
    }
    /// <summary>
    /// Call this function from another script to begin a new timer with a specified length
    /// </summary>
    /// <param name="length">
    /// How long the timer will last, in seconds.
    /// </param>
    public void StartTimer(float length)
    {
        startTime = length;
        currentTime = startTime;
        isTiming = true;
    }
}
