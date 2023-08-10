// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.IO;
using System.Threading;
using BitmapVectorizer;
using PaintDotNet;
using PaintDotNet.IndirectUI;
using PaintDotNet.IndirectUI.Extensions;
using PaintDotNet.PropertySystem;
using SvgFileTypePlugin.Export;
using SvgFileTypePlugin.Import;
using SvgFileTypePlugin.Localization;
using SR = SvgFileTypePlugin.Localization.StringResources;

namespace SvgFileTypePlugin;

[PluginSupportInfo(typeof(MyPluginSupportInfo))]
public sealed class SvgFileType : PropertyBasedFileType
{
    private string shapePath;

    #region Constructor

    public SvgFileType() : base(BaseName, BaseOptions)
    {
    }

    #endregion

    #region OnLoad

    protected override Document OnLoad(Stream input) => SvgImport.Load(input);

    #endregion

    #region OnSave

    protected override void OnSaveT(Document input, Stream output, PropertyBasedSaveConfigToken token, Surface scratchSurface, ProgressEventHandler progressCallback)
        => SvgExport.Export(input, output, token, scratchSurface, progressCallback, Interlocked.Exchange(ref shapePath, null));

    private void PdnShape_Click(object sender, ValueEventArgs<object> args)
    {
        if ((int)args.Value != int.MinValue)
        {
            Interlocked.Exchange(ref shapePath, SvgExport.ShowSaveShapeDialog());
        }
    }

    public override ControlInfo OnCreateSaveConfigUI(PropertyCollection props)
    {
        Ensure.IsNotNull(props, nameof(props));

        PropertyControlInfo CommonSettingsF(PropertyControlInfo p)
            => p.SliderSmallChange(.01).SliderLargeChange(.01).UpDownIncrement(.01).DecimalPlaces(2).ShowResetButton();

        PropertyControlInfo CommonSettings(PropertyControlInfo p)
            => p.SliderSmallChange(1).SliderLargeChange(1).UpDownIncrement(1).ShowResetButton();

        PropertyControlInfoCollection pcic = new PropertyControlInfoCollection(props);
        pcic.Configure(PropertyNames.ScanMode, SR.ScanMode, p => p
                 .ValueDisplayNameCallback<ScanMode>(Localize.GetDisplayName))
            .Configure(PropertyNames.PreviewMode, SR.PreviewMode, p => p
                .ValueDisplayNameCallback<PreviewMode>(Localize.GetDisplayName))
            .Configure(PropertyNames.PdnShapeName, SR.PdnShapeExportOptions, SR.PdnShapeExportOptionsDesc)
            .Configure(PropertyNames.PdnShape, String.Empty, p => p
                .ControlType(PropertyControlType.IncrementButton)
                .ButtonText(SR.ExportAsPdnShape)
                .OnValueChanged(PdnShape_Click))
            .Configure(PropertyNames.BrightnessCutoff, SR.BrightnessCutoff, SR.BrightnessCutoffDesc, CommonSettingsF)
            .Configure(PropertyNames.HighpassFilter, SR.HighpassFilterRadius, SR.HighpassFilterRadiusDesc, CommonSettings)
            .Configure(PropertyNames.GreymapScale, SR.ScaleBeforeThreshold, CommonSettings)
            .Configure(PropertyNames.LowpassFilter, SR.LowpassFilterRadius, SR.LowpassFilterRadiusDesc, CommonSettings)
            .Configure(PropertyNames.SuppressSpeckles, SR.SuppressSpeckles, SR.SuppressSpecklesDesc, CommonSettings)
            .Configure(PropertyNames.SmoothCorners, SR.SmoothCorners, SR.SmoothCornersDesc, CommonSettingsF)
            .Configure(PropertyNames.Optimize, SR.Optimize, SR.OptimizeDesc, p => p
                .ShowResetButton())
            .Configure(PropertyNames.TurnPolicy, SR.TurnPolicy, SR.TurnPolicyDesc, p => p
                .ValueDisplayNameCallback<TurnPolicy>(Localize.GetDisplayName))
            .Configure(PropertyNames.Color, SR.Color, p => p
                .ControlType(PropertyControlType.ColorWheel)
                .ShowResetButton())
            .Configure(PropertyNames.FillColor, SR.FillColor, p => p
                .ControlType(PropertyControlType.ColorWheel)
                .ShowResetButton())
            .Configure(PropertyNames.Invert, String.Empty, SR.Invert)
            .Configure(PropertyNames.Tight, String.Empty, SR.Tight)
            .Configure(PropertyNames.Enclose, String.Empty, SR.Enclose)
            .Configure(PropertyNames.Scale, SR.Scale, CommonSettingsF)
            .Configure(PropertyNames.Angle, SR.Angle, p => p
                .ControlType(PropertyControlType.AngleChooser)
                .ShowResetButton())
            .Configure(PropertyNames.GitHubLink, String.Format(SR.PluginVersion, MyPluginSupportInfo.Instance.Version), SR.GitHubLink)
            .Configure(PropertyNames.DiscussionLink, String.Empty, SR.DiscussionLink);
        PanelControlInfo panel = pcic.CreatePanel();
        return panel;
    }

