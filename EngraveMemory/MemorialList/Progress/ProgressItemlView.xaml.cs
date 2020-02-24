using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EngraveMemory.MemorialList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressItemView
    {
        private CancellationTokenSource _source;

        public ProgressItemView()
        {
            InitializeComponent();
            InvalidateRepeatFrame();
            AnimationFrame.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsVisible))
                {
                    InvalidateRepeatFrame();
                }
            };
        }

        private void InvalidateRepeatFrame()
        {
            _source?.Cancel();

            if (AnimationFrame.IsVisible)
            {
                _source = new CancellationTokenSource();
                AnimateRepeatFrame(_source.Token);
            }
        }

        private async void AnimateRepeatFrame(CancellationToken cancellationToken)
        {
            var targetScale = 1.6;
            var direction = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                await AnimationFrame.ScaleTo(targetScale, 700);
                targetScale = direction ? 1 : 1.6;
                direction = !direction;
            }
        }
    }
}
