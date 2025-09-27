using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class TerrainSelector
    {
        public List<TerrainDef> terrainDefs = new List<TerrainDef>();//public for debug purpouses

        public virtual bool Check(TerrainDef terrain)
        {
            return terrainDefs != null ? terrainDefs.Any(t => t.defName == terrain.defName) : false;
        }
    }
}
