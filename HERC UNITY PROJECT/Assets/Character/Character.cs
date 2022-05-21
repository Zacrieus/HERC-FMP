using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    [Range(0, 5)] public float moveSpeed;
    public string lookDirection = "Right";
 
    [Header("General Settings")]
    public float setHealth;
    [Range(0, 5)] public float setMoveSpeed;
    [Range(0, 3)] public float hurtImmunity;

    [Header("Dash Setup")]
    [SerializeField] [Range(0,5)] float dashCD;
    [SerializeField] [Range(1, 25)] float dashSpeed;
    [SerializeField] [Range(0, 5)] float dashDuration;
    float dashtimer = 0f;
    bool canDash = true;

    [Header("Attack Setup")]
    [SerializeField] [Range(0, 5)] float attackCD;
    [SerializeField] [Range(0, 5)] float attackUpTime;
    [SerializeField] [Range(0, 5)] float attackDamage;
    float attackTimer;
    bool canAttack = true;
    bool animate = true;
    //public bool PriortizeVerticalOverHorizontal;

    [Header("Setup (IGNORE)")]
    Image bloodVFX;
    Animator animator;
    string currentState;
    [HideInInspector] public bool isMoving = false;
    public Vector2 moveVector;
    [HideInInspector] public GameObject slashHitbox;
    [HideInInspector] public bool immune = false;
    SpriteRenderer sr;
    Rigidbody2D rb;
    GameObject hitbox;
    public Vector3 spawnLocation;
    [SerializeField] AudioSource hit;
    [SerializeField] AudioSource hurt;
    [SerializeField] AudioSource footsteps;
    [SerializeField] AudioSource dashSound;

    // Start is called before the first frame update
    void Start()
    {
        bloodVFX = GameObject.Find("Ving").GetComponent<Image>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.transform.Find("HercAnims").GetComponent<SpriteRenderer>();
        animator = gameObject.transform.Find("HercAnims").GetComponent<Animator>();
        moveSpeed = setMoveSpeed;
        health = setHealth;
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        spawnLocation = transform.position;

        if (sceneName == "The Cattle")
        { GameObject.Find("Dialogue").GetComponent<Dialogue>().newText(gameObject,"Time to become a hero. First step, The Cattle.",4f, new Color(1, .4f, 0)); }
        if (sceneName == "The Belt")
        { GameObject.Find("Dialogue").GetComponent<Dialogue>().newText(gameObject,"That wasnt so bad. lets go find The Belt now.",4f, new Color(1, .4f, 0)); }
        if (sceneName == "The Hind")
        { GameObject.Find("Dialogue").GetComponent<Dialogue>().newText(gameObject, "This Animal its Artemis' Hind", 4f, new Color(1, .4f, 0)); }

        footsteps.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetAxisRaw("Horizontal") > 0)
        { 
            lookDirection = "Right"; 
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        { 
            lookDirection = "Left"; 
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        { 
            lookDirection = "Up"; 
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        { 
            lookDirection = "Down"; 
        }

        //ismoving
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal"))  + Mathf.Abs(Input.GetAxisRaw("Vertical")) != 0)
        {   
            isMoving = true;
            footsteps.volume = .5f;
        }
        else
        { 
            isMoving = false;
            //footsteps.time = 0f; 
            footsteps.volume = 0;
        }

        //Animations
        directionAnims();
        //if ( animate = true)
        //{ directionAnims(); }

        //Attacking
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {

            if (canAttack == true)
            {
                //need to rotate object in right direction but lazy
                if (lookDirection == "Up")
                {
                    hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, slashHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.Euler(0,0,90),transform);
                    ChangeAnim("UpAttack");
                }
                else if (lookDirection == "Down")
                {
                    hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, -slashHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.Euler(0, 0, -90), transform);
                    ChangeAnim("DownAttack");
                }
                if (lookDirection == "Right")
                { 
                    hitbox = Instantiate(slashHitbox, transform.position + new Vector3(slashHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity,transform);
                    ChangeAnim("RightAttack");
                }
                else if (lookDirection == "Left")
                { 
                    hitbox = Instantiate(slashHitbox, transform.position + new Vector3(-slashHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity,transform);
                    ChangeAnim("RightAttack");
                }
                canAttack = false;
                animate = false;
                hit.Play();
                //rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        if (canAttack == false)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackCD)
            {
                canAttack = true;
                attackTimer = 0f;
            }
            if (attackTimer > attackUpTime)
            {
                Object.Destroy(hitbox, 0);
                animate = true;
                //rb.constraints = RigidbodyConstraints2D.None;
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                animate = true;
            }
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canDash == true && isMoving == true)
            {
                //Debug.Log("Dash");
                canDash = false;
                moveSpeed *= dashSpeed;
                dashSound.time = 0f;
                dashSound.Play();
                StartCoroutine(iFrames());
            }
        }
        if (canDash == false)
        {
            dashtimer += Time.deltaTime;
            if (dashtimer > dashCD)
            {
                canDash = true;
                dashtimer = 0f;
            }
            if (dashtimer > dashDuration)
            { moveSpeed = setMoveSpeed;}
        }

        if (bloodVFX.color.a > 0)
        {
            bloodVFX.color = new Color(bloodVFX.color.r, bloodVFX.color.g, bloodVFX.color.b, bloodVFX.color.a - 1f * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVector = moveVector.normalized;
        rb.velocity = moveVector * moveSpeed;
    }

    public void ChangeAnim(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    public void takeDamage(float amount)
    {
        if (immune == false)
        {
            hurt.Play();
            health -= amount;
            StartCoroutine(onHurt());
            GameObject.Find("Hearts").GetComponent<HealthUI>().heartsChange();
            if (health <= 0)
            { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        }

    }

    void directionAnims()
    {
        if (isMoving == true && animate == true)
        {
            if (lookDirection == "Right")
            {
                ChangeAnim("MoveRight");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (lookDirection == "Left")
            {
                ChangeAnim("MoveRight");
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (lookDirection == "Up")
            {
                ChangeAnim("MoveUp");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (lookDirection == "Down")
            {
                ChangeAnim("MoveDown");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

        }
        else if (isMoving == false && animate == true)
        {
            if (lookDirection == "Right")
            {
                ChangeAnim("IdleRight");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (lookDirection == "Left")
            {
                ChangeAnim("IdleRight");
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (lookDirection == "Up")
            {
                ChangeAnim("IdleUp");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (lookDirection == "Down")
            {
                ChangeAnim("IdleDown");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    IEnumerator onHurt()
    {
        sr.color = Color.red;
        immune = true;
        bloodVFX.color = Color.white;
        yield return new WaitForSeconds(hurtImmunity);
        immune = false;
        sr.color = Color.white;
    }
    
    IEnumerator iFrames()
    {
        immune = true;
        gameObject.tag = "Untagged";
        yield return new WaitForSeconds(dashDuration +.5f);
        gameObject.tag = "Player";
        immune = false;
    }
}
