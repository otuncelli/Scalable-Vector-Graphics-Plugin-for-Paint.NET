// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using PaintDotNet;
using Svg;

namespace SvgFileTypePlugin.Import;

interface ISvgConverter
{
    string Name { get; }

    Document GetNoPathDocument();

    Document GetFlatDocument(SvgDocument svg, SvgImportConfig config, CancellationToken cancellationToken = default);

    Document GetFlatDocument(Stream stream, SvgImportConfig config, CancellationToken cancellationToken = default);

    Document GetFlatDocument(IReadOnlyCollection<SvgVisualElement> elements, SvgImportConfig config, Action<int> progress = null, CancellationToken cancellationToken = default);

    IEnumerable<SvgVisualElement> Prepare(SvgDocument svg, SvgImportConfig config, CancellationToken cancellationToken = default);

    Document GetLayeredDocument(IReadOnlyCollection<SvgVisualElement> preparedElements, SvgImportConfig config, Action<int> progress = null, CancellationToken cancellationToken = default);
}
