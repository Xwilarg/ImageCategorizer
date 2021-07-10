using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace ImageCategorizer.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            LoadImage = ReactiveCommand.CreateFromTask(async () =>
            {
                await ShowSelectImageDialog.Handle(Unit.Default);
            });
        }

        public Interaction<Unit, Unit> ShowSelectImageDialog { get; } = new();

        public ICommand LoadImage { get; }
        public ICommand Save { get; }
        public ICommand Clear { get; }

        public Guid Guid { private set; get; } = Guid.NewGuid();
    }
}
