using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] string HercText1;
    [SerializeField] string HercText2;
    [SerializeField] string ApolloText1;
    [SerializeField] string ApolloText2;
    [SerializeField] string ApolloText3;
    [SerializeField] string ApolloText4;
    [SerializeField] string ApolloText5;
    [SerializeField] string ApolloText6;
    [SerializeField] string ApolloText7;
    [SerializeField] string ApolloText8;
    [SerializeField] string ApolloText9;
    [SerializeField] string ApolloText10;


    [Header("Setup")]
    [SerializeField] GameObject Scene1;
    [SerializeField] GameObject Scene2;
    [SerializeField] GameObject FadeScreen;
    [SerializeField] GameObject Background;
    [SerializeField] GameObject Herc;
    [SerializeField] GameObject moveTo;
    [SerializeField] GameObject Apollo;

    bool walkIn = true;

    Dialogue dialogue;

    float textProgresion = 0f;
    float textDuration = 7.5f;
    float textTimer = 5f;

    float sceneDuration = 5f;
    float timer = 0f;
    float fadeDuration = 3f;
    float fadeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = GameObject.Find("Dialogue").GetComponent<Dialogue>();
        Scene2.SetActive(false);
        Background.SetActive(false);
        //Herc.SetActive(false);
        //Apollo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < sceneDuration)
        { timer += Time.deltaTime; Debug.Log(timer); }
        else if (timer >= sceneDuration)
        { 
            //fade("FadeIn");
        }


        if ((Herc.transform.position - moveTo.transform.position).magnitude > 1 && walkIn == true)
        {
            Herc.GetComponentInChildren<Animator>().Play("MoveRight");
            Herc.GetComponent<Rigidbody2D>().velocity = (moveTo.transform.position - Herc.transform.position).normalized * 3;
        }
        else if (walkIn == true)
        {
            Herc.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Apollo.SetActive(true);
            Herc.GetComponent<AudioSource>().Stop();
            Herc.GetComponentInChildren<Animator>().Play("IdleRight");
            walkIn = false;

        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            textTimer = textDuration;
            Destroy(GameObject.Find("Text"));
        }


        if (Apollo.activeInHierarchy == true)
        {
            if (textTimer < textDuration && textProgresion < 13)
            { textTimer += Time.deltaTime; }
            else if (textProgresion < 13)
            {
                textTimer = 0f;
                textProgresion += 1;

                if (textProgresion == 1)
                { dialogue.newText(Herc, HercText1, textDuration, new Color(1,.4f,0) ); }
                else if (textProgresion == 2)
                { dialogue.newText(Herc, HercText2, textDuration, new Color(1, .4f, 0)); }
                else if (textProgresion == 3)
                { dialogue.newText(Apollo, ApolloText1, textDuration, Color.yellow); }
                else if (textProgresion == 4)
                { dialogue.newText(Apollo, ApolloText2, textDuration, Color.yellow); }
                else if (textProgresion == 5)
                { dialogue.newText(Apollo, ApolloText3, textDuration, Color.yellow); }
                else if (textProgresion == 6)
                { dialogue.newText(Apollo, ApolloText4, textDuration, Color.yellow); }
                else if (textProgresion == 7)
                { dialogue.newText(Apollo, ApolloText5, textDuration, Color.yellow); }
                else if (textProgresion == 8)
                { dialogue.newText(Apollo, ApolloText6, textDuration, Color.yellow); }
                else if (textProgresion == 9)
                { dialogue.newText(Apollo, ApolloText7, textDuration, Color.yellow); }
                else if (textProgresion == 10)
                { dialogue.newText(Apollo, ApolloText8, textDuration, Color.yellow); }
                else if (textProgresion == 11)
                { dialogue.newText(Apollo, ApolloText9, textDuration, Color.yellow); }
                else if (textProgresion == 12)
                { dialogue.newText(Apollo, ApolloText10, textDuration, Color.yellow); }
            }
            else
            {
                SceneManager.LoadScene("The Cattle");
            }
        }


    }


    IEnumerator fade(string inOrOut)
    {
        Animator animator = FadeScreen.GetComponent<Animator>();
        animator.Play(inOrOut);
        yield return new WaitForSeconds(1);

    }

    void fading(string inOrOut)
    {
        Animator animator = FadeScreen.GetComponent<Animator>();
        animator.Play(inOrOut);
    }

}
