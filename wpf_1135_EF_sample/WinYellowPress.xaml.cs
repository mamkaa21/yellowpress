﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    /// Логика взаимодействия для WinYellowPress.xaml
    /// </summary>
    public partial class WinYellowPress : Window, INotifyPropertyChanged
    {
        private List<Singer> singers;

        public List<Singer> Singers
        {
            get => singers;
            set
            {
                singers = value;
                Signal();
            }
        }

        private ObservableCollection<YellowPress> yellowPresss;
        
        public ObservableCollection<YellowPress> YellowPresses
        {
            get => yellowPresss;
            set
            {
                yellowPresss = value;
                Signal();
            }
        }

        public YellowPress SelectedYellowPress { get; set; }
        public Singer SelectedSinger { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public WinYellowPress()
        {
            InitializeComponent();
            DataContext = this;

            Loaded += WinYellowPress_Loaded;
        }

        private void WinYellowPress_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            using (var db = new _1135New2024Context())
            {
                YellowPresses = new ObservableCollection<YellowPress>( db.YellowPresses.
                    Include(s => s.IdSingerNavigation).ToList());
            }
        }

        private void DeleteYellowPress(object sender, RoutedEventArgs e)
        {
            //if (SelectedYellowPress == null)
            //    return;

            using (var db = new _1135New2024Context())
            {
                
                db.Remove(SelectedYellowPress);

                db.SaveChanges();
            }
            YellowPresses.Remove(SelectedYellowPress);
           
        }

        private void ddclick(object sender, MouseButtonEventArgs e)
        {
            WinFullYellowPress winFullYellowPress = new WinFullYellowPress(SelectedYellowPress);
            winFullYellowPress.Show();
        }
    }
}
