namespace KD.Papa;

interface IApiKeyService
{
    string GetApiKey();

    void StoreApiKey(string apiKey);
}
