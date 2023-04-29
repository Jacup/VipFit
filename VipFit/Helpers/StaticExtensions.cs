namespace VipFit.Helpers
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Media;

    public static class StaticExtensions
    {
        public static FrameworkElement VisualTreeFindName(this DependencyObject element, string name)
        {
            if (element == null || string.IsNullOrWhiteSpace(name))
                return null;

            if (name.Equals((element as FrameworkElement)?.Name, StringComparison.OrdinalIgnoreCase))
                return element as FrameworkElement;

            var childCount = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < childCount; i++)
            {
                var result = VisualTreeHelper.GetChild(element, i).VisualTreeFindName(name);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
