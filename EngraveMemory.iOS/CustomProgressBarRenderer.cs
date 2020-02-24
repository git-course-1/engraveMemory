using CoreGraphics;
using EngraveMemory.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(ProgressBar), typeof(CustomProgressBarRenderer))]
namespace EngraveMemory.iOS
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(
            ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

//            Control.ProgressTintColor = Color.FromRgb(182, 231, 233).ToUIColor();// If you want to change the color

        }


        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var X = 1.0f;
            var Y = 10.0f; //This changes the height

            CGAffineTransform transform = CGAffineTransform.MakeScale(X, Y);
            this.Transform = transform;
        }
    }
}