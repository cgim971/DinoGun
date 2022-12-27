using C.Proto.DinoGun;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketManager : MonoBehaviour
{
    private Dictionary<ushort, Action<ArraySegment<byte>, ushort>> _OnRecv;
    private Dictionary<ushort, IPacketHandler> _Handlers;

    public PacketManager()
    {
        _OnRecv = new Dictionary<ushort, Action<ArraySegment<byte>, ushort>>();
        _Handlers = new Dictionary<ushort, IPacketHandler>();

        Register();
    }

    private void Register()
    {
        _OnRecv.Add((ushort)MSGID.SInit, MakePacket<S_Init>);
        _Handlers.Add((ushort)MSGID.SInit, new SInitHandler());
    }

    public IPacketHandler GetPacketHandler(ushort id)
    {
        IPacketHandler handler = null;
        if (_Handlers.TryGetValue(id, out handler))
        {
            return handler;
        }
        else return null;
    }

    public int OnRecvPacket(ArraySegment<byte> buffer)
    {
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        ushort code = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);

        if (_OnRecv.ContainsKey(code))
        {
            _OnRecv[code].Invoke(buffer, code);
            return size;
        }
        else
        {
            Debug.LogError($"There is no packet handler for this packet : {code}, {size}");
            return 0;
        }
    }

    private void MakePacket<T>(ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {
        T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

        PacketQueue.Instance.Push(id, packet);
    }
}
