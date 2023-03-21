using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SortingExample : MonoBehaviour
{
    public List<float> numbers = new List<float>();
    public List<PlayerData> players = new List<PlayerData>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 30; i++)
        {
            numbers.Add(Random.Range(0, 100));
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            numbers.Sort(SortFunc);
            players.Sort(SortPlayerFunc);
        }
    }
    int SortFunc(float a, float b)
    {
        if(a < b)
        {
            return +1;
        }
        else if(a > b)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
    int SortPlayerFunc(PlayerData a, PlayerData b)
    {
        if (a.wavesSurvived < b.wavesSurvived)
        {
            return +1;
        }
        else if (a.wavesSurvived > b.wavesSurvived)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
