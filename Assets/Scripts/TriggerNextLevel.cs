using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextLevel : MonoBehaviour
{

    enum Level
    {
        Title = 0,

        Begin = 1,
        Alleyway = 2,
        QuickTime = 3,
        Swing = 4,

        Bounce = 5,
        Loop = 6,
        Inward = 7,
        Distracted = 8,

        ThankYou = 9,
    }

    private void Update()
    {
        // load level 1 when r is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(((Level)0).ToString(), LoadSceneMode.Single);
        }

        int levelNumber = (int)Enum.Parse(typeof(Level), SceneManager.GetActiveScene().name);
        if (levelNumber == 0 && Input.GetMouseButtonUp(0))
        {
            LoadNextScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        int levelNumber = (int)Enum.Parse(typeof(Level), SceneManager.GetActiveScene().name);

        string nextSceneName = ((Level)(levelNumber + 1)).ToString();

        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
    }


}
