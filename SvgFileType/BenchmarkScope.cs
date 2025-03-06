// Copyright 2025 Osman Tunçelli. Allrights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace SvgFileTypePlugin;

internal struct BenchmarkScope : IDisposable
{
    private readonly Stopwatch timer;
    private readonly string? name;
    private int disposed;

    public BenchmarkScope(string? name) : this()
    {
        this.name = name;
    }

    public BenchmarkScope()
    {
        timer = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref disposed, 1, 0) == 0)
        {
            StringBuilder sb = new StringBuilder(nameof(BenchmarkScope));
            sb.Append(' ');
            if (name is not null)
            {
                sb.Append($"({name}) ");
            }
            sb.Append($"took {timer.ElapsedMilliseconds} ms.");
            Logger.WriteLine(sb.ToString());
        }
    }
}
