using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    enum BrellaInputs
    {
        Aim,
        Swing,
    }

    internal class UmbrellaInput
    {
        private static readonly Dictionary<BrellaInputs, KeyCode> keyInputs = new()
        {
            { BrellaInputs.Aim, KeyCode.Q },
            { BrellaInputs.Swing, KeyCode.W },
        };

        private static readonly Dictionary<BrellaInputs, int> mouseInputs = new()
        {
            { BrellaInputs.Aim, 0 },
            { BrellaInputs.Swing, 1 },
        };

        public static bool Get(BrellaInputs input)
        {
            return Input.GetKey(keyInputs[input]) || Input.GetMouseButton(mouseInputs[input]);
        }

        public static bool GetDown(BrellaInputs input)
        {
            return Input.GetKeyDown(keyInputs[input]) || Input.GetMouseButtonDown(mouseInputs[input]);
        }

        public static bool GetUp(BrellaInputs input)
        {
            return Input.GetKeyUp(keyInputs[input]) || Input.GetMouseButtonUp(mouseInputs[input]);
        }

    }
}
