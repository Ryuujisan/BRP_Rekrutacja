using System;

namespace Utils
{
    public class GlobalVariable <T>
    {
        public Action<T> OnValueChanged = EventUtils.Empty;
        private T _value;

        public GlobalVariable(T value)
        {
            _value = value;
        }
        
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged.Invoke(_value);
            }
        }
    }
}