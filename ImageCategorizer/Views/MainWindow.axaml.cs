using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using ImageCategorizer.Models;
using ImageCategorizer.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

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
                ViewModel!.SaveAll.RegisterHandler(SaveAll);
                ViewModel!.ClearAllFields.RegisterHandler(ClearAllFields);
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
                    Dispatcher.UIThread.Post(() =>
                    {
                        _currentImage = files[0];
                        this.FindControl<Image>("PreviewImage").Source = new Bitmap(_currentImage);
                    });
                }
            }).ConfigureAwait(false);
            interaction.SetOutput(Unit.Default);
        }

        private string _currentImage;

        private Task SaveAll(InteractionContext<Unit, ImageInfo> interaction)
        {

            interaction.SetOutput(new()
            {
                PreviewImage = _currentImage,
                SerieNames = this.FindControl<TextBox>("SerieNames").Text?.Split(',')?.Select(x => x.Trim())?.ToArray() ?? Array.Empty<string>(),
                Characters = this.FindControl<TextBox>("Characters").Text?.Split(',')?.Select(x => x.Trim())?.ToArray() ?? Array.Empty<string>(),
                SourceName = this.FindControl<TextBox>("SourceName").Text,
                SourceUrl = this.FindControl<TextBox>("SourceName").Text,
                Rating = this.FindControl<ComboBox>("Rating").SelectedIndex,
                RatingTags = this.FindControl<TextBox>("RatingTags").Text?.Split(',')?.Select(x => x.Trim())?.ToArray() ?? Array.Empty<string>()
            });
            return Task.CompletedTask;
        }

        private Task ClearAllFields(InteractionContext<Unit, Unit> interaction)
        {
            _currentImage = null;
            this.FindControl<Image>("PreviewImage").Source = null;
            this.FindControl<TextBox>("SerieNames").Text = "";
            this.FindControl<TextBox>("Characters").Text = "";
            this.FindControl<TextBox>("SourceName").Text = "";
            this.FindControl<TextBox>("SourceUrl").Text = "";
            this.FindControl<ComboBox>("Rating").SelectedIndex = 0;
            this.FindControl<TextBox>("RatingTags").Text = "";
            interaction.SetOutput(Unit.Default);
            return Task.CompletedTask;
        }
    }
}
