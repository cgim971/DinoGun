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
    private Transform _dinoTs;

    private Animator _animator;

    private void Start()
    {
        _dinoTs = transform.Find("Dino");

        _animator = _dinoTs.GetComponent<Animator>();
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
