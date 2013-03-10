using Demacia.Services;
using Microsoft.Windows.Controls.PropertyGrid.Attributes;

namespace Demacia
{
    public sealed class FontItemsSource : IItemsSource
    {
        public ItemCollection GetValues()
        {
            var fonts = new ItemCollection();
            AssetService.FontAssets.ForEach(fa => fonts.Add(fa.Font, fa.Font.Name));
            return fonts;
        }
    }
}
