using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Attached to a player prefab, this stores the players personal details for display and contributing to scores
/// </summary>
public class PlayerData : MonoBehaviour
{
    //this value will be displayed by UI and used to final score boards.
    //this value is set in another scene and passed to this script when the level is loaded.
    public string playerName;
    public int playerNum;

    public int kills;
    public int deaths;
    public int wavesSurvived;

    public float totalScore;
}
