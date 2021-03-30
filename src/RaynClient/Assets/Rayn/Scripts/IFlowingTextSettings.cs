using System;

namespace ScreenOverwriter
{
    public interface IFlowingTextSettings
    {
        float FontSize { get; set; }
        IObservable<float> OnFontSizeChange();
    }
}