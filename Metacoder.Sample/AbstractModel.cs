using System.ComponentModel;

namespace Metacoder.Sample
{
    public abstract class AbstractModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}