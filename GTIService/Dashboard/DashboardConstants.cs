using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Dashboard
{
    public class DashboardConstants
    {
        //Type of chart tool to be used
        public const DashboardsEnum IsMSChart = DashboardsEnum.MSCHART;
        public const string ReportDataset = "DataSet";
        public const string ReportPath = "~/Reports/";
        public const string OneToTwoStyle = "one-two-graph";
        public const string TwoToOneStyle = "two-one-graph";
        public const string HalfStyle = "half-graph";
        public const string FullStyle = "full-graph";
        public const string NullStyle = "null-graph";
        public const string GridWrapStyle = "gridwrap";
        public const string FilterName = "Name";
        public const string FilterKey = "Value";
        public const string HeaderImage = "HeaderImage";
        public const string FilterTitle = "FilterTitle";
        public const string ImagePath = "..\\Reports\\Images\\";
    }
    #region Common Constants and Enums

    #region Common Enums

    public enum DashboardsEnum
    {
        COMPONENTART,
        MSCHART,
        RDLCCHART
    }

    /// <summary>
    /// Dashlet Layout Enum
    /// </summary>
    public enum LayoutEnum
    {
        ONETOTWO = 1,
        TWOTOONE,
        ONETOONE,
        FULL
    }
    /// <summary>
    /// Chart Type Enum
    /// </summary>
    public enum ChartsEnum
    {
        BAR = 1,
        PIE,
        LINE,
        STACKED,
        MULTILINE,
        GAUGE,
        COMPOSITE,
        GRID,
        MERGED,
        RDLC
    }
    /// <summary>
    /// Chart Subtype Enum
    /// </summary>
    public enum SubChartsEnum
    {
        BarBlock = 1,
        BarCone,
        BarCylinder,
        BarHexagon,
        BarParaboloid,
        BarPrism3,
        BarPyramid,
        BarPyramid3,
        BarPyramid6,
        BarRectangle,
        PieDoughnut,
        PieDoughnutInverted,
        PieDoughnutRound,
        PieDoughnutThin,
        Pie,
        PieRound,
        PieThin,
        Line,
        Line2D,
        Line2DSmooth,
        Line2DStep,
        LineSmooth,
        LineStep,
        LineRadarLine,
        StBarBlock,
        StBarCone,
        StBarCylinder,
        StBarHexagon,
        StBarParaboloid,
        StBarPrism3,
        StBarPyramid,
        StBarPyramid3,
        StBarPyramid6,
        StBarRectangle,
        MltLine,
        MltLine2D,
        MltLine2DSmooth,
        MltLine2DStep,
        MltLineSmooth,
        MltLineStep,
        MltLineRadarLine,
        GugHalfCircleE,
        GugHalfCircleN,
        GugHalfCircleW,
        GugHalfCircleS,
        GugQuarterCircleE,
        GugQuarterCircleNE,
        GugQuarterCircleN,
        GugQuarterCircleNW,
        GugQuarterCircleW,
        GugQuarterCircleSW,
        GugQuarterCircleS,
        GugQuarterCircleSE,
        GugLinearHorizontal,
        GugLinearVertical,
        GugNumeric,
        Composite,
        GugCircular,
        Grid,
        MgBarBlock,
        MgBarCone,
        MgBarCylinder,
        MgBarHexagon,
        MgBarParaboloid,
        MgBarPrism3,
        MgBarPyramid,
        MgBarPyramid3,
        MgBarPyramid6,
        MgBarRectangle,
    }
    /// <summary>
    /// Dashlet Chart Property Enum
    /// </summary>
    public enum ChartPropertiesEnum
    {
        BarDataPointColor = 2,
        BarLegendText,
        BarMarkerStyle,
        BarToolTip,
        BarTransparency,
        PieLegendText,
        PieTooltip,
        PieTransparency,
        BarDepth,
        BarGradientStyle,
        BarMarkerSize,
        BarStyleName,
        PieDataPointColor,
        PieHeight,
        PieLift,
        PieShift,
        PieWidth,
        PieStyleName,
        LineGradientStyle,
        LineLegendText,
        LineToolTip,
        LineTransparency,
        StBarDataPointColor,
        StBarLegendText,
        StBarMarkerStyle,
        StBarToolTip,
        StBarTransparency,
        StBarDepth,
        StBarGradientStyle,
        StBarMarkerSize,
        StBarStyleName,
        MLineGradientStyle,
        MLineLegendText,
        MLineToolTip,
        MLineTransparency,
        GugThemeName,
        GugToolTip,
        GugStyleName,
        LineStyleName,
        MLineStyleName,
        CompositeToolTip,
        CompositeTransparency,
        CompositeDataPointColor,
        CompositeStyleName,
        GridToolTip,
        GridStyleName
    }
    /// <summary>
    /// Grid Size Enum
    /// </summary>
    public enum GridSizeEnum
    {
        OneThird = 1,
        TwoThird,
        Half,
        Full,
        Popup
    }
    #endregion

    #region Common Constatns
    /// <summary>
    /// Dashlet Chart Property
    /// </summary>
    public class DshProperties
    {
        public const string Title = "title";
        public const string StyleCss = "stylecss";
        public const string DataPointColor = "datapointcolor";
        public const string LegendText = "legendtext";
        public const string ToolTip = "tooltip";
        public const string Transparency = "transparency";
        public const string ThemeName = "ThemeName";
        public const string GradientStyle = "GradientStyle";
    }
    /// <summary>
    /// Chart Subtype
    /// </summary>
    public class ChartSubTypes
    {
        public const string Block = "Block";
        public const string Cone = "Cone";
        public const string Cylinder = "Cylinder";
        public const string Hexagon = "Hexagon";
        public const string Paraboloid = "Paraboloid";
        public const string Prism3 = "Prism3";
        public const string Pyramid = "Pyramid";
        public const string Pyramid3 = "Pyramid3";
        public const string Pyramid6 = "Pyramid6";
        public const string Rectangle = "Rectangle";
        public const string Doughnut = "Doughnut";
        public const string DoughnutInverted = "DoughnutInverted";
        public const string DoughnutRound = "DoughnutRound";
        public const string DoughnutThin = "DoughnutThin";
        public const string Pie = "Pie";
        public const string PieRound = "PieRound";
        public const string PieThin = "PieThin";
        public const string Line = "Line";
        public const string Line2D = "Line2D";
        public const string Line2DSmooth = "Line2DSmooth";
        public const string Line2DStep = "Line2DStep";
        public const string LineSmooth = "LineSmooth";
        public const string LineStep = "LineStep";
        public const string RadarLine = "RadarLine";
        public const string HalfCircleE = "HalfCircleE";
        public const string HalfCircleN = "HalfCircleN";
        public const string HalfCircleW = "HalfCircleW";
        public const string HalfCircleS = "HalfCircleS";
        public const string QuarterCircleE = "QuarterCircleE";
        public const string QuarterCircleNE = "QuarterCircleNE";
        public const string QuarterCircleN = "QuarterCircleN";
        public const string QuarterCircleNW = "QuarterCircleNW";
        public const string QuarterCircleW = "QuarterCircleW";
        public const string QuarterCircleSW = "QuarterCircleSW";
        public const string QuarterCircleS = "QuarterCircleS";
        public const string QuarterCircleSE = "QuarterCircleSE";
        public const string LinearHorizontal = "LinearHorizontal";
        public const string LinearVertical = "LinearVertical";
        public const string Numeric = "Numeric";
        public const string Circular = "Circular";
    }
    /// <summary>
    /// Guage Themes
    /// </summary>
    public class ThemeNames
    {
        public const string BlackIce = "Black Ice";
        public const string ArcticWhite = "Arctic White";
        public const string Default = "Default";
        public const string Monochrome = "Monochrome";
    }
    /// <summary>
    /// MSChart properties
    /// </summary>
    public class MSChartProperties
    {
        /// <summary>
        /// Backgroud color of the Chart
        /// </summary>
        public const string BackColor = "BackColor";
        /// <summary>
        /// Css Class of the Chart
        /// </summary>
        public const string StyleCss = "StyleCss";
        /// <summary>
        /// Is Chart 3D
        /// </summary>
        public const string Enable3D = "Enable3D";
        /// <summary>
        /// To show Legends
        /// </summary>
        public const string EnableLegend = "EnableLegend";
        /// <summary>
        /// To show ednd point values of the X-Scale
        /// </summary>
        public const string IsXEndLabelVisible = "IsXEndLabelVisible";
        /// <summary>
        /// To show scale values as Labels
        /// </summary>
        public const string IsValueShownAsLabel = "IsValueShownAsLabel";
        /// <summary>
        /// To enable X-axis margin
        /// </summary>
        public const string IsXMarginVisible = "IsXMarginVisible";
        /// <summary>
        /// To show X-axis marker lines
        /// </summary>
        public const string ShowMarkerLines = "ShowMarkerLines";
        /// <summary>
        /// Position of Legend(top, bottom, left, right)
        /// </summary>
        public const string LegendDocking = "LegendDocking";
        /// <summary>
        /// Alignment of Legend
        /// </summary>
        public const string LegendAlignment = "LegendAlignment";
        /// <summary>
        /// The Style effect for the Chart
        /// For Bar, Column,Pie and Doughnut
        /// </summary>
        public const string DrawingStyle = "DrawingStyle";
        /// <summary>
        /// Width of the Bars
        /// For Bar and Column
        /// </summary>
        public const string PointWidth = "PointWidth";
        /// <summary>
        /// Position of the Label
        /// For Bar and Column
        /// </summary>
        public const string BarLabelStyle = "BarLabelStyle";
        /// <summary>
        /// Position of the Label
        /// For Pie and Doughnut
        /// </summary>
        public const string PieLabelStyle = "PieLabelStyle";
        /// <summary>
        /// Radius of the Chart
        /// For Pie and Doughnut
        /// </summary>
        public const string Radius = "Radius";
    }

    #endregion

    #endregion
}
