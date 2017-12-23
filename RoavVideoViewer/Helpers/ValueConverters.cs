﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#pragma warning disable SA1649 // File name must match first type name
namespace RoavVideoViewer.Helpers
{   
    /// <summary>
    /// Converts between TimeSpans and double-precision Seconds time measures
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class TimeSpanToSecondsConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is TimeSpan) return ((TimeSpan)value).TotalSeconds;
            if (value is Duration)
                return ((Duration)value).HasTimeSpan ? ((Duration)value).TimeSpan.TotalSeconds : 0d;

            return 0d;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var result = TimeSpan.FromTicks((long)Math.Round(TimeSpan.TicksPerSecond * (double)value, 0));

            // Do the conversion from visibility to bool
            if (targetType == typeof(TimeSpan)) return result;
            if (targetType == typeof(Duration)) return new Duration(result);

            return Activator.CreateInstance(targetType);
        }
    }

    /// <summary>
    /// Formsts timespan time measures as string with 3-decimal milliseconds
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class TimeSpanFormatter : IValueConverter
    {
        /// <summary>
        /// Converts the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The object converted</returns>
        public object Convert(object position, Type targetType, object duration, CultureInfo culture)
        {
            if (duration != null)
            {
                var media = duration as MediaElement;
                duration = media?.NaturalDuration ?? TimeSpan.Zero;
            }

            var p = TimeSpan.Zero;
            var d = TimeSpan.Zero;

            if (position is TimeSpan span) p = span;
            if (position is Duration duration1)
                p = duration1.HasTimeSpan ? duration1.TimeSpan : TimeSpan.Zero;

            if (duration != null)
            {
                if (duration is TimeSpan) d = (TimeSpan)duration;
                if (duration is Duration)
                    d = ((Duration)duration).HasTimeSpan ? ((Duration)duration).TimeSpan : TimeSpan.Zero;

                if (d == TimeSpan.Zero) return string.Empty;
                p = TimeSpan.FromTicks(d.Ticks - p.Ticks);
            }

            return $"{(int)p.TotalHours:00}:{p.Minutes:00}:{p.Seconds:00}.{p.Milliseconds:000}";
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="NotImplementedException">Expected error</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Formats a fractional value as a percentage string.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class PercentageFormatter : IValueConverter
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="format">The format.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converted value</returns>
        public object Convert(object value, Type targetType, object format, CultureInfo culture)
        {
            var percentage = 0d;
            if (value is double d) percentage = d;

            percentage = Math.Round(percentage * 100d, 0);

            if (format == null || percentage == 0d)
                return $"{percentage,3:0}%";

            return $"{((percentage > 0d) ? "R " : "L ")} {Math.Abs(percentage),3:0}%";
        }

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
#pragma warning restore SA1649 // File name must match first type name