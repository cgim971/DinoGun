using C.Proto.DinoGun;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPosHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Pos msg = packet as S_Pos;

        // Debug.Log($"{msg.X}, {msg.Y}");
    }
}
