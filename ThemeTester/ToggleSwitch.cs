using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ThemeTester
{
    public class ToggleSwitch : Control
    {
        // DependencyProperty for IsChecked
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ToggleSwitch),
                new PropertyMetadata(false, OnIsCheckedChanged));

        // DependencyProperty for ToggleWidth
        public static readonly DependencyProperty ToggleWidthProperty =
            DependencyProperty.Register(nameof(ToggleWidth), typeof(double), typeof(ToggleSwitch),
                new PropertyMetadata(20.0, OnSizeChanged));

        // DependencyProperty for ToggleHeight
        public static readonly DependencyProperty ToggleHeightProperty =
            DependencyProperty.Register(nameof(ToggleHeight), typeof(double), typeof(ToggleSwitch),
                new PropertyMetadata(10.0, OnSizeChanged));

        public static readonly DependencyProperty ToggleRadiusProperty =
            DependencyProperty.Register(nameof(ToggleRadius), typeof(double), typeof(ToggleSwitch),
                new PropertyMetadata(5.0, OnSizeChanged));


        private Border _background;
        private Ellipse _knob;

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public double ToggleWidth
        {
            get => (double)GetValue(ToggleWidthProperty);
            set => SetValue(ToggleWidthProperty, value);
        }

        public double ToggleHeight
        {
            get => (double)GetValue(ToggleHeightProperty);
            set => SetValue(ToggleHeightProperty, value);
        }        
        public double ToggleRadius
        {
            get => (double)GetValue(ToggleRadiusProperty);
            set => SetValue(ToggleRadiusProperty, value);
        }

        static ToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
        }

        // Override OnApplyTemplate to get references to the template parts
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Retrieve the template parts
            _background = GetTemplateChild("PART_Background") as Border;
            _knob = GetTemplateChild("PART_Knob") as Ellipse;

            // Apply initial size
            UpdateSize();

            if (_background != null)
            {
                _background.MouseLeftButtonDown += ToggleSwitch_MouseLeftButtonDown;
            }
        }

        private void ToggleSwitch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsChecked = !IsChecked;
        }

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ToggleSwitch)d;
            control.UpdateSize();
        }

        private void UpdateSize()
        {
            if (_background != null)
            {
                // Update background size
                _background.Width = ToggleWidth;
                _background.Height = ToggleHeight;
                _background.CornerRadius = new CornerRadius(ToggleRadius);
            }

            if (_knob != null)
            {
                // Update knob size
                _knob.Width = _knob.Height = ToggleHeight - 4; // Knob size based on height
                _knob.Margin = new Thickness(IsChecked ? ToggleWidth - ToggleHeight : 2, 2, 0, 2);
            }
        }

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ToggleSwitch)d;
            control.UpdateVisualState((bool)e.NewValue);
        }

        private void UpdateVisualState(bool isChecked)
        {
            if (_knob != null)
            {
                // Determine the target position for the knob (left or right)
                double toValue = isChecked ? ToggleWidth - ToggleHeight : 2;

                Color fromColor = isChecked ? Colors.LightGray : Colors.LightGreen; // Off to On
                Color toColor = isChecked ? Colors.LightGreen : Colors.LightGray; // On to Off
                var colorAnimation = new ColorAnimation
                {
                    From = fromColor,
                    To = toColor,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase()
                };

                var backgroundBrush = new SolidColorBrush(fromColor);
                _background.Background = backgroundBrush;
                backgroundBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

                // Create and run the animation
                var animation = new ThicknessAnimation
                {
                    To = new Thickness(toValue, 2, 0, 2),
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase()
                };
                _knob.BeginAnimation(MarginProperty, animation);
            }
        }
    }
}
