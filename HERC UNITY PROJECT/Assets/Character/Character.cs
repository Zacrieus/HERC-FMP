using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    [Range(0, 5)] public float moveSpeed;
    public string lookDirection = "Up";
 
    [Header("General Settings")]
    public float setHealth;
    [Range(0, 5)] public float setMoveSpeed;

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
    Animator animator;
    string currentState;
    [HideInInspector] public bool isMoving = false;
    public Vector2 moveVector;
    [HideInInspector] public GameObject slashHitbox;

    Rigidbody2D rb;
    GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
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
        if (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") != 0)
        { isMoving =true; }
        else
        { isMoving = false; }

        //Animations
        if (isMoving == false)
        {
            if (lookDirection == "Up")
            { ChangeAnim("IdleUp"); }
            else if (lookDirection == "Down")
            { ChangeAnim("IdleDown"); }
            if (lookDirection == "Right")
            { }
            else if (lookDirection == "Left")
            { }
        }
        else
        {
            if (lookDirection == "Up")
            { ChangeAnim("IdleUp"); }
            else if (lookDirection == "Down")
            { ChangeAnim("IdleDown"); }
            if (lookDirection == "Right")
            { }
            else if (lookDirection == "Left")
            { }
        }

        //Attacking
        if (Input.GetMouseButtonDown(0))
        {

            if (canAttack == true)
            {
                //need to rotate object in right direction but lazy
                if (lookDirection == "Up")
                { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, slashHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity,transform); }
                else if (lookDirection == "Down")
                { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, -slashHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity,transform); }
                if (lookDirection == "Right")
                { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(slashHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity,transform);  }
                else if (lookDirection == "Left")
                { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(-slashHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity,transform);  }


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
            { Object.Destroy(hitbox, 0); }
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canDash == true && isMoving == true)
            {
                Debug.Log("Dash");
                canDash = false;
                moveSpeed *= dashSpeed;
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

    //sending variables
    public bool IsMoving()
    {return isMoving;}
}
