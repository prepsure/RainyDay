using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextLevel : MonoBehaviour
{

    enum Level
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,

        Level5 = 5,
        Level6 = 6,
        Level7 = 7,
        Level8 = 8,
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        int levelNumber = (int)Enum.Parse(typeof(Level), SceneManager.GetActiveScene().name);

        string nextSceneName = ((Level)(levelNumber + 1)).ToString();

        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
    }


}
