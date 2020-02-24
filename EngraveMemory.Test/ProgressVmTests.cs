using EngraveMemory.MemorialList;

namespace ClassLibrary1
{
    public class ProgressVmTests
    {
        public void Test()
        {
            var memorial = new Memorial();
            var progressVm = new ProgressVm(memorial);
            progressVm.Invalidate(memorial);
            Thread.Sleep(1000);
            Assert(progressVm.NextRepeatPoint.IsActive);
        }
    }
}SS