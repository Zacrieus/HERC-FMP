using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hind : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    [SerializeField] float hurtDuration;
    SpriteRenderer sr;
    
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage()
    {
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
}
