using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ImageCategorizer.Views;
using ReactiveUI;

namespace ImageCategorizer
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            return new MainWindow();
        }

        public bool Match(object data)
        {
            return data is ReactiveObject;
        }
    }
}
