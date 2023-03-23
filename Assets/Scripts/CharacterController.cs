using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    const float SPEED = 8f;
    const float POLE_GRAB_DISTANCE = 5f;
    const int PUSH_OFF_MASK = 1 << 6;
    const float TIME_SLOW_MULTIPLIER = 1f / 6f;

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

            _attachedToPole = false;
        };
    }

    // Update is called once per frame
    void Update()
    {
        // mouse events
        if (Input.GetMouseButtonUp(0))
        {
            OnLeftMouseUp();
        }

        if (Input.GetMouseButtonUp(1))
        {
            OnRightMouseUp();
        }

        // look for pole to grab
        if (Input.GetMouseButton(1))
        {
            if (NearPole(transform.position, out _poleAttachedTo))
            {
                // only lock to pole if near the position where their velocity is pointing

                _attachedToPole = true;
            }
        }

        SetTimeScale();
        UpdateVelocity();
    }

    void SetTimeScale()
    {
        if (Input.GetMouseButton(0))
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
        Debug.Log($"{transform.position}");

        if (_attachedToPole)
        {
            RotateAround(transform.position, _poleAttachedTo.transform.position, 1, Time.deltaTime);
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

    bool NearPole(Vector3 v3, out GameObject p)
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
        Vector3 radiusVector_i = Vector3.Scale(characterPos - polePos, new Vector3(1, 0, 1));
        float angle_i = VectorToAngle(radiusVector_i);
        float dtheta = dt * radiusVector_i.magnitude * SPEED;
        float angle_f = angle_i + dtheta;
        Vector3 radiusVector_f = AngleToVector(angle_f);

        transform.Translate(radiusVector_f + polePos - characterPos);
    }

    Vector3 AngleToVector(float a)
    {
        return new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
    }

    float VectorToAngle(Vector3 v)
    {
        return Mathf.Atan2(v.z, v.x);
    }
}
