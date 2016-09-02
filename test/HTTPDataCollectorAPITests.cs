using Xunit;
using System.Collections.Generic;

namespace HTTPDataCollectorAPI.Test
{
    public class HTTPDataCollectorAPITests
    {
        public class MySerializableClass{
            public int MyIntAttribute{get;set;}
            public string MyStringAttribute{get;set;}
            public List<string> MyListAttribute{get;set;}
        }
        [Fact]
        public void SendingValidPayload()
        {
            var collector = new  HTTPDataCollectorAPI.Collector("{Your_Workspace_Id}","{Your_Workspace_Key}");
            collector.Collect("Test", "{'test':'a'}");
        }        

        [Fact]
        public void SendingValidObject()
        {
            var anObject = new MySerializableClass(){
                MyIntAttribute = 4,
                MyStringAttribute = "hello",
                MyListAttribute = new List<string>(){"one","two"}
            };
            var collector = new  HTTPDataCollectorAPI.Collector("{Your_Workspace_Id}","{Your_Workspace_Key}");
            collector.Collect("Test", anObject);
        }   
    }
}
