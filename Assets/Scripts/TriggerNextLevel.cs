using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextLevel : MonoBehaviour
{

    enum Level
    {
        Begin = 1,
        Alleyway = 2,
        QuickTime = 3,
        Swing = 4,

        Bounce = 5,
        Loop = 6,
        Inward = 7,
        Distracted = 8,
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        int levelNumber = (int)Enum.Parse(typeof(Level), SceneManager.GetActiveScene().name);

        Debug.Log((Level)(levelNumber + 1));
        string nextSceneName = ((Level)(levelNumber + 1)).ToString();

        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
    }


}
