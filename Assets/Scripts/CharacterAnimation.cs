﻿using Assets.Scripts;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator _animController;

    // Use this for initialization
    void Start()
    {
        _animController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UmbrellaInput.GetDown(BrellaInputs.Swing))
        {
            _animController.SetTrigger("StartSwing");
        }

        if (UmbrellaInput.GetUp(BrellaInputs.Swing))
        {
            _animController.SetTrigger("StopSwing");
        }



        if (UmbrellaInput.GetDown(BrellaInputs.Aim))
        {
            _animController.SetTrigger("Close");
        }
        else if (UmbrellaInput.GetUp(BrellaInputs.Aim))
        {
            _animController.SetTrigger("Open");
        }
    }


}