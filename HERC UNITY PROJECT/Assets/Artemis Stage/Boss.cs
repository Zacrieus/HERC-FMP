using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    GameObject player;
    [SerializeField] float health;
    SpriteRenderer sr;
    [SerializeField][Range(1,5)] int noOfCounters;
    [SerializeField] float exhaust;
    int attackCounter = 0;
    float attackTimer = 0f;
    float basicCharge = 1;
    float shotgunCharge = 3;
    float homingCharge = 3;
    bool hasAttacked = true;
    float rng;

    [SerializeField] GameObject arrow;

    Vector2 playerDirection;
    Vector3 newPos;
    GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    { 
        sr = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCounter < noOfCounters)
        {

            if (hasAttacked == true)
            { rng = Mathf.Floor(Random.Range(1f, 3f)); hasAttacked = false; }

            //Debug.Log(rng);
            if (rng == 1f)
            {
                //attack
                if (attackTimer < basicCharge)
                { attackTimer += Time.deltaTime; }
                else
                {
                    Debug.Log("Attack");
                    rangeAttack();
                    attackCounter += 1;
                    hasAttacked = true;
                    attackTimer = 0f;
                }
            }
            else if (rng == 2f)
            {
                //Shotgun
                if (attackTimer < shotgunCharge)
                { attackTimer += Time.deltaTime; }
                else
                {
                    Debug.Log("Shotgun");
                    shotgunAttack();
                    attackCounter += 1;
                    hasAttacked = true;
                    attackTimer = 0f;
                }
            }
            else if (rng == 3f)
            {
                //Homing
                if (attackTimer < homingCharge)
                { attackTimer += Time.deltaTime; }
                else
                {
                    Debug.Log("Homing");
                    attackCounter += 1;
                    hasAttacked = true;
                    rangeAttack();
                    attackTimer = 0f;
                }
            }
        }
        else if(attackCounter >= noOfCounters)
        {
            Debug.Log(attackCounter);
            attackCounter = 0;
        }
    }

    void rangeAttack()
    {
        //Debug.Log("Pew");
        hitbox = Instantiate(arrow, transform.position, Quaternion.identity);
        rotateTo2D(hitbox, player.transform.position);
        playerDirection = (player.transform.position - transform.position).normalized;
        hitbox.GetComponent<Rigidbody2D>().AddForce(playerDirection * 500, ForceMode2D.Force);
        Object.Destroy(hitbox, 10);
    }

    void shotgunAttack()
    {
        //Debug.Log("Pew");
        float shotgunAmmount = 5;
        for (int i = 0; i < shotgunAmmount; i++)
        {
            hitbox = Instantiate(arrow, transform.position, Quaternion.identity);
            rotateTo2D(hitbox, player.transform.position);
            float varienceScale = 5;
            Vector3 varience = new Vector3(Random.Range(-varienceScale, varienceScale), Random.Range(-varienceScale, varienceScale),1);
            Debug.Log(varience);
            playerDirection = ((player.transform.position + varience) - transform.position).normalized;
            rotateTo2D(hitbox, player.transform.position + varience);
            hitbox.GetComponent<Rigidbody2D>().AddForce((playerDirection * 500), ForceMode2D.Force);
            Object.Destroy(hitbox, 10);
        }
    }

    void rotateTo2D(GameObject objecy, Vector3 destination)
    {
        //Rotate Towards Player
        float yVal = objecy.transform.position.y - destination.y;
        float xVal = objecy.transform.position.x - destination.x;
        float zAngRotation = Mathf.Atan2(yVal, xVal) * Mathf.Rad2Deg;
        objecy.transform.rotation = Quaternion.Euler(0, 0, zAngRotation + 90);
    }

    public void takeDamage()
    {
        health -= .5f;
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
        yield return new WaitForSeconds(1);
        sr.color = Color.white;
    }

    void newRandomPos()
    {
        float centeredScale = .75f;
        float inverse = Mathf.Abs(1 - centeredScale);

        float RandomY = Random.Range
        (Camera.main.ScreenToWorldPoint(new Vector3(0, 0 + Screen.height * inverse, 1)).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * centeredScale, 1)).y);

        float RandomX = Random.Range
        (Camera.main.ScreenToWorldPoint(new Vector3(0 + Screen.width * inverse, 0, 1)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * centeredScale, 0, 1)).x);

        newPos = new Vector3(RandomX, RandomY, 1);
    }
}
