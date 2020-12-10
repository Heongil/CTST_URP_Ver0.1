using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingImageMovement : MonoBehaviour
{

    Dictionary<string, Sprite> dicSprites = new Dictionary<string, Sprite>();
    public Image targetBoard;
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            dicSprites.Add(sprites[i].name, sprites[i]);
        }
    }

    public void SetImage(string key)
    {
        if(dicSprites.ContainsKey(key))
        {
            targetBoard.gameObject.SetActive(true);
            targetBoard.sprite = dicSprites[key];
        }
     
    }

    public void OffBoard()
    {
        targetBoard.gameObject.SetActive(false);
    }

}
