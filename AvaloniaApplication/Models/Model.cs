﻿using Avalonia.Collections;
using ConsoleApp;
using Figure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Collections.Generic;

namespace AvaloniaApplication.Models;

/// <summary>
/// Класс модели предметной области
/// </summary>
public class Model
{
    #region Private Fields
    private int _N;
    private int _L;
    private int _sleep;
    private int[] _oddNumbers = new int[8];
    private List<double> _amounts = [];
    #endregion

    #region Properties
    public int Sleep { get => _sleep; }
    #endregion

    #region Constructors
    public Model()
    {
        _N = App.Current.Services.GetRequiredService<ConfigurationService>().GetN();
        _L = App.Current.Services.GetRequiredService<ConfigurationService>().GetL();
        _sleep = App.Current.Services.GetRequiredService<ConfigurationService>().GetSleep();
        _oddNumbers = Calculations.OddNumbersInit();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Вычисление сумм
    /// </summary>
    /// <returns></returns>
    public AvaloniaList<DoubleValue> GetAmounts()
    {
        Log.Debug("Model.GetAmounts: Start");
        AvaloniaList<DoubleValue> amountsAvalonia = [];
        _amounts = Calculations.CountAmounts(_oddNumbers, _N, _L);
        foreach (double value in _amounts) amountsAvalonia.Add(new DoubleValue(value));
        Log.Debug("Model.GetAmounts: Done");
        return amountsAvalonia;
    }

    /// <summary>
    /// Инициализация фигур
    /// </summary>
    /// <returns></returns>
    public AvaloniaList<IFigure> FiguresInit()
    {
        Log.Debug("Model.FiguresInit: Start");
        List<double> speeds = Calculations.SpeedsInit(_amounts);
        List<IFigure> figures = Calculations.FiguresInit(speeds);
        AvaloniaList<IFigure> figuresAvalonia = [];
        foreach (var figure in figures) figuresAvalonia.Add(figure);
        Log.Debug("Model.FiguresInit: Done");
        return figuresAvalonia;
    }
    #endregion
}
