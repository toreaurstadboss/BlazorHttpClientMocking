using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BlazorHttpClientMocking.Test.Helpers
{
    public static class SerializationHelpers
    {

        public static async Task<T?> DeserializeJsonAsync<T>(string path, JsonSerializerOptions? options = null)
        {
            if (options == null)
            {
                options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                };
            }

            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (File.Exists(path) && stream.Length > 0)
                {
                    T? obj = await JsonSerializer.DeserializeAsync<T>(stream, options);
                    return obj;
                }
                return default(T);                
            }

        }

    }
}
