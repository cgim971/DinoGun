using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerController _playerController;
    public float _speed = 5;

    public void Init(PlayerController playerController)
    {
        _playerController = playerController;
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
}
