# Azure Log Analytics HTTP Collector API wrapper

This package is a .Net adaptation of the Powershell code for implementing the HTTP Collector API for Azure Log Analytics as seen on [the announcement](https://blogs.technet.microsoft.com/msoms/2016/08/30/http-data-collector-api-send-us-data-from-space-or-anywhere/).

[Full API Documentation](https://azure.microsoft.com/documentation/articles/log-analytics-data-collector-api/)

## Get it

You can obtain this project as a [Nuget Package](https://www.nuget.org/packages/HTTPDataCollectorAPI). 

    Install-Package HTTPDataCollectorAPI

Or reference it and use it according to the [License](./LICENSE).

## Use it

Using it is simple:

    var collector = new  HTTPDataCollectorAPI.Collector("{Your_Workspace_Id}", "{Your_Workspace_Key}");
    await collector.Collect("TestLogType", "{\"TestAttribute\":\"TestValue\"}");

You can also pass serializable objects (they will be serialized with [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)):

    var anObject = new MySerializableClass(){
      MyIntAttribute = 4,
      MyStringAttribute = "hello",
      MyListAttribute = new List<string>(){"one","two"}
    };
    var collector = new  HTTPDataCollectorAPI.Collector("{Your_Workspace_Id}", "{Your_Workspace_Key}");
    await collector.Collect("TestLogType", anObject);

If you are using a Depedency Injection mechanism (or ASP.NET Core) you can use the available interface. In ASP.NET Core for example:

    public void ConfigureServices(IServiceCollection services)
    {
      //... some other code
      services.AddSingleton<HTTPDataCollectorAPI.ICollector, HTTPDataCollectorAPI.Collector>();
    }
    
## Does it work on Azure Gov clouds?

Yes, just use the constructor overload to define the cloud service endpoint:

    var collector = new  HTTPDataCollectorAPI.Collector("{Your_Workspace_Id}", "{Your_Workspace_Key}", "ods.opinsights.azure.us");
    
## Issues

Please feel free to [report any issues](https://github.com/ealsur/HTTPDataCollectorAPI/issues) you might encounter. Keep in mind that this library won't assure that your JSON payloads are being indexed, it will make sure that the HTTP Data Collection API [responds an Accept](https://azure.microsoft.com/en-us/documentation/articles/log-analytics-data-collector-api/#return-codes) but there is no way (right now) to know when has the payload been indexed completely. 

## Supported Frameworks

* .Net 4.5 Full Framework
* .Net 4.6 Full Framework
* .Net 4.6.1 Full Framework
* [.Net Standard Library](https://docs.microsoft.com/en-us/dotnet/articles/standard/library) 1.3
