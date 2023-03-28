using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class ArrowPoint : MonoBehaviour
    {

        CharacterController controller;

        // Use this for initialization
        void Start()
        {
            controller = transform.parent.GetComponentInParent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {

            SpriteRenderer render = GetComponent<SpriteRenderer>();
            render.enabled = controller.RotationDirection != 0;
            render.flipX = controller.RotationDirection > 0;
        }
    }
}