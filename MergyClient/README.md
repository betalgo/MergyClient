# Mergy Client

Mergy Client is a C# console application that provides a question-answering system using the Anthropic AI service. It allows users to interact with an AI model based on a provided knowledge base.

## Links:
- [Youtube video](https://www.youtube.com/watch?v=4gbvmFCAN0E) 
- [Blog Post](https://blog.kayhantolga.com/mergy-a-quick-tool-for-claude-projects)
- [Mergy](https://github.com/betalgo/Mergy)

## Features

- Loads a custom knowledge base from a file
- Interacts with the Anthropic AI service using the Claude 3.5 Sonnet model
- Maintains a conversation history
- Supports streaming responses from the AI
- Allows clearing of conversation history
- Handles errors and provides user-friendly messages

## Prerequisites

- .NET 8.0 SDK
- An Anthropic API key

## Setup

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/MergyClient.git
   cd MergyClient
   ```

2. Set up your Anthropic API key:
   - Option 1: Use user secrets
     ```
     dotnet user-secrets set "AnthropicServiceOptions:ApiKey" "your-api-key-here"
     ```
   - Option 2: Set an environment variable named `AnthropicServiceOptions__ApiKey`

3. Prepare your knowledge base:
   - Create a file named `knowledge_base.txt` in the project directory
   - Add your knowledge base content to this file

## Usage

1. Build and run the application:
   ```
   dotnet run
   ```

2. When prompted, either:
   - Press Enter to use the default `knowledge_base.txt` file
   - Enter the path to your custom knowledge base file

3. Start asking questions. The AI will respond based on the provided knowledge base.

4. Special commands:
   - Type 'exit' to quit the program
   - Type 'clear' to clear the conversation history

## Configuration

You can modify the following constants in `Program.cs` to adjust the behavior:

- `modelName`: The Anthropic model to use (default: "claude-3-5-sonnet-20240620")
- `maxHistoryItems`: Maximum number of conversation history items to keep (default: 10)
- `maxTokens`: Maximum number of tokens for AI responses (default: 1000)
- `defaultKnowledgeBasePath`: Default path for the knowledge base file (default: "knowledge_base.txt")

## Dependencies

- Betalgo.Ranul.Anthropic (v0.0.1)
- Microsoft.Extensions.Configuration (v8.0.0)
- Microsoft.Extensions.Configuration.UserSecrets (v8.0.0)
- Microsoft.Extensions.DependencyInjection (v8.0.0)
- Microsoft.Extensions.Options.ConfigurationExtensions (v8.0.0)

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
