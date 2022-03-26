using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;
    
    [Header("Stats")]
    public float health;
    public float moveSpeed;

    [Header("General Settings")]
    public float setHealth;
    [Range(0, 5)] public float setMoveSpeed;
    [Range(0, 25)] public float attackRange;

    [Header("Attack Settings")]
    bool canAttack = true;
    bool isSwinging = false;
    float attackTimer = 0f;
    [SerializeField] [Range(0, 3)] float attackWindUp;
    [SerializeField] [Range(0, 5)] float attackDuration;
    [SerializeField] [Range(0, 10)] float attackCd;
    //[SerializeField] bool isProjectile;
    [SerializeField] GameObject attackHitbox;
    GameObject hitbox;

    //Setup
    Vector2 moveDirection;
    float distanceFromPlayer;
    string attckDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        health = setHealth;
        moveSpeed = setMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement and Distance
        moveDirection = (player.transform.position - transform.position).normalized;
        distanceFromPlayer = (transform.position - player.transform.position).magnitude;


        //attacking
        if (canAttack == false)
        {
            attackTimer += Time.deltaTime;
            //Debug.Log(attackTimer);
            if (attackTimer >= attackCd)
            {
                //cooldown
                canAttack = true;
                attackTimer = 0f;
            }
            else if (isSwinging == true && attackTimer >= attackDuration + attackWindUp)
            {
                isSwinging = false;
                Object.Destroy(hitbox, 0);

            }
            else if (isSwinging == false && attackTimer >= attackWindUp && attackTimer < attackDuration + attackWindUp)
            {
                //attackStart
                isSwinging = true;

                if (attckDirection == "Up")
                { hitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform); }
                else if (attckDirection == "Down")
                { hitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, -attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform); }
                if (attckDirection == "Right")
                { hitbox = Instantiate(attackHitbox, transform.position + new Vector3(attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform); }
                else if (attckDirection == "Left")
                { hitbox = Instantiate(attackHitbox, transform.position + new Vector3(-attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform); }
            }

        }

    }

    void FixedUpdate()
    {
        //Debug.Log(distanceFromPlayer +"---" + attackRange);
        if (distanceFromPlayer > attackRange -.5f && distanceFromPlayer != 0)
        { rb.velocity = moveDirection * moveSpeed; }
        else
        {
            rb.velocity = Vector2.zero;
            if (canAttack)
            {attack();}
        }
    }

    void attack()
    {
        //Debug.Log(moveDirection);

        if (Mathf.Abs(moveDirection.x) >= Mathf.Abs(moveDirection.y))
        {
            //Is Left or Right
            if (moveDirection.x >= 0)
            {
                attckDirection = "Right";
            }
            else if (moveDirection.x < 0)
            {
                attckDirection = "Left";
            }


        }
        else if (Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.y))
        {
            //Is Up or Down
            if (moveDirection.y >= 0)
            {
                attckDirection = "Up";
            }
            else if (moveDirection.y < 0)
            {
                attckDirection = "Down";
            }
        }

        //Debug.Log(attckDirection);
        if(canAttack == true)
        {
            //Debug.Log("Enemy Attack " + attckDirection);
            canAttack = false;
        }
    }
}