    public override PropertyCollection OnCreateSavePropertyCollection()
    {
        PropertyName[] targets1 = new PropertyName[] 
        {
            PropertyNames.PdnShape, 
            PropertyNames.PdnShapeName 
        };

        PropertyName[] targets2 = new PropertyName[]
        {
            PropertyNames.LowpassFilter,
            PropertyNames.GreymapScale
        };

        FluentPropertyCollection properties = new FluentPropertyCollection()
            .AddStaticListChoice(PropertyNames.ScanMode, ScanMode.Transparent)
            .AddStaticListChoice(PropertyNames.PreviewMode, PreviewMode.Fast)
            .AddString(PropertyNames.PdnShapeName, SR.Untitled)
            .AddInt32(PropertyNames.PdnShape, int.MinValue)
            .AddDouble(PropertyNames.BrightnessCutoff, 0.45, 0.01, 1)
            .AddInt32(PropertyNames.HighpassFilter, 0, 0, 10)
            .AddInt32(PropertyNames.GreymapScale, 1, 1, 4)
            .AddInt32(PropertyNames.LowpassFilter, 0, 0, 5)
            .AddInt32(PropertyNames.SuppressSpeckles, PotraceBitmap.TurdSizeDef, PotraceBitmap.TurdSizeMin, PotraceBitmap.TurdSizeMax)
            .AddDouble(PropertyNames.SmoothCorners, Potrace.AlphaMaxDef, Potrace.AlphaMaxMin, Potrace.AlphaMaxMax)
            .AddDouble(PropertyNames.Optimize, Potrace.OptToleranceDef, Potrace.OptToleranceMin, Potrace.OptToleranceMax)
            .AddStaticListChoice(PropertyNames.TurnPolicy, TurnPolicy.Minority)
            .AddInt32(PropertyNames.Color, ColorBgra.Black)
            .AddInt32(PropertyNames.FillColor, ColorBgra.White)
            .AddBoolean(PropertyNames.Invert)
            .AddBoolean(PropertyNames.Tight)
            .AddBoolean(PropertyNames.Enclose)
            .AddDouble(PropertyNames.Scale, 1, 0.01, 4)
            .AddDouble(PropertyNames.Angle, 0, 0, 360)
            .AddUri(PropertyNames.GitHubLink, MyPluginSupportInfo.Instance.WebsiteUri)
            .AddUri(PropertyNames.DiscussionLink, MyPluginSupportInfo.Instance.ForumUri)
            .WithReadOnlyRule(targets1, PropertyNames.ScanMode, ScanMode.Transparent, inverse: true)
            .WithReadOnlyRule(targets2, PropertyNames.HighpassFilter, 0)
            .WithReadOnlyRule(PropertyNames.FillColor, PropertyNames.ScanMode, ScanMode.Opaque, inverse: true);
        return properties;
    }

    protected override bool ShouldSerializeTokenProperty(Property property) => IsSerializable(property);

    #endregion

    #region Static Private

    private static readonly FileTypeOptions BaseOptions = new()
    {
        LoadExtensions = new[] { ".svg", ".svgz" },
        SupportsCancellation = true,
        SupportsLayers = false,
        SaveExtensions = new[] { ".svg" }
    };

    private static readonly string BaseName = $"SVG - Scalable Vector Graphics Plugin v{MyPluginSupportInfo.Instance.Version}";

    private static bool IsSerializable(Property property)
    {
        switch (property.Name)
        {
            case nameof(PropertyNames.GitHubLink):
            case nameof(PropertyNames.DiscussionLink):
            case nameof(PropertyNames.PdnShape):
            case nameof(PropertyNames.PdnShapeName):
            case nameof(PropertyNames.PreviewMode):
                return false;
            default:
                return Enum.IsDefined(typeof(PropertyNames), property.Name);
        }
    }

    #endregion
}
