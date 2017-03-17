using System.Threading.Tasks;

namespace HTTPDataCollectorAPI
{
    /// <summary>
    /// Interface for HTTP Data Collector API wrapper
    /// </summary>
    public interface ICollector
    {
        Task Collect(string LogType, string JsonPayload, string ApiVersion = "2016-04-01", string timeGeneratedPropertyName = null);
        Task Collect(string LogType, object ObjectToSerialize, string ApiVersion = "2016-04-01", string timeGeneratedPropertyName = null);
    }
}