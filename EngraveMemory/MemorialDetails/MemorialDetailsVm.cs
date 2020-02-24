using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EngraveMemory.Common;
using EngraveMemory.Engine;
using EngraveMemory.MemorialList;
using Plugin.LocalNotifications;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms.Internals;

namespace EngraveMemory.MemorialDetails
{
    public class MemorialDetailsVm : ReactiveObject
    {
        private readonly Memorial _memorial;
        private readonly MemorialRepository _memorialRepository;
        private readonly MemorialListVm _memorialListVm;
        private readonly RootPageNavigation _rootPageNavigation;

        public MemorialDetailsVm(Memorial memorial, MemorialRepository memorialRepository,
            MemorialListVm memorialListVm, RootPageNavigation rootPageNavigation,
            Func<Memorial, ProgressVm> progressFactory)
        {            
            _memorial = memorial;
            _memorialRepository = memorialRepository;
            _memorialListVm = memorialListVm;
            _rootPageNavigation = rootPageNavigation;
            SaveCommand = ReactiveCommand.CreateFromTask(()=> Save(false)
                ,this.WhenAnyValue(d => d.Header).Select(h => true)
                );
            ButtonText = memorial.Id == 0 ? "Создать" : "Я повторил(а)";
            RepeatCommand = memorial.Id != 0? ReactiveCommand.CreateFromTask(()=> Save(true)) : null;            
            if (memorial.Id == 0) return;
            HasProgress = true;
            Header = memorial.Header;
            Text = memorial.Content;
            Progress = progressFactory(memorial);
            Progress.Invalidate(memorial);
        }
        
        public bool HasProgress { get; }

        public ProgressVm Progress { get; }
        
        public string ButtonText { get; }
        
        public ICommand RepeatCommand { get; }
        
        public async Task Save(bool repeat)
        {
            Text = Text.Trim();
            Header = Header.Trim();
            var now = DateTime.Now;
            if (_memorial.Id == 0)
            {
                if (string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(Header)) return;
                var memorial = new Memorial(Header, Text, now);
                now.GetRepeatDateTimes().ForEach(p => CrossLocalNotifications.Current.Show("title", "body", 101, p));
                await _memorialRepository.Add(memorial);
                _memorialListVm.Add(memorial);
            }
            else
            {
                var memorial = await _memorialRepository.Get(_memorial.Id);
                memorial.Content = Text;
                memorial.Header = Header;
                if (repeat) memorial.LastRepeatDate = now;
                await _memorialRepository.Update(memorial);
                _memorialListVm.InvalidateMemorial(memorial);
            }

            if (repeat) await _rootPageNavigation.Navigation.PopAsync();
        }

        public ICommand SaveCommand { get; }

        [Reactive]
        public string Header { get; set; } = "";

        [Reactive]
        public string Text { get; set; } = "";
    }
}