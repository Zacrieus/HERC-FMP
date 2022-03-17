using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Rigidbody2D rb;
    public float movementSpeed; 


    float moveX;
    float moveY;
    public Vector2 moveVector;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        //Movement
        moveY = Input.GetAxis("Vertical") * movementSpeed;
        moveX = Input.GetAxis("Horizontal") * movementSpeed;
        moveVector = new Vector2(moveX,moveY);

        rb.velocity += moveVector;

        //Atack
        if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed primary button.");

        //Block
        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        //Dodge
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Dodge");
        }
    }

    void MoveCharacter()
    {
        //character.Trasnform.Position = 
    }
}
