using System;
namespace FormsPinView.Core
{
    public class PinChangedEventArgs : EventArgs
    {
        public string OldPin { get; set; }
        public string NewPin { get; set; }
    }
}
