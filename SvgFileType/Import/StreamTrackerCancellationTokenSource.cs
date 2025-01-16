// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.IO;
using System.Threading;

namespace SvgFileTypePlugin.Import;

/// <summary>
/// Paint.NET input stream can throw <see cref="OperationCanceledException"></see> internally.
/// This class helps us to track these cancellation events. Provides us ability to gracefully end 
/// the ongoing operation.
/// <br />
/// <b>! Note: Must be used after reading the whole stream.</b>
/// </summary>
internal sealed class StreamTrackerCancellationTokenSource : CancellationTokenSource
{
    private bool disposed;
    private readonly Timer? timer;
    private readonly Func<bool>? action;

    public StreamTrackerCancellationTokenSource(Stream stream, int period = 1000)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentOutOfRangeException.ThrowIfNegative(period);

        if (!stream.CanSeek)
            return;

        action = () => IsStreamCanceled(stream);
        timer = new Timer(OnTimer, null, 0, period);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposed)
        {
            disposed = true;
            timer?.Dispose();
        }
    }

    private void OnTimer(object? state)
    {
        try
        {
            if (action!.Invoke())
            {
                Cancel();
                timer!.Dispose();
            }
        }
        catch (ObjectDisposedException)
        {
            // ignore
        }
    }

    private static bool IsStreamCanceled(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        try
        {
            stream.Position = 0;
            return false;
        }
        catch (ObjectDisposedException)
        {
            Logger.WriteLine("Completed!");
            return true;
        }
        catch (OperationCanceledException)
        {
            Logger.WriteLine("Canceled!");
            return true;
        }
    }
}
