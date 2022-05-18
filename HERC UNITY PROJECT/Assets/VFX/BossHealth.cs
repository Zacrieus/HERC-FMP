using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    float health;
    public float numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite EmptyHeart;

    // Start is called before the first frame update
    void Start()
    {
        numOfHearts = GameObject.Find("Boss").GetComponent<Boss>().health;
    }

    // Update is called once per frame
    void Update()
    {
        health = GameObject.Find("Boss").GetComponent<Boss>().health;
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
                    hearts[i].sprite = EmptyHeart;
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
