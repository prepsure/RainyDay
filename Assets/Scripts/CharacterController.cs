using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    const float SPEED = 8f;
    const int PUSH_OFF_MASK = 1 << 6;

    event Action OnMouseUp;
    event Action OnRightMouseButtonDown;
    event Action OnRightMouseUp;

    Camera _camera;

    bool _lastLeftMouse;
    Vector3 _velocity = Vector3.zero;

    float _timeScale = 1f/8f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = FindObjectOfType<Camera>();

        OnMouseUp += () =>
        {
            Vector3 vel = SnapToOrthogonal(GetDirectionOfMouseRelativeToCharacter(Input.mousePosition));

            if(VelocityHittingWall(vel))
            {
                SnapPositionToInt();
                BlastInDirection(-vel);
            }
        };

        OnRightMouseButtonDown += () =>
        {
            // check if near pole
            // snap to pole
            // make rotate
        };

        OnRightMouseUp += () =>
        {
            // wait until the character is roughly in the right place

            // snap position to a grid space
            // snap velocity to an orthogonal direction
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }

        if (Input.GetMouseButtonUp(1))
        {
            OnRightMouseUp();
        }

        if (Input.GetMouseButtonUp(1))
        {
            OnRightMouseUp();
        }

        if (Input.GetMouseButton(0))
        {
            _timeScale = 1 / 8f;
        }
        else
        {
            _timeScale = 1;
        }

        // velocity updates
        if (VelocityHittingWall(_velocity)) {
            Hault();
        } else
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

    bool IsNearPole(Vector3 v3)
    {
        foreach (GameObject pole in GameObject.FindGameObjectsWithTag("SwingPole"))
        {
            Vector3 polePos = pole.GetComponent<Transform>().position;

            if ((v3 - polePos).magnitude < 0)
            {

            }
        }

        return false;
    }
}
