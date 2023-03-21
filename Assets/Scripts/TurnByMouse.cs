using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class TurnByMouse : MonoBehaviour
    {
        Quaternion INITIAL_ROTATION = Quaternion.Euler(Vector3.forward * 90);
        float TURN_SPEED = 10f;

        float _lastAngle = 0;
        float _currentAngle = 0;
        float _realAngle = 0;

        float _realCharacterAngle = 0;
        float _realCharacterAngleWhenTurned = 0;

        float _lastTurnedTime = 0;

        bool _isTurning = true;

        Camera _camera;

        // Use this for initialization
        void Start()
        {
            _camera = FindObjectOfType<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                _realAngle = Mathf.Rad2Deg * GetAngleBetweenMouseAndCharacter();
                _currentAngle = RoundTo90Deg(_realAngle);
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

                transform.rotation = _camera.transform.rotation * Quaternion.Euler(Vector3.forward * _realCharacterAngle) * INITIAL_ROTATION;
                _isTurning = turnAlpha < 1;
            }

            _lastAngle = _currentAngle;
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