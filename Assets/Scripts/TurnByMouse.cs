using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class TurnByMouse : MonoBehaviour
    {
        // swing mode vars
        Quaternion SWING_INITIAL_ROTATION = Quaternion.Euler(Vector3.forward * 270);

        // aim mode vars
        Quaternion AIM_INITIAL_ROTATION = Quaternion.Euler(Vector3.forward * 90);
        const float TURN_SPEED = 10f;

        float _lastAngle = 45; // set to an arbitrary number not divisible by 90 degrees so it will always respond to the mouse on init
        float _currentAngle = 0;

        float _realCharacterAngleWhenTurned = 0;
        float _lastTurnedTime = 0;

        bool _isTurning = false;

        // other vars
        

        Camera _camera;

        BrellaInputs _mode;
        float _realCharacterAngle = 0;

        // Use this for initialization
        void Start()
        {
            _camera = FindObjectOfType<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (UmbrellaInput.GetUp(BrellaInputs.Swing))
            {
                // snap to orthogonal
            }

            if (UmbrellaInput.Get(BrellaInputs.Swing) && CharacterController.NearPole(transform.position, out _))
            {
                _mode = BrellaInputs.Swing;
            }
            else if (UmbrellaInput.Get(BrellaInputs.Aim))
            {
                _mode = BrellaInputs.Aim;
            }
            else
            {
                _mode = BrellaInputs.None;
            }

            if (_mode == BrellaInputs.Aim || _isTurning)
            {
                Aim();
            } 
            else if (_mode == BrellaInputs.Swing)
            {
                Swing();
            }
        }

        void Aim()
        {
            if (UmbrellaInput.Get(BrellaInputs.Aim))
            {
                _currentAngle = RoundTo90Deg(Mathf.Rad2Deg * GetAngleBetweenMouseAndCharacter());
            }

            if (NormalizeAngle(_currentAngle) != NormalizeAngle(_lastAngle))
            {
                _isTurning = true;
                _realCharacterAngleWhenTurned = _realCharacterAngle;
                _lastTurnedTime = Time.time;
            }

            if (_isTurning)
            {
                float turnAlpha = (Time.time - _lastTurnedTime) * TURN_SPEED;
                _realCharacterAngle = Mathf.LerpAngle(_realCharacterAngleWhenTurned, _currentAngle, Mathf.Min(1, turnAlpha));

                transform.rotation = _camera.transform.rotation * Quaternion.Euler(Vector3.forward * _realCharacterAngle) * AIM_INITIAL_ROTATION;
                _isTurning = turnAlpha < 1;
            }

            _lastAngle = _currentAngle;
        }

        void Swing()
        {
            CharacterController.NearPole(transform.position, out GameObject pole);

            // turn to face it
            if (pole != null)
            {
                Vector3 radiusVector = Vector3.Scale(Vector3.Normalize(transform.position - pole.transform.position), new Vector3(1, 0, 1));
                float angle = CharacterController.VectorToAngle(radiusVector);
                transform.rotation = _camera.transform.rotation * Quaternion.Euler(angle * Mathf.Rad2Deg * Vector3.forward) * SWING_INITIAL_ROTATION;

                // required because character angle changes, so aiming needs to know its not aiming at the right place anymore
                _lastAngle = angle;
            }
        }

        float GetAngleBetweenMouseAndCharacter()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 charPos = _camera.WorldToScreenPoint(transform.position);

            Vector2 difference = mousePos - charPos;

            return Mathf.Atan2(difference.y, difference.x);
        }

        float Mod(float a, float n)
        {
            return a - n * Mathf.Floor(a / n);
        }

        float NormalizeAngle(float angle)
        {
            return Mod(angle, 360);
        }

        float RoundTo90Deg(float degAngle)
        {
            return Mathf.Round(degAngle / 90f) * 90f;
        }
    }
}