using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ImageCategorizer.ViewModels;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageCategorizer.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(_ =>
            {
                ViewModel!.ShowSelectImageDialog.RegisterHandler(ShowSelectImageDialog);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void Render(DrawingContext context)
        {
            this.FindControl<TextBlock>("ID").Text = ViewModel!.Guid.ToString();
        }

        private async Task ShowSelectImageDialog(InteractionContext<Unit, Unit> interaction)
        {
            var dialog = new OpenFileDialog
            {
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter
                    {
                        Name = "Image",
                        Extensions = new() { "png", "jpg", "jpeg", "gif" }
                    }
                }
            };
            await dialog.ShowAsync((Window)VisualRoot).ContinueWith(async (s) =>
            {
                var files = await s.ConfigureAwait(false);
                if (files.Length > 0)
                {
                    this.FindControl<Image>("PreviewImage").Source = new BitmapImage()
                }
            }).ConfigureAwait(false);
            interaction.SetOutput(Unit.Default);
        }
    }
}
