using ImageCategorizer.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Json;
using System.Windows.Input;

namespace ImageCategorizer.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            if (!Directory.Exists("img"))
            {
                Directory.CreateDirectory("img");
            }
            if (File.Exists("output.json"))
            {
                _imageInfos = JsonSerializer.Deserialize<ImageInfo[]>(File.ReadAllText("output.json"))!.ToList();
            }
            else
            {
                _imageInfos = new();
            }

            LoadImage = ReactiveCommand.CreateFromTask(async () =>
            {
                await ShowSelectImageDialog.Handle(Unit.Default);
            });
            Save = ReactiveCommand.CreateFromTask(async () =>
            {
                var iInfo = await SaveAll.Handle(Unit.Default);

                if (iInfo.PreviewImage != null)
                {
                    var fi = new FileInfo(iInfo.PreviewImage);
                    var newPath = "img/" + string.Join("_", iInfo.Characters
                        ?.Select(x => string.Join("", x.ToCharArray().Where(x => char.IsLetterOrDigit(x))))
                        ?.Where(x => x.Length > 0)
                        ?.OrderBy(x => x)
                        ?.ToArray() ?? Array.Empty<string>()) + "_" + Guid + fi.Extension;
                    File.Move(iInfo.PreviewImage, newPath);
                    iInfo.PreviewImage = newPath;
                    _imageInfos.Add(iInfo);
                    File.WriteAllText("output.json", JsonSerializer.Serialize(_imageInfos));
                    Guid = Guid.NewGuid();
                    await ClearAllFields.Handle(Unit.Default);
                }
            });
            Clear = ReactiveCommand.CreateFromTask(async () =>
            {
                await ClearAllFields.Handle(Unit.Default);
            });
        }

        private List<ImageInfo> _imageInfos;

        public Interaction<Unit, Unit> ShowSelectImageDialog { get; } = new();
        public Interaction<Unit, ImageInfo> SaveAll { get; } = new();
        public Interaction<Unit, Unit> ClearAllFields { get; } = new();

        public ICommand LoadImage { get; }
        public ICommand Save { get; }
        public ICommand Clear { get; }

        public Guid Guid { private set; get; } = Guid.NewGuid();
    }
}
