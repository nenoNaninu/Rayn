using System;

namespace Rayn
{
    public interface IFlowingTextSettings
    {
        float FontSize { get; set; }
        IObservable<float> OnFontSizeChange();
    }
}