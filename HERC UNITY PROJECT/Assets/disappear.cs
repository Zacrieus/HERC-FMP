using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Enemies").GetComponent<task>().enemiesDefeated == true)
        {
            Debug.Log("Done");
            Object.Destroy(gameObject,0f);
        }
    }
}
