using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace XFExpandableListViewSample.Models
{
    public class NotifyingObject : BindableObject
    {
        private readonly Dictionary<string, object> _properties;

        public NotifyingObject()
        {
            _properties = new Dictionary<string, object>();
        }

        #region [Notify Property Changed]

        protected virtual bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
