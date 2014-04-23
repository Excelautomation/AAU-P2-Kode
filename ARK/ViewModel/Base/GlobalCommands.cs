using System.Windows;
using System.Windows.Input;

namespace ARK.ViewModel.Base
{
    public static class GlobalCommands
    {
        private static readonly RoutedUICommand toggleKeyboard = new RoutedUICommand("Sl�r keyboardet til og fra",
            "ToggleKeyboard", typeof(GlobalCommands));

        public static RoutedUICommand ToggleKeyboard { get { return toggleKeyboard; } }
    }
}