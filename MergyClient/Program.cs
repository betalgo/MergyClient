using Anthropic.Extensions;
using Anthropic.ObjectModels;
using Anthropic.Services;
using MergyClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Constants for configuration and system settings
const string modelName = "claude-3-5-sonnet-20240620";
const int maxHistoryItems = 10;
const int maxTokens = 1000;
const string defaultKnowledgeBasePath = "knowledge_base.txt";

try
{
    /*
     Add this setting to user-secrets or add you key from environment :
     {
         "AnthropicServiceOptions": {
             "ApiKey": "sk-ant-********************"
         }
     }
     */

    // Setup Configuration and Services
    var builder = new ConfigurationBuilder().AddUserSecrets<Program>();


    IConfiguration configuration = builder.Build();
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddScoped(_ => configuration);
    serviceCollection.AddAnthropicService();
    var serviceProvider = serviceCollection.BuildServiceProvider();
    var anthropicService = serviceProvider.GetRequiredService<IAnthropicService>();

    // Get knowledge base path from user
    Console.Write("Enter the path to the knowledge base file (or press Enter for default): ");
    var knowledgeBasePath = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(knowledgeBasePath))
    {
        knowledgeBasePath = defaultKnowledgeBasePath;
    }

    // Load Knowledge Base
    string knowledgeBase;
    try
    {
        knowledgeBase = await File.ReadAllTextAsync(knowledgeBasePath);
        ConsoleExtensions.WriteLine("Knowledge base loaded successfully.", ConsoleColor.Green);
    }
    catch (Exception ex)
    {
        ConsoleExtensions.WriteLine($"Error reading knowledge base: {ex.Message}", ConsoleColor.Red);
        return;
    }

    ConsoleExtensions.WriteLine("Welcome to the Mergy QA System!", ConsoleColor.Cyan);
    ConsoleExtensions.WriteLine("Type 'exit' to quit the program or 'clear' to clear the conversation history.", ConsoleColor.Yellow);

    var systemPrompt = $"Knowledge Base:\n{knowledgeBase}\n\nUse the above knowledge base to answer user questions. If the answer is not in the knowledge base, say 'I don't have enough information to answer that question.'";
    var aiHelper = new AiHelper(anthropicService, modelName, maxHistoryItems, maxTokens);

    while (true)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("\nEnter your question: ");
        Console.ResetColor();
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            break;

        if (input.Equals("clear", StringComparison.OrdinalIgnoreCase))
        {
            aiHelper.ClearConversationHistory();
            ConsoleExtensions.WriteLine("Conversation history cleared.", ConsoleColor.Yellow);
            continue;
        }

        aiHelper.AddConversationHistory(Message.FromUser(input));
        await aiHelper.GetStreamingAnswerAsync(systemPrompt);
    }

    ConsoleExtensions.WriteLine("Thank you for using the Mergy QA System. Goodbye!", ConsoleColor.Cyan);
}
catch (Exception ex)
{
    ConsoleExtensions.WriteLine($"An unexpected error occurred: {ex.Message}", ConsoleColor.Red);
}