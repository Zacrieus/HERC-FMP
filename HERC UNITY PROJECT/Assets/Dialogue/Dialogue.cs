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
        //newText.transform.SetParent(charater.transform);  //flips
        //newText.GetComponent<TextPositiion>().follow = charater;

        newText.GetComponent<TMPro.TextMeshPro>().text = dialogueText;
        newText.GetComponent<TMPro.TextMeshPro>().color = col;
        Object.Destroy(newText, duration);
        newText.GetComponent<TextPositiion>().follow = charater;
    }
}
