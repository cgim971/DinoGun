using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _mainMap;

    public GameManager Instance => _instance;
    private static GameManager _instance;

    private void Awake()
    {
        if (_instance != null)
            Debug.LogError("Multiple GameManager is running!");
        _instance = this;

        MapManager.Instance = new MapManager(_mainMap);
    }

    private void Update()
    {
        Test();
    }

    void Test()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
            worldPos.z = 0;

            Vector3Int tilePos = MapManager.Instance.GetTilePosition(worldPos);
            MapCategory mc = MapManager.Instance.GetTileCategory(tilePos);

            Debug.Log($"{tilePos}, {mc}");
        }
    }
}
