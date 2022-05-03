using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] GameObject Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newText(GameObject charater, string dialogueText,float duration, Color col)
    {
        // new
        GameObject newText = Instantiate(Text,charater.transform.position,Quaternion.identity);
        newText.GetComponent<TextMesh>().text = dialogueText;
        newText.GetComponent<TextMesh>().color = col;
        Object.Destroy(newText, duration);
    }
}
