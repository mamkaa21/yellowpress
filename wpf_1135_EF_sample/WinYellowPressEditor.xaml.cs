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
    /// Логика взаимодействия для WinYellowPressEditor.xaml
    /// </summary>
    public partial class WinYellowPressEditor : Window
    {
        public YellowPress SelectedYellowPress { get; set; } = new YellowPress();
        public Singer Singer { get; set; }

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

        private List<YellowPress> yellowPresss;

        public List<YellowPress> YellowPresses
        {
            get => yellowPresss;
            set
            {
                yellowPresss = value;
                Signal();
            }
        }

        public WinYellowPressEditor(Singer selectedSinger)
        {
            InitializeComponent();
            Singer = selectedSinger;
            DataContext = this;
        }

        void Signal([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public event PropertyChangedEventHandler? PropertyChanged;

        private void SaveClose(object sender, RoutedEventArgs e)
        {
            using (var db = new _1135New2024Context())
            {
                List<YellowPress> originalYellowPress = new();

                if (Singer != null)
                {
                    var original = db.Singers.Include(s => s.YellowPresses).
                        FirstOrDefault(s => s.Id == Singer.Id);
                    original.YellowPresses.Add(SelectedYellowPress);// это для бд
                    Singer.YellowPresses.Add(SelectedYellowPress);// это для предыдущего окна
                    db.SaveChanges();
                }
            }
            Close();
        }
    }
}
