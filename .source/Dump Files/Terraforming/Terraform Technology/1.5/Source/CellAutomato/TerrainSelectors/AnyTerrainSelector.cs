using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class AnyTerrainSelector : TerrainSelector
    {
        public override bool Check(TerrainDef terrain)
        {
            return true;
        }
    }
}
