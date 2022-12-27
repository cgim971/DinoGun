using C.Proto.DinoGun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _mainMap;

    [SerializeField] private string _connectUrl;

    public GameManager Instance => _instance;
    private static GameManager _instance;

    private void Awake()
    {
        if (_instance != null)
            Debug.LogError("Multiple GameManager is running!");
        _instance = this;

        NetworkManager.Instance = gameObject.AddComponent<NetworkManager>();
        NetworkManager.Instance.Init(_connectUrl);
        NetworkManager.Instance.Connection();

        MapManager.Instance = new MapManager(_mainMap);
    }

    private void OnDestroy() => NetworkManager.Instance.Disconnect();

    private void Update() => Test();

    void Test()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Position spawnPosition = new Position { Rotate = 0, X = 0, Y = 0, GunRotate = 0 };

            C_Move cPos = new C_Move { PlyaerId = 1, SpawnPosition = spawnPosition };
            NetworkManager.Instance.RegisterSend((ushort)MSGID.CMove, cPos);
        }
    }
}
