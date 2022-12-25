protoc -I=.\ --ts_out=..\src\packet packet.proto

protoc -I=.\ --csharp_out=..\..\Client\Assets\02.Scripts\Packet packet.proto

pause