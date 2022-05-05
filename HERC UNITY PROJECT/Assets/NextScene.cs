using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] public string nextScene;
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene(nextScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log(other);
            if (GameObject.Find("Enemies").transform.childCount <= 0)
            {
                Debug.Log("End");
                Scene currentScene = SceneManager.GetActiveScene();
                string sceneName = currentScene.name;

                Scene newScene = SceneManager.GetSceneByName(nextScene);
                if ((sceneName != nextScene) && (newScene.IsValid() == true))
                { SceneManager.LoadScene(nextScene); }
            }
        }
        else
        { Debug.Log(other); }
    }

    */
    public void loadScene()
    {
        SceneManager.LoadScene(nextScene);

        /*
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        Scene newScene = SceneManager.GetSceneByName(nextScene);
        if ((sceneName != nextScene) && (newScene.IsValid() == true))
        { SceneManager.LoadScene(nextScene); }
        else
        { Debug.Log("Invalid Scene: " + nextScene); SceneManager.LoadScene(nextScene); }
        */
    }
}
