using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public Transform GunTs => _gunTs;
    private Transform _gunTs;

    private void Start()
    {
        _gunTs = transform.Find("Gun");
    }

    public void CheckFire()
    {

    }


}
