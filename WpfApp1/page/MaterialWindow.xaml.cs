using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using WpfApp1.class_;

namespace WpfApp1.page
{
    /// <summary>
    /// Логика взаимодействия для MaterialWindow.xaml
    /// </summary>
    public partial class MaterialWindow : Window, INotifyPropertyChanged
    {
        public Material CurrentMaterial { get; set; }
        public string WindowName
        {
            get
            {
                return CurrentMaterial.ID == 0 ? "Новая услуга" : "Редактирование услуги";
            }
        }
        public MaterialWindow(Material material)
        {
            InitializeComponent();
            CurrentMaterial = material;
            this.DataContext = this;

        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMaterial.Cost <= 0)
            {
                MessageBox.Show("Стоимость товара должна быть больше нуля");
                return;
            }
            if (CurrentMaterial.MinCount <= -1)
            {
                MessageBox.Show("Минимальное количество товара не должно быть отрицательным");
                return;
            }
            if (CurrentMaterial.CountInStock <= -1)
            {
                MessageBox.Show("Остаток товара не должно быть отрицательным");
                return;
            }
            if (CurrentMaterial.CountInPack <= 0)
            {
                MessageBox.Show("Количество товара не должно быть отрицательным");
                return;
            }


            // если запись новая, то добавляем ее в список
            if (CurrentMaterial.ID == 0)
                Core.DB.Material.Add(CurrentMaterial);

            // сохранение в БД
            if (DialogResult != true)
            {
                Core.DB.SaveChanges();
                this.Close();
            }

        }

        private void GetImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog();
            GetImageDialog.Filter = "Файлы изображений: (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg";
            GetImageDialog.InitialDirectory = Environment.CurrentDirectory;
            if (GetImageDialog.ShowDialog() == true)
            {
                CurrentMaterial.Image = GetImageDialog.FileName.Substring(Environment.CurrentDirectory.Length + 1);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentService"));
                }
            }
        }
    }
}
