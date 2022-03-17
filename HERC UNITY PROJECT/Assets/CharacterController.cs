using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [Header("Settings")]
    public float moveSpeed;

    [Header("Setup")]
    public string lookDirection;
    public GameObject slashHitbox;

    
    Rigidbody2D rb;
    float moveX;
    float moveY;
    Vector2 moveVector;
    GameObject hitbox;
    Vector2 lastVector;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX= 0f;
        moveY = 0f;

        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        /*
        if (Input.GetKey(KeyCode.W))
        { moveY = +1f; }
        if (Input.GetKey(KeyCode.A))
        { moveX = -1f; }
        if (Input.GetKey(KeyCode.S))
        { moveY = -1f; }
        if (Input.GetKey(KeyCode.D))
        { moveX = +1f; }
        */

        moveVector = new Vector2(moveX, moveY).normalized;

        Debug.Log(moveVector +" + "+ lastVector );
   

        LookDirection();


        if (Input.GetMouseButtonDown(0))
        {
            //GameObject hitbox;
            Debug.Log("Attack");

            //need to rotate object in right direction but lazy

            if (lookDirection == "Right")
            { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(slashHitbox.transform.localScale.x, 0, 0), Quaternion.identity, transform); }
            else if (lookDirection == "Left")
            { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(-slashHitbox.transform.localScale.x, 0, 0), Quaternion.identity, transform); }
            else if (lookDirection == "Up")
            { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, slashHitbox.transform.localScale.y, 0), Quaternion.identity, transform); }
            else if (lookDirection == "Down")
            { hitbox = Instantiate(slashHitbox, transform.position + new Vector3(0, -slashHitbox.transform.localScale.y, 0), Quaternion.identity, transform); }

   

            Object.Destroy(hitbox, 1);


        }


    }
    private void FixedUpdate()
    {
        rb.velocity = moveVector;
        if (moveVector != new Vector2(0, 0))
        { lastVector = moveVector; }
    }

    void hitboxSpawns()
    {
        if (moveVector != new Vector2(0, 0))
        {
            hitbox = Instantiate(slashHitbox, transform.position + new Vector3(moveVector.x, moveVector.y, 0), Quaternion.identity, transform);
        }
        else
        {
            hitbox = Instantiate(slashHitbox, transform.position + new Vector3(lastVector.x, lastVector.y, 0), Quaternion.identity, transform);
        }
    }

    void LookDirection()
    {
        if (moveVector == new Vector2(-1, -1))
        { lookDirection = "Left"; }
        if (moveVector == new Vector2(-1, 0))
        { lookDirection = "Left"; }
        if (moveVector == new Vector2(-1, 1))
        { lookDirection = "Left"; }
        if (moveVector == new Vector2(0, -1))
        { lookDirection = "Down"; }
        if (moveVector == new Vector2(0, 1))
        { lookDirection = "Up"; }
        if (moveVector == new Vector2(1, -1))
        { lookDirection = "Right"; }
        if (moveVector == new Vector2(1, 0))
        { lookDirection = "Right"; }
        if (moveVector == new Vector2(1, 1))
        { lookDirection = "Right"; }
    }
}
