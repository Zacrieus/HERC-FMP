using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] float health;
    SpriteRenderer sr;
    [SerializeField][Range(1,5)] int noOfCounters;
    int attackCounter = 0;
    float attackTimer = 0f;
    float basicCharge = 1;
    float shotgunCharge = 3;
    float homignCharge = 3;
    bool hasAttacked = true;
    float rng;

    // Start is called before the first frame update
    void Start()
    { 
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCounter < noOfCounters)
        {

            if (hasAttacked == true)
            { rng = Random.Range(0, 4f); hasAttacked = false; }

            Debug.Log(rng);
            if (rng == 1f)
            {
                //attack
                if (attackTimer < basicCharge)
                { attackTimer += Time.deltaTime; }
                else
                {
                    Debug.Log("Attack");
                    attackTimer = 0f;
                    hasAttacked = true;
                    attackCounter += 1;
                }
            }
            else if (rng == 2f)
            {
                //Shotgun
            }
            else if (rng == 3f)
            {
                //Homing
            }
        }
        else if  (attackCounter > noOfCounters)
        {
            Debug.Log(attackCounter);
            attackCounter = 0;
        }
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
}
