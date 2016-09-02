using Xunit;

namespace HTTPDataCollectorAPI.Test
{
    public class HTTPDataCollectorAPITests
    {
        [Fact]
        public void SendingValidPayload()
        {
            var collector = new  HTTPDataCollectorAPI.Collector("{Your_Workspace_Id}","{Your_Workspace_Key}");
            collector.Collect("Test", "{'test':'a'}");
        }        
    }
}
