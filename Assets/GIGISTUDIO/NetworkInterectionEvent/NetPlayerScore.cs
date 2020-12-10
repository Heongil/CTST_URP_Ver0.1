using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetPlayerScore : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScore;
    public Image colorImage;

    public void SetPlayerScore(string name,string score)
    {
        playerName.text = name;
        playerScore.text = score;
    }

    public void SetPlayerScore(Color color, int score)
    {
        colorImage.color = color;
        playerScore.text = score.ToString();
        gameObject.SetActive(true);
    }
}
