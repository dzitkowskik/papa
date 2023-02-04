using System.CommandLine;
using KD.Papa;

var apiKeyService = new SecretStoreApiKeyService();
var chatService = new DavinciOpenAIChatService(apiKeyService);

await new PapaCommand(chatService, apiKeyService).InvokeAsync(args);
