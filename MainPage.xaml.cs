
using ProgressApp.ViewModel;

namespace ProgressApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

        }
    }
}
