using C.Proto.DinoGun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkObject
{
    public bool IsRemote => _isRemote;
    bool _isRemote = false;

    public Rigidbody2D Rigidbody => _rigidbody;
    public DinoController DinoController => _dinoController;


    private Rigidbody2D _rigidbody;
    private PlayerMove _playerMove;
    private DinoController _dinoController;
    private GunController _gunController;

    float _width = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerMove = GetComponent<PlayerMove>();
        _dinoController = GetComponent<DinoController>();
        _gunController = GetComponent<GunController>();

        _playerMove.Init(this);
        _dinoController.Init(this);
        _gunController.Init(this);

        _width = Screen.width / 2;
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
            _playerMove.CheckInput();
            LookMouse();
            PlayAnimation();
            _gunController.CheckInput();
        }
    }

    public void LookMouse()
    {
        float x = Input.mousePosition.x;
        int scaleX = (int)(x == _width ? transform.localScale.x : x > _width ? 1 : -1);

        _dinoController.SetScale(scaleX);
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

    public void SetPositionData(PositionData positionData, bool isImmediate = false)
    {
        _dinoController.SetScale(positionData.dinoScaleX);
        _gunController.SetScaleAndRot(positionData.gunScaleY, positionData.gunRot);
        _playerMove.SetPositionData(positionData.pos, isImmediate);
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
            position.DinoScaleX = _dinoController.DinoTs.localScale.x;
            position.GunScaleY = _gunController.GunTs.localScale.y;
            position.GunRotate = _gunController.GunTs.eulerAngles.z;

            C_Move cMove = new C_Move { PlayerId = PlayerId, Position = position };

            NetworkManager.Instance.RegisterSend((ushort)MSGID.CMove, cMove);
        }
    }
}
