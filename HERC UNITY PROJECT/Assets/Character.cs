using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;
    [SerializeField] float dashCD;
    [SerializeField] float attackCD;
    //public bool PriortizeVerticalOverHorizontal;

    [Header("Setup")]
    public string lookDirection = "Up";
    public Vector2 moveVector;
    public GameObject slashHitbox;


    bool canDash;
    bool canAttack;
    Rigidbody2D rb;
    GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //if  (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") == 0)

        /*
        if (Input.GetKeyDown(KeyCode.W))
        {lookDirection = "Up";}
        if (Input.GetKeyDown(KeyCode.S))
        { lookDirection = "Down"; }
        if (Input.GetKeyDown(KeyCode.A))
        { lookDirection = "Left"; }
        if (Input.GetKeyDown(KeyCode.D))
        { lookDirection = "Right"; }
        //*/

        if (Input.GetAxisRaw("Horizontal") > 0)
        { lookDirection = "Right"; }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        { lookDirection = "Left"; }


        if (Input.GetAxisRaw("Vertical") > 0)
        { lookDirection = "Up"; }
        else if (Input.GetAxisRaw("Vertical") < 0)
        { lookDirection = "Down"; }


        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log("Attack");

            //need to rotate object in right direction but lazy
            if (lookDirection == "Right")
            { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(slashHitbox.transform.localScale.x, 0, 0), Quaternion.identity, transform); }
            else if (lookDirection == "Left")
            { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(-slashHitbox.transform.localScale.x, 0, 0), Quaternion.identity, transform); }
            if (lookDirection == "up")
            { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, slashHitbox.transform.localScale.y, 0), Quaternion.identity, transform); }
            else if (lookDirection == "Down")
            { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, -slashHitbox.transform.localScale.y, 0), Quaternion.identity, transform); }

            Object.Destroy(hitbox, 1);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.AddForce(moveVector * 3);
        }

    }

    void FixedUpdate()
    {
        moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVector = moveVector.normalized;

        rb.MovePosition(rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }

    //private IEnumerator CoolDownTimer();
    //{

    //}

}
