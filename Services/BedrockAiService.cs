using System.Text;
using System.Text.Json;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;

namespace CellarS.Api.Services
{
    public class BedrockAiService
    {
        private readonly IAmazonBedrockRuntime _client;

        public BedrockAiService(IAmazonBedrockRuntime client)
        {
            _client = client;
        }

        public async Task<string> SimplifyAsync(string input)
        {
            var modelId = "anthropic.claude-3-haiku-20240307-v1:0";

            var payload = new
            {
                anthropic_version = "bedrock-2023-05-31",

                max_tokens = 512,
                messages = new[]
                {
                    new {
                        role = "user",
                        content = new[]
                        {
                            new {
                                type = "text",
                                text =
                                    "You are a helpful assistant. " +
                                    "Please simplify and shorten the following paragraph so that it is easy " +
                                    "for a college student to understand. Keep key meaning, remove extra details.\n\n" +
                                    input
                            }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var bytes = Encoding.UTF8.GetBytes(json);
            using var body = new MemoryStream(bytes);

            var request = new InvokeModelRequest
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = body
            };

            var response = await _client.InvokeModelAsync(request);

            using var reader = new StreamReader(response.Body);
            var responseJson = await reader.ReadToEndAsync();

            // return format：content[0].text
            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            if (root.TryGetProperty("content", out var contentArray) &&
                contentArray.ValueKind == JsonValueKind.Array &&
                contentArray.GetArrayLength() > 0)
            {
                var textElement = contentArray[0].GetProperty("text");
                return textElement.GetString() ?? string.Empty;
            }

            // Defensive: if parsing fails, return the original JSON for debugging
            return responseJson;
        }
    }
}
