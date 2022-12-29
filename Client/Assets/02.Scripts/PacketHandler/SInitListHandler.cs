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

            PositionData positionData = Util.ChangePositionInfo(playerInfo.Position);

            GameManager.Instance.SpawnPlayer(positionData.pos, playerInfo.PlayerId, false);
        }
    }
}
