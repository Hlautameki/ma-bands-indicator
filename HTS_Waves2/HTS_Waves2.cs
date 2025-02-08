using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    [Cloud("Slow MA High", "Slow MA Low",  Opacity = 0.2)]
    [Cloud("Fast MA High", "Fast MA Low",  Opacity = 0.2)]
    public class FourMovingAveragesWithCloud : Indicator
    {
        [Parameter("Fast MA Period", DefaultValue = 33)]
        public int FastMAPeriod { get; set; }

        [Parameter("Slow MA Period", DefaultValue = 144)]
        public int SlowMAPeriod { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.WilderSmoothing)]
        public MovingAverageType MAType { get; set; }

        [Parameter("TimeFrame", DefaultValue = null)]
        public TimeFrame TimeFrameCustom { get; set; }

        [Parameter("AutoTimeFrame", DefaultValue = true)]
        public bool AutoTimeFrame { get; set; }

        [Output("Fast MA High", LineColor = "SkyBlue")]
        public IndicatorDataSeries FastHighMA { get; set; }

        [Output("Fast MA Low", LineColor = "SkyBlue")]
        public IndicatorDataSeries FastLowMA { get; set; }

        [Output("Slow MA High", LineColor = "Red")]
        public IndicatorDataSeries SlowHighMA { get; set; }

        [Output("Slow MA Low", LineColor = "Red")]
        public IndicatorDataSeries SlowLowMA { get; set; }

        private MovingAverage _fastHighMA, _fastLowMA, _slowHighMA, _slowLowMA;

        protected override void Initialize()
        {
            Bars bars;
            // Get the bar data for the specified timeframe
            if (AutoTimeFrame)
            {
                bars = Bars;
            }
            else
            {
                bars = MarketData.GetBars(TimeFrameCustom);
            }

            _fastHighMA = Indicators.MovingAverage(bars.HighPrices, FastMAPeriod, MAType);
            _fastLowMA = Indicators.MovingAverage(bars.LowPrices, FastMAPeriod, MAType);
            _slowHighMA = Indicators.MovingAverage(bars.HighPrices, SlowMAPeriod, MAType);
            _slowLowMA = Indicators.MovingAverage(bars.LowPrices, SlowMAPeriod, MAType);
        }

        public override void Calculate(int index)
        {
            // Assign the calculated values to the output series
            FastHighMA[index] = _fastHighMA.Result[index];
            FastLowMA[index] = _fastLowMA.Result[index];
            SlowHighMA[index] = _slowHighMA.Result[index];
            SlowLowMA[index] = _slowLowMA.Result[index];
        }
    }
}
