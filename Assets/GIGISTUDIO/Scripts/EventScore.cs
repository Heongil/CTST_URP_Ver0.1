using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScore : MonoBehaviour
{
    [SerializeField]
    int PositiveSocre = 0;

    [SerializeField]
    int NagativeScore = 0;


    [SerializeField]
    int score = 0;
    [SerializeField]
    bool IsPositive;

    public void UpdateEventScore(bool IsEnter)
    {
        if (IsPositive && IsEnter)
        {
            score = PositiveSocre;
        }
        else
        {
            score = NagativeScore;
        }

        if(!IsPositive && IsEnter)
        {
            score = NagativeScore;
        }
        else
        {
            score = PositiveSocre;
        }
       
    }

    public void AddScore(int velue)
    {
        score = velue;
    }
    public int GetScore()
    {
        return score;
    }
}
