using System;
using System.Windows.Input;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    public class KeyboardViewModel : ContentViewModelBase
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Notify();

                if (TextChanged != null)
                    TextChanged(this, new KeyboardEventArgs(Text));
            }
        }

        public ICommand SendKeyCommand
        {
            get { return GetCommand<string>(s => { Text += s; }); }
        }

        public ICommand SendBackspaceCommand
        {
            get
            {
                return GetCommand<string>(s =>
                {
                    if (Text.Length > 0)
                        Text = Text.Substring(0, Text.Length - 1);
                });
            }
        }

        public ICommand ClearCommand
        {
            get { return GetCommand<string>(s => { Text = ""; }); }
        }

        public event EventHandler<KeyboardEventArgs> TextChanged;
    }
}