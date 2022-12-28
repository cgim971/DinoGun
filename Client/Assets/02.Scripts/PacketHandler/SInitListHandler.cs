using C.Proto.DinoGun;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SInitListHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_InitList msg = packet as S_InitList;

        PlayerController playerController = GameManager.Instance.Player;

        foreach (PlayerInfo playerInfo in msg.PlayerList)
        {
            if (playerInfo.PlayerId == playerController.PlayerId) continue;

            Vector2 pos = new Vector2(playerInfo.Position.X, playerInfo.Position.Y);
            GameManager.Instance.SpawnPlayer(pos, playerInfo.PlayerId, false);
        }
    }
}
