using System.Collections.Generic;
using UnityEngine;


public class Init : MonoBehaviour
{

    //Mandatory scene that must be loaded
    List<string> MandatoryScenesToLoad = new List<string> { "LevelManager", "AudioManager" };

    //Extra scenes to load depending on the need
    public List<string> ExtraScenesToLoad = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {

        foreach (string sceneToLoad in MandatoryScenesToLoad)
        {
            if (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneToLoad).IsValid()) //If scene isnt loaded
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad, UnityEngine.SceneManagement.LoadSceneMode.Additive); //then load it additive(ly)
        }


        //Loop through the ExtraScenesToLoad List
        foreach (string s in ExtraScenesToLoad)
        {
            if (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(s).IsValid()) //If scene isnt loaded
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(s, UnityEngine.SceneManagement.LoadSceneMode.Additive); //then load it additive(ly)
        }

    }





}
