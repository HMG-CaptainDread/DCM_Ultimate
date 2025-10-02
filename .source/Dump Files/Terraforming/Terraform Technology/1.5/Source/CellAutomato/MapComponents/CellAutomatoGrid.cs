using System;
using System.Collections.Generic;
using System.Linq;
using TerraformTech;
using Verse;

namespace CellAutomato
{

    public class CellRuleTimer : IExposable
    {
        //tick to apply rule on
        public int Tick;
        //this is to re-decide which rule is applied if scheduled rule fails checks 
        public int Time;
        //cell to apply to
        public int terrainCell;
        public string defRuleActivated;
        internal bool debug;

        public void ExposeData()
        {
            Scribe_Values.Look(ref Tick, "ti", 0);

            Scribe_Values.Look(ref Time, "tm", 0);

            Scribe_Values.Look(ref terrainCell, "c", 0);
            Scribe_Values.Look(ref defRuleActivated, "r", string.Empty);
        }
    }

    public class CellAutomatoGrid : MapComponent
    {
        public const int CheckModulus = 4;
        public const int TickInterval = 100;
        public const int RandomCheckTickInterval = 2500;

        //cache rules for terrains to not use AllDefs all the time
        static List<CellLayerDef> layerDefs = new List<CellLayerDef>();
        static List<TerrainCellRuleDef> ruleCache = new List<TerrainCellRuleDef>();
        //cache rules by defname
        static Dictionary<string, TerrainCellRuleDef> ruleCellDefs = new Dictionary<string, TerrainCellRuleDef>();
        //cache rules by terrain affected
        static Dictionary<string, List<TerrainCellRuleDef>> terrainRuleCache = new Dictionary<string, List<TerrainCellRuleDef>>();

        //sorted tick timers
        Dictionary<string, Dictionary<int, CellRuleTimer>> terrainCellChecks = 
            new Dictionary<string, Dictionary<int, CellRuleTimer>>();

        //stored data
        List<CellRuleTimer> cellRuleTickTimers = new List<CellRuleTimer>();
        int lastCheckTick;

