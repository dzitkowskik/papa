namespace KD.Papa;

interface IChatService
{
    IAsyncEnumerable<string> Query(string prompt, ChatQueryOptions options);
}
