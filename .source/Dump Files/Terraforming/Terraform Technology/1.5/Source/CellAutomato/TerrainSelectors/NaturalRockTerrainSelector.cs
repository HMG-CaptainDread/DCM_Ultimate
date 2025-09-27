using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class NaturalRockTerrainSelector : TerrainSelector
    {
        public override bool Check(TerrainDef terrain)
        {
            return terrainDefs?.Any(t => t.defName == terrain.defName) == true || 
                TerraformTech.ResourceBank.NaturalStoneTerrains.ContainsKey(terrain.defName);
        }
    }
}