        public CellAutomatoGrid(Map map) : base(map)
        {
            if(ruleCache.Count == 0)
            {
                var allRules = DefDatabase<TerrainCellRuleDef>.AllDefs;
                ruleCache.AddRange(allRules.Where(r => r.isNatural));
                foreach (var rule in allRules)
                {
                    ruleCellDefs[rule.defName] = rule;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref cellRuleTickTimers, "cellRuleTickTimers", LookMode.Deep);
            Scribe_Values.Look(ref lastCheckTick, "lastCheckTick", 0);
        }

        public override void FinalizeInit()
        {
            //Log.Message("test1");

            foreach(var layerDef in DefDatabase<CellLayerDef>.AllDefs)
            {
                layerDefs.Add(layerDef);
                terrainCellChecks[layerDef.defName] = new Dictionary<int, CellRuleTimer>();
            }

            if (!terrainCellChecks.ContainsKey(string.Empty)) terrainCellChecks[string.Empty] = new Dictionary<int, CellRuleTimer>();


            if (cellRuleTickTimers != null)
            {
                TerrainCellRuleDef rule;
                foreach (var cellCheck in cellRuleTickTimers)
                {
                    if (ruleCellDefs.TryGetValue(cellCheck.defRuleActivated, out rule))
                    {
                        if(rule.layer != null)
                        {
                            terrainCellChecks[rule.layer.defName][cellCheck.terrainCell] = cellCheck;
                        }
                        else
                        {
                            terrainCellChecks[string.Empty][cellCheck.terrainCell] = cellCheck;
                        }
                    }
                }

                //Log.Message("" + cellRuleTickTimers.Count);
            }
            else
            {
                cellRuleTickTimers = new List<CellRuleTimer>();
            }
            //Log.Message("test2");

            cellRuleTickTimers = cellRuleTickTimers.OrderBy(rule => rule.Tick).ToList();
            //Log.Message("test3");
        }

        public override void MapComponentTick()
        {
            if(Find.TickManager.TicksGame % TickInterval == 0)
            {
                RuleTick();
            }

            if(Find.TickManager.TicksGame % RandomCheckTickInterval == 0)
            {
                //randomly check cells to start ticking
                RandRuleCheck();
            }
        }

        private void RuleTick()
        {
            lastCheckTick = Find.TickManager.TicksGame;

            CellRuleTimer rule;
            for (int i = 0; i < cellRuleTickTimers.Count;)
            {
                rule = cellRuleTickTimers[i];
                if (rule.Tick > lastCheckTick)
                {
                    //not the time to aplly rule
                    ++i;
                    continue;
                }

                if (ruleCellDefs.ContainsKey(rule.defRuleActivated))
                {
                    var ruleActivated = ruleCellDefs[rule.defRuleActivated];

                    if (!CheckRule(ruleActivated, rule.terrainCell, false, rule.debug))// ruleActivated.checker != null && !ruleActivated.checker.Check(rule.terrainCell, map))
                    {
                        UnassignRuleAt(i, rule);
                        //if current rule fails check
                        //assign and/or activate another rule
                        if (ruleActivated.failActions != null && ruleActivated.failActions.Count > 0)
                        {
                            ApplyFailActions(ruleActivated, rule.terrainCell, rule.debug);
                        }
                        else
                        {
                            //attempt to reassign anohther rule
                            ReassignRule(rule, ruleActivated, rule.debug);
                        }
                    }
                    else
                    {
                        //if rule chek succeeeds or rule has no check
                        UnassignRuleAt(i, rule);
                        ApplyRule(ruleActivated, rule.terrainCell, rule.debug);
                        AssignNeighbourCells(rule.terrainCell);
                    }
                }
                else
                {
                    if (rule.debug)
                    {
                        Log.Message("rule does not exist in rule list, unassigning");
                    }
                    UnassignRuleAt(i, rule);
                }
            }
        }
        
        public List<CellRuleTimer> GetRulesAtCell(int center)
        {
            List<CellRuleTimer> ret = new List<CellRuleTimer>();
            foreach(var terrainLayer in terrainCellChecks)
            {
                if (terrainLayer.Value.ContainsKey(center))
                {
                    ret.Add(terrainLayer.Value[center]);
                }
            }
            return ret;
        }

        private void UnassignRuleAt(int i, CellRuleTimer rule)
        {
            cellRuleTickTimers.RemoveAt(i);
            TerrainCellRuleDef ruleOut;
            if (ruleCellDefs.TryGetValue(rule.defRuleActivated, out ruleOut))
            {
                if (ruleOut.layer != null)
                {
                    terrainCellChecks[ruleOut.layer.defName].Remove(rule.terrainCell);
                }
                else
                {
                    terrainCellChecks[string.Empty].Remove(rule.terrainCell);
                }
            }
        }

        private bool ReassignRule(CellRuleTimer rule, TerrainCellRuleDef concreteRule, bool debug = false)
        {
            if (debug)
            {
                Log.Message("[" + rule.defRuleActivated + "] reassignemnt attempt at cell [" + CellIndicesUtility.IndexToCell(rule.terrainCell, map.Size.x) + "]");
            }

            TerrainDef terrain = TerraformHelper.GetTerrain(map, rule.terrainCell);
            if (!terrainRuleCache.ContainsKey(terrain.defName))
            {
                AssignTerrainRuleCache(terrain, debug);
            }

            foreach (var cellRule in terrainRuleCache[terrain.defName])
            {
                if (cellRule.layer == concreteRule.layer &&
                    CheckRuleActivation(cellRule, rule.terrainCell, debug) &&
                    CheckRule(cellRule, rule.terrainCell, true, debug))
                {
                    //higher priority rules override
                    int ruleTime = Verse.Rand.RangeInclusive(cellRule.time.TrueMin, cellRule.time.TrueMax);
                    //modifyTIme
                    if(cellRule.timeModifiers != null && cellRule.timeModifiers.Count > 0)
                    {
                        foreach (var modifier in cellRule.timeModifiers)
                            ruleTime = modifier.Modify(rule.terrainCell, map, ruleTime);
                    }

                    int ruleTick = lastCheckTick - rule.Time + ruleTime;
                    if (lastCheckTick >= ruleTick)
                    {
                        ApplyRule(cellRule, rule.terrainCell, debug);
                        AssignNeighbourCells(rule.terrainCell);
                        return false;
                    }
                    else
                    {
                        if (debug)
                        {
                            Log.Message("[" + cellRule.defName + "][" + ruleTime + "] reassigned rule at cell [" + CellIndicesUtility.IndexToCell(rule.terrainCell, map.Size.x) + "]");
                        }

                        //reassign rule data
                        rule.defRuleActivated = cellRule.defName;
                        rule.Tick = ruleTick;
                        rule.Time = ruleTime;
                        
                        TerrainCellRuleDef ruleOut;
                        if (ruleCellDefs.TryGetValue(rule.defRuleActivated, out ruleOut))
                        {
                            if (ruleOut.layer != null)
                            {
                                terrainCellChecks[ruleOut.layer.defName][rule.terrainCell] = rule;
                            }
                            else
                            {
                                terrainCellChecks[string.Empty][rule.terrainCell] = rule;
                            }
                        }

                        cellRuleTickTimers.Add(rule);
                        return true;
                    }
                }
            }

            return false;
        }

        private void AssignTerrainRuleCache(TerrainDef terrain, bool debug = false)
        {
            if (debug)
            {
                Log.Message("AssignTerrainRuleCache start");
            }
            List<TerrainCellRuleDef> rules = new List<TerrainCellRuleDef>();

            if (debug)
            {
                Log.Message("rulecache length:" + ruleCache.Count);
            }

            foreach (var rule in ruleCache)
            {
                if (debug)
                {
                    Log.Message("[" + rule.defName + "] rule cellcelestor length :" + rule.cellSelector.terrainDefs.Count);

                    foreach (var rule1 in rule.cellSelector.terrainDefs)
                    {
                        Log.Message("[" + rule.defName + "] rule :" + rule1.defName);
                    }
                }

                if (rule.cellSelector != null && rule.cellSelector.Check(terrain))
                {
                    if (debug)
                    {
                        Log.Message("[" + terrain.defName + "] rule assign :" + rule.defName);
                    }
                    rules.Add(rule);
                }
            }

            rules = rules.OrderBy(r => r.priority).ToList();

            terrainRuleCache[terrain.defName] = rules;

            if (debug)
            {
                Log.Message("AssignTerrainRuleCache end");
            }
        }
        
        private bool CheckRuleActivation(TerrainCellRuleDef cellRule, int terrainCell, bool debug = false)
        {
            if (debug)
            {
                Log.Message("[" + cellRule.defName + "] rule activation check at cell: [" + CellIndicesUtility.IndexToCell(terrainCell, map.Size.x) + "]");
            }
            //Log.Message("CheckRuleActivation:" + cellRule.defName);
            bool ret = true;
            if (cellRule.conditions != null)
            {
                foreach (var condition in cellRule.conditions)
                {
                    ret &= condition.Check(terrainCell, map);
                }
            }

            if (debug)
            {
                Log.Message("activation check result:" + ret);

                foreach (var condition in cellRule.conditions)
                {
                    Log.Message("activator check:<" + condition.patchTag + "> " + condition.Check(terrainCell, map));
                }
            }
            return ret;
        }

        private bool CheckRule(TerrainCellRuleDef cellRule, int terrainCell, bool firstCheck, bool debug = false)
        {
            if (debug)
            {
                Log.Message("[" + cellRule.defName + "] rule check");
            }
            bool ret = true;
            if (cellRule.checker != null)
            {
                ret = cellRule.checker.Check(terrainCell, map, !firstCheck, debug);
            }

            if (debug)
            {
                Log.Message("check result:" + ret);
            }
            return ret;
        }

        private void ApplyFailActions(TerrainCellRuleDef ruleActivated, int terrainCell, bool debug = false)
        {
            if (debug)
            {
                Log.Message("[" + ruleActivated.defName + "] on fail actions at cell [" + CellIndicesUtility.IndexToCell(terrainCell, map.Size.x) + "]");
            }

            if (ruleActivated.failActions != null)
            {
                foreach (var action in ruleActivated.failActions)
                {
                    action.Apply(terrainCell, map);
                }
            }
        }

        //activate rule actions 
        private void ApplyRule(TerrainCellRuleDef ruleActivated, int terrainCell, bool debug = false)
        {
            if (debug)
            {
                Log.Message("[" + ruleActivated.defName + "] activated at cell [" + CellIndicesUtility.IndexToCell(terrainCell, map.Size.x) + "]");
            }

            if (ruleActivated.actions != null)
            {
                foreach (var action in ruleActivated.actions)
                {
                    action.Apply(terrainCell, map);
                }
            }
        }

        //assign nearby cells for check
        private void AssignNeighbourCells(int terrainCellIndex)
        {
            var cellX = terrainCellIndex % map.Size.x;

            var cellZ = terrainCellIndex / map.Size.x;

            for (int x = cellX - 1; x < cellX + 1; ++x)
            {
                if (x < 0 || x >= map.Size.x) continue;

                for (int z = cellZ - 1; z < cellZ + 1; ++z)
                {
                    if (z < 0 || z >= map.Size.z) continue;
                    if (cellX == x || cellZ == z) continue;

                    AssignRuleToCell(z * map.Size.x + x);
                }
            }
        }

        private void RandRuleCheck()
        {
            int cellToCheck = 0;
            TerrainDef[] grid = map.terrainGrid.topGrid;
            for (int i = 0; i < grid.Length; i += CheckModulus)
            {
                if (i + CheckModulus > grid.Length)
                {
                    cellToCheck = i + Verse.Rand.Range(0, grid.Length - i);
                }
                else
                {
                    cellToCheck = i + Verse.Rand.Range(0, CheckModulus);
                }

                AssignRuleToCell(cellToCheck);
            }
        }

        public void AssignRuleDefToCell(int terrainCellIndex, TerrainCellRuleDef ruleDef)
        {
            if (ruleDef == null) return;

            CellRuleTimer rule;

            string layer;
            if (ruleDef.layer != null)
            {
                layer = ruleDef.layer.defName;
            }
            else
            {
                layer = string.Empty;
            }

            terrainCellChecks[layer].TryGetValue(terrainCellIndex, out rule);

            if (rule != null) return;

            rule = new CellRuleTimer
            {
                terrainCell = terrainCellIndex
            };

            var ruleTime = Verse.Rand.RangeInclusive(ruleDef.time.TrueMin, ruleDef.time.TrueMax);

            if (ruleDef.timeModifiers != null && ruleDef.timeModifiers.Count > 0)
            {
                foreach (var modifier in ruleDef.timeModifiers)
                    ruleTime = modifier.Modify(rule.terrainCell, map, ruleTime);
            }
            rule.Time = ruleTime;
            rule.Tick = lastCheckTick + rule.Time;
            rule.defRuleActivated = ruleDef.defName;
            rule.debug = false;

            terrainCellChecks[layer][terrainCellIndex] = rule;
        }

        public void AssignRuleToCell(int terrainCellIndex, bool debug = false)
        {
            Dictionary<int, CellRuleTimer> currLayer;
            foreach(var layer in layerDefs)
            {
                currLayer = terrainCellChecks[layer.defName];
                if (currLayer.ContainsKey(terrainCellIndex))
                {
                    if (debug)
                    {
                        Log.Message("layer [" + layer.defName + "] rule already assigned:" + currLayer[terrainCellIndex].defRuleActivated);
                    }
                    return;
                }

                var rule = new CellRuleTimer
                {
                    terrainCell = terrainCellIndex
                };

                TerrainDef terrain = TerraformHelper.GetTerrain(map, rule.terrainCell);
                if (!terrainRuleCache.ContainsKey(terrain.defName))
                {
                    AssignTerrainRuleCache(terrain, debug);
                }

                if (debug)
                {
                    Log.Message("terrain to test:" + terrain.defName);
                    Log.Message("terrain test rules:");
                    foreach (var rule1 in terrainRuleCache[terrain.defName])
                    {
                        Log.Message("terrain test rule:" + rule1.defName);
                    }

                    Log.Message("current layer to test:" + layer.defName);
                }

                foreach (var cellRule in terrainRuleCache[terrain.defName])
                {
                    if (debug)
                    {
                        Log.Message("cell rule layer:" + cellRule.layer.defName);
                    }
                    if (cellRule.layer == layer &&
                        CheckRuleActivation(cellRule, rule.terrainCell, debug) &&
                        CheckRule(cellRule, rule.terrainCell, true, debug))
                    {
                        //higher priority rules override
                        var ruleTime = Verse.Rand.RangeInclusive(cellRule.time.TrueMin, cellRule.time.TrueMax);

                        if (cellRule.timeModifiers != null && cellRule.timeModifiers.Count > 0)
                        {
                            foreach (var modifier in cellRule.timeModifiers)
                                ruleTime = modifier.Modify(rule.terrainCell, map, ruleTime);
                        }
                        rule.Time = ruleTime;
                        rule.Tick = lastCheckTick + rule.Time;
                        rule.defRuleActivated = cellRule.defName;
                        rule.debug = debug;

                        currLayer[terrainCellIndex] = rule;
                        cellRuleTickTimers.Add(rule);

                        if (debug)
                        {
                            Log.Message("layer [" + layer.defName + "] terrain cell test rule assigned:" + rule.defRuleActivated + "[" + ruleTime + "]");
                        }

                        break;
                    }
                }
            }
            debug = false;
        }

        public override void MapRemoved()
        {
            base.MapRemoved();
            cellRuleTickTimers.Clear();
        }
    }
}
