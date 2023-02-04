using System.Globalization;
using System.CommandLine;
using System.Web;
using OpenAI_API;

namespace KD.Papa;

class PapaCommand : RootCommand
{
    private readonly IChatService _chatService;
    private readonly IApiKeyService _apiKeyService;

    public PapaCommand(IChatService chatService, IApiKeyService apiKeyService) : base("Command line client for ChatGPT from OpenAI")
    {
        ArgumentNullException.ThrowIfNull(apiKeyService);
        ArgumentNullException.ThrowIfNull(chatService);
        
        this._chatService = chatService;
        this._apiKeyService = apiKeyService;

        this.Configure();
    }

    private void Configure()
    {
        this.AddSubCommands();

        var chatQueryArgument = AddChatQueryArgument();
        var maxTokensOption = AddMaxTokensOption();
        var temperatureOption = AddTemperatureOption();
        var codeOption = AddCodeOption();

        var optionsBinder = new ChatQueryOptionsBinder(maxTokensOption, temperatureOption, codeOption);

        this.SetHandler(
            async (inputTokens, options) => await this.HandleCommand(inputTokens, options),
            chatQueryArgument,
            optionsBinder);
    }

    private void AddSubCommands()
    {
        this.Add(new OptionsCommand(_apiKeyService));
    }

    private Option<bool> AddCodeOption()
    {
        var codeOption = new Option<bool>(new[] { "--code", "--codex", "-c" }, "Use Davinci Codex model");
        this.Add(codeOption);
        return codeOption;
    }

    private Option<double> AddTemperatureOption()
    {
        var temperatureOption = new Option<double>(
                    new[] { "--temperature", "--temp", "-t" },
                    () => 0.1,
                    "Value from 0.0 to 1.0 saying how 'creative' should the answer be");
        this.Add(temperatureOption);
        return temperatureOption;
    }

    private Option<int> AddMaxTokensOption()
    {
        var maxTokensOption = new Option<int>(new[] { "--max-tokens", "-m" }, () => 500, "Maximum amount of tokens for the answer");
        this.Add(maxTokensOption);
        return maxTokensOption;
    }

    private Argument<string[]> AddChatQueryArgument()
    {
        var chatQueryArgument = new Argument<string[]>("A query for the chatGPT");
        this.Add(chatQueryArgument);
        return chatQueryArgument;
    }

    private async Task<int> HandleCommand(string[] inputTokens, ChatQueryOptions options)
    {
        try
        {
            var prompt = string.Join(' ', inputTokens);

            await foreach(var responsePart in this._chatService.Query(prompt, options))
            {
                Console.Write(responsePart);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return 0;
        }

        return 1;
    }
}
