using KeySharp;

namespace KD.Papa;

class SecretStoreApiKeyService : IApiKeyService
{
    public string GetApiKey() => Keyring.GetPassword(
        Constants.PasswordPackageName, Constants.ServiceName, Constants.ApiKeySecretName);

    public void StoreApiKey(string apiKey) => Keyring.SetPassword(
        Constants.PasswordPackageName, Constants.ServiceName, Constants.ApiKeySecretName, apiKey);
}
