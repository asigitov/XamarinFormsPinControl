using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FormsPinView.Core
{
    public partial class PinView : AbsoluteLayout
    {
        public event EventHandler<EventArgs> AuthenticateModeSuccessHandler;
        public event EventHandler<EventArgs> AuthenticateModeFailureHandler;

        public static readonly BindableProperty AuthenticateModeSuccessCommandProperty =
            BindableProperty.Create(propertyName: nameof(AuthenticateModeSuccessCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        public ICommand AuthenticateModeSuccessCommand
        {
            get { return (ICommand)GetValue(AuthenticateModeSuccessCommandProperty); }
            set { SetValue(AuthenticateModeSuccessCommandProperty, value); }
        }

        public static readonly BindableProperty AuthenticateModeFailureCommandProperty =
            BindableProperty.Create(propertyName: nameof(AuthenticateModeFailureCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        public ICommand AuthenticateModeFailureCommand
        {
            get { return (ICommand)GetValue(AuthenticateModeFailureCommandProperty); }
            set { SetValue(AuthenticateModeFailureCommandProperty, value); }
        }

        public bool ValidateAuthenticateMode(string pin)
        {
            if (string.IsNullOrEmpty(Pin))
                throw new InvalidOperationException("Pin property must be set for the Authenticate mode");

            return Pin.Equals(pin);
        }

        public void AuthenticateModeOnSuccess()
        {
            AuthenticateModeSuccessHandler?.Invoke(this, new EventArgs());

            if (AuthenticateModeSuccessCommand != null && AuthenticateModeSuccessCommand.CanExecute(null))
            {
                AuthenticateModeSuccessCommand.Execute(null);
            }
        }

        public void AuthenticateModeOnFailure()
        {
            AuthenticateModeFailureHandler?.Invoke(this, new EventArgs());

            if (AuthenticateModeFailureCommand != null && AuthenticateModeFailureCommand.CanExecute(null))
            {
                AuthenticateModeFailureCommand.Execute(null);
            }
        }
    }
}
