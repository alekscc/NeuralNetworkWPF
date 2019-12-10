using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NeuralNetworkWPF
{
    class StatisticsWindowViewModel : INotifyPropertyChanged
    {


        private PlotModel plotModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public PlotModel PlotModel
        {
            get
            {
                return plotModel;
            }
            set
            {
                plotModel = value; OnPropertyChanged("PlotModel");
            }
        }


        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
        public StatisticsWindowViewModel()
        {
            SetUpModel();
        }
        private void SetUpModel()
        {

            PlotModel = new PlotModel();

            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;
            // AxisPosition.Bottom, "Date", "dd/MM/yy HH:mm"
            //var dateAxis = new DateTimeAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
            //dateAxis.Title = "Rok";

            var genAxis = new LinearAxis() { Position = AxisPosition.Bottom, Title = "Generation", MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot };



            var errAxis = new LinearAxis() { Position = AxisPosition.Left, Title = "Error", MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot };




            //foreach (var item in  Global.Instance.Stats.IndividualRecords)
            //{
            //    var lineSeries = new LineSeries
            //    {
            //        StrokeThickness = 2,
            //        MarkerSize = 1,
            //        Color = OxyColor.FromArgb(255, 255, 0, 0),
            //        LineStyle = LineStyle.Solid,
            //        CanTrackerInterpolatePoints = true,
            //        Points = new List<DataPoint>()
            //        LineJoin = LineJoin.Round,
            //        MarkerType = MarkerType.Plus,
            //        MarkerStroke = OxyColor.FromArgb(255, 0, 255, 0),

            //    };

            //    //collection.ForEach(x => lineSeries.Points.Add(new DataPoint(LinearAxis.ToDouble(item), LinearAxis.ToDouble(25))));
            //    

            //    plotModel.Series.Add(lineSeries);
            //}

            var lineSeriesLearning = new LineSeries()
            {
                Color = OxyColors.Green,
                LineStyle = LineStyle.Solid,
                CanTrackerInterpolatePoints = true,
                LineJoin = LineJoin.Round,
                InterpolationAlgorithm = InterpolationAlgorithms.ChordalCatmullRomSpline,
            };

            var lineSeriesValidation = new LineSeries()
            {
                Color = OxyColors.Aqua,
                LineStyle = LineStyle.Solid,
                CanTrackerInterpolatePoints = true,
                LineJoin = LineJoin.Round,
                InterpolationAlgorithm = InterpolationAlgorithms.ChordalCatmullRomSpline,
            };

            foreach (var item in Global.Instance.Stats.IndividualRecords)
            {
                lineSeriesLearning.Points.Add(new DataPoint(item.X, item.Y));
                lineSeriesValidation.Points.Add(new DataPoint(item.X, item.Z));
            }



            plotModel.Series.Add(lineSeriesLearning);
            plotModel.Series.Add(lineSeriesValidation);

            //dateAxis.StringFormat = "dd/MM/yy HH:mm";
            PlotModel.Axes.Add(genAxis);
            PlotModel.Axes.Add(errAxis);



        }
    }
}
