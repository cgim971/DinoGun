using C.Proto.DinoGun;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SInitHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Init s_init = packet as S_Init;

        Position position = s_init.SpawnPosition;

        Debug.Log($"{position.X}, {position.Y}");
    }
}
