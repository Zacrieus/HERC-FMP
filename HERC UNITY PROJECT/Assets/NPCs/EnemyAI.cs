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
    [Range(0, 100)] public float criticalChance;
    [SerializeField] float hurtDuration;

    //Attack
    bool canAttack = true;
    bool isSwinging = false;
    float attackTimer = 0f;
    bool isCrit;

    [Header("AttackSettings")]
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
    Color origColor; 

    //Setup
    SpriteRenderer sr;
    Vector3 spawnLocation;
    Vector2 moveDirection;
    float distanceFromPlayer;
    string attckDirection;
    Animator animator;
    string currentState;
    [SerializeField] AudioSource hit;
    [SerializeField] AudioSource hurt;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        animator = gameObject.transform.Find("Sprite").GetComponent<Animator>();
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

        if (Mathf.Abs(moveDirection.x) >= Mathf.Abs(moveDirection.y))
        {
            //Is Left or Right
            if (moveDirection.x >= 0)
            {
                gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(1, 1, 1);
            }
            else if (moveDirection.x < 0)
            {
                gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(-1, 1, 1);
            }


        }
        else if (Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.y))
        {
            //Is Up or Down
            if (moveDirection.y >= 0)
            {
                //attckDirection = "Up";
            }
            else if (moveDirection.y < 0)
            {
                //attckDirection = "Down";
            }
        }


        if (rb.velocity.x >= 0 && isSwinging == false)
        {
            gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
            ChangeAnim("MoveRight");
        }
        else if (rb.velocity.x < 0 && isSwinging == false)
        {
            gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(-Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
            ChangeAnim("MoveRight");
        }



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
                {
                    critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform);
                    gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
                    ChangeAnim("AttackUp");
                }
                else if (attckDirection == "Down")
                {
                    critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, -attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform);
                    gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
                    ChangeAnim("AttackDown");
                }
                if (attckDirection == "Right")
                {
                    critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform);
                    ChangeAnim("AttackRight");
                    gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
                }
                else if (attckDirection == "Left")
                {
                    critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(-attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform);
                    gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(-Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
                    ChangeAnim("AttackRight");
                }
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
                hit.Play();
                if (attckDirection == "Up")
                { 
                    critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform);
                    gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
                    ChangeAnim("AttackUp");
                }
                else if (attckDirection == "Down")
                { 
                    critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(0, -attackHitbox.transform.localScale.y * transform.localScale.y, 0), Quaternion.identity, transform);
                    gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
                    ChangeAnim("AttackDown");
                }
                if (attckDirection == "Right")
                { 
                    critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform);
                    ChangeAnim("AttackRight");
                    gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
                }
                else if (attckDirection == "Left")
                { 
                    critHitbox = Instantiate(attackHitbox, transform.position + new Vector3(-attackHitbox.transform.localScale.x * transform.localScale.y, 0, 0), Quaternion.identity, transform);
                    gameObject.GetComponentInChildren<Transform>().localScale = new Vector3(-Mathf.Abs(gameObject.GetComponentInChildren<Transform>().localScale.x), gameObject.GetComponentInChildren<Transform>().localScale.y, gameObject.GetComponentInChildren<Transform>().localScale.z);
                    ChangeAnim("AttackRight");
                }
                //critHitbox.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 156f / 255f, 0f / 255f, .5f);
                critHitbox.GetComponent<DamagePlayer>().damage *= 2;
                //CritFX HERE
                //Hit box is crit?
            }
        }
    }

    void FixedUpdate()
    {
        if (distanceFromPlayer > detectionRange)
        {
            Vector3 spawnDirection = (spawnLocation - transform.position).normalized;
            float spawnDistance = (transform.position - spawnLocation).magnitude;
            if (spawnDistance < 1f)
            { 
                rb.velocity = Vector2.zero;
                ChangeAnim("Idle");
            }
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
            {attckDirection = "Right";}
            else if (moveDirection.x < 0)
            {attckDirection = "Left";}


        }
        else if (Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.y))
        {
            //Is Up or Down
            if (moveDirection.y >= 0)
            {attckDirection = "Up";}
            else if (moveDirection.y < 0)
            {attckDirection = "Down";}
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

    void ChangeAnim(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    public void takeDamage()
    {
        hurt.Play();
        health -= 1;
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
    }

    void attackChance()
    { 
        if (Random.Range(0,100) > 100 - criticalChance)
        {isCrit = true;}
        else
        {isCrit = false;}
    }

}
