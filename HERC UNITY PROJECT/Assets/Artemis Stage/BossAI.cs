using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;

    [Header("Stats")]
    public bool rageState;
    public float health;

    [SerializeField] GameObject arrow;
    [SerializeField] [Range(0,10)]float arrowSpeed;
    [Range(0, 100)] public float criticalChance;
    [Range(0, 100)] public float movePercentageChance;
    [SerializeField] [Range(0, 20)] float MoveSpeed;

    [Header("Attack Settings")]
    bool canAttack = true;
    bool hasShot;
    float attackTimer;
    [SerializeField] [Range(0, 2)] float attackWindUp;
    [SerializeField][Range(0,5)]float attackCD;

    [SerializeField] [Range(0, 10)] float critkWindUp;
    [SerializeField] [Range(0, 10)] float critCD;

    [Header("Setup")]
    Vector3 newPos;
    bool isCrit;
    GameObject smoke;
    [SerializeField] GameObject smokeFX;
    GameObject critTri;
    [SerializeField] GameObject critFX;
    bool Indicate = false;
    SpriteRenderer sr;
    float hurtDuration = 1;

    //Setup
    Vector2 playerDirection;
    GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        newRandomPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > transform.position.x)
        { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }
        else
        { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }

        if ((transform.position - newPos).magnitude > 1)
        {
            Vector3 moveDirection = (newPos - transform.position).normalized;
            rb.velocity = moveDirection * MoveSpeed;
        }
        else
        {rb.velocity = Vector2.zero;}

        if (canAttack)
        {
            attackChance();
            canAttack = false;
            hasShot = false;
            attackTimer = 0f;
        }

        if (canAttack == false && isCrit == false)
        {
            if (rb.velocity == Vector2.zero)
            { attackTimer += Time.deltaTime; }

            if (attackTimer >= attackWindUp - .75f && Indicate == false)
            {
                Indicate = true;
                smoke = Instantiate(smokeFX, transform.position + new Vector3(0, -2, 0), Quaternion.identity);
                smoke.transform.parent = transform;
                Object.Destroy(smoke, .7f);
            }

            if (attackTimer >= attackWindUp)
            {   
                if (hasShot == false)
                {
                    rangeAttack();
                    hasShot = true;
                }
            }

            if (attackTimer >= attackCD)
            { Indicate = false; canAttack = true;}
        }
        else if (canAttack == false && isCrit == true)
        {
            if (rb.velocity == Vector2.zero) 
            { attackTimer += Time.deltaTime; }
            if (attackTimer >= critkWindUp - .75f && Indicate == false)
            {
                Indicate = true;
                smoke = Instantiate(smokeFX, transform.position + new Vector3(0, -2, 0), Quaternion.identity);
                smoke.transform.parent = transform;
                Object.Destroy(smoke, .7f);
                critTri = Instantiate(critFX, transform.position + new Vector3(0, +2, 0), Quaternion.identity);
                critTri.transform.parent = transform;
                Object.Destroy(critTri, .75f);
            }

            if (attackTimer >= critkWindUp )
            {
                if (hasShot == false)
                {
                    CritAttack();
                    hasShot = true;
                }
            }

            if (attackTimer >= critCD)
            {
                Indicate = false;
                canAttack = true;
                newRandomPos();
            }
        }
    }

    public void takeDamage()
    {
        health -= 1;
        StartCoroutine(onHurt());
        if (health <= 0)
        {
            //GameObject.Find("Enemies").GetComponent<task>().onEventCheck();
            Object.Destroy(gameObject, 0);
        }
    }

    IEnumerator onHurt()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(hurtDuration);
        sr.color = Color.white;
    }

    void rangeAttack()
    {
        //Debug.Log("Pew");
        moveChance();
        hitbox = Instantiate(arrow, transform.position, Quaternion.identity);
        rotateTo2D(hitbox,player.transform.position);
        playerDirection = (player.transform.position - transform.position).normalized;
        hitbox.GetComponent<Rigidbody2D>().AddForce(playerDirection * arrowSpeed * 100, ForceMode2D.Force);
        Object.Destroy(hitbox, 10);
    }

    void CritAttack()
    {
        hitbox = Instantiate(arrow, transform.position, Quaternion.identity);
        rotateTo2D(hitbox, player.transform.position);
        playerDirection = (player.transform.position - transform.position).normalized;
        hitbox.GetComponent<Rigidbody2D>().AddForce(playerDirection * arrowSpeed * 200, ForceMode2D.Force);
        hitbox.GetComponent<DamagePlayer>().damage *= 2;
        Object.Destroy(hitbox, 10);
    }

    void rotateTo2D(GameObject objecy,Vector3 destination)
    {
        //Rotate Towards Player
        float yVal = objecy.transform.position.y - destination.y;
        float xVal = objecy.transform.position.x - destination.x;
        float zAngRotation = Mathf.Atan2(yVal, xVal) * Mathf.Rad2Deg;
        objecy.transform.rotation = Quaternion.Euler(0, 0, zAngRotation + 90);
    }

    void attackChance()
    {
        if (Random.Range(0, 100) > 100 - criticalChance)
        {
            isCrit = true;
        }
        else
        {
            isCrit = false;
        }
    }

    void moveChance()
    {
        if (Random.Range(0, 100) > 100 - movePercentageChance)
        { newRandomPos(); }
    }

    void newRandomPos()
    {
        float centeredScale = .75f;
        float inverse = Mathf.Abs(1 - centeredScale);

        float RandomY = Random.Range
        (Camera.main.ScreenToWorldPoint(new Vector3(0, 0+ Screen.height * inverse, 1)).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * centeredScale, 1)).y);

        float RandomX = Random.Range
        (Camera.main.ScreenToWorldPoint(new Vector3(0+Screen.width * inverse, 0, 1)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * centeredScale, 0, 1)).x);

        newPos = new Vector3(RandomX,RandomY,1);
    }
}
