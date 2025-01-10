// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;

namespace SvgFileTypePlugin.Import;

internal static class Disposable
{
    public static IDisposable FromAction(Action action)
    {
        return new DisposableAction(action);
    }

    /// <summary>    
    /// The disposable action.    
    /// </summary>    
    private sealed class DisposableAction : IDisposable
    {
        private Action? _dispose;

        /// <summary>    
        /// Initializes a new instance of the <see cref="DisposableAction"/> class.    
        /// </summary>    
        /// <param name="dispose">    
        /// The dispose.    
        /// </param>    
        public DisposableAction(Action dispose)
        {
            _dispose = dispose ?? throw new ArgumentNullException(nameof(dispose));
        }

        /// <summary>    
        /// Initializes a new instance of the <see cref="DisposableAction"/> class.    
        /// </summary>    
        /// <param name="construct">    
        /// The construct.    
        /// </param>    
        /// <param name="dispose">    
        /// The dispose.    
        /// </param>    
        public DisposableAction(Action construct, Action dispose)
        {
            ArgumentNullException.ThrowIfNull(construct);
            ArgumentNullException.ThrowIfNull(dispose);

            construct();

            _dispose = dispose;
        }

        /// <summary>    
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.    
        /// </summary>    
        /// <filterpriority>2</filterpriority>    
        public void Dispose()
        {
            try
            {
                _dispose?.Invoke();
            }
            finally
            {
                _dispose = null;
            }
        }
    }
}
