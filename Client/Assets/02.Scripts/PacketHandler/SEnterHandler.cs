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

        PositionData positionData = Util.ChangePositionInfo(playerInfo.Position);

        GameManager.Instance.SpawnPlayer(positionData.pos, playerInfo.PlayerId, false);
    }
}
