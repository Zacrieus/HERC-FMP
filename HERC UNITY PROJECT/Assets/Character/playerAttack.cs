using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
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
        if (col)
        {
            //Debug.Log(col);

            if (col.transform.tag == "Enemy")
            {
                col.transform.GetComponent<EnemyAI>().takeDamage();
                //Object.Destroy(gameObject, 0);
            }
            if (col.transform.tag == "Boss")
            {
                col.transform.GetComponent<BossAI>().takeDamage();
                //Object.Destroy(gameObject, 0);
            }
            if (col.transform.tag == "Objective")
            {
                Debug.Log("Load");
                col.transform.GetComponent<NextScene>().loadScene();
            }
        }
    }
}
