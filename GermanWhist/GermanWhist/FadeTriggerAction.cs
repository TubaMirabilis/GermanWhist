using Xamarin.Forms;

namespace GermanWhist
{
    public class FadeTriggerAction : TriggerAction<VisualElement>
    {
        public int StartsFrom { set; get; }
        protected override void Invoke(VisualElement sender)
        {
            if (StartsFrom == 1)
            {
                sender.Animate("FadeTriggerAction", new Animation(v => sender.Scale = v, 1, 0),
                length: (uint)App.AnimationDuration,
                easing: Easing.CubicIn);
            }
            if (StartsFrom == 0)
            {
                sender.Animate("FadeTriggerAction", new Animation(v => sender.Scale = v, 0, 1),
                length: 20,
                easing: Easing.CubicIn);
            }
        }
    }
}