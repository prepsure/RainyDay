using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerRestartScene : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }


}
