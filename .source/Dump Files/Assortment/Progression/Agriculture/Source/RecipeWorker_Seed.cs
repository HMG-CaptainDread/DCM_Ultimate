using Verse;
using System.Collections.Generic;

namespace ProgressionAgriculture
{
    public class RecipeWorker_Seed : RecipeWorker
    {
        public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
        {
            base.Notify_IterationCompleted(billDoer, ingredients);
            billDoer.CurJob.bill.billStack.bills.Remove(billDoer.CurJob.bill);
        }
    }
}