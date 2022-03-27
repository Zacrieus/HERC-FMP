using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] [Range(0,2)] int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<Character>().takeDamage(damage);
            Object.Destroy(gameObject, 0);
        }
    }
}
