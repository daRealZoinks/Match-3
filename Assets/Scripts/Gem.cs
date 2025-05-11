using UnityEngine;

public class Gem : MonoBehaviour
{
    public Vector2Int position;
    public GemType gemType;

    public void SetPosition(Vector2Int position)
    {
        this.position = position;

        transform.localPosition = new Vector3(position.x, position.y, 0);
    }
}

public enum GemType
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Violet,
    White
}