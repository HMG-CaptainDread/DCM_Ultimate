using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace ChickenExplosives
{
    public static class Utils
    {
        public static HashSet<IntVec3> GetStartingCells(CompProperties_ProjectileSprayer props, Rot4 rot, CellRect cellRect, Map map)
        {
            HashSet<IntVec3> startingCells = new HashSet<IntVec3>();
            foreach (var pos in cellRect.Cells)
            {
                IntVec3 startCell = IntVec3.Invalid;
                IntVec3 curCell = pos;
                while (true)
                {
                    if (curCell.DistanceTo(pos) == props.projectileDistanceRange.min)
                    {
                        startCell = curCell;
                        break;
                    }
                    else
                    {
                        curCell = curCell + rot.FacingCell;
                    }
                }
                startingCells.Add(startCell);
                int i = 1;
                while (startingCells.Count < props.projectileWidth)
                {
                    if (rot.IsHorizontal)
                    {
                        if (startingCells.Count < props.projectileWidth)
                        {
                            startingCells.Add(startCell + (Rot4.South.FacingCell * i));
                        }
                        if (startingCells.Count < props.projectileWidth)
                        {
                            startingCells.Add(startCell + (Rot4.North.FacingCell * i));
                        }
                    }
                    else
                    {
                        if (startingCells.Count < props.projectileWidth)
                        {
                            startingCells.Add(startCell + (Rot4.West.FacingCell * i));
                        }
                        if (startingCells.Count < props.projectileWidth)
                        {
                            startingCells.Add(startCell + (Rot4.East.FacingCell * i));
                        }
                    }
                    i++;
                }
            }
            startingCells.RemoveWhere(x => !x.InBounds(map));
            return startingCells;
        }

        public static HashSet<IntVec3> GetAffectedCells(HashSet<IntVec3> affectedCells, CompProperties_ProjectileSprayer props, Rot4 rot, CellRect cellRect, Map map)
        {
            var newTiles = new HashSet<IntVec3>();
            foreach (var pos in affectedCells)
            {
                IntVec3 curCell = pos;
                while (true)
                {
                    curCell = curCell + rot.FacingCell;
                    if (curCell.DistanceTo(pos) <= props.projectileDistanceRange.max)
                    {
                        newTiles.Add(curCell);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            newTiles.RemoveWhere(x => !x.InBounds(map));
            return newTiles;
        }

        public static HashSet<IntVec3> GetTotalAffectedCells(CompProperties_ProjectileSprayer props, Rot4 rot, CellRect cellRect, Map map)
        {
            var cells = GetStartingCells(props, rot, cellRect, map);
            cells.AddRange(GetAffectedCells(cells, props, rot, cellRect, map));
            return cells;
        }
    }
}