using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class task : MonoBehaviour
{
    GameObject enemies;

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
            eventChange();
        }

    }

    public void eventChange()
    {

    }
}
