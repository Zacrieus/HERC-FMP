using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GameObject").GetComponent<Dialogue>().newText(GameObject.Find("Herc"), "Hello Custome Text", 5f, Color.grey);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
