using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace EngraveMemory.MemorialList
{
    public class MemorialFilterSettings
    {
        private static ISettings AppSettings => CrossSettings.Current;
        
        public bool ShowAll
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowAll), false);
            set => AppSettings.AddOrUpdateValue(nameof(ShowAll), value);
        }
        
        public bool ShowCompleted
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowCompleted), false);
            set => AppSettings.AddOrUpdateValue(nameof(ShowCompleted), value);
        }
        
        public bool ShowInProgress
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowInProgress), false);
            set => AppSettings.AddOrUpdateValue(nameof(ShowInProgress), value);
        }
        
        public bool ShowNeedRepeat
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowNeedRepeat), false);
            set => AppSettings.AddOrUpdateValue(nameof(ShowNeedRepeat), value);
        }
    }
}