using System.Collections;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public AnimationClip _brellaOpen;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // play animation ONCE
        }

        if (Input.GetMouseButtonDown(0))
        {
            // set to frame 0 and stop playing animations
        }
    }
}