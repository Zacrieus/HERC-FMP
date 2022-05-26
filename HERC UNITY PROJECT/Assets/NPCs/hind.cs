using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hind : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    [SerializeField] float hurtDuration;
    SpriteRenderer sr;
    bool agro = false;
    int currentStep = 0;
    [SerializeField] int Steps;
    [SerializeField] float MoveSpeed;
    Vector3 newPos;
    Rigidbody2D rb;
    bool isMoving = false;
    bool drifting = false;
    bool canHit = true;

    Animator animator;
    string currentState;
    AudioSource moo;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        moo = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agro == true && isMoving == false)
        {
            ChangeAnim("hindCharge");
            facePosition(GameObject.Find("Herc").transform.position);
            isMoving = true;
            drifting = false;
            if (currentStep == 0)
            {
                newRandomPos();
                facePosition(newPos);
            }
            else
            {
                Vector3 player = GameObject.Find("Herc").transform.position;
                //newPos = player.normalized * (player.magnitude + 3f);
                newPos = player;
                facePosition(GameObject.Find("Herc").transform.position);
            }
        }

        //While true do
        if ((transform.position - newPos).magnitude > 1 && drifting == false && isMoving == true)
        {
            Vector3 moveDirection = (newPos - transform.position).normalized;
            if (currentStep == 0)
            { rb.velocity = moveDirection * (MoveSpeed / 2f); }
            else
            { rb.velocity = moveDirection * MoveSpeed; }
        }
        else if ((transform.position - newPos).magnitude <= 1 && drifting == false && isMoving == true)
        {
            drifting = true;
        }

        //Debug.Log(currentStep);

        if (rb.velocity == Vector2.zero && drifting == true && agro == true && Steps >= currentStep)
        {
            currentStep += 1;
            isMoving = false;
            if (Steps < currentStep)
            {
                gameObject.tag = "MiniBoss";
                agro = false;
                ChangeAnim("hindIdle");
                isMoving = false;
                currentStep = 0;
            }
        }
    }

    public void takeDamage()
    {
        health -= 1;
        moo.Play();
        StartCoroutine(onHurt());
        if (health <= 0)
        {
            //Death
            Object.Destroy(gameObject, 0);
        }
    }

    IEnumerator onHurt()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(hurtDuration);
        sr.color = Color.white;
        agro = true;
        gameObject.tag = "Untagged";
    }

    IEnumerator hitDelay()
    {
        canHit = false;
        yield return new WaitForSeconds(1);
        canHit = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player" && agro == true)
        {
            if (canHit == true)
            {
                col.transform.GetComponent<Character>().takeDamage(.5f);
                StartCoroutine(hitDelay());
            }
        }
    }

    void newRandomPos()
    {
        float centeredScale = .75f;
        float inverse = Mathf.Abs(1 - centeredScale);

        float RandomY = Random.Range
        (Camera.main.ScreenToWorldPoint(new Vector3(0, 0 + Screen.height * inverse, 1)).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * centeredScale, 1)).y);

        float RandomX = Random.Range
        (Camera.main.ScreenToWorldPoint(new Vector3(0 + Screen.width * inverse, 0, 1)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * centeredScale, 0, 1)).x);

        newPos = new Vector3(RandomX, RandomY, 1);
    }

    void facePosition(Vector3 pos)
    {
        if (pos.x > transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (pos.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void ChangeAnim(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}
