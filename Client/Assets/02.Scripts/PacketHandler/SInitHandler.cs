using C.Proto.DinoGun;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SInitHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Init msg = packet as S_Init;

        Vector2 pos = new Vector2(msg.SpawnPosition.X, msg.SpawnPosition.Y);
        PlayerController playerController = GameManager.Instance.SpawnPlayer(pos, msg.PlayerId, true);

        pos = playerController.transform.position;
        Position position = new Position { X = pos.x, Y = pos.y, GunRotate = 0, ScaleX = 1 };

        C_Enter enterMsg = new C_Enter { Position = position };
        NetworkManager.Instance.RegisterSend((ushort)MSGID.CEnter, enterMsg);
    }
}
