using C.Proto.DinoGun;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _mainMap;

    [SerializeField] private string _connectUrl;

    [SerializeField] private PlayerController _playerPrefab;
    public PlayerController Player => _player;
    private PlayerController _player;


    public static GameManager Instance => _instance;
    private static GameManager _instance;


    private CinemachineVirtualCamera _cmVcam;




    private void Awake()
    {
        if (_instance != null)
            Debug.LogError("Multiple GameManager is running!");
        _instance = this;

        NetworkManager.Instance = gameObject.AddComponent<NetworkManager>();
        NetworkManager.Instance.Init(_connectUrl);
        NetworkManager.Instance.Connection();

        MapManager.Instance = new MapManager(_mainMap);

        _cmVcam = GameObject.Find("FollowCam").GetComponent<CinemachineVirtualCamera>();
    }

    private void OnDestroy() => NetworkManager.Instance.Disconnect();

    // private void Update() => Test();

    void Test()
    {
        if (Input.GetMouseButtonDown(0))
        {
            C_Pos cPos = new C_Pos { X = 1, Y = 2 };

            NetworkManager.Instance.RegisterSend((ushort)MSGID.CPos, cPos);
        }
    }

    internal PlayerController SpawnPlayer(Vector3 pos, int playerId, bool isPlayer)
    {
        PlayerController playerController = Instantiate(_playerPrefab, pos, Quaternion.identity);
        playerController.SetUp(isPlayer, playerId);

        if (isPlayer)
        {
            _player = playerController;
            _cmVcam.m_Follow = _player.transform;
        }
        else
        {
            // Ãß°¡
        }

        return playerController;
    }

}
