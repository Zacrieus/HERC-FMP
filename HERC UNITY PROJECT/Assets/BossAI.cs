using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    GameObject player;

    [Header("Stats")]
    public bool rageState;
    [SerializeField] GameObject arrow;
    [SerializeField] [Range(0,10)]float arrowSpeed;

    [Header("Attack Settings")]
    bool canAttack = true;
    float attackTimer;
    [SerializeField][Range(3,10)]float attackCD;


    //Setup
    Vector2 playerDirection;
    float distanceFromPlayer;
    string attckDirection;
    GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
        float rng = Mathf.Round(Random.Range(1f, 1f));
        Debug.Log(rng);
        
        //attack 1 rannge homing
        if (rng == 1)
        {rangeAttack();}
        else if (rng == 2)
        {
            Debug.Log("Multishot");
        }
        //attack 2 multishot
    }

    void rangeAttack()
    {
        Debug.Log("Pew");
        hitbox = Instantiate(arrow, transform.position, Quaternion.identity, transform);
        //hitbox.transform.LookAt(player.transform,);
        //hitbox.transform.rotation.
        hitbox.GetComponent<Rigidbody2D>().AddForce(playerDirection * arrowSpeed * 100, ForceMode2D.Force);
        Object.Destroy(hitbox, 10);
    }
}
