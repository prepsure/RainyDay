﻿using System.Threading;
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
        if (Input.GetMouseButtonUp(0))
        {
            _animController.SetTrigger("Open");
        }

        if (Input.GetMouseButtonDown(0))
        {
            _animController.SetTrigger("Close");
        }
    }


}