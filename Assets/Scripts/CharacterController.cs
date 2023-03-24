using Assets.Scripts;
using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    const float SPEED = 8f;
    const float SWING_SPEED = 2 / 3f * SPEED;

    const int PUSH_OFF_MASK = 1 << 6;
    const float TIME_SLOW_MULTIPLIER = 1f / 8f;

    const float SWING_RADIUS = 0.8f;
    const float POLE_GRAB_DISTANCE = 0.9f;

    event Action OnLeftMouseUp;
    event Action OnRightMouseUp;

    Camera _camera;
    GameObject _poleAttachedTo;
    
    Vector3 _velocity = Vector3.zero;
    bool _attachedToPole = false;
    float _timeScale = 1;

    // Start is called before the first frame update
    void Start()
    {
        _camera = FindObjectOfType<Camera>();

        OnLeftMouseUp += () =>
        {
            Vector3 vel = SnapToOrthogonal(GetDirectionOfMouseRelativeToCharacter(Input.mousePosition));

            if(VelocityHittingWall(vel))
            {
                SnapPositionToInt();
                BlastInDirection(-vel);
            }
        };

        OnRightMouseUp += () =>
        {
            // wait until the character is roughly in the right place

            // snap position to a grid space
            // snap velocity to an orthogonal direction
            SnapPositionToInt();
            _attachedToPole = false;
        };
    }

    // Update is called once per frame
    void Update()
    {
        // mouse events
        if (UmbrellaInput.GetUp(BrellaInputs.Aim))
        {
            OnLeftMouseUp();
        }

        if (UmbrellaInput.GetUp(BrellaInputs.Swing))
        {
            OnRightMouseUp();
        }

        // look for pole to grab
        if (UmbrellaInput.Get(BrellaInputs.Swing))
        {
            if (NearPole(transform.position, out var pole))
            {
                // only lock to pole if near the position where their velocity is pointing
                _poleAttachedTo = pole;
                _attachedToPole = true;
            }
        }

        SetTimeScale();
        UpdateVelocity();
    }

    void SetTimeScale()
    {
        if (UmbrellaInput.Get(BrellaInputs.Aim))
        {
            _timeScale = TIME_SLOW_MULTIPLIER;
        }
        else
        {
            _timeScale = 1;
        }
    }

    void UpdateVelocity()
    {
        if (_attachedToPole)
        {
            float rotationDirection = GetRotationDirection(_poleAttachedTo.transform.position, transform.position, _velocity);
            RotateAround(transform.position, _poleAttachedTo.transform.position, rotationDirection, Time.deltaTime);
        } else if (VelocityHittingWall(_velocity))
        {
            Hault();
        }
        else
        {
            MoveWithVelocity();
        }
    }

    Vector3 GetDirectionOfMouseRelativeToCharacter(Vector2 mouseScreenPos)
    {
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(mouseScreenPos);
        Vector3 playerPos = transform.position;
        Vector3 zeroedDir = Vector3.Scale(mouseWorldPos - playerPos, Vector3.one - Vector3.up);

        return Vector3.Normalize(zeroedDir);
    }

    float GetRotationDirection(Vector3 centerOfCircle, Vector3 pointOnCircle, Vector3 velocity)
    {
        centerOfCircle = Vector3.Scale(centerOfCircle, Vector3.one - Vector3.up);
        pointOnCircle = Vector3.Scale(pointOnCircle, Vector3.one - Vector3.up);

        Vector3 radius = -pointOnCircle + centerOfCircle;

        return Mathf.Sign(Vector3.Dot(Vector3.Cross(radius, velocity), Vector3.up));
    }

    Vector3 SnapToOrthogonal(Vector3 v3)
    {
        Vector3 primaryDir = Vector3.forward;

        if (Math.Abs(v3.x) > Math.Abs(v3.z))
        {
            primaryDir = Vector3.right;
        }

        Vector3 correctedDir = Vector3.Scale(primaryDir, v3);

        return Vector3.Normalize(correctedDir);
    }

    void BlastInDirection(Vector3 dir)
    {
        _velocity = dir * SPEED;
    }

    bool VelocityHittingWall(Vector3 vel)
    {
        // raycast from the player transform by the velocity to see if its hitting the wall
        return Physics.Raycast(transform.position, vel, transform.localScale.x/2f + 0.1f, PUSH_OFF_MASK);
    }

    void SnapPositionToInt()
    {
        transform.position = Vector3.Scale(RoundToInt(transform.position), Vector3.forward + Vector3.right) + Vector3.Scale(transform.position, Vector3.up);
    }

    void Hault()
    {
        _velocity = Vector2.zero;
        SnapPositionToInt();
    }

    void MoveWithVelocity()
    {
        transform.Translate(_velocity * Time.deltaTime * _timeScale);
    }

    Vector3 RoundToInt(Vector3 v3)
    {
        return new Vector3(Mathf.Round(v3.x), Mathf.Round(v3.y), Mathf.Round(v3.z));
    }

    public static bool NearPole(Vector3 v3, out GameObject p)
    {
        foreach (GameObject pole in GameObject.FindGameObjectsWithTag("SwingPole"))
        {
            Vector3 polePos = pole.GetComponent<Transform>().position;

            if ((v3 - polePos).magnitude < POLE_GRAB_DISTANCE)
            {
                p = pole;

                return true;
            }
        }

        p = null;

        return false;
    }

    void RotateAround(Vector3 characterPos, Vector3 polePos, float rotationDirection, float dt)
    {
        Vector3 radiusVector_i = Vector3.Scale(Vector3.Normalize(characterPos - polePos), new Vector3(1, 0, 1));
        float angle_i = VectorToAngle(radiusVector_i);
        float dtheta = dt * rotationDirection * _timeScale / SWING_RADIUS * SWING_SPEED;
        float angle_f = angle_i + dtheta;
        Vector3 radiusVector_f = AngleToVector(angle_f) * SWING_RADIUS;

        transform.Translate(radiusVector_f + polePos - characterPos);
        _velocity = SnapToOrthogonal(Vector3.Cross(radiusVector_f, Vector3.up * rotationDirection)) * SPEED;
    }

    public static Vector3 AngleToVector(float a)
    {
        return new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
    }

    public static float VectorToAngle(Vector3 v)
    {
        return Mathf.Atan2(v.z, v.x);
    }
}
