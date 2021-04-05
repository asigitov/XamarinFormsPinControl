using Xamarin.Forms;

namespace FormsPinViewSample.Core.Views
{
    public partial class PinViewSetMode : ContentPage
    {
        public PinViewSetMode()
        {
            InitializeComponent();
        }

        void PinView_SetModeConfirmationSuccessHandler(System.Object sender, FormsPinView.Core.PinChangedEventArgs e)
        {
            SetModeText.Text = "Pin was successfully set";
        }

        void PinView_SetModeConfirmationFailureHandler(System.Object sender, System.EventArgs e)
        {
            SetModeText.Text = "Ups, something went wrong. Please try again";
        }

        void PinView_SetModeDefineSuccessHandler(System.Object sender, System.EventArgs e)
        {
            SetModeText.Text = "Please enter the Pin again to confirm";
        }
    }
}
