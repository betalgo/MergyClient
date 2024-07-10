using System.Text;
using Anthropic.ObjectModels;
using Anthropic.Services;

namespace MergyClient;
/// <summary>
/// Helper class for interacting with the Anthropic AI service.
/// </summary>
internal class AiHelper
{
    private readonly IAnthropicService _anthropicService;
    private readonly List<Message> _conversationHistory = new();
    private readonly string _modelName;
    private readonly int _maxHistoryItems;
    private readonly int _maxTokens;

    /// <summary>
    /// Initializes a new instance of the AiHelper class.
    /// </summary>
    /// <param name="anthropicService">The Anthropic service to use for API calls.</param>
    /// <param name="modelName">The name of the AI model to use.</param>
    /// <param name="maxHistoryItems">The maximum number of conversation history items to keep.</param>
    /// <param name="maxTokens">The maximum number of tokens to generate in responses.</param>
    public AiHelper(IAnthropicService anthropicService, string modelName, int maxHistoryItems, int maxTokens)
    {
        _anthropicService = anthropicService;
        _modelName = modelName;
        _maxHistoryItems = maxHistoryItems;
        _maxTokens = maxTokens;
    }

    /// <summary>
    /// Gets a streaming answer from the AI service based on the current conversation history and system prompt.
    /// </summary>
    /// <param name="systemPrompt">The system prompt to use for the question.</param>
    public async Task GetStreamingAnswerAsync(string systemPrompt)
    {
        try
        {
            var request = new MessageRequest
            {
                Model = _modelName,
                System = systemPrompt,
                Messages = _conversationHistory,
                MaxTokens = _maxTokens,
                Stream = true
            };

            ConsoleExtensions.WriteLine("\nAnswer:", ConsoleColor.Green);
            Console.ForegroundColor = ConsoleColor.White;
            var fullResponse = new StringBuilder();

            await foreach (var response in _anthropicService.Messages.CreateAsStream(request))
            {
                if (response.Successful)
                {
                    if (response.Content != null && response.Content.Count != 0)
                    {
                        var content = response.Content.First().Text;
                        Console.Write(content);
                        fullResponse.Append(content);
                    }
                }
                else
                {
                    ConsoleExtensions.WriteLine($"\nError: {response.Error?.Message ?? "Unknown error occurred"}", ConsoleColor.Red);
                    break;
                }
            }

            Console.ResetColor();
            Console.WriteLine(); // New line after the complete answer

            // Add the assistant's response to the conversation history
            _conversationHistory.Add(Message.FromAssistant(fullResponse.ToString()));

            // Trim history if it gets too long
            if (_conversationHistory.Count > _maxHistoryItems)
            {
                _conversationHistory.RemoveRange(0, 2); // Remove the oldest user question and AI response
            }
        }
        catch (Exception ex)
        {
            ConsoleExtensions.WriteLine($"\nAn error occurred: {ex.Message}", ConsoleColor.Red);
        }
    }

    /// <summary>
    /// Clears the conversation history.
    /// </summary>
    public void ClearConversationHistory()
    {
        _conversationHistory.Clear();
    }

    /// <summary>
    /// Adds a message to the conversation history.
    /// </summary>
    /// <param name="message">The message to add to the history.</param>
    public void AddConversationHistory(Message message)
    {
        _conversationHistory.Add(message);
    }
}