using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task SendingValidPayload()
        {
            var collector = new  HTTPDataCollectorAPI.Collector("{Your_Workspace_Id}", "{Your_Workspace_Key}");
            await collector.Collect("TestLogType", "{\"TestAttribute\":\"TestValue\"}");
        }        

        [Fact]
        public async Task SendingValidObject()
        {
            var anObject = new MySerializableClass(){
                MyIntAttribute = 4,
                MyStringAttribute = "hello",
                MyListAttribute = new List<string>(){"one","two"}
            };
            var collector = new  HTTPDataCollectorAPI.Collector("{Your_Workspace_Id}", "{Your_Workspace_Key}");
            await collector.Collect("TestLogType", anObject);
        }   
    }
}
