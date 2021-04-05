using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FormsPinView.Core
{
    public partial class PinView : AbsoluteLayout
    {
        public event EventHandler<EventArgs> SetModeDefineSuccessHandler;
        public event EventHandler<PinChangedEventArgs> SetModeConfirmationSuccessHandler;
        public event EventHandler<EventArgs> SetModeConfirmationFailureHandler;

        public static readonly BindableProperty SetModeDefineSuccessCommandProperty =
            BindableProperty.Create(propertyName: nameof(SetModeDefineSuccessCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        public ICommand SetModeDefineSuccessCommand
        {
            get { return (ICommand)GetValue(SetModeDefineSuccessCommandProperty); }
            set { SetValue(SetModeDefineSuccessCommandProperty, value); }
        }

        public static readonly BindableProperty SetModeConfirmationSuccessCommandProperty =
            BindableProperty.Create(propertyName: nameof(SetModeConfirmationSuccessCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        public ICommand SetModeConfirmationSuccessCommand
        {
            get { return (ICommand)GetValue(SetModeConfirmationSuccessCommandProperty); }
            set { SetValue(SetModeConfirmationSuccessCommandProperty, value); }
        }

        public static readonly BindableProperty SetModeConfirmationFailureCommandProperty =
            BindableProperty.Create(propertyName: nameof(SetModeConfirmationFailureCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        public ICommand SetModeConfirmationFailureCommand
        {
            get { return (ICommand)GetValue(SetModeConfirmationFailureCommandProperty); }
            set { SetValue(SetModeConfirmationFailureCommandProperty, value); }
        }

        public bool ValidateSetMode(string pin)
        {
            if (Stage == Stage.SetDefine)
            {
                _cachePin = pin;
                return true;
            }

            if (Stage == Stage.SetConfirm)
            {
                return _cachePin.Equals(pin);
            }

            throw new InvalidOperationException("Invalid Stage for Set mode");
        }

        public void SetModeOnSuccess()
        {
            if (Stage == Stage.SetDefine)
            {
                Stage = Stage.SetConfirm;
                EnteredPin.Clear();
                UpdateDisplayedText(resetUI: false);

                SetModeDefineSuccessHandler?.Invoke(this, new EventArgs());

                if (SetModeDefineSuccessCommand != null && SetModeDefineSuccessCommand.CanExecute(null))
                {
                    SetModeDefineSuccessCommand.Execute(_cachePin);
                }

                return;
            }

            if (Stage == Stage.SetConfirm)
            {
                SetModeConfirmationSuccessHandler?.Invoke(this, new PinChangedEventArgs() { NewPin = _cachePin });

                if (SetModeConfirmationSuccessCommand != null && SetModeConfirmationSuccessCommand.CanExecute(null))
                {
                    SetModeConfirmationSuccessCommand.Execute(_cachePin);
                }
            }
        }

        public void SetModeOnFailure()
        {
            if (Stage == Stage.SetConfirm)
            {
                Stage = Stage.SetDefine;

                SetModeConfirmationFailureHandler?.Invoke(this, new EventArgs());

                if (SetModeConfirmationFailureCommand != null && SetModeConfirmationFailureCommand.CanExecute(null))
                {
                    SetModeConfirmationFailureCommand.Execute(null);
                }
            }
        }
    }
}
