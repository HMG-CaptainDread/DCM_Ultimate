using Verse;

namespace CellAutomato
{
    public class TestCompAssignRule : CompProperties
    {        
        public TestCompAssignRule()
        {
            compClass = typeof(TestAssignRule);
        }
    }

    public class TestAssignRule : ThingComp
    {
        public override void CompTick()
        {
            var map = parent.Map;
            var center = parent.Position;
            var cellGirdCOmp = map.GetComponent<CellAutomatoGrid>();
            parent.Destroy();

            cellGirdCOmp.AssignRuleToCell(CellIndicesUtility.CellToIndex(center, map.Size.x), true);

        }
    }
}
