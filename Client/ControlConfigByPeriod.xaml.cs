using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Messages;

namespace Client
{
    public partial class ControlConfigByPeriod : UserControl
    {
        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
            "From", typeof(DateTime), typeof(ControlConfigByPeriod), new PropertyMetadata(default(DateTime)));

        public DateTime From
        {
            get { return (DateTime) GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To", typeof(DateTime), typeof(ControlConfigByPeriod), new PropertyMetadata(default(DateTime)));

        public DateTime To
        {
            get { return (DateTime) GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            "Item", typeof(IEnumerable<HardwareConfig>), typeof(ControlConfigByPeriod), new PropertyMetadata(default(IEnumerable<HardwareConfig>)));

        public IEnumerable<HardwareConfig> Item
        {
            get { return (IEnumerable<HardwareConfig>) GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
        public ControlConfigByPeriod()
        {
            InitializeComponent();
        }
    }
}