using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    public int PlayerId
    {
        get => _playerId;
        set => _playerId = value;
    }
    private int _playerId;
}
