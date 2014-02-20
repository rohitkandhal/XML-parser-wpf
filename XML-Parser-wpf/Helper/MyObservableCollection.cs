using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Test1.Helper
{
    /// <summary>
    /// Custom observable collection which allow disabling update notification.
    /// Use this collection when you are adding large number of items in a collection at a time.
    /// E.g. In this application, initially we add 10,000 items to a collection. Instead of sending
    /// update notification on each update, send update when all items have been added
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyObservableCollection<T> : ObservableCollection<T>
    {
        #region Fields

        private readonly Queue<NotifyCollectionChangedEventArgs> collectionChangeQueue;

        #endregion

        #region Properties

        private bool isUpdatePaused;

        /// <summary>
        /// Flag to send update notification. If true, update notification not sent to framework. 
        /// </summary>
        public bool IsUpdatePaused
        {
            get
            {
                return isUpdatePaused;
            }
            set
            {
                isUpdatePaused = value;

                // On resetting flag, send all queued change notification
                if (!value)
                {
                    while (collectionChangeQueue.Count > 0)
                    {
                        OnCollectionChanged(collectionChangeQueue.Dequeue());
                    }
                }
            }
        }
        #endregion

        #region Constructor

        public MyObservableCollection()
            : base()
        {
            this.collectionChangeQueue = new Queue<NotifyCollectionChangedEventArgs>();
        }

        #endregion

        #region IOvervableCollection Methods

        /// <summary>
        /// Overridden OnCollectionChanged behavior to allow disabling collection change event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!IsUpdatePaused)
            {
                base.OnCollectionChanged(e);
            }
            else
            {
                collectionChangeQueue.Enqueue(e);
            }
        }

        #endregion
    }
}
