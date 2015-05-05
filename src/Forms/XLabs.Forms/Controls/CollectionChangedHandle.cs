
namespace XLabs.Forms.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    /// <summary>
    /// Small utility class that takes
    /// gyuwon's idea to it's logical 
    /// conclusion.
    /// The code in the ItemsCollectionChanged methods
    /// rarely changes.  The only real change is projecting 
    /// from source type T to targeted type TSyncType which
    /// is then inserted into the target collection
    /// </summary>
    public class CollectionChangedHandle<TSyncType> : IDisposable where TSyncType : class
    {
        private readonly Func<object, TSyncType> _projector;
        private readonly Action<TSyncType, object, int> _postadd;
        private readonly Action<TSyncType> _cleanup;
        private readonly INotifyCollectionChanged _itemsSourceCollectionChangedImplementation;
        private readonly IEnumerable _sourceCollection;
        private readonly IList<TSyncType> _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedHandle{TSyncType,T}"/> class.
        /// </summary>
        /// <param name="target">The collection to be kept in sync with <see cref="source"/>source</param>
        /// <param name="source">The original collection</param>
        /// <param name="projector">A function that returns {TSyncType} for a {T}</param>
        /// <param name="postadd">A functino called right after insertion into the synced collection</param>
        /// <param name="cleanup">A function that performs any needed cleanup when {TSyncType} is removed from the <see cref="target"/></param>
        public CollectionChangedHandle(IList<TSyncType> target, IEnumerable source, Func<object, TSyncType> projector, Action<TSyncType, object, int> postadd = null, Action<TSyncType> cleanup = null)
        {
            if (source == null) return;
            this._itemsSourceCollectionChangedImplementation = source as INotifyCollectionChanged;
            _sourceCollection = source;
            _target = target;
            _projector = projector;
            _postadd = postadd;
            _cleanup = cleanup;
            this.InitialPopulation();
            if (this._itemsSourceCollectionChangedImplementation == null)  return;
            this._itemsSourceCollectionChangedImplementation.CollectionChanged += this.CollectionChanged;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this._itemsSourceCollectionChangedImplementation == null) return;
            this._itemsSourceCollectionChangedImplementation.CollectionChanged -= this.CollectionChanged;
        }

        /// <summary>Keeps <see cref="_target"/> in sync with <see cref="_sourceCollection"/>.</summary>
        /// <param name="sender">The sender, completely ignored.</param>
        /// <param name="args">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        /// Element created at 15/11/2014,2:57 PM by Charles
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                SafeClearTarget();
            }
            else
            {
                //Create a temp list to prevent multiple enumeration issues

                var tlist = new List<object>();
                foreach (var item in _sourceCollection)
                {
                    tlist.Add(item);
                }

                if (args.OldItems != null)
                {
                    var syncitem = _target[args.OldStartingIndex];
                    if (syncitem != null && _cleanup != null) _cleanup(syncitem);
                    _target.RemoveAt(args.OldStartingIndex);
                }

                if (args.NewItems == null) return; 
                foreach (var obj in args.NewItems)
                {
                    var item = obj;
                    if (item == null) continue;
                    var index = tlist.IndexOf(item);
                    var newsyncitem = this._projector(item);
                    this._target.Insert(index, newsyncitem);
                    if (_postadd != null) _postadd(newsyncitem,item, index);
                }
            }
            
        }

        /// <summary>Initials the population.</summary>
        /// Element created at 15/11/2014,2:53 PM by Charles
        private void InitialPopulation()
        {
            SafeClearTarget();
            foreach (var t in this._sourceCollection)
            {
                if (t == null)
                    continue;
                _target.Add(this._projector(t));
            }
        }

        private void SafeClearTarget()
        {
            while (_target.Count > 0)
            {
                var syncitem = _target[0];
                _target.RemoveAt(0);
                if (_cleanup != null) _cleanup(syncitem);
            }
        }
    }
}
