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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.class_;
using WpfApp1.page;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class Material
    {
        public Uri ImageUri
        {
            get
            {
                var imageName = Environment.CurrentDirectory + (Image ?? "");
                return System.IO.File.Exists(imageName) ? new Uri(imageName) :null;
            }
        }
        public string CostString
        {
            get
            {
                return Cost.ToString("#.##");
            }
        }
        public string MinCountString
        {
            get
            {
                return MinCount.ToString("Минимальное Количество: "+"#.##"+" шт");
            }
        }
        public double CountInStockString
        {
            get
            {
                return Convert.ToDouble(CountInStock);
            }
        }
        public string CountInStockString1
        {
            get
            {
                return CountInStockString.ToString("Остаток: " + "#.##" + " шт");
            }
        }
        public string MaterialString
        {
            get
            {
                return MaterialType.Title.ToString() +" | "+ Title.ToString();
            }
        }
        public string SupplierString
        {
            get
            {
                string str = "";
                foreach(var item in Supplier)
                {
                    str += item.Title + " | ";
                }
                return ("Поставщики: ") + str;
            }
        }
        public Boolean Conter300
        {
            get
            {
                return CountInPack >= (MinCount * 3);
            }
        }
        public Boolean MinCounter
        {
            get
            { 
                return CountInPack < MinCount;
            }
        }

    }
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IEnumerable<Material> _MaterialList;
        PageChange pc = new PageChange();
        private int SortType = 0;


        public IEnumerable<Material> MaterialList
        {

            get
            {
                var FilteredMaterialList = _MaterialList;

                if (SearchFilter != "")
                    FilteredMaterialList = FilteredMaterialList.Where(item =>
                        item.MinCountString.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) != -1 ||
                        item.CountInStockString1.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) != -1 ||
                        item.SupplierString.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) != -1 ||
                        item.Title.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) != -1).ToList();

                if (FilterItems != null)
                    FilteredMaterialList = FilteredMaterialList.Where(item =>
                       item.MaterialType.Title.IndexOf(FilterItems, StringComparison.OrdinalIgnoreCase) != -1).ToList();

                switch(SortType)
                {
                    case 1:
                        FilteredMaterialList = FilteredMaterialList.OrderBy(p => p.Title);
                        break;
                    case 2:
                        FilteredMaterialList = FilteredMaterialList.OrderByDescending(p => p.Title);
                        break;
                    case 3:
                        FilteredMaterialList = FilteredMaterialList.OrderByDescending(p => p.CountInStock);
                        break;
                    case 4:
                        FilteredMaterialList = FilteredMaterialList.OrderBy(p => p.CountInStock);
                        break;
                    case 5:
                        FilteredMaterialList = FilteredMaterialList.OrderByDescending(p => p.MinCount);
                        break;
                    case 6:
                        FilteredMaterialList = FilteredMaterialList.OrderBy(p => p.MinCount);
                        break;
                }
                return FilteredMaterialList;
            }
            set
            {
                _MaterialList = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MaterialList"));
            }
        }

        //public List<MaterialType> MaterialTypesList { get; set; }
        public MainWindow()
        {

            this.DataContext = this;
            MaterialList = Core.DB.Material.ToList();
            InitializeComponent();
            for (byte i = 0; i < ListMaterials.Count; i++)
            {
                MaterialFilterComboBox.Items.Add(ListMaterials[i]);
            }

            for (byte i = 0; i < ListSort.Count; i++)
            {
                SortFilterComboBox.Items.Add(ListSort[i]);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            var NewMaterial = new Material();

            var NewMaterialWindow = new MaterialWindow(NewMaterial);
            if ((bool)NewMaterialWindow.ShowDialog())
            {
                MaterialList = Core.DB.Material.ToList();
                PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsPrice"));
                PropertyChanged(this, new PropertyChangedEventArgs("ProductsPrice"));
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private string _SearchFilter = "";
        public string SearchFilter
        {
            get
            {
                return _SearchFilter;
            }
            set
            {
                _SearchFilter = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MaterialList"));
                }
            }
        }
        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchFilter = SearchTextBox.Text;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = MainGrid.SelectedItem as Material;
                if (item.Supplier.Count > 0)
                {
                    MessageBox.Show("Нельзя удалять товар, если есть поставщик этого товара");
                    return;
                }
                Core.DB.Material.Remove(item);
                Core.DB.SaveChanges();
                MaterialList = Core.DB.Material.ToList();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при удалении!");
            }

        }

        public static List<string> ListMaterials = new List<string> { "Все типы", "Гранулы", "Рулон", "Нарезка", "Пресс" };

        private string _FiltrItems;
        public string FilterItems
        {
            get => _FiltrItems;
            set
            {
                _FiltrItems = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MaterialList"));
                }
            }
        }
        private void MaterialFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialFilterComboBox.SelectedIndex > 0)
                FilterItems = MaterialFilterComboBox.SelectedItem.ToString();
            else FilterItems = null;

        }

        public static List<string> ListSort = new List<string> { "Нет сортировки", "Наименование: А -> Я", "Наименование: Я -> А", "Остаток: убывание", "Остаток: возрастание", "Количество: убывание", "Количество: возрастание" };

        private void SortFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortType = SortFilterComboBox.SelectedIndex;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("MaterialList"));
            }
        }

        private void MainGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var SelectedMaterial = MainGrid.SelectedItem as Material;
            var EditMaterialWindow = new MaterialWindow(SelectedMaterial);
            if ((bool)EditMaterialWindow.ShowDialog())
            {
                PropertyChanged(this, new PropertyChangedEventArgs("MaterialList"));
            }
        }
    }
}
