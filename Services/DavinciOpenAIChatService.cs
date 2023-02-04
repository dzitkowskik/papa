using System.Web;
using OpenAI_API;

namespace KD.Papa;

class DavinciOpenAIChatService : IChatService
{
    private readonly IApiKeyService _apiKeyService;

    public DavinciOpenAIChatService(IApiKeyService apiKeyService)
    {
        ArgumentNullException.ThrowIfNull(apiKeyService);
        this._apiKeyService = apiKeyService;
    }

    public async IAsyncEnumerable<string> Query(string prompt, ChatQueryOptions options)
    {
        var api = new OpenAIAPI(new APIAuthentication(this._apiKeyService.GetApiKey()));

        var model = options.UseCodeModel ? Model.DavinciCode : Model.DavinciText;
        var query = new CompletionRequest(
            prompt: prompt,
            model: model,
            max_tokens: options.MaxResultTokens,
            temperature: options.Temperature);

        var completion = api.Completions.StreamCompletionEnumerableAsync(query);
        await foreach (var token in completion)
        {
            yield return HttpUtility.HtmlDecode(token.Completions.Select(t => t.Text).First());
        }
    }
}
