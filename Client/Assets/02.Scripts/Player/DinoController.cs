using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState
{
    IDLE,
    RUN,
}

public class DinoController : MonoBehaviour
{
    public Transform DinoTs => _dinoTs;
    private Transform _dinoTs;
    private PlayerController _playerController;
    private Animator _animator;

    public void Init(PlayerController playerController)
    {
        _dinoTs = transform.Find("Dino");

        _playerController = playerController;

        _animator = _dinoTs.GetComponent<Animator>();
    }

    public void SetScale(int scaleX)
    {
        _dinoTs.localScale = new Vector3(scaleX, 1, 1);
    }           

    public void SetAnimator(AnimationState state)
    {
        switch (state)
        {
            case AnimationState.IDLE:
                IdleState();
                break;
            case AnimationState.RUN:
                RunState();
                break;
        }
    }

    void IdleState()
    {
        _animator.SetBool("move", false);
    }

    void RunState()
    {
        _animator.SetBool("move", true);
    }

}
