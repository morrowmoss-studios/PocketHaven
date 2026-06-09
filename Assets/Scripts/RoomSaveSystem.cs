using System;
using System.IO;
using UnityEngine;

// RoomSaveSystem.cs
// Saves and loads one room's placed decorations to disk as JSON.
public class RoomSaveSystem : MonoBehaviour
{
    public Transform placedParent;       // same object the controller parents to
    public DecorationLibrary library;    // resolves saved ids back to decorations
    public string roomId = "stump_living_room";

    private string SavePath => Path.Combine(Application.persistentDataPath, roomId + ".json");

    [ContextMenu("Save")]
    public void Save()
    {
        RoomSaveData saveData = new RoomSaveData { roomId = roomId };

        foreach (PlacedDecoration placed in placedParent.GetComponentsInChildren<PlacedDecoration>())
        {
            saveData.placedItems.Add(placed.ToSaveItem());
        }

        File.WriteAllText(SavePath, JsonUtility.ToJson(saveData));
        Debug.Log("Saved " + saveData.placedItems.Count + " items to " + SavePath);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file yet");
            return;
        }

        for (int i = placedParent.childCount - 1; i >= 0; i--)
        {
            Destroy(placedParent.GetChild(i).gameObject);
        }

        RoomSaveData saveData = JsonUtility.FromJson<RoomSaveData>(File.ReadAllText(SavePath));

        foreach (PlacedItem item in saveData.placedItems)
        {
            DecorationData data = library.GetById(item.decorationId);
            if (data == null) { continue; }   // id not in library, skip

            GameObject go = new GameObject(data.displayName);
            go.transform.SetParent(placedParent, true);
            go.transform.position = new Vector3(item.posX, item.posY, 0f);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = data.sprite;
            sr.sortingOrder = Mathf.RoundToInt(-item.posY * 100f);

            PlacedDecoration placed = go.AddComponent<PlacedDecoration>();
            placed.data = data;
            placed.facingIndex = item.facingIndex;
            placed.colorIndex = item.colorIndex;
        }
    }

    private void Start()
    {
        Load();
    }
}