using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FormsPinView.Core
{
    public partial class PinView : AbsoluteLayout
    {
        public event EventHandler<EventArgs> ResetModeAuthorizeSuccessHandler;
        public event EventHandler<EventArgs> ResetModeAuthorizeFailureHandler;
        public event EventHandler<PinChangedEventArgs> ResetModeConfirmationSuccessHandler;
        public event EventHandler<EventArgs> ResetModeConfirmationFailureHandler;

        public static readonly BindableProperty ResetModeAuthorizeSuccessCommandProperty =
            BindableProperty.Create(propertyName: nameof(ResetModeAuthorizeSuccessCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        public ICommand ResetModeAuthorizeSuccessCommand
        {
            get { return (ICommand)GetValue(ResetModeAuthorizeSuccessCommandProperty); }
            set { SetValue(ResetModeAuthorizeSuccessCommandProperty, value); }
        }

        public static readonly BindableProperty ResetModeAuthorizeFailureCommandProperty =
            BindableProperty.Create(propertyName: nameof(ResetModeAuthorizeFailureCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        public ICommand ResetModeAuthorizeFailureCommand
        {
            get { return (ICommand)GetValue(ResetModeAuthorizeFailureCommandProperty); }
            set { SetValue(ResetModeAuthorizeFailureCommandProperty, value); }
        }

        public static readonly BindableProperty ResetModeConfirmationSuccessCommandProperty =
            BindableProperty.Create(propertyName: nameof(ResetModeConfirmationSuccessCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        public ICommand ResetModeConfirmationSuccessCommand
        {
            get { return (ICommand)GetValue(ResetModeConfirmationSuccessCommandProperty); }
            set { SetValue(ResetModeConfirmationSuccessCommandProperty, value); }
        }

        public static readonly BindableProperty ResetModeConfirmationFailureCommandProperty =
    BindableProperty.Create(propertyName: nameof(ResetModeConfirmationFailureCommand),
                            returnType: typeof(ICommand),
                            declaringType: typeof(PinView),
                            defaultValue: null);

        public ICommand ResetModeConfirmationFailureCommand
        {
            get { return (ICommand)GetValue(ResetModeConfirmationFailureCommandProperty); }
            set { SetValue(ResetModeConfirmationFailureCommandProperty, value); }
        }

        public bool ValidateResetMode(string pin)
        {
            if (string.IsNullOrEmpty(Pin))
                throw new InvalidOperationException("Pin property must be set for the Reset mode");

            if (Stage == Stage.ResetAuthorize)
            {
                return Pin.Equals(pin);
            }

            if (Stage == Stage.ResetDefine)
            {
                _cachePin = pin;
                return true;
            }

            if (Stage == Stage.ResetConfirm)
            {
                return _cachePin.Equals(pin);
            }

            throw new InvalidOperationException("Invalid Stage for Reset mode");
        }

        public void ResetModeOnSuccess()
        {
            if (Stage == Stage.ResetAuthorize)
            {
                Stage = Stage.ResetDefine;
                ResetModeAuthorizeSuccessHandler?.Invoke(this, new EventArgs());

                if (ResetModeAuthorizeSuccessCommand != null && ResetModeAuthorizeSuccessCommand.CanExecute(null))
                {
                    ResetModeAuthorizeSuccessCommand.Execute(null);
                }

                return;
            }

            if (Stage == Stage.ResetDefine)
            {
                Stage = Stage.ResetConfirm;
                return;
            }

            if (Stage == Stage.ResetConfirm)
            {
                ResetModeConfirmationSuccessHandler?.Invoke(this, new PinChangedEventArgs() { OldPin = Pin, NewPin = _cachePin });

                if (ResetModeConfirmationSuccessCommand != null && ResetModeConfirmationSuccessCommand.CanExecute(null))
                {
                    ResetModeConfirmationSuccessCommand.Execute(_cachePin);
                }

                return;
            }
        }

        public void ResetModeOnFailure()
        {
            if (Stage == Stage.ResetAuthorize)
            {
                ResetModeAuthorizeFailureHandler?.Invoke(this, new EventArgs());

                if (ResetModeAuthorizeFailureCommand != null && ResetModeAuthorizeFailureCommand.CanExecute(null))
                {
                    ResetModeAuthorizeFailureCommand.Execute(null);
                }

                return;
            }

            if (Stage == Stage.ResetConfirm)
            {
                Stage = Stage.ResetDefine;
                ResetModeConfirmationFailureHandler?.Invoke(this, new EventArgs());

                if (ResetModeConfirmationFailureCommand != null && ResetModeConfirmationFailureCommand.CanExecute(null))
                {
                    ResetModeConfirmationFailureCommand.Execute(null);
                }
            }
        }
    }
}
