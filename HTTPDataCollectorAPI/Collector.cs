using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HTTPDataCollectorAPI
{
    /// <summary>
    /// HTTP Data Collector API wrapper
    /// </summary>
    public class Collector : ICollector
    {
        private string _WorkspaceId;
        private string _SharedKey;
		private byte[] _SharedKeyBytes;
        /// <summary>
        /// Wrapper for reporting custom JSON events to Azure Log Analytics
        /// </summary>
        /// <param name="WorkspaceId">Workspace Id obtained from your Microsoft Operations Management Suite account, Settings > Connected Sources.</param>
        /// <param name="SharedKey">Primary or Secondary Key obtained from your Microsoft Operations Management Suite account, Settings > Connected Sources.</param>
        public Collector(string WorkspaceId, string SharedKey)
        {
            _WorkspaceId = WorkspaceId;
            _SharedKey = SharedKey;
			_SharedKeyBytes = Convert.FromBase64String(_SharedKey);
		}

        /// <summary>
        /// Collect a JSON log to Azure Log Analytics
        /// </summary>
        /// <param name="LogType">Name of the Type of Log. Can be any name you want to appear on Azure Log Analytics.</param>
        /// <param name="ObjectToSerialize">Object to serialize and collect.</param>
        /// <param name="ApiVersion">Optional. Api Version.</param>
        public async Task Collect(string LogType, object ObjectToSerialize, string ApiVersion="2016-04-01", string timeGeneratedPropertyName = null)
        {
            await Collect(LogType, Newtonsoft.Json.JsonConvert.SerializeObject(ObjectToSerialize), ApiVersion, timeGeneratedPropertyName);
        }
        /// <summary>
        /// Collect a JSON log to Azure Log Analytics
        /// </summary>
        /// <param name="LogType">Name of the Type of Log. Can be any name you want to appear on Azure Log Analytics.</param>
        /// <param name="JsonPayload">JSON string. Can be an array or single object.</param>
        /// <param name="ApiVersion">Optional. Api Version.</param>
        public async Task Collect(string LogType, string JsonPayload, string ApiVersion="2016-04-01", string timeGeneratedPropertyName = null)
        {
            var utf8Encoding = new UTF8Encoding();
            Byte[] content = utf8Encoding.GetBytes(JsonPayload);

            string url = "https://" + _WorkspaceId + ".ods.opinsights.azure.com/api/logs?api-version=" + ApiVersion;
            var rfcDate = DateTime.Now.ToUniversalTime().ToString("r");
            var signature = HashSignature("POST", content.Length, "application/json", rfcDate, "/api/logs");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["Log-Type"] = LogType;
            request.Headers["x-ms-date"] = rfcDate;
            request.Headers["Authorization"] = signature;
            if (!string.IsNullOrEmpty(timeGeneratedPropertyName))
            {
                request.Headers["time-generated-field"] = timeGeneratedPropertyName;
            }
            request.Proxy = null;
            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                requestStream.Write(content, 0, content.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
            {
                if (!response.StatusCode.Equals(HttpStatusCode.OK) && !response.StatusCode.Equals(HttpStatusCode.Accepted))
                {
                    //Take response body and throw it as an exception
                    Stream dataStream = response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        throw new Exception(reader.ReadToEnd());
                    }
                }
            }
        }

        /// <summary>
        /// SHA256 signature hash
        /// </summary>
        /// <returns></returns>
        private string HashSignature(string method, int contentLength, string contentType, string date, string resource)
        {
            var stringtoHash = method + "\n" + contentLength + "\n" + contentType + "\nx-ms-date:" + date + "\n" + resource;
            var encoding = new System.Text.ASCIIEncoding();
            var bytesToHash = encoding.GetBytes(stringtoHash);            
            using (var sha256 = new HMACSHA256(_SharedKeyBytes))
            {
                var calculatedHash = sha256.ComputeHash(bytesToHash);
                var stringHash = Convert.ToBase64String(calculatedHash);
                return "SharedKey " + _WorkspaceId + ":" + stringHash;
            }
        }
    }
}
