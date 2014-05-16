using System;
using System.Windows.Input;

using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    public class KeyboardViewModel : ContentViewModelBase
    {
        #region Fields

        private string _text;

        #endregion

        #region Public Events

        public event EventHandler<KeyboardEventArgs> TextChanged;

        #endregion

        #region Public Properties

        public ICommand ClearCommand
        {
            get
            {
                return this.GetCommand(s => { this.Text = string.Empty; });
            }
        }

        public ICommand SendBackspaceCommand
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            if (this.Text.Length > 0)
                            {
                                this.Text = this.Text.Substring(0, this.Text.Length - 1);
                            }
                        });
            }
        }

        public ICommand SendKeyCommand
        {
            get
            {
                return this.GetCommand(s => { this.Text += (string)s; });
            }
        }

        public string Text
        {
            get
            {
                return this._text;
            }

            set
            {
                // Tjek at værdien er ændret
                if (this._text != value)
                {
                    this._text = value;

                    this.Notify();

                    if (this.TextChanged != null)
                    {
                        this.TextChanged(this, new KeyboardEventArgs(this.Text));
                    }
                }
                else
                {
                    this._text = value;
                }
            }
        }

        #endregion
    }
}