using ConsoleApp;
using Figure;
using Serilog;

namespace SignalRApp
{
    public class ModelService
    {
        #region Private Fields
        private int _N;
        private int _L;
        private int _sleep;
        private int[] _oddNumbers = new int[8];
        private List<double> _amounts = [];
        #endregion

        #region Constructors
        public ModelService(ConfigurationManager manager)
        {
            _N = manager.GetValue<int>("N");
            _L = manager.GetValue<int>("L");
            _sleep = manager.GetValue<int>("Sleep");

            _oddNumbers = Calculations.OddNumbersInit();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Вычисление сумм
        /// </summary>
        /// <returns></returns>
        public List<double> GetAmounts()
        {
            Log.Debug("Model.GetAmounts: Start");
            List<double> amounts = [];
            _amounts = Calculations.CountAmounts(_oddNumbers, _N, _L);
            foreach (double value in _amounts) amounts.Add(value);
            Log.Debug("Model.GetAmounts: Done");
            return amounts;
        }

        /// <summary>
        /// Инициализация фигур
        /// </summary>
        /// <returns></returns>
        public List<IFigure> FiguresInit()
        {
            Log.Debug("Model.FiguresInit: Start");
            List<double> speeds = Calculations.SpeedsInit(_amounts);
            List<IFigure> figures = Calculations.FiguresInit(speeds);
            Log.Debug("Model.FiguresInit: Done");
            return figures;
        }

        public int GetSleep() => _sleep;
        #endregion
    }
}
