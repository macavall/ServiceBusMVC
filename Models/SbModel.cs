namespace ServiceBusMVC.Models
{
    public class SbModel
    {
        //public string[] sbQueueNames = { "sbqueuepartbatch", "sbqueuepart", "sbqueueonlybatchnopart" };
        public int MessageCount { get; set; }
        public int ScheduledMessageCount { get; set; }
    }
}
