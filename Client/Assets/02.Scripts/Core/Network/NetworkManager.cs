using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get => _instance; set => _instance = value; }
    private static NetworkManager _instance;

    private string _url;

    private ClientWebSocket _socket;
    private RecvBuffer _recvBuffer;

    private CancellationTokenSource _quitSource;
    private CancellationToken _quitToken;

    private PacketManager _packetManager;
    private Queue<PacketMessage> _sendQueue = null; // 보내질 명단
    private bool _isReadyToSend = true; // 다른 걸 보낼 때는 보내지 못 한다.

    private void Awake()
    {
        if (_instance != null)
            Debug.LogError("Multiple NetworkManager is running!");

        _instance = this;
    }

    public void Init(string url)
    {
        _url = url;
        _recvBuffer = new RecvBuffer(1024 * 10);
        _packetManager = new PacketManager();
        _sendQueue = new Queue<PacketMessage>();
    }

    private void Update()
    {
        if (PacketQueue.Instance.Count > 0)
        {
            List<PacketMessage> list = PacketQueue.Instance.PopAll();

            foreach (PacketMessage pMsg in list)
            {
                IPacketHandler handler = _packetManager.GetPacketHandler(pMsg.Id);
                if (handler != null)
                {
                    handler.Process(pMsg.Message);  // 실행
                }
                else
                {
                    Debug.LogError($"There is no handler for this packet : {pMsg.Id}");
                }
            }
        }

        if (_isReadyToSend && _sendQueue.Count > 0)
        {
            SendMessage(); // 메시지를 보내라
        }
    }

    public void RegisterSend(ushort code, IMessage msg) => _sendQueue.Enqueue(new PacketMessage { Id = code, Message = msg });

    private async void SendMessage()
    {
        if (_socket != null && _socket.State == WebSocketState.Open)
        {
            _isReadyToSend = false; // 보내는 중

            List<PacketMessage> sendList = new List<PacketMessage>();
            while (_sendQueue.Count > 0)
            {
                sendList.Add(_sendQueue.Dequeue());
            }

            byte[] sendBuffer = new byte[1024 * 10];
            foreach (PacketMessage pMsg in sendList)
            {
                int len = pMsg.Message.CalculateSize();     // 패킷의 크기를 가져오고
                Array.Copy(BitConverter.GetBytes((ushort)(len + 4)), 0, sendBuffer, 0, sizeof(ushort));
                Array.Copy(BitConverter.GetBytes(pMsg.Id), 0, sendBuffer, 2, sizeof(ushort));
                Array.Copy(pMsg.Message.ToByteArray(), 0, sendBuffer, 4, len);

                ArraySegment<byte> segment = new ArraySegment<byte>(sendBuffer, 0, len + 4);
                await _socket.SendAsync(segment, WebSocketMessageType.Binary, true, CancellationToken.None);
            }

            _isReadyToSend = true;
        }
    }

    public async void Connection() // 연결 시도
    {
        if (_socket != null && _socket.State == WebSocketState.Open)
        {
            Debug.LogError("Already Connected!!");
            return;
        }

        _socket = new ClientWebSocket();
        Uri serverUri = new Uri(_url);

        try
        {
            await _socket.ConnectAsync(serverUri, CancellationToken.None);
            ReceiveLoop();
        }
        catch (Exception e)
        {
            Debug.LogError("Connection Error : check server status... " + e.Message);
        }
    }

    private async void ReceiveLoop()  // 서버가 있다면 계속 돌 함수
    {
        while (_socket != null && _socket.State == WebSocketState.Open)
        {
            try
            {
                WebSocketReceiveResult result = await _socket.ReceiveAsync(_recvBuffer.WriteSegment, CancellationToken.None); // 값을 받을 때 까지

                if (result.MessageType == WebSocketMessageType.Binary) // 2진 데이터로 왔는가
                {
                    if (result.EndOfMessage == true) // 내용이 끝났는지
                    {
                        _recvBuffer.OnWrite(result.Count);
                        int readByte = ProcessPacket(_recvBuffer.ReadSegment);
                        // 패킷 처리를 할거
                        if (readByte == 0) // 문제 생겼다는 의미 
                        {
                            Disconnect();
                            break;
                        }

                        _recvBuffer.OnRead(readByte); // 읽음 처리 후
                        _recvBuffer.Clean(); // 내용 지움
                    }
                    else
                    {
                        _recvBuffer.OnWrite(result.Count); // 다 쓰긴 함 근데 더 쓸거임
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    Debug.LogError("Socket closed by server in normal status");
                    break;
                }
            }
            catch (WebSocketException we)
            {
                Debug.LogError(we.Message);
                break;
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.GetType()} : {e.Message}");
                break;
            }
        }
    }

    private int ProcessPacket(ArraySegment<byte> buffer) => _packetManager.OnRecvPacket(buffer);

    public void Disconnect()
    {
        if (_socket != null && _socket.State == WebSocketState.Open)
        {
            _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "quite Client", CancellationToken.None);
        }
    }
}
