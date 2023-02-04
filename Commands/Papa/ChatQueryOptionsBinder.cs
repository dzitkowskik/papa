using System.CommandLine;
using System.CommandLine.Binding;

namespace KD.Papa;

public class ChatQueryOptionsBinder : BinderBase<ChatQueryOptions>
{
    private readonly Option<int> _maxTokensOption;
    private readonly Option<double> _temperatureOption;
    private readonly Option<bool> _codeOption;

    public ChatQueryOptionsBinder(Option<int> maxTokensOption, Option<double> temperatureOption, Option<bool> codeOption)
    {
        ArgumentNullException.ThrowIfNull(maxTokensOption);
        ArgumentNullException.ThrowIfNull(temperatureOption);
        ArgumentNullException.ThrowIfNull(codeOption);

        _maxTokensOption = maxTokensOption;
        _temperatureOption = temperatureOption;
        _codeOption = codeOption;
    }

    protected override ChatQueryOptions GetBoundValue(BindingContext bindingContext) =>
        new ChatQueryOptions(
            bindingContext.ParseResult.GetValueForOption(_maxTokensOption),
            bindingContext.ParseResult.GetValueForOption(_temperatureOption),
            bindingContext.ParseResult.GetValueForOption(_codeOption)
        );
}
