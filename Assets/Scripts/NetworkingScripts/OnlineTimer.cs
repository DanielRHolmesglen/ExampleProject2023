using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class OnlineTimer : MonoBehaviour, IOnEventCallback
{
    public float startTime;
    public float currentTime;

    public string displayTime;
    public bool isTiming = false;

    //Define the photon event
    private const byte TIMER_TICK = 1;

    public UnityEvent TimesUp;

    #region Photon Raise Event Code
    //enable and disable the ability to listen to events
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    //listent to the event and react to it.
    public void OnEvent(EventData data)
    {
        if(data.Code == TIMER_TICK)
        {
            object[] localData = (object[])data.CustomData;
            displayTime = (string)localData[0];
            currentTime = (float)localData[1];
        }
    }
    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (isTiming)
        {
            currentTime -= Time.deltaTime;

            //format the time
            string minutes = Mathf.Floor(currentTime / 60).ToString("00");
            string seconds = (currentTime % 60).ToString("00");

            if (currentTime <= 0)
            {
                displayTime = "00:00";
                isTiming = false;
                TimesUp.Invoke();
            }
            else
            {
                displayTime = string.Format("{0}:{1}", minutes, seconds);
                object[] data = new object[] { displayTime, currentTime };
                PhotonNetwork.RaiseEvent(TIMER_TICK, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
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