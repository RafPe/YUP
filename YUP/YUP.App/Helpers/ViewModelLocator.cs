using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using YUP.App.Yupis;

namespace YUP.App.Helpers
{
    public static class ViewModelLocator
    {
        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoWireViewModelProperty, value);
        }

        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel",
            typeof(bool), typeof(ViewModelLocator),
            new PropertyMetadata(false, AutoWireViewModelChanged));

        private static void AutoWireViewModelChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d)) return;
            var viewType = d.GetType();
            var viewTypeName = viewType.FullName;
            //var viewModelTypeName = viewType + "Model";
            var viewModelTypeName = viewTypeName + "Model";
            Type viewModelType = Type.GetType(viewModelTypeName);
            //var viewModel = Activator.CreateInstance();
            var viewModel = ContainerHelper.Container.Resolve(viewModelType);
            ((FrameworkElement)d).DataContext = viewModel;

            // enable by adding in XAML 
            //Services: ViewModelLocator.AutoWireViewModel = "True"

        }
    }
}
