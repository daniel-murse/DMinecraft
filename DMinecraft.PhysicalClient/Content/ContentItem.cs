using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Content
{
    internal abstract class ContentItem<T>
    {
        public abstract int ReferenceCount { get; }

        /// <summary>
        /// Increments the reference counter for this item.
        /// </summary>
        public abstract void AcquireReference();

        /// <summary>
        /// Decrements the reference counter for this object.
        /// </summary>
        public abstract void ReleaseReference();

        /// <summary>
        /// Immediately disposes of the item and removes it from the content manager.
        /// </summary>
        public abstract void DisposeImmediately();

        /// <summary>
        /// Transfers this item to a new content manager.
        /// </summary>
        /// <param name="manager"></param>
        public abstract void Transfer(ContentManager<T> manager);

        /// <summary>
        /// The item loaded.
        /// </summary>
        public abstract T Item { get; }

        /// <summary>
        /// The key identifying the item.
        /// </summary>
        public abstract string Key { get; }

        /// <summary>
        /// The manager of this item.
        /// </summary>
        public abstract ContentManager<T>? Manager { get; }
    }
}
