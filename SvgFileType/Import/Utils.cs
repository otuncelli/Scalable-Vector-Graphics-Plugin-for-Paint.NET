// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Runtime;
#if !DONTCHECKUPDATES
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
#endif

namespace SvgFileTypePlugin.Import;

internal static class Utils
{
#if !DONTCHECKUPDATES
    private static readonly Lazy<Task<Version>> GetLatestVersionAsyncLazy = new(async () =>
    {
        Uri uri = new Uri("https://api.github.com/repos/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases/latest");
        string s = null;
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd(".");
            client.Timeout = TimeSpan.FromSeconds(5);
            using Stream stream = await client.GetStreamAsync(uri).ConfigureAwait(false);
            using JsonDocument json = await JsonDocument.ParseAsync(stream, new JsonDocumentOptions()).ConfigureAwait(false);
            if (json.RootElement.TryGetProperty("tag_name", out JsonElement element))
            {
                s = element.GetString()?.TrimStart('v');
            }
        }
        return Version.Parse(s);
    });

    public static Task<Version> GetLatestVersionAsync() => GetLatestVersionAsyncLazy.Value;
#endif

    public static IDisposable UseMemoryFailPoint(int width, int height, int count)
    {
        return UseMemoryFailPoint(height * 4L * width * count);
    }

    public static IDisposable UseMemoryFailPoint(int width, int height)
    {
        return UseMemoryFailPoint(height * 4L * width);
    }

    public static void EnsureMemoryAvailable(int width, int height, int count)
    {
        using (UseMemoryFailPoint(width, height, count))
        {
        }
    }

    public static IDisposable UseMemoryFailPoint(long bytes)
    {
        int mb = checked((int)Math.Max(bytes / (1024 * 1024), 1));
        return new MemoryFailPoint(mb);
    }
}
