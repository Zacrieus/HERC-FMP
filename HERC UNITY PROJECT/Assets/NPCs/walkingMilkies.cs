using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingMilkies : MonoBehaviour
{
    Rigidbody2D rb;

    Vector2 moveDir;
    float moveDuration;
    float moveTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        moveDuration = Random.Range(1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(moveTime < moveDuration)
        { moveTime += Time.deltaTime; } 
        else
        { 
            moveDuration = moveDuration = Random.Range(1f, 3f);
            moveDir = new Vector2(Random.RandomRange(-1f, 1), Random.RandomRange(-1f, 1));
            moveTime = 0f;
        }

        rb.velocity = moveDir;
    }
}
