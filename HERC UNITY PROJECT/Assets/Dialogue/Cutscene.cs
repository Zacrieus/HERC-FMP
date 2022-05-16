using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    GameObject player;
    GameObject boss;
    BossAI bossCode;
    Dialogue dialogue;

    float textTime = 5f;
    float timer = 0f;
    [SerializeField] GameObject hearts;
    [SerializeField] GameObject artemisHearts;

    int textProgresion = 1;
    [SerializeField] string aretmisText1;
    [SerializeField] string aretmisText2;
    [SerializeField] string HercText1;
    [SerializeField] string HercText2;
    [SerializeField] string aretmisText3;
    [SerializeField] string aretmisText4;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Herc");
        boss = GameObject.Find("Boss");
        dialogue = GameObject.Find("Dialogue").GetComponent<Dialogue>();
        //hearts = GameObject.Find("Hearts");
        hearts.active = false;
        //artemisHearts = GameObject.Find("BossHealth");
        artemisHearts.active = false;
        
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll; ;
        bossCode = boss.GetComponent<BossAI>();
        bossCode.enabled = false;
        
        dialogue.newText(boss, aretmisText1, textTime, Color.green);

        //CutsceneEnd();

        Debug.Log(hearts.active);
        Debug.Log(hearts.GetComponentInChildren<Image>().IsActive());
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (timer < textTime && textProgresion < 7)
        { timer += Time.deltaTime; }
        else if (textProgresion < 7)
        {
            timer = 0f;
            textProgresion += 1;

            if (textProgresion == 2)
            { dialogue.newText(boss, aretmisText2, textTime, Color.green); }
            else if (textProgresion == 3)
            { dialogue.newText(player, HercText1, textTime, Color.yellow); }
            else if (textProgresion == 4)
            { dialogue.newText(player, HercText2, textTime, Color.yellow); }
            else if (textProgresion == 5)
            { dialogue.newText(boss, aretmisText3, textTime, Color.green); }
            else if (textProgresion == 6)
            { dialogue.newText(boss, aretmisText4, textTime, Color.green); }
            else if (textProgresion == 7)
            { CutsceneEnd(); }

        }
        //*/
    }

    void CutsceneEnd()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        bossCode.enabled = true;
        hearts.active = true;
        artemisHearts.active = true;

        Object.Destroy(GameObject.Find("TopBar"),0);
        Object.Destroy(GameObject.Find("BotBar"), 0);
    }
}
