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

    [Header("GeneralSettings")]
    public float setHealth;
    [Range(0, 5)] public float setMoveSpeed;
    [Range(0, 25)] public float detectionRange;
                                                                                                             
    //Attack
    bool canAttack = true;
    bool isSwinging = false;
    float attackTimer = 0f;
    bool isCrit;

    [Header("AttackSettings")]
    [Range(0, 100)] public float criticalChance;
    [SerializeField] [Range(0, 3)] float critkWindUp;
    [SerializeField] [Range(0, 5)] float critDuration;

    [Range(0, 25)] public float attackRange;
    [SerializeField] [Range(0, 3)] float attackWindUp;
    [SerializeField] [Range(0, 5)] float attackDuration;
    [SerializeField] [Range(0, 10)] float attackCd;

    [Header("Setup")]
    [SerializeField] GameObject attackHitbox;
    [SerializeField] GameObject smokeFX;
    [SerializeField] GameObject critFX;
    GameObject smoke;
    GameObject critTri;
    GameObject critHitbox;
    GameObject hitbox;

    //Setup
    Vector3 spawnLocation;
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
        spawnLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement and Distance
        moveDirection = (player.transform.position - transform.position).normalized;
        distanceFromPlayer = (transform.position - player.transform.position).magnitude;


        //attacking
        if (canAttack == false && isCrit == false)
        {
            attackTimer += Time.deltaTime;
            //Debug.Log(attackTimer);
            if (attackTimer >= attackCd)
            {
                //cooldown
                canAttack = true;
                attackTimer = 0f;
                isSwinging = false;
                Object.Destroy(hitbox, 0);
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

        //Crit Attack
        else if (canAttack == false && isCrit == true)
        {
            attackTimer += Time.deltaTime;
            
            if (attackTimer >= attackCd)
            {
                //cooldown
                canAttack = true;
                attackTimer = 0f;
                isCrit = false;
                isSwinging = false;
                Object.Destroy(critHitbox, 0);
            }
            if (isSwinging == true && attackTimer >= critDuration)
            {
                isSwinging = false;
                Object.Destroy(critHitbox, 0);

            }
            else if (isSwinging == false && attackTimer >= critkWindUp && attackTimer < critDuration + critkWindUp)
            {
                //Debug.Log("hi");
                //attackStart
                isSwinging = true;

                if (attckDirection == "Up")
                { critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform); }
                else if (attckDirection == "Down")
                { critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, -attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform); }
                if (attckDirection == "Right")
                { critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform); }
                else if (attckDirection == "Left")
                { critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(-attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform); }
                Debug.Log(critHitbox);
                critHitbox.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 156f / 255f, 0f / 255f,.5f);
                //CritFX HERE
                //Hit box is crit?
            }

        }

        Debug.Log(isSwinging+"---"+isCrit);

    }

    void FixedUpdate()
    {
        if (distanceFromPlayer > detectionRange)
        {
            Vector3 spawnDirection = (spawnLocation - transform.position).normalized;
            float spawnDistance = (transform.position - spawnLocation).magnitude;
            if (spawnDistance < 1f)
            { rb.velocity = Vector2.zero; }
            else
            {rb.velocity = spawnDirection * moveSpeed;}
        }
        else if (distanceFromPlayer <= attackRange && distanceFromPlayer != 0)
        {
            rb.velocity = Vector2.zero;
            if (canAttack)
            {attack();}
        }
        else if (distanceFromPlayer < detectionRange)
        { rb.velocity = moveDirection * moveSpeed; }
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

        attackChance();

        //Inditcators
        smoke = Instantiate(smokeFX, transform.position + new Vector3(0,-Mathf.Abs(transform.localScale.y/2), 0), Quaternion.identity, transform);
        Object.Destroy(smoke, .7f);
        if (isCrit == true)
        {
            critTri = Instantiate(critFX, transform.position + new Vector3(0, +2, 0), Quaternion.identity, transform);
            Object.Destroy(critTri,1);
        }

        if (canAttack == true)
        { canAttack = false; }
    }


    void attackChance()
    { 
        if (Random.Range(0,100) > 100 - criticalChance)
        {
            isCrit = true;
        }
        else
        {
            isCrit = false;
        }
    }
}
