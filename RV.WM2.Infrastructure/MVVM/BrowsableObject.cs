namespace RV.WM2.Infrastructure.MVVM
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class BrowsableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
