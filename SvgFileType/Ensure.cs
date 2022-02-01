// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SvgFileTypePlugin;

internal static class Ensure
{
    [return: NotNullIfNotNull(nameof(param))]
    public static T IsNotNull<T>(T param, string paramName) where T : class
        => param ?? throw new ArgumentNullException(paramName);

    public static IntPtr IsNotNull(IntPtr param, string paramName)
        => param != IntPtr.Zero ? param : throw new ArgumentNullException(paramName);

    public static T IsNotNullOrEmpty<T>(T param, string paramName) where T : class, ICollection
    {
        IsNotNull(param, paramName);
        return param.Count == 0 ? throw new ArgumentException("Collection is empty.", paramName) : param;
    }

    public static string IsNotNullOrEmpty(string param, string paramName)
    {
        IsNotNull(param, paramName);
        return param.Length == 0 ? throw new ArgumentException("String is empty.", paramName) : param;
    }

    public static string IsNotNullOrWhiteSpace(string param, string paramName)
    {
        IsNotNull(param, paramName);
        return param.Trim().Length == 0 ? throw new ArgumentException("String is empty.", paramName) : param;
    }

    public static T IsInRange<T>(T param, T min, T max, string paramName) where T : IComparable<T>
        => param.CompareTo(min) >= 0 && param.CompareTo(max) <= 0 ? param : throw new ArgumentOutOfRangeException(paramName, $"Value must be between {min} and {max}.");

    public static T IsGreaterThan<T>(T param, T comparand, string paramName) where T : IComparable<T>
        => param.CompareTo(comparand) > 0 ? param : throw new ArgumentOutOfRangeException(paramName, $"Value must be greater than {comparand}.");

    public static T IsGreaterThanOrEqualTo<T>(T param, T comparand, string paramName) where T : IComparable<T>
        => param.CompareTo(comparand) >= 0 ? param : throw new ArgumentOutOfRangeException(paramName, $"Value must be greater than or equal to {comparand}.");

    public static T IsLessThan<T>(T param, T comparand, string paramName) where T : IComparable<T>
        => param.CompareTo(comparand) < 0 ? param : throw new ArgumentOutOfRangeException(paramName, $"Value must be less than {comparand}.");

    public static T IsLessThanOrEqualTo<T>(T param, T comparand, string paramName) where T : IComparable<T>
        => param.CompareTo(comparand) <= 0 ? param : throw new ArgumentOutOfRangeException(paramName, $"Value must be less than or equal to {comparand}.");

    public static T IsEqualTo<T>(T param, T comparand, string paramName)
        => param.Equals(comparand) ? param : throw new ArgumentException($"Value must be equal to {comparand}.", paramName);

    public static void IsTrue(bool condition, Action throwException)
    {
        if (!condition)
        {
            throwException();
        }
    }

    public static T Test<T>(Func<T> action, string message)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            throw new WarningException(message, ex);
        }
    }
}
