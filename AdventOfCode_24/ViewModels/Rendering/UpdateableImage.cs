using Avalonia;
using Avalonia.Controls;

namespace AdventOfCodeUI.ViewModels.Rendering;

public class UpdateableImage : Image
{
    public static readonly AttachedProperty<bool> UpdateProperty =
        AvaloniaProperty.RegisterAttached<UpdateableImage, bool>("Update", typeof(UpdateableImage));

    public static bool GetUpdate(UpdateableImage element)
    {
        element.InvalidateVisual();
        return element.GetValue(UpdateProperty);
    }

    public static void SetUpdate(UpdateableImage element, bool value)
    {
        element.InvalidateVisual();
        element.SetValue(UpdateProperty, value);
    }
}