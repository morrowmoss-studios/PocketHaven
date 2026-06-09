using System.Collections.Generic;
using UnityEngine;

// DecorationLibrary.cs
// Master list of every decoration. Used by load to resolve saved ids, and
// later by the catalog UI. Make one asset, drag every DecorationData in.
[CreateAssetMenu(fileName = "DecorationLibrary", menuName = "PocketHaven/Library")]
public class DecorationLibrary : ScriptableObject
{
    public List<DecorationData> decorations = new List<DecorationData>();

    private Dictionary<string, DecorationData> lookup;

    public DecorationData GetById(string id)
    {
        if (lookup == null)
        {
            lookup = new Dictionary<string, DecorationData>();
            foreach (DecorationData d in decorations)
            {
                if (d != null && !string.IsNullOrEmpty(d.id))
                {
                    lookup[d.id] = d;
                }
            }
        }

        lookup.TryGetValue(id, out DecorationData result);
        return result;
    }
}