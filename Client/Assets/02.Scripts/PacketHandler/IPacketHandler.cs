using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPacketHandler
{
    public void Process(IMessage packet);
}