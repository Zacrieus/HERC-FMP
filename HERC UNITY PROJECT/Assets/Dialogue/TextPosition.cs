using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPosition : MonoBehaviour
{
    GameObject followCharacter;
    bool canFollow = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followCharacter)
        { gameObject.transform.position = followCharacter.transform.position + new Vector3(0, 2, 0); }
    }

    public void follow(GameObject character)
    {
        followCharacter = character;
        canFollow = true;
    }
}
