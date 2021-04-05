using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FormsPinView.Core
{
    public enum Mode
    {
        Default,
        Set,
        Reset,
        Authenticate
    }

    public enum Stage
    {
        SetDefine,
        SetConfirm,
        ResetAuthorize,
        ResetDefine,
        ResetConfirm,
    }

    public partial class PinView : AbsoluteLayout
    {
        private string _cachePin;

        public static readonly BindableProperty PinProperty =
            BindableProperty.Create(
                propertyName: nameof(Pin),
                returnType: typeof(string),
                declaringType: typeof(PinView),
                defaultValue: null);

        public string Pin
        {
            get => (string)GetValue(PinProperty);
            set => SetValue(PinProperty, value);
        }

        public static readonly BindableProperty ModeProperty =
            BindableProperty.Create(
                propertyName: nameof(Mode),
                returnType: typeof(Mode),
                declaringType: typeof(PinView),
                defaultValue: Mode.Default);

        public Mode Mode
        {
            get => (Mode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public Stage Stage { get; private set; }

        public bool Validate(IList<char> pin)
        {
            var pinString = string.Concat(pin);

            return Mode switch
            {
                Mode.Set => ValidateSetMode(pinString),
                Mode.Reset => ValidateResetMode(pinString),
                Mode.Authenticate => ValidateAuthenticateMode(pinString),
                _ => false
            };
        }

        public void OnSuccess()
        {
            switch(Mode)
            {
                case Mode.Set: SetModeOnSuccess(); break;
                case Mode.Reset: ResetModeOnSuccess(); break;
                case Mode.Authenticate: AuthenticateModeOnSuccess(); break;
                case Mode.Default: throw new NotImplementedException();
                default: throw new NotImplementedException();
            };
        }

        public void OnFailure()
        {
            switch (Mode)
            {
                case Mode.Set: SetModeOnFailure(); break;
                case Mode.Reset: ResetModeOnFailure(); break;
                case Mode.Authenticate: AuthenticateModeOnFailure(); break;
                case Mode.Default: throw new NotImplementedException();
                default: throw new NotImplementedException();
            };
        }

    }
}

