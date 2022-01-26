using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLifeXamarin.Common
{
    public class InvertableBool
    {
        private bool value = false;

        public bool Value { get { return value; } }
        public bool Invert { get { return !value; } }

        public InvertableBool(bool b)
        {
            value = b;
        }

        public static implicit operator InvertableBool(bool b)
        {
            return new InvertableBool(b);
        }

        public static implicit operator bool(InvertableBool b)
        {
            return b.value;
        }

    }
}
