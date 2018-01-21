using System;
using Prism.Behaviors;
using Xamarin.Forms;

namespace XamarinConversationClient.Behaviors
{
    public class OpenUrlBehavior : BehaviorBase<View>
    {
        public static readonly BindableProperty TargetProperty =
            BindableProperty.Create("Target", typeof(object), typeof(OpenUrlBehavior));

        public object TargetParameter
        {
            get { return GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);

            var gesture = new TapGestureRecognizer();
            gesture.Tapped += bindable_Clicked;
            bindable.GestureRecognizers.Add(gesture);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.GestureRecognizers.Clear();
        }

        void bindable_Clicked(object sender, EventArgs e)
        {
            Uri uri;
            var param = this.TargetParameter?.ToString();
            if (Uri.TryCreate(param, UriKind.RelativeOrAbsolute, out uri))
            {
                Device.OpenUri(uri);
            }
        }
    }
}
