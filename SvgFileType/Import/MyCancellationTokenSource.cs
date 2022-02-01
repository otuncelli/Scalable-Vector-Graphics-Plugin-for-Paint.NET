// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.IO;
using System.Threading;

namespace SvgFileTypePlugin.Import;

internal sealed class MyCancellationTokenSource : CancellationTokenSource
{
    private bool disposed;
    private bool paused;
    private readonly Timer timer;
    private readonly int period;
    private Func<bool> cb;
    private readonly object sync = new();

    public MyCancellationTokenSource(Func<bool> cb, int period = 1000)
    {
        Ensure.IsNotNull(cb, nameof(cb));
        Ensure.IsGreaterThan(period, 0, nameof(period));

        this.period = period;
        this.cb = cb;
        timer = new Timer(OnTimer, null, 0, period);
    }

    public MyCancellationTokenSource(Stream stream, int period = 1000) 
        : this(() => IsStreamCanceled(stream), period)
    {
    }

    public void Pause()
    {
        if(paused) { return; }
        lock (sync)
        {
            timer.Change(0, Timeout.Infinite);
            paused = true;
        }
    }

    public void Resume()
    {
        if (!paused) { return; }
        lock (sync)
        {
            timer.Change(0, period);
            paused = false;
        }
    }

    public void SetCallback(Func<bool> callback)
    {
        lock (sync)
        {
            cb = callback;
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposed)
        {
            disposed = true;
            lock (sync)
            {
                timer.Dispose();
            }
        }
    }

    private void OnTimer(object state)
    {
        if (Monitor.TryEnter(sync, 0))
        {
            try
            {
                if (paused) { return; }
                if (cb.Invoke())
                {
                    Cancel();
                    timer.Dispose();
                }
            }
            catch (ObjectDisposedException)
            {
                // ignore
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }
    }

    private static bool IsStreamCanceled(Stream stream)
    {
        if (!stream.CanSeek) { return false; }
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
