using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace TerraformTech
{
    public class TerrainTerraformRule
    {
        public string patchTag = null;//solely for patching
        //this is dynamic
        public TerrainTerraformDef terraformDef = null;

        public TerraformAction action = null;
        public Type ruleActionClass = null;


        public TerrainDef resultDef = null;
        public List<TerrainDef> sourceDefs = new List<TerrainDef>();
        public int WorkToBuild;
        
        public List<ThingDefCountClass> costList = new List<ThingDefCountClass>();
        public List<ThingDefCountClass> resultList = new List<ThingDefCountClass>();
    }
}
