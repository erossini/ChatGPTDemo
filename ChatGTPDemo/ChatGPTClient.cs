using ChatGTPDemo.Enums;
using ChatGTPDemo.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ChatGTPDemo
{
    /// <summary>
    /// Class ChatGPTClient.
    /// </summary>
    public class ChatGPTClient
    {
        #region Variables

        /// <summary>
        /// The chat request URI
        /// </summary>
        private readonly string chatRequestUri;
        /// <summary>
        /// The include history with chat completion
        /// </summary>
        private readonly bool includeHistoryWithChatCompletion;
        /// <summary>
        /// The message history
        /// </summary>
        private readonly List<Message> messageHistory;
        /// <summary>
        /// The model
        /// </summary>
        private readonly OpenAIModels model;
        /// <summary>
        /// The open aiapi key
        /// </summary>
        private readonly string openAIAPIKey;
        /// <summary>
        /// The temperature
        /// </summary>
        private readonly double temperature;
        /// <summary>
        /// The top p
        /// </summary>
        private readonly double top_p;

        #endregion Variables

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatGPTClient"/> class.
        /// </summary>
        /// <param name="includeHistoryWithChatCompletion">if set to <c>true</c> [include history with chat completion].</param>
        /// <param name="model">The model.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="top_p">The top p.</param>
        public ChatGPTClient(bool includeHistoryWithChatCompletion = true,
            OpenAIModels model = OpenAIModels.gpt_3_5_turbo,
            double temperature = 1,
            double top_p = 1)
        {
            chatRequestUri = "https://api.openai.com/v1/chat/completions";
            openAIAPIKey = Environment.GetEnvironmentVariable("OpenAIAPIKey")!;
            messageHistory = new List<Message>();

            this.includeHistoryWithChatCompletion = includeHistoryWithChatCompletion;
            this.model = model;
            this.temperature = temperature;
            this.top_p = top_p;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>System.Nullable&lt;ChatResponse&gt;.</returns>
        public async Task<ChatResponse?> SendMessage(string message)
        {
            var chatResponse = await Chat(message);

            if (chatResponse != null)
            {
                AddMessageToHistory(new Message { Role = "user", Content = message });

                foreach (var responseMessage in chatResponse.Choices!.Select(c => c.Message!))
                    AddMessageToHistory(responseMessage);
            }

            return chatResponse;
        }

        /// <summary>
        /// Adds the message to history.
        /// </summary>
        /// <param name="message">The message.</param>
        private void AddMessageToHistory(Message message) =>
            messageHistory.Add(message);

        /// <summary>
        /// Chats the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>System.Nullable&lt;ChatResponse&gt;.</returns>
        private async Task<ChatResponse?> Chat(string message)
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, chatRequestUri);
            request.Headers.Add("Authorization", $"Bearer {openAIAPIKey}");

            var requestBody = new
            {
                model = GetModel(),
                temperature,
                top_p,
                messages = GetMessageObjects(message)
            };

            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var chatResponse = await response.Content.ReadFromJsonAsync<ChatResponse>();

                if (chatResponse != null &&
                    chatResponse.Choices != null &&
                    chatResponse.Choices.Any(c => c.Message != null))
                    return chatResponse;
            }

            return null;
        }

        /// <summary>
        /// Gets the message objects.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>IEnumerable&lt;System.Object&gt;.</returns>
        private IEnumerable<object> GetMessageObjects(string message)
        {
            foreach (var historicMessage in includeHistoryWithChatCompletion ? messageHistory : Enumerable.Empty<Message>())
            {
                yield return new { role = historicMessage.Role, content = historicMessage.Content };
            }

            yield return new { role = "user", content = message };
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetModel() =>
            model.ToString().Replace("3_5", "3.5").Replace("_", "-");
    }
}