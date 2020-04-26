using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    public class MemorialListVm : ReactiveObject
    {
        public MemorialFilterVm MemorialFilter { get; }
        public const int MemorialsInRowCount = 2;
        private readonly MemorialRepository _memorialRepository;
        private readonly Factory<Memorial, MemorialDetailsVm> _defailsFactory;
        private readonly Func<Memorial, Func<INavigation>, MemorialVm> _memorialVmFactory;

        public MemorialListVm(MemorialRepository memorialRepository,
            Factory<Memorial, MemorialDetailsVm> defailsFactory,
            Func<Memorial, Func<INavigation>, MemorialVm> memorialVmFactory,
            MemorialFilterVm memorialFilter)
        {
            MemorialFilter = memorialFilter;
            _memorialRepository = memorialRepository;
            _defailsFactory = defailsFactory;
            _memorialVmFactory = memorialVmFactory;
            Rows = new ObservableCollection<MemorialRow>(GetAllRows());
            AddMemorialCommand = ReactiveCommand.CreateFromTask(AddMemorial);
            ShowFilterCommand = ReactiveCommand.CreateFromTask(ShowFilter);
        }
        
        [Reactive]
        public INavigation Navigation { get; set; }

        private  async Task ShowFilter()
        {
            var changed = await MemorialFilter.Show();
            if (changed)
            {
                Rows = new ObservableCollection<MemorialRow>(GetAllRows());
            }
        }

        public ICommand ShowFilterCommand { get; }

        [Reactive]
        public ObservableCollection<MemorialRow> Rows { get; private set; }

        public ICommand AddMemorialCommand { get; }

        private async Task AddMemorial()
        {
            var detailsVm = _defailsFactory(new Memorial());
            await Navigation.PushAsync(new MemorialDetailsPage(detailsVm), true);
        }

        public IEnumerable<MemorialRow> GetAllRows()
        {
            var currentRow = new List<MemorialVm>();
            foreach (var memorialVm in _memorialRepository.GetAll().Select(m => _memorialVmFactory(m, ()=> Navigation)).Where(Filter))
            {
                if (currentRow.Count < MemorialsInRowCount) currentRow.Add(memorialVm);
                else
                {
                    yield return new MemorialRow(currentRow[0], currentRow[1]);
                    currentRow = new List<MemorialVm> {memorialVm};
                }
            }

            if (currentRow.Count > 0) yield return new MemorialRow(currentRow[0], currentRow.Count > 1 ? currentRow[1] : null);
        }

        private bool Filter(MemorialVm memorial)
        {
            try
            {
                if (memorial == null) return true;

                if (MemorialFilter.ShowAll) return true;

                return MemorialFilter.ShowCompleted && memorial.Progress.Point4.Repeated ||
                       MemorialFilter.ShowNeedRepeat && (memorial.Progress.NextRepeatPoint != null && memorial.Progress.NextRepeatPoint.NeedRepeat)
                    || MemorialFilter.ShowInProgress && (memorial.Progress.NextRepeatPoint != null && !memorial.Progress.NextRepeatPoint.NeedRepeat);
            }
            catch (Exception e)
            {
                return false; 
            }
        }

        public void Add(Memorial memorial)
        {
            var firstRow = Rows.FirstOrDefault();
            if (firstRow == null)
            {
                Rows.Add(new MemorialRow(_memorialVmFactory(memorial, ()=>Navigation), null));
                return;
            }
            var needAddRow = Rows.Last().Right != null;

            var prevRight = firstRow.Right;
            firstRow.ChangeItems(_memorialVmFactory(memorial, ()=>Navigation), firstRow.Left);
            
            for (var i = 1; i < Rows.Count; i++)
            {
                var row = Rows[i];
                var prevRight1 = row.Right;
                row.ChangeItems(prevRight, row.Left);
                prevRight = prevRight1;
            }
            
            if (needAddRow) Rows.Add(new MemorialRow(prevRight, null));
        }

        public void InvalidateMemorial(Memorial memorial)
        {
            Rows.SelectMany(r => r.Left.AsArray().Union(r.Right.AsArray())).FirstOrDefault(m => m.Id == memorial.Id)?.InvalidateWith(memorial);
        }
    }
}