using System.CommandLine;

namespace KD.Papa;

internal class OptionsCommand : Command
{
    private const string ApiKeyOption = "--apikey";

    private const string ApiKeyOptionDescription = "API key to use for OpenAI API authentication";
    
    private readonly IApiKeyService _apiKeyService;

    public OptionsCommand(IApiKeyService apiKeyService) : base("options", "Configure access to OpenAI API")
    {
        ArgumentNullException.ThrowIfNull(apiKeyService);
        this._apiKeyService = apiKeyService;

        ConfigureSet();
    }

    private void ConfigureSet()
    {
        var setCommand = new Command("set", "update configuration");

        var apiKeyOption = new Option<string>(ApiKeyOption, ApiKeyOptionDescription);

        setCommand.Add(apiKeyOption);

        setCommand.SetHandler(
            (string apiKeyOptionValue) => _apiKeyService.StoreApiKey(apiKeyOptionValue),
            apiKeyOption);

        Add(setCommand);
    }
}
