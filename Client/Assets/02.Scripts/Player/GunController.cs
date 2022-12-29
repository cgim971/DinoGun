using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    PlayerController _playerController = null;

    public Transform GunTs => _gunTs;
    private Transform _gunTs;

    private Camera _mainCam;

    Transform _dinoTs = null;


    public void Init(PlayerController playerController)
    {
        _playerController = playerController;

        _gunTs = transform.Find("Gun");
        _mainCam = Camera.main;

        _dinoTs = _playerController.DinoController.DinoTs;
    }

    public void SetScaleAndRot(int scale, Quaternion rot)
    {
        _gunTs.localScale = new Vector3(1, scale, 1);
        _gunTs.rotation = rot;
    }

    public void CheckInput()
    {
        CheckFire();
    }

    public void CheckFire()
    {
        // 발사 로직

        Vector3 worldMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0;

        Vector3 delta = worldMousePos - transform.position;
        float degree = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        _gunTs.localScale = new Vector3(1, _dinoTs.transform.localScale.x, 1);
        _gunTs.rotation = Quaternion.AngleAxis(degree, Vector3.forward);
    }
}
