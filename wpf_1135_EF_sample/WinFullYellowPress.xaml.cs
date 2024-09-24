using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpf_1135_EF_sample
{
    /// <summary>
    /// Логика взаимодействия для WinFullYellowPress.xaml
    /// </summary>
    public partial class WinFullYellowPress : Window
    { 
        private YellowPress selectedYellowPress;

        public YellowPress SelectedYellowPress
        {
            get => selectedYellowPress;
            set
            {
                selectedYellowPress = value;
                Signal();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        
        
        public WinFullYellowPress(YellowPress selectedYellowPress)
        {
            InitializeComponent();
            DataContext = this;

            SelectedYellowPress = selectedYellowPress;
        }
       
    }
}
