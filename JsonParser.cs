
using Newtonsoft.Json.Linq;

namespace PtxBuddy
{
    public static class JsonParser
    {
        public static string ExtractOutputValue(string jsonString)
        {
            // Parse the JSON into a JObject
            var jsonObject = JObject.Parse(jsonString);

            // Extract the value of the "output" key
            if (jsonObject.ContainsKey("output"))
            {
                return jsonObject["output"].ToString();
            }

            return null; // Handle missing key
        }
    }
}
