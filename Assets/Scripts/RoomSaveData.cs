using System;
using System.Collections.Generic;

// Serializer agnostic save schema for a single room. Primitive types only
// (no Vector3) so it survives JsonUtility, Newtonsoft, and Firestore the
// same way, so it can link with TriviaForge.

[Serializable]
public class PlacedItem
{
    public string decorationId;   // which DecorationData this instance is
    public float posX;            // world position. Free under the hood,
    public float posY;            // snapped on placement depending on platform.
    public int facingIndex;       // discrete iso facing (0..3), matches drawn angles
    public int colorIndex;        // recolor palette index, 0 is the base color
}

[Serializable]
public class RoomSaveData
{
    public string roomId;
    public List<PlacedItem> placedItems = new List<PlacedItem>();
}