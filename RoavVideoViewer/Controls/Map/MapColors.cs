using System;
using Mapsui.Styles;

namespace RoavVideoViewer.Controls.Map
{
    class MapColors
    {
        private const double SymbolScale = 0.2;
        private const int PenWidth = 5;

        private static readonly Color TenColor = Color.Cyan;
        private static readonly Color TwentyColor = Color.Blue;
        private static readonly Color ThirtyColor = Color.Violet;
        private static readonly Color FourtyColor = Color.Green;
        private static readonly Color FiftyColor = Color.Yellow;
        private static readonly Color SixtyColor = Color.Orange;
        private static readonly Color SeventyColor = Color.Red;
        private static readonly Color EightyColor = Color.FromArgb(255, 99, 33, 33);
        private static readonly Color OverEightyColor = Color.Black;

        public static SymbolStyle ToTenMphStyle = new SymbolStyle
        {
            SymbolScale = SymbolScale,
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(TenColor)
        };

        public static SymbolStyle ToTwentyMphStyle = new SymbolStyle
        {
            SymbolScale = SymbolScale,
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(TwentyColor)
        };

        public static SymbolStyle ToThirtyMphStyle = new SymbolStyle
        {
            SymbolScale = SymbolScale,
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(ThirtyColor)
        };

        public static SymbolStyle ToFourtyMphStyle = new SymbolStyle
        {
            SymbolScale = SymbolScale,
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(FourtyColor)
        };

        public static SymbolStyle ToFiftyMphStyle = new SymbolStyle
        {
            SymbolScale = SymbolScale,
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(FiftyColor)
        };

        public static SymbolStyle ToSixtyMphStyle = new SymbolStyle
        {
            SymbolScale = SymbolScale,
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(SixtyColor)
        };

        public static SymbolStyle ToSeventyMphStyle = new SymbolStyle
        {
            SymbolScale = SymbolScale,
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(SeventyColor)
        };

        public static SymbolStyle ToEightyMphStyle = new SymbolStyle
        {
            SymbolScale = SymbolScale,
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(EightyColor)
        };

        public static SymbolStyle OverEightyMphStyle = new SymbolStyle
        {
            SymbolScale = SymbolScale,
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(OverEightyColor)
        };

        public static SymbolStyle[] PointStyles = { ToTenMphStyle, ToTwentyMphStyle, ToThirtyMphStyle, ToFourtyMphStyle,
            ToFiftyMphStyle, ToSixtyMphStyle, ToSeventyMphStyle, ToEightyMphStyle, OverEightyMphStyle };

        public static VectorStyle ToTenMphVectorStyle = new VectorStyle
        {
            Fill = null,
            Outline = null,
            Line = new Pen(TenColor, PenWidth)
        };

        public static VectorStyle ToTwentyMphVectorStyle = new VectorStyle
        {
            Fill = null,
            Outline = null,
            Line = new Pen(TwentyColor, PenWidth)
        };

        public static VectorStyle ToThirtyMphVectorStyle = new VectorStyle
        {
            Fill = null,
            Outline = null,
            Line = new Pen(ThirtyColor, PenWidth)
        };

        public static VectorStyle ToFourtyMphVectorStyle = new VectorStyle
        {
            Fill = null,
            Outline = null,
            Line = new Pen(FourtyColor, PenWidth)
        };

        public static VectorStyle ToFiftyMphVectorStyle = new VectorStyle
        {
            Fill = null,
            Outline = null,
            Line = new Pen(FiftyColor, PenWidth)
        };

        public static VectorStyle ToSixtyMphVectorStyle = new VectorStyle
        {
            Fill = null,
            Outline = null,
            Line = new Pen(SixtyColor, PenWidth)
        };

        public static VectorStyle ToSeventyMphVectorStyle = new VectorStyle
        {
            Fill = null,
            Outline = null,
            Line = new Pen(SeventyColor, PenWidth)
        };

        public static VectorStyle ToEightyMphVectorStyle = new VectorStyle
        {
            Fill = null,
            Outline = null,
            Line = new Pen(EightyColor, PenWidth)
        };

        public static VectorStyle OverEightyMphVectorStyle = new VectorStyle
        {
            Fill = null,
            Outline = null,
            Line = new Pen(OverEightyColor, PenWidth)
        };

        public static VectorStyle[] LineStyles = { ToTenMphVectorStyle, ToTwentyMphVectorStyle, ToThirtyMphVectorStyle, ToFourtyMphVectorStyle,
            ToFiftyMphVectorStyle, ToSixtyMphVectorStyle, ToSeventyMphVectorStyle, ToEightyMphVectorStyle, OverEightyMphVectorStyle };

        public static int GetStyleIndexFromSpeed(double speedMph)
        {
            if (speedMph < 10)
                return 0;
            if (speedMph < 20)
                return 1;
            if (speedMph < 33)
                return 2;
            if (speedMph < 43)
                return 3;
            if (speedMph < 53)
                return 4;
            if (speedMph < 63)
                return 5;
            if (speedMph < 73)
                return 6;
            if (speedMph < 83)
                return 7;
            return 8;
        }
    }
}
