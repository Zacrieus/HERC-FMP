using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtemisSetup : MonoBehaviour
{
    Vector3 spawnLocation;
    float spawnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = GameObject.Find("Herc").transform.position;
        spawnSpeed = GameObject.Find("Herc").GetComponent<Character>().setMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0)
        {
            //Debug.Log("Work");

            //GameObject.Find("Herc").GetComponent<Rigidbody2D>().velocity = (GameObject.Find("Herc").GetComponent<Character>().spawnLocation - transform.position).normalized * GameObject.Find("Herc").GetComponent<Character>().setMoveSpeed;

            //Debug.Log((GameObject.Find("Herc").transform.position - spawnLocation).magnitude);
            if ((GameObject.Find("Herc").transform.position - spawnLocation).magnitude > 1)
            {
                GameObject.Find("Herc").GetComponent<Character>().enabled = false;

                Transform hercTransform = GameObject.Find("Herc").transform;
                if (spawnLocation.x > hercTransform.position.x)
                { hercTransform.localScale = new Vector3(Mathf.Abs(hercTransform.localScale.z), hercTransform.localScale.y, hercTransform.localScale.z); }
                else
                { hercTransform.localScale = new Vector3(-Mathf.Abs(hercTransform.localScale.z), hercTransform.localScale.y, hercTransform.localScale.z); }
                GameObject.Find("Herc").GetComponentInChildren<Animator>().Play("MoveRight");

                GameObject.Find("Herc").GetComponent<Rigidbody2D>().velocity = (spawnLocation - hercTransform.position).normalized * spawnSpeed;
            }
            else
            {
                //GameObject.Find("Herc").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                //GameObject.Find("Herc").GetComponent<Character>().ChangeAnim("IdleRight");
                //GameObject.Find("Herc").transform.localScale = new Vector3(1, 1, 1);
                //GameObject.Find("Herc").GetComponent<Character>().enabled = false;

                SceneManager.LoadScene("Artemis");
                //StartCoroutine(wait());

            } 
        }

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Artemis");
    }
}
