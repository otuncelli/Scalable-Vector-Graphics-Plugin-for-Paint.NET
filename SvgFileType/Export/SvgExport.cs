// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BitmapVectorizer;
using PaintDotNet;
using PaintDotNet.AppModel;
using PaintDotNet.PropertySystem;
using PaintDotNet.PropertySystem.Extensions;
using SvgFileTypePlugin.Extensions;
using SvgFileTypePlugin.Localization;
using static PaintDotNet.UserBlendOps;

namespace SvgFileTypePlugin.Export;

internal static partial class SvgExport
{
    private static Lazy<string?> ShapesDirectory => new(() =>
    {
        try
        {
            IFileSystemService fss = Services.Get<IFileSystemService>();
            string path = Path.Combine(fss.PerUserAppFilesPath, "Shapes");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        catch
        {
            return null;
        }
    });

    public static string? ShowSaveShapeDialog()
    {
        if (!UIHelper.IsSaveConfigDialogVisible())
        {
            return null;
        }

        return UIHelper.RunOnUIThread(() =>
        {
            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = ShapesDirectory.Value;
            sfd.Filter = $"{StringResources.Shape}|*.xaml";
            sfd.OverwritePrompt = true;
            return sfd.ShowDialog() == DialogResult.OK ? sfd.FileName : null;
        });
    }

    private static T ThrowIfOverflow<T>(Func<T> action, string message)
    {
        try
        {
            return action();
        }
        catch (OverflowException ex)
        {
            throw new WarningException(message, ex);
        }
    }

    public static void Export(Surface input, Stream output, PropertyCollection props, ProgressEventHandler progressCallback, string shapePath)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(props);
        ArgumentNullException.ThrowIfNull(progressCallback);
        ThrowIfOverflow(() => checked(input.Stride * input.Height), StringResources.CanvasIsTooBig);

#pragma warning disable format // @formatter:off
        ScanMode scanMode                   = props.GetPropertyValue<ScanMode>(PropertyNames.ScanMode);
        PreviewMode previewMode             = props.GetPropertyValue<PreviewMode>(PropertyNames.PreviewMode);
        double brightnessCutoff             = props.GetPropertyValue<double>(PropertyNames.BrightnessCutoff);
        int highpass                        = props.GetPropertyValue<int>(PropertyNames.HighpassFilter);
        int gmScale                         = props.GetPropertyValue<int>(PropertyNames.GreymapScale);
        int lowpass                         = props.GetPropertyValue<int>(PropertyNames.LowpassFilter);
        int turdsize                        = props.GetPropertyValue<int>(PropertyNames.SuppressSpeckles);
        float alphamax                      = props.GetPropertyValue<float>(PropertyNames.SmoothCorners);
        float opttolerance                  = props.GetPropertyValue<float>(PropertyNames.Optimize);
        TurnPolicy turnpolicy               = props.GetPropertyValue<TurnPolicy>(PropertyNames.TurnPolicy);
        int color                           = props.GetPropertyValue<int>(PropertyNames.Color);
        int fillcolor                       = props.GetPropertyValue<int>(PropertyNames.FillColor);
        bool invert                         = props.GetPropertyValue<bool>(PropertyNames.Invert);
        bool tight                          = props.GetPropertyValue<bool>(PropertyNames.Tight);
        bool enclose                        = props.GetPropertyValue<bool>(PropertyNames.Enclose);
        float angle                         = props.GetPropertyValue<float>(PropertyNames.Angle);
        float scale                         = props.GetPropertyValue<float>(PropertyNames.Scale);
        string shapeName                    = props.GetPropertyValue<string>(PropertyNames.PdnShapeName);
#pragma warning restore format // @formatter:on

        PotraceBitmap bm;
        TraceResult? trace;
        ImageInfo imginfo;
        SvgBackEnd backend = new SvgBackEnd
        {
            Color = color,
            ColumnWidth = int.MaxValue
        };
        backend.Rx /= scale;
        backend.Ry /= scale;

        bool isSaveConfigDialogVisible = UIHelper.IsSaveConfigDialogVisible();
        Surface surface;
        const int maxDimForPreview = 1280;

        if (isSaveConfigDialogVisible && previewMode == PreviewMode.Fast && input.Width * input.Height > maxDimForPreview * maxDimForPreview)
        {
            // Preview Only
            // Image downscaled to speed up preview generation.
            double ar = input.Width / (double)input.Height;
            int newWidth = maxDimForPreview;
            int newHeight = maxDimForPreview;
            if (input.Width > input.Height)
            {
                newHeight = (int)Math.Round(maxDimForPreview / ar);
            }
            else
            {
                newWidth = (int)Math.Round(maxDimForPreview * ar);
            }

            surface = new Surface(newWidth, newHeight);
            surface.FitSurface(ResamplingAlgorithm.Cubic, input);
        }
        else
        {
            surface = input.Clone();
        }

        using var _ = surface;
        surface.BlendOnto<NormalBlendOp>(ColorBgra.White);

        using CancellationTokenSource cts = new CancellationTokenSource();
        float full = isSaveConfigDialogVisible ? 90f : 100f;

        CancellationToken cancellationToken = cts.Token;
        if (scanMode == ScanMode.Opaque)
        {
            backend.Opaque = true;
            backend.FillColor = fillcolor;
        }

        if (highpass == 0)
        {
            bm = PotraceBitmap.FromRgbx(surface.Scan0.Pointer, surface.Width, surface.Height, c: brightnessCutoff);
        }
        else
        {
            using Greymap gm = Greymap.FromRgbx(surface.Scan0.Pointer, surface.Width, surface.Height);
            gm.HighPassFilter(lambda: highpass);

            if (lowpass > 0)
            {
                gm.LowPassFilter(lambda: lowpass);
            }

            if (gmScale > 1)
            {
                bm = gm.InterpolateCubicBilevel(gmScale, c: brightnessCutoff);
                backend.Rx *= gmScale;
                backend.Ry *= gmScale;
            }
            else
            {
                bm = gm.Threshold(c: brightnessCutoff);
            }
        }

        using (bm)
        {
            if (invert)
            {
                bm.Invert();
            }

            if (enclose)
            {
                bm.Enclose();
            }

            Progress<ProgressArgs> progress = new Progress<ProgressArgs>((prog) => OnProgress(ConvertProgressValue(prog)));
            trace = bm.Trace(turdsize, turnpolicy, alphamax, opttolerance, progress, cancellationToken);
            imginfo = bm.Info;
            imginfo.Tight = tight;
        }
        imginfo.Angle = angle;
        if (trace is null)
        {
            if (UIHelper.IsSaveConfigDialogVisible())
            {
                return;
            }
            else
            {
                throw new InvalidOperationException(StringResources.NoPath);
            }
        }

        if (isSaveConfigDialogVisible && scanMode == ScanMode.Transparent && shapePath.Length > 0)
        {
            PdnShapeBackEnd pdnbackend = new PdnShapeBackEnd
            {
                ColumnWidth = int.MaxValue,
                DisplayName = shapeName
            };
            using (FileStream shapeStream = File.Open(shapePath, FileMode.Create, FileAccess.Write))
            {
                pdnbackend.Save(shapeStream, trace, imginfo, cancellationToken);
            }

            StringBuilder msg = new StringBuilder();
            msg.AppendFormat(StringResources.ShapeSaved, shapePath);

            if (ShapesDirectory.Value != null && Path.GetDirectoryName(shapePath)?.StartsWith(ShapesDirectory.Value, StringComparison.OrdinalIgnoreCase) == true)
            {
                msg.AppendLine();
                msg.AppendLine();
                msg.AppendLine(StringResources.ShapeSavedRestart);
            }

            UIHelper.RunOnUIThread(() =>
            {
                MessageBox.Show(msg.ToString(), StringResources.ShapeSavedCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }
        backend.Save(output, trace, imginfo, cancellationToken);

        void OnProgress(float prog)
        {
            try
            {
                progressCallback.Invoke(null, new ProgressEventArgs(prog * full));
            }
            catch (OperationCanceledException)
            {
                try
                {
                    cts.Cancel();
                }
                catch (ObjectDisposedException)
                {
                    // ignore
                }
            }
        }
    }

    public static void Export(Document input, Stream output, PropertyBasedSaveConfigToken token, Surface scratchSurface, ProgressEventHandler progressCallback, string shapePath)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(token);
        ArgumentNullException.ThrowIfNull(scratchSurface);
        ArgumentNullException.ThrowIfNull(progressCallback);
        ThrowIfOverflow(() => checked(scratchSurface.Stride * scratchSurface.Height), StringResources.CanvasIsTooBig);

        input.Flatten(scratchSurface);
        Export(scratchSurface, output, token.Properties, progressCallback, shapePath);
    }

    private static float ConvertProgressValue(ProgressArgs args)
    {
        float progress = args.Progress * .5f;
        if (args.Level == ProgressLevel.Tracing)
        {
            progress += .5f;
        }
        return progress;
    }
}
