using Avalonia.Collections;
using AvaloniaApplication.ViewModels;
using ConsoleApp1;
using Figure;
using Serilog;
using Serilog.Core;
using System.Collections.Generic;

namespace AvaloniaApplication.Models;

public class Model
{
    private int _N = 0;
    private int _L = 0;
    private int _sleep = 0;
    private int[] _oddNumbers = new int[8];
    private List<double?> _amounts = [];

    /// <summary>
    /// Логгер для уровня Information
    /// </summary>
    private Logger _infoLog = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File("InformationLog.json")
        .CreateLogger();

    /// <summary>
    /// Логгер для уровня Debug
    /// </summary>
    private Logger _debugLog = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.File("DebugLog.json")
        .CreateLogger();

    /// <summary>
    /// Логгер для уровня Error
    /// </summary>
    private Logger _errorLog = new LoggerConfiguration()
        .MinimumLevel.Error()
        .WriteTo.File("ErrorLog.json")
        .CreateLogger();

    public int Sleep { get => _sleep; }

    public Model()
    {
        SettingUpLoggers();
        Calculations.StartConfiguration(ref _N, ref _L, ref _sleep);
        _oddNumbers = Calculations.OddNumbersInit();
    }

    public AvaloniaList<DoubleValue> GetAmounts()
    {
        AvaloniaList<DoubleValue> amountsAvalonia = [];
        _amounts = Calculations.CountAmounts(_oddNumbers, _N, _L);
        foreach (double value in _amounts) amountsAvalonia.Add(new DoubleValue(value));
        return amountsAvalonia;
    }

    public AvaloniaList<IFigure> FiguresInit()
    {
        List<double> speeds = Calculations.SpeedsInit(_amounts);
        List<IFigure> figures = Calculations.FiguresInit(speeds);
        AvaloniaList<IFigure> figuresAvalonia = [];
        foreach (var figure in figures) figuresAvalonia.Add(figure);
        return figuresAvalonia;
    }

    private void SettingUpLoggers()
    {
        Configuration.Logger = _debugLog;
        Formulas.Logger = _debugLog;
        Extensions.Logger = _debugLog;
        Extensions.LoggerError = _errorLog;
        AbstractFigure.Logger = _debugLog;
        Calculations.InfoLog = _infoLog;
        Calculations.ErrorLog = _errorLog;
    }
}
