using C.Proto.DinoGun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PositionData
{
    public Vector3 pos;
    public Quaternion gunRot;
    public int scaleX;
}

public class Util
{
    public static PositionData ChangePositionInfo(Position info)
    {
        PositionData data = new PositionData
        {
            pos = new Vector3(info.X, info.Y, 0),
            gunRot = Quaternion.Euler(0, 0, info.GunRotate),
            scaleX = (int)(info.ScaleX)
        };

        return data;
    }
}
