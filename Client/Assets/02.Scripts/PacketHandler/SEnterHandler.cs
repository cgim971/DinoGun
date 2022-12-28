using C.Proto.DinoGun;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEnterHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Enter msg = packet as S_Enter;

        PlayerInfo playerInfo = msg.PlayerInfo;

        Vector2 pos = new Vector2(playerInfo.Position.X, playerInfo.Position.Y);
        GameManager.Instance.SpawnPlayer(pos, playerInfo.PlayerId, false);
    }
}
