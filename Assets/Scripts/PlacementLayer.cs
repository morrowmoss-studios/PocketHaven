
// Which surface a decoration lives on. Drives sorting groups and, later,
// placement rules (wall items snap to walls, surface items sit on top of
// furniture). Floor is the only one we wire up in the first slice.
public enum PlacementLayer
{
    Floor,
    Wall,
    Surface
}