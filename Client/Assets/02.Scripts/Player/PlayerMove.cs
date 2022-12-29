using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerController _playerController;
    public float _speed = 5;

    Vector3 _targetPos = Vector3.zero;

    public void Init(PlayerController playerController)
    {
        _playerController = playerController;
        _targetPos = transform.position;
    }

    public void CheckInput()
    {
        CheckMove();
    }

    public void CheckMove()
    {
        Vector2 velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            velocity.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x += 1;
        }

        velocity.Normalize();

        _playerController.Rigidbody.velocity = velocity * _speed;
    }

    public void SetPositionData(Vector3 pos, bool isImmediate)
    {
        if (isImmediate)
        {
            _playerController.transform.position = pos;
        }
        else
        {
            _targetPos = pos;
        }
    }

    private void Update()
    {
        float lerpRatio = 15;
        if (_playerController.IsRemote)
        {
            Vector3 pos = Vector3.Lerp(_playerController.transform.position, _targetPos, Time.deltaTime * lerpRatio);

            _playerController.transform.position = pos;
        }
    }
}
