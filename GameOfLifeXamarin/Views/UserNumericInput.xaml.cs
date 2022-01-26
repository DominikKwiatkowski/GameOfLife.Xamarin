using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameOfLifeXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserNumericInput : ContentView
    {
        public UserNumericInput()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(string), typeof(UserNumericInput));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }


        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string), typeof(UserNumericInput));

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }
    }
}