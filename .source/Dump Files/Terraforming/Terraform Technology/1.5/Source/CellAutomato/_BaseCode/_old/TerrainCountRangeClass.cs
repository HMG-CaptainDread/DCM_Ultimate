using System.Collections.Generic;
using System.Xml;
using Verse;

namespace CellAutomato
{
    public class TerrainCountRangeClass
    {
        private TerrainDef terrainDef;
        private FloatRange countRange;

        public TerrainDef TerrainDef => terrainDef;

        public FloatRange CountRange => countRange;

        public TerrainCountRangeClass()
        {
        }

        public TerrainCountRangeClass(TerrainDef terrainDef, FloatRange range)
        {
            if (range.TrueMin < 0)
            {
                Log.Warning("Tried to set TerrainDistanceClass range to " + range + ". terrainDef=" + terrainDef);
                range = FloatRange.Zero;
            }
            this.terrainDef = terrainDef;
            this.countRange = range;
        }

        public void ExposeData()
        {
            Scribe_Defs.Look(ref terrainDef, "terrainDef");
            Scribe_Values.Look(ref countRange, "count", FloatRange.Zero);
        }

        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            if (xmlRoot.ChildNodes.Count != 1)
            {
                Log.Error("Misconfigured TerrainDistanceRangeClass: " + xmlRoot.OuterXml);
                return;
            }
            DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "terrainDef", xmlRoot.Name);

            countRange = FloatRange.FromString(xmlRoot.FirstChild.Value);
        }

        public override string ToString()
        {
            return "(" + countRange.ToString() + " " + ((terrainDef != null) ? terrainDef.defName : "null") + ")";
        }

        public override int GetHashCode()
        {
            return Gen.HashCombineStruct(terrainDef.shortHash, countRange.GetHashCode());
        }
    }
}
