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

        public WinYellowPressEditor()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += WinYellowPressEditor_Loaded;
        }

        private void WinYellowPressEditor_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            using (var db = new _1135New2024Context())
            {
                YellowPresses = new ObservableCollection<YellowPress>(db.YellowPresses.
                    Include(s => s.IdSingerNavigation).ToList());
            }
        }

        private void SaveClose(object sender, RoutedEventArgs e)
        {
            using (var db = new _1135New2024Context())
            {
                ObservableCollection<YellowPress> originalYellowPress = new();
                if (SelectedSinger.Id == 0)
                {
                    db.Singers.Add(SelectedSinger);
                }
                else
                {
                    var original = db.Singers.Include(s => s.YellowPresses).
                        FirstOrDefault(s => s.Id == SelectedSinger.Id);
                    //originalYellowPress.AddRange(original.Musics);                 
                    db.Entry(original).CurrentValues.SetValues(SelectedSinger);
                }

                foreach (var yellowPresses in SelectedSinger.YellowPresses)
                    if (yellowPresses.Id == 0)
                    {
                        yellowPresses.IdSinger = SelectedSinger.Id;
                        db.YellowPresses.Add(yellowPresses);
                    }
                    else
                    {
                        var original = db.YellowPresses.Find(yellowPresses.Id);
                        db.Entry(original).CurrentValues.SetValues(yellowPresses);
                    }
                var currentYellowPress = SelectedSinger.YellowPresses.Select(s => s.Id);
                var toRemove = originalYellowPress.Where(s => !currentYellowPress.Contains(s.Id));
                foreach (var r in toRemove)
                    db.Remove(r);
                db.SaveChanges();
            }
            Close();
        }
    }
}
