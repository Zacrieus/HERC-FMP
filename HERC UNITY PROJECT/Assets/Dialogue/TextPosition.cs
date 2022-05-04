using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPositiion : MonoBehaviour
{
    public GameObject follow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (follow)
        {
            Debug.Log(follow);
            gameObject.transform.position = follow.transform.position + new Vector3(0, 2, 0);
        }
    }
}
