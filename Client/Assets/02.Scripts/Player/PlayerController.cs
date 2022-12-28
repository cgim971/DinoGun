using C.Proto.DinoGun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkObject
{

    public bool IsRemote => _isRemote;
    bool _isRemote = false;

    public Rigidbody2D Rigidbody => _rigidbody;

    private Rigidbody2D _rigidbody;
    private PlayerMove _playerMove;
    private DinoController _dinoController;
    private GunController _gunController;

    float width = 0;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerMove = GetComponent<PlayerMove>();
        _dinoController = GetComponent<DinoController>();
        _gunController = GetComponent<GunController>();


        _playerMove.Init(this);

        width = Screen.width / 2;
    }

    public void SetUp(bool isPlayer, int playerId)
    {
        _isRemote = !isPlayer;
        PlayerId = playerId;

        if (_isRemote == false)
        {
            StartCoroutine(SendPositionAndRotation());
        }
    }

    private void Update()
    {
        if (IsRemote == false)
        {
            _playerMove.CheckMove();
            LookMouse();
            PlayAnimation();
        }
    }

    public void LookMouse()
    {
        float x = Input.mousePosition.x;

        transform.localScale = new Vector3(x == width ? transform.localScale.x : x > width ? 1 : -1, 1, 1);
    }

    public void PlayAnimation()
    {
        if (_rigidbody.velocity.magnitude > 0)
        {
            _dinoController.SetAnimator(AnimationState.RUN);
        }
        else
        {
            _dinoController.SetAnimator(AnimationState.IDLE);
        }
    }

    private IEnumerator SendPositionAndRotation()
    {
        Position position = new Position();

        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.04f);

            Vector2 pos = transform.position;

            position.X = pos.x;
            position.Y = pos.y;
            position.ScaleX = transform.localScale.x;
            position.GunRotate = _gunController.GunTs.rotation.z;

            C_Move cMove = new C_Move { PlayerId = PlayerId, Position = position };

            NetworkManager.Instance.RegisterSend((ushort)MSGID.CMove, cMove);
        }
    }
}
