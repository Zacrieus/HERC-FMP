using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    GameObject player;
    GameObject boss;
    Dialogue dialogue;

    float textTime = 5f;
    float timer = 0f;
    GameObject hearts;

    int textProgresion = 1;
    [SerializeField] string aretmisText1;
    [SerializeField] string HercText1;
    [SerializeField] string HercText2;
    [SerializeField] string HercText3;
    [SerializeField] string HercText4;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Herc");
        boss = GameObject.Find("Boss");
        dialogue = GameObject.Find("Dialogue").GetComponent<Dialogue>();
        hearts = GameObject.Find("Hearts");
        hearts.active = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll; ;
        boss.GetComponent<BossAI>().enabled = false;

        dialogue.newText(boss, aretmisText1, textTime, Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < textTime && textProgresion < 6)
        { timer += Time.deltaTime; }
        else
        {
            timer = 0f;
            textProgresion += 1;

            if (textProgresion == 2)
            { dialogue.newText(player, HercText1, textTime, Color.yellow); }
            else if (textProgresion == 3)
            { dialogue.newText(player, HercText2, textTime, Color.yellow); }
            else if (textProgresion == 4)
            { dialogue.newText(player, HercText3, textTime, Color.yellow); }
            else if (textProgresion == 5)
            { dialogue.newText(player, HercText4, textTime, Color.yellow); }
            else
            { CutsceneEnd(); }

        }
    }

    void CutsceneEnd()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        hearts.active = true;
        boss.GetComponent<BossAI>().enabled = true;
        Object.Destroy(GameObject.Find("TopBar"),0);
        Object.Destroy(GameObject.Find("BotBar"), 0);
    }
}
