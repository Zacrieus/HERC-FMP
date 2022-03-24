using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;

    [Header("Stats")]
    public bool rageState;
    [SerializeField] GameObject arrow;
    [SerializeField] [Range(0,10)]float arrowSpeed;

    [Header("Attack Settings")]
    bool canAttack = true;
    float attackTimer;
    [SerializeField][Range(0,10)]float attackCD;


    //Setup
    Vector2 playerDirection;
    float distanceFromPlayer;
    string attckDirection;
    GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = (player.transform.position - transform.position).normalized;
        distanceFromPlayer = (transform.position - player.transform.position).magnitude;
        if (canAttack)
        {
            random();
            canAttack = false;
            //rb.MovePosition(player.transform.position);
        }

        if (canAttack == false)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCD)
            {
                canAttack = true;
                attackTimer = 0f;
            }
        }
    }

    void random()
    {
        float rng = Mathf.Round(Random.Range(1f, 2f));
        //Debug.Log(rng);
        
        //attack 1 rannge homing
        if (rng == 1)
        {rangeAttack();}
        else if (rng == 2)
        {
            shotgunAttack(5,4);
        }
        //attack 2 multishot
    }

    void rangeAttack()
    {
        //Debug.Log("Pew");
        hitbox = Instantiate(arrow, transform.position, Quaternion.identity, transform);

        rotateTo2D(hitbox,player.transform.position);

        hitbox.GetComponent<Rigidbody2D>().AddForce(playerDirection * 5 * arrowSpeed * 20, ForceMode2D.Force);
        Object.Destroy(hitbox, 10);
    }

    void shotgunAttack(float amount,float scale)
    {
        for (int i = 0; i < amount; i++)
        {
            hitbox = Instantiate(arrow, transform.position, Quaternion.identity, transform);
            float randomX = Random.Range(playerDirection.x - scale/10, playerDirection.x + scale/10);
            float randomY = Random.Range(playerDirection.y - scale/10, playerDirection.y + scale/10);
            Vector2 randomDirection = new Vector2(randomX, randomY)*5;
            
            rotateTo2D(hitbox, randomDirection * 5 * arrowSpeed *20);
            hitbox.GetComponent<Rigidbody2D>().AddForce(randomDirection * 5 * arrowSpeed * 20, ForceMode2D.Force);
        }
    }


    void rotateTo2D(GameObject objecy,Vector3 destination)
    {
        //Rotate Towards Player
        float yVal = objecy.transform.position.y - destination.y;
        float xVal = objecy.transform.position.x - destination.x;
        float zAngRotation = Mathf.Atan2(yVal, xVal) * Mathf.Rad2Deg;
        objecy.transform.rotation = Quaternion.Euler(0, 0, zAngRotation + 90);
    }
}
