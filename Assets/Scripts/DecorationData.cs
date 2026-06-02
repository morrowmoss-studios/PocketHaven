using UnityEngine;

// Static design data for a single placeable decoration. Authored as an
// asset in the editor, one asset per item. No runtime or player state
// lives here, that belongs in PlacedItem.
[CreateAssetMenu(fileName = "Decoration_", menuName = "PocketHaven/Decoration")]
public class DecorationData : ScriptableObject
{
    [Header("Identity")]
    public string id;                 // stable unique key, referenced by save data
    public string displayName;
    [TextArea] public string description;

    [Header("Presentation")]
    public Sprite sprite;             // art used in the catalog and when placed
    public DecorationCategory category;
    public PlacementLayer layer = PlacementLayer.Floor;

    // Footprint in grid cells, 1x1 by default. Multi cell logic comes
    // later, the field sits here now so assets never need reauthoring.
    public Vector2Int footprint = Vector2Int.one;

    [Header("Customization")]
    // Free recolor is a base feature. Patterns are a paid IAP layer, later.
    public bool supportsRecolor = true;

    [Header("Monetization")]
    // Base set ships free (isPremium = false). Premium items name the IAP
    // bundle that unlocks them. Store wiring comes much later.
    public bool isPremium;
    public string iapBundleId;
}