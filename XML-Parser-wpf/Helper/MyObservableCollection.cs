using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Test1.Helper
{
    public class MyObservableCollection<T> : ObservableCollection<T>
    {
        private readonly Queue<NotifyCollectionChangedEventArgs> collectionChangeQueue
                = new Queue<NotifyCollectionChangedEventArgs>();

        private bool isUpdatePaused = false;
        public bool IsUpdatePaused
        {
            get
            {
                return isUpdatePaused;
            }
            set
            {
                isUpdatePaused = value;
                if (!value)
                {
                    while (collectionChangeQueue.Count > 0)
                    {
                        OnCollectionChanged(collectionChangeQueue.Dequeue());
                    }
                }
            }
        }

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
    }
}
