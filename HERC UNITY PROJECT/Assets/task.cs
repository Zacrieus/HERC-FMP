using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class task : MonoBehaviour
{
    GameObject enemies;
    public int taskStage = 1;

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
        if (enemies.transform.childCount == 0)
        {
            taskStage += 1;
        }

    }

}
