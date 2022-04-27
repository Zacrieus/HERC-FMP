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

    // Start is called before the first frame update
    void Start()
    {
        bloodVFX = GameObject.Find("Ving").GetComponent<Image>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.transform.Find("HercAnims").GetComponent<SpriteRenderer>();
        animator = gameObject.transform.Find("HercAnims").GetComponent<Animator>();
        moveSpeed = setMoveSpeed;
        health = setHealth;
    }

    // Update is called once per frame
    void Update()
    {

        //animator.SetBool("isMoving", isMoving);
        //animator.SetInteger("Direction", lookDirection);

        //LookDirection 1 = Up/W    2=Down/S      3=Left/A      4=Right/D
        if (Input.GetAxisRaw("Vertical") > 0)
        { lookDirection = "Up"; }
        else if (Input.GetAxisRaw("Vertical") < 0)
        { lookDirection = "Down"; }

        if (Input.GetAxisRaw("Horizontal") > 0)
        { lookDirection = "Right"; }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        { lookDirection = "Left"; }

        //ismoving
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal"))  + Mathf.Abs(Input.GetAxisRaw("Vertical")) != 0)
        { isMoving = true; }
        else
        { isMoving = false; }

        //Animations
        if (attackTimer < attackUpTime && canAttack == true)
        { directionAnims(); }

        //Attacking
        if (Input.GetMouseButtonDown(0))
        {

            if (canAttack == true)
            {
                //need to rotate object in right direction but lazy
                if (lookDirection == "Up")
                { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, slashHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.Euler(0,0,90),transform); }
                else if (lookDirection == "Down")
                { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, -slashHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.Euler(0, 0, -90), transform); }
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

                //hitbox.transform.lo
                canAttack = false;
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
                directionAnims();
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

        //rb.MovePosition(rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
        rb.velocity = moveVector * moveSpeed;
    }

    void ChangeAnim(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    public void takeDamage(float amount)
    {
        //Debug.Log("Take Damage");
        if (immune == false)
        {
            health -= amount;
            StartCoroutine(onHurt());
            GameObject.Find("Hearts").GetComponent<HealthUI>().heartsChange();
            if (health <= 0)
            { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        }

    }

    void directionAnims()
    {
        if (isMoving == true && canAttack == true)
        {
            if (lookDirection == "Up")
            {
                ChangeAnim("MoveUp");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (lookDirection == "Down")
            {
                ChangeAnim("MoveDown");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
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
        }
        else if (isMoving == false && canAttack == true)
        {
            if (lookDirection == "Up")
            {
                ChangeAnim("IdleUp");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (lookDirection == "Down")
            {
                ChangeAnim("IdleDown");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
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

    //sending variables
    public bool IsMoving()
    {return isMoving;}
}
