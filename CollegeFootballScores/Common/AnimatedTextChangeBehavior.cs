using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace CollegeFootballScores.Common
{
    public class AnimatedTextChangeBehavior : Behavior<TextBlock>
    {
        public Duration AnimationDuration { get; set; }

        private string OldValue = null;
        private string NewValue = null;

        DoubleAnimation AnimationOut;
        DoubleAnimation AnimationIn;

        protected override void OnAttached()
        {
            base.OnAttached();

            AnimationOut = new DoubleAnimation(1, 0, AnimationDuration, FillBehavior.HoldEnd);
            AnimationIn = new DoubleAnimation(0, 1, AnimationDuration, FillBehavior.HoldEnd);
            AnimationOut.Completed += (sOut, eOut) =>
            {
                AssociatedObject.SetCurrentValue(TextBlock.TextProperty, NewValue);
                OldValue = NewValue;
                AssociatedObject.BeginAnimation(TextBlock.OpacityProperty, AnimationIn);
            };

            Binding.AddTargetUpdatedHandler(AssociatedObject, new EventHandler<DataTransferEventArgs>(Updated));
        }

        private void Updated(object sender, DataTransferEventArgs e)
        {
            string value = AssociatedObject.GetValue(TextBlock.TextProperty) as string;
            AssociatedObject.BeginAnimation(TextBlock.OpacityProperty, AnimationOut);
            NewValue = value;
            if (OldValue == null)
            {
                OldValue = value;
            }
            AssociatedObject.SetCurrentValue(TextBlock.TextProperty, OldValue);
        }
    }
}
