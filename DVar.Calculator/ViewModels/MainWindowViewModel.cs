using System.Windows.Input;
using NCalc;
using ReactiveUI;

namespace DVar.Calculator.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _displayText = "";

    public string DisplayText
    {
        get => _displayText;
        set => this.RaiseAndSetIfChanged(ref _displayText, value);
    }

    public ICommand PutSymbolCommand { get; }
    public ICommand PutSymbolWithSpacesCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand EraseCommand { get; }
    public ICommand CalculateCommand { get; }

    private bool _calculated;

    public MainWindowViewModel()
    {
        PutSymbolCommand = ReactiveCommand.Create<string>(PutSymbol);
        PutSymbolWithSpacesCommand = ReactiveCommand.Create<string>(PutSymbolWithSpaces);
        ClearCommand = ReactiveCommand.Create(() => DisplayText = "");
        EraseCommand = ReactiveCommand.Create(EraseSymbol);
        CalculateCommand = ReactiveCommand.Create(Calculate);
    }

    private void PutSymbol(string symbol)
    {
        ClearIfCalculated();
        DisplayText += symbol;
    }

    private void PutSymbolWithSpaces(string symbol)
    {
        ClearIfCalculated();
        DisplayText += $" {symbol} ";
    }

    private void EraseSymbol()
    {
        if (string.IsNullOrEmpty(DisplayText))
        {
            return;
        }

        int removeCount = 1;
        if (char.IsWhiteSpace(DisplayText[^1]))
        {
            removeCount += 1;
        }

        string updatedDisplayText = DisplayText[..^removeCount];
        DisplayText = updatedDisplayText;
    }

    private void Calculate()
    {
        string updatedDisplayText = DisplayText
            .Replace(" ", "")
            .Replace("\t", "");

        var expression = new Expression(updatedDisplayText);
        object? result = expression.Evaluate();
        if (result != null)
        {
            DisplayText = result.ToString()!;
            _calculated = true;
        }
    }

    private void ClearIfCalculated()
    {
        if (_calculated)
        {
            DisplayText = "";
            _calculated = false;
        }
    }
}