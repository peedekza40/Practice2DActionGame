using UnityEngine;

[System.Serializable]
public class GameData
{
    public float PlayerHP;
    public Vector3 PlayerPosition;

    public GameData()
    {
        this.PlayerHP = 0;
        this.PlayerPosition = Vector3.zero;
    }
}
