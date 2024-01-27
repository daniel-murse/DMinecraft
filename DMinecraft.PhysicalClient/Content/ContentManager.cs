using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Content
{
    /// <summary>
    /// Manages generic items, tracking their lifetime. This mechanism obviously complicates simple cases
    /// where the items used are constant, but in cases where which items are loaded changes based
    /// on multiple parameters determined at runtime, the mechanisms provided by this class can shorten
    /// loading times.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ContentManager<T> : IDisposable
    {
        private Dictionary<string, ContentItem<T>> contentItems;
        private bool disposedValue;

        public ContentManager() 
        { 
            contentItems = new Dictionary<string, ContentItem<T>>();
        }

        /// <summary>
        /// Unloads content items loaded with no strong references to it.
        /// </summary>
        public void UnloadWeak()
        {
            Unload(1);
        }

        public void Unload(int minReferenceCount)
        {
            var newContentItems = new Dictionary<string, ContentItem<T>>(contentItems.Count);

            foreach (var item in contentItems.Values)
            {
                if (item.ReferenceCount < minReferenceCount)
                {
                    if(item.Item is IDisposable disposable)
                        disposable.Dispose();
                }
                else
                {
                    newContentItems[item.Key] = item;
                }
            }

            contentItems = newContentItems;
        }

        /// <summary>
        /// Unloads a specific item.
        /// </summary>
        /// <param name="contentItem"></param>
        internal void Unload(ContentItem<T> contentItem)
        {
            contentItems.Remove(contentItem.Key);
            if (contentItem.Item is IDisposable disposable)
                disposable.Dispose();
            ((ContentItemImpl)contentItem).manager = null;
        }

        /// <summary>
        /// Adds an item with a key, creating a content item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public ContentItem<T> AddItem(string key, T item)
        {
            var contentItem = new ContentItemImpl(this, item);
            contentItems.Add(key, contentItem);
            return contentItem;
        }

        /// <summary>
        /// Transfers all items to a new content manager.
        /// </summary>
        /// <param name="manager"></param>
        public void Transfer(ContentManager<T> manager)
        {
            foreach (var item in contentItems.Values)
            {
                manager.contentItems.Add(item.Key, item);
                ((ContentItemImpl)item).manager = manager;
            }
            contentItems.Clear();
        }

        public ContentItem<T> GetItem(string key)
        {
            return contentItems[key];
        }

        private class ContentItemImpl : ContentItem<T>
        {
            private int referenceCount;
            private T item;
            private string key;
            public ContentManager<T>? manager;

            public ContentItemImpl(ContentManager<T> contentManager, T item)
            {
                this.manager = contentManager;
                this.item = item;
            }

            public override int ReferenceCount => referenceCount;

            public override T Item => item;

            public override string Key => key;

            public override ContentManager<T>? Manager => manager;

            public override void AcquireReference()
            {
                referenceCount++;
            }

            public override void DisposeImmediately()
            {
                ArgumentNullException.ThrowIfNull(Manager);
                referenceCount = 0;
                Manager.contentItems.Remove(Key);
                manager = null;
                if (Item is IDisposable disposable)
                    disposable.Dispose();
            }

            public override void ReleaseReference()
            {
                referenceCount--;
            }

            public override void Transfer(ContentManager<T> manager)
            {
                manager.contentItems.Remove(Key);
                manager.contentItems.Add(Key, this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                foreach (var item in contentItems.Values)
                {
                    if (item.Item is IDisposable disposable)
                        disposable.Dispose();
                }
                contentItems.Clear();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ContentManager()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
