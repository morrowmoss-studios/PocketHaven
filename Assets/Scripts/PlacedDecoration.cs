using UnityEngine;

// PlacedDecoration.cs
// Sits on every placed item. Remembers which DecorationData it came from
// plus its variant state, so the save system can serialize and rebuild it.
public class PlacedDecoration : MonoBehaviour
{
    public DecorationData data;   // which catalog item this is
    public int facingIndex;       // current iso facing
    public int colorIndex;        // current recolor index

    public PlacedItem ToSaveItem()
    {
        return new PlacedItem
        {
            decorationId = data.id,
            posX = transform.position.x,
            posY = transform.position.y,
            facingIndex = facingIndex,
            colorIndex = colorIndex
        };
    }
}