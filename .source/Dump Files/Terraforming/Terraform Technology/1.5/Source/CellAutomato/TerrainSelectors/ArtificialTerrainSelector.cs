using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class ArtificialTerrainSelector : TerrainSelector
    {
        public override bool Check(TerrainDef terrain)
        {
            return terrainDefs?.Any(t => t.defName == terrain.defName) == true || !terrain.natural;
        }
    }
}
