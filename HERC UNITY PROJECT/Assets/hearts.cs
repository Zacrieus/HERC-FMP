using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    float health;
    public float numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    // Start is called before the first frame update
    void Start()
    {
        heartsChange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void heartsChange()
    {
        health = GameObject.Find("Herc").GetComponent<Character>().health;
        numOfHearts = health;
        for (int i = 0; i < hearts.Length; i++) //for i,v in pairs hearts.length
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                if (hearts[i].sprite != halfHeart)
                {
                    hearts[i].sprite = emptyHeart;
                }
            }



            //Health Bars
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if (Mathf.Floor(health) < health)
        {
            int heartsnumber = (int)Mathf.Floor(health);
            hearts[heartsnumber].sprite = halfHeart;
        }
    }
}
