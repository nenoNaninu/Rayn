using System;
using UniRx;

namespace ScreenOverwriter
{
    public class FlowingTextSettings : IFlowingTextSettings
    {
        private float _fontSize = 50f;
        private readonly Subject<float> _fontChangeSubject = new Subject<float>();

        public float FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                _fontChangeSubject.OnNext(_fontSize);
            }
        }

        public IObservable<float> OnFontSizeChange()
        {
            return _fontChangeSubject.AsObservable();
        }
    }
}