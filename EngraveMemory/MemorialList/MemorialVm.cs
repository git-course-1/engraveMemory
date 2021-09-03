using System;
using System.Windows.Input;
using DevsTeam.Framework.Autofac;
using EngraveMemory.Common;
using EngraveMemory.Engine;
using EngraveMemory.MemorialDetails;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace EngraveMemory.MemorialList
{
    public class MemorialVm : ReactiveObject
    {
        private readonly Factory<Memorial, MemorialDetailsVm> _defailsFactory;
        private readonly Func<INavigation> _navigation;
        private Memorial _memorial;

        public MemorialVm(Memorial memorial, 
            Func<INavigation> navigation,
            Factory<Memorial, MemorialDetailsVm> defailsFactory,
            Func<Memorial, ProgressVm> progressFactory)
        {
            _defailsFactory = defailsFactory;
            _navigation = navigation;
            Id = memorial.Id;
            Progress = progressFactory(memorial);
            ShowDetailsCommand = ReactiveCommand.Create(ShowDetails);
            CreatedDate = $"{memorial.Timestamp.ToGoodDateString()} в {memorial.Timestamp.ToString("HH:mm")}";
            InvalidateWith(memorial);
        }

        public string CreatedDate { get; }

        [Reactive]
        public string NextPointDate { get; private set; }
        
        [Reactive]
        public string TimeToRepeat { get; private set; }

        public ProgressVm Progress { get; }

        public int? Id { get; }

        public ICommand ShowDetailsCommand { get; }

        [Reactive]
        public string Header { get; private set; }

        private void ShowDetails()
        {
            var detailsVm = _defailsFactory(_memorial);
            _navigation().PushAsync(new MemorialDetailsPage( detailsVm), true);
        }

        public void InvalidateWith(Memorial memorial)
        {
            Header = !string.IsNullOrEmpty(memorial.Header) ? memorial.Header : memorial.Content;
            _memorial = memorial;
            Progress.Invalidate(_memorial);
        }
    }
}