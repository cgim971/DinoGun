using C.Proto.DinoGun;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public static PlayerManager Instance
    {
        get => _instance;
        set => _instance = value;
    }
    private static PlayerManager _instance;

    // 자신이 아닌 플레이어들
    private Dictionary<int, PlayerController> _remotePlayerList;

    public PlayerManager()
    {
        _remotePlayerList = new Dictionary<int, PlayerController>();
    }

    public void AddRemotePlayer(PlayerController player)
    {
        _remotePlayerList.Add(player.PlayerId, player);
    }

    public void RemoveRemotePlayer(int id)
    {
        PlayerController player = null;
        if (_remotePlayerList.TryGetValue(id, out player))
        {
            _remotePlayerList.Remove(id);

            Debug.Log($"Player {player.PlayerId} 님이 접속 종료");
            GameObject.Destroy(player.gameObject);
        }
    }

    public PlayerController GetRemotePlayer(int id)
    {
        PlayerController player = null;
        _remotePlayerList.TryGetValue(id, out player);
        return player;
    }

    public void UpdateRemotePlayer(RepeatedField<PlayerInfo> playerList)
    {
        foreach (PlayerInfo playerInfo in playerList)
        {
            PositionData positionData = Util.ChangePositionInfo(playerInfo.Position);

            PlayerController player = null;
            if (_remotePlayerList.TryGetValue(playerInfo.PlayerId, out player))
            {
                player.SetPositionData(positionData);
            }
        }
    }
}
