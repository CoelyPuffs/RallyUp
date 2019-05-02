using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace RallyUp
{
    class GrowingEditor : Editor
    {
        public GrowingEditor()
        {
            TextChanged += OnTextChanged;
        }

        ~GrowingEditor()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidateMeasure();
        }
    }
}