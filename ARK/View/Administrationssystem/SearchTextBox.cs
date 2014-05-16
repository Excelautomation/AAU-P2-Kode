using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ARK.View.Administrationssystem
{
    public enum SearchMode
    {
        Instant, 

        Delayed, 
    }

    public class SearchTextBox : TextBox
    {
        #region Static Fields

        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent(
            "Search", 
            RoutingStrategy.Bubble, 
            typeof(RoutedEventHandler), 
            typeof(SearchTextBox));

        public static DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        public static DependencyProperty IsMouseLeftButtonDownProperty =
            IsMouseLeftButtonDownPropertyKey.DependencyProperty;

        public static DependencyProperty LabelTextColorProperty = DependencyProperty.Register(
            "LabelTextColor", 
            typeof(Brush), 
            typeof(SearchTextBox));

        public static DependencyProperty LabelTextProperty = DependencyProperty.Register(
            "LabelText", 
            typeof(string), 
            typeof(SearchTextBox));

        public static DependencyProperty SearchEventTimeDelayProperty =
            DependencyProperty.Register(
                "SearchEventTimeDelay", 
                typeof(Duration), 
                typeof(SearchTextBox), 
                new FrameworkPropertyMetadata(
                    new Duration(new TimeSpan(0, 0, 0, 0, 500)), 
                    OnSearchEventTimeDelayChanged));

        public static DependencyProperty SearchModeProperty = DependencyProperty.Register(
            "SearchMode", 
            typeof(SearchMode), 
            typeof(SearchTextBox), 
            new PropertyMetadata(SearchMode.Instant));

        private static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
            "HasText", 
            typeof(bool), 
            typeof(SearchTextBox), 
            new PropertyMetadata());

        private static readonly DependencyPropertyKey IsMouseLeftButtonDownPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "IsMouseLeftButtonDown", 
                typeof(bool), 
                typeof(SearchTextBox), 
                new PropertyMetadata());

        #endregion

        #region Fields

        private readonly DispatcherTimer searchEventDelayTimer;

        #endregion

        #region Constructors and Destructors

        static SearchTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SearchTextBox), 
                new FrameworkPropertyMetadata(typeof(SearchTextBox)));
        }

        public SearchTextBox()
        {
            this.searchEventDelayTimer = new DispatcherTimer();
            this.searchEventDelayTimer.Interval = this.SearchEventTimeDelay.TimeSpan;
            this.searchEventDelayTimer.Tick += this.OnSeachEventDelayTimerTick;
        }

        #endregion

        #region Public Events

        public event RoutedEventHandler Search
        {
            add
            {
                this.AddHandler(SearchEvent, value);
            }

            remove
            {
                this.RemoveHandler(SearchEvent, value);
            }
        }

        #endregion

        #region Public Properties

        public bool HasText
        {
            get
            {
                return (bool)this.GetValue(HasTextProperty);
            }

            private set
            {
                this.SetValue(HasTextPropertyKey, value);
            }
        }

        public bool IsMouseLeftButtonDown
        {
            get
            {
                return (bool)this.GetValue(IsMouseLeftButtonDownProperty);
            }

            private set
            {
                this.SetValue(IsMouseLeftButtonDownPropertyKey, value);
            }
        }

        public string LabelText
        {
            get
            {
                return (string)this.GetValue(LabelTextProperty);
            }

            set
            {
                this.SetValue(LabelTextProperty, value);
            }
        }

        public Brush LabelTextColor
        {
            get
            {
                return (Brush)this.GetValue(LabelTextColorProperty);
            }

            set
            {
                this.SetValue(LabelTextColorProperty, value);
            }
        }

        public Duration SearchEventTimeDelay
        {
            get
            {
                return (Duration)this.GetValue(SearchEventTimeDelayProperty);
            }

            set
            {
                this.SetValue(SearchEventTimeDelayProperty, value);
            }
        }

        public SearchMode SearchMode
        {
            get
            {
                return (SearchMode)this.GetValue(SearchModeProperty);
            }

            set
            {
                this.SetValue(SearchModeProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var iconBorder = this.GetTemplateChild("PART_SearchIconBorder") as Border;
            if (iconBorder != null)
            {
                iconBorder.MouseLeftButtonDown += this.IconBorder_MouseLeftButtonDown;
                iconBorder.MouseLeftButtonUp += this.IconBorder_MouseLeftButtonUp;
                iconBorder.MouseLeave += this.IconBorder_MouseLeave;
            }
        }

        #endregion

        #region Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape && this.SearchMode == SearchMode.Instant)
            {
                this.Text = string.Empty;
            }
            else if ((e.Key == Key.Return || e.Key == Key.Enter) && this.SearchMode == SearchMode.Delayed)
            {
                this.RaiseSearchEvent();
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            this.HasText = this.Text.Length != 0;

            if (this.SearchMode == SearchMode.Instant)
            {
                this.searchEventDelayTimer.Stop();
                this.searchEventDelayTimer.Start();
            }
        }

        private static void OnSearchEventTimeDelayChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var stb = o as SearchTextBox;
            if (stb != null)
            {
                stb.searchEventDelayTimer.Interval = ((Duration)e.NewValue).TimeSpan;
                stb.searchEventDelayTimer.Stop();
            }
        }

        private void IconBorder_MouseLeave(object obj, MouseEventArgs e)
        {
            this.IsMouseLeftButtonDown = false;
        }

        private void IconBorder_MouseLeftButtonDown(object obj, MouseButtonEventArgs e)
        {
            this.IsMouseLeftButtonDown = true;
        }

        private void IconBorder_MouseLeftButtonUp(object obj, MouseButtonEventArgs e)
        {
            if (!this.IsMouseLeftButtonDown)
            {
                return;
            }

            if (this.HasText && this.SearchMode == SearchMode.Instant)
            {
                this.Text = string.Empty;
            }

            if (this.HasText && this.SearchMode == SearchMode.Delayed)
            {
                this.RaiseSearchEvent();
            }

            this.IsMouseLeftButtonDown = false;
        }

        private void OnSeachEventDelayTimerTick(object o, EventArgs e)
        {
            this.searchEventDelayTimer.Stop();
            this.RaiseSearchEvent();
        }

        private void RaiseSearchEvent()
        {
            var args = new RoutedEventArgs(SearchEvent);
            this.RaiseEvent(args);
        }

        #endregion
    }
}