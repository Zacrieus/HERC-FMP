using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;
    
    [Header("Enemey Settings")]
    public float health;
    public float attackRange;
    public float moveSpeed;

    [Header("Attack Settings")]
    bool canAttack;
    float attackTimer;
    [SerializeField] float attackWindUp;
    [SerializeField] float attackDuration;
    [SerializeField] float attackCd;
    [SerializeField] bool isProjectile;
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
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = (player.transform.position - transform.position).normalized;
        distanceFromPlayer = (transform.position - player.transform.position).magnitude;
        //moveDirection = moveDirection.magnitude;
        //Debug.Log(moveDirection.normalized);

        if (canAttack == false)
        {
            attackTimer += Time.deltaTime;
            //Debug.Log(attackTimer);
            if (attackTimer >= attackCd)
            {
                //cooldown
                Debug.Log("Reset");
                canAttack = true;
                attackTimer = 0f;
            }
            if (attackTimer >= attackWindUp)
            {
                //attackStart
                Debug.Log("swing");
                if (attckDirection == "Up")
                { hitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform); }
                else if (attckDirection == "Down")
                { hitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, -attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform); }
                if (attckDirection == "Right")
                { hitbox = Instantiate(attackHitbox, transform.position + new Vector3(attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform); }
                else if (attckDirection == "Left")
                { hitbox = Instantiate(attackHitbox, transform.position + new Vector3(-attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform); }
                //Object.Destroy(hitbox, 1);

            }

            if (attackTimer >= attackDuration)
            {
                Debug.Log("End");
                Object.Destroy(hitbox, 0);
            }
        }

    }

    void FixedUpdate()
    {
        ///Debug.Log(distanceFromPlayer +"---" + attackRange);
        if (distanceFromPlayer > attackRange -.5f )
        { rb.velocity = moveDirection * moveSpeed; }
        else
        { 
            rb.velocity = Vector2.zero;
            attack();
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

        Debug.Log(attckDirection);
        if(canAttack == true)
        {
            Debug.Log("Enemy Attack " + attckDirection);
            canAttack = false;
        }
    }
}
