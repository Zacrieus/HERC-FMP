using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class task : MonoBehaviour
{
    GameObject enemies;
    public bool enemiesDefeated = false;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.Find("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
        //enemies.transform.childCount
    }

    public void onEventCheck()
    {
        Debug.Log(enemies.transform.childCount);
        if (enemies.transform.childCount <= 0)
        {
            Debug.Log("Complete");
            enemiesDefeated = true;
        }

    }

}
