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
        numOfHearts = GameObject.Find("Boss").GetComponent<BossAI>().health;
    }

    // Update is called once per frame
    void Update()
    {
        health = GameObject.Find("Boss").GetComponent<BossAI>().health;
        numOfHearts = health;
        for (int i = 0; i < hearts.Length; i++) //for i,v in pairs hearts.length
        {


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
    }
}
