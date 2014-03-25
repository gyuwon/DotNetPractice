using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;

namespace ContactManager.ViewModels
{
    public class ViewModel : ViewModelBase
    {
        protected void Set<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            this.Set(propertyName, ref field, newValue);
        }
    }
}
