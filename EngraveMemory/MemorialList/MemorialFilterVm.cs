using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Rg.Plugins.Popup.Services;

namespace EngraveMemory.MemorialList
{
    public class MemorialFilterVm : ReactiveObject
    {
        private readonly MemorialFilterSettings _settings;
        private bool _invalidatingShowAll;
        private bool _onShowAllChanging;

        public MemorialFilterVm(MemorialFilterSettings settings)
        {
            _settings = settings;
            ShowAll = settings.ShowAll;
            ShowNeedRepeat = settings.ShowNeedRepeat;
            ShowCompleted = settings.ShowCompleted;
            ShowInProgress = settings.ShowInProgress;

            this.WhenAnyValue(f => f.ShowAll).Skip(1).Where(b => !_invalidatingShowAll).Subscribe(b => OnShowAllChanged());
            this.WhenAnyValue(f => f.ShowNeedRepeat).Skip(1).Where(b => !_onShowAllChanging).Subscribe(b => InvalidateShowAll());
            this.WhenAnyValue(f => f.ShowInProgress).Skip(1).Where(b => !_onShowAllChanging).Subscribe(b => InvalidateShowAll());
            this.WhenAnyValue(f => f.ShowCompleted).Skip(1).Where(b => !_onShowAllChanging).Subscribe(b => InvalidateShowAll());
            this.WhenAnyValue(f => f.ShowAll).Select(all => "all").ToPropertyEx(this, f => f.Test);
        }

        public extern string Test { [ObservableAsProperty] get; }
        
        private string _initialState;
        
        private string GetState() => JsonConvert.SerializeObject(_settings);
        private void SaveSelection()
        {
            _settings.ShowAll = ShowAll;
            _settings.ShowNeedRepeat = ShowNeedRepeat;
            _settings.ShowInProgress = ShowInProgress;
            _settings.ShowCompleted = ShowCompleted;
        }

        public async Task<bool> Show()
        {
            var tcs = new TaskCompletionSource<bool>();
            var filterView = new MemorialFilterView {BindingContext = this};
            await PopupNavigation.Instance.PushAsync(filterView);
            _initialState = GetState();

            void OnFilterViewOnDisappearing(object sender, EventArgs args)
            {
                filterView.Disappearing -= OnFilterViewOnDisappearing;
                tcs.SetResult(_initialState != GetState());
            }

            filterView.Disappearing += OnFilterViewOnDisappearing;
            return await tcs.Task;
        }

        [Reactive]
        public bool ShowAll { get; set; }

        [Reactive]
        public bool ShowNeedRepeat { get; set; }

        [Reactive]
        public bool ShowInProgress { get; set; }

        [Reactive]
        public bool ShowCompleted { get; set; }

        private void OnShowAllChanged()
        {
            _onShowAllChanging = true;
            ShowNeedRepeat = ShowAll;
            ShowInProgress = ShowAll;
            ShowCompleted = ShowAll;
            _onShowAllChanging = false;
            SaveSelection();
        }

        private void InvalidateShowAll()
        {
            _invalidatingShowAll = true;
            ShowAll = ShowNeedRepeat && ShowInProgress && ShowCompleted;
            _invalidatingShowAll = false;
            SaveSelection();
        }
    }
}