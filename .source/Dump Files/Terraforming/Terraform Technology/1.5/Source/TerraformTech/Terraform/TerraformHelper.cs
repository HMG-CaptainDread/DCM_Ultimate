using Verse;

namespace TerraformTech
{
    public static class TerraformHelper
    {
        private static TerrainDef tempTerrain;

        public static TerrainDef GetTerrain(Map map, IntVec3 center)
        {
            return GetTerrain(map, CellIndicesUtility.CellToIndex(center, map.Size.x));
        }

        public static TerrainDef GetTerrain(Map map, int center)
        {
            tempTerrain = map.terrainGrid.TerrainAt(center);
            if (tempTerrain.bridge)
            {
                return map.terrainGrid.UnderTerrainAt(center);
            }

            return tempTerrain;
        }


        public static void SetTerrain(Map map, TerrainDef terrainDef, int center)
        {
            SetTerrain(map, terrainDef, CellIndicesUtility.IndexToCell(center, map.Size.x));
        }

        public static void SetTerrain(Map map, TerrainDef terrainDef, IntVec3 center)
        {
            if(terrainDef != null)
            {
                tempTerrain = map.terrainGrid.TerrainAt(center);
                if (tempTerrain.bridge)
                {
                    map.terrainGrid.SetUnderTerrain(center, terrainDef);
                }
                else
                {
                    map.terrainGrid.SetTerrain(center, terrainDef);
                }
            }
        }

        public static TerrainDef GetUnderTerrain(Map map, IntVec3 center)
        {
            return GetUnderTerrain(map, CellIndicesUtility.CellToIndex(center, map.Size.x));
        }

        public static TerrainDef GetUnderTerrain(Map map, int center)
        {
            tempTerrain = map.terrainGrid.TerrainAt(center);
            if (tempTerrain.bridge)
            {
                return null;
            }

            return map.terrainGrid.UnderTerrainAt(center);
        }

        public static void SetUnderTerrain(Map map, TerrainDef terrainDef, int center)
        {
            SetUnderTerrain(map, terrainDef, CellIndicesUtility.IndexToCell(center, map.Size.x));
        }

        public static void SetUnderTerrain(Map map, TerrainDef terrainDef, IntVec3 center)
        {
            if (terrainDef != null)
            {
                tempTerrain = map.terrainGrid.TerrainAt(center);
                if (tempTerrain.bridge)
                {

                }
                else
                {
                    map.terrainGrid.SetUnderTerrain(center, terrainDef);
                }
            }
        }
    }
}
