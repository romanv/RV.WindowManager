namespace RV.WM2.WindowManager.Core
{
    using System;
    using System.Windows;
    using System.Windows.Interactivity;

    public class ActivateBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty ActivatedProperty = DependencyProperty.Register(
            "Activated",
            typeof(bool),
            typeof(ActivateBehavior),
            new PropertyMetadata(OnActivatedChanged));

        private bool _isActivated;

        public bool Activated
        {
            get
            {
                return (bool)GetValue(ActivatedProperty);
            }

            set
            {
                SetValue(ActivatedProperty, value);
            }
        }

        protected override void OnAttached()
        {
            AssociatedObject.Activated += OnActivated;
            AssociatedObject.Deactivated += OnDeactivated;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Activated -= OnActivated;
            AssociatedObject.Deactivated -= OnDeactivated;
        }

        private static void OnActivatedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (ActivateBehavior)dependencyObject;

            if (!behavior.Activated || behavior._isActivated)
            {
                return;
            }

            if (behavior.AssociatedObject.WindowState == WindowState.Minimized)
            {
                behavior.AssociatedObject.WindowState = WindowState.Normal;
            }

            behavior.AssociatedObject.Activate();
        }

        private void OnActivated(object sender, EventArgs eventArgs)
        {
            _isActivated = true;
            Activated = true;
        }

        private void OnDeactivated(object sender, EventArgs eventArgs)
        {
            _isActivated = false;
            Activated = false;
        }
    }
}
