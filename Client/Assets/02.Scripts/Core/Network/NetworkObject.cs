using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    public int Id
    {
        get => _id;
        set => _id = value;
    }
    private int _id;
}
