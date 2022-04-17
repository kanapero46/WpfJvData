using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfApp1.form.payBalance
{
    /// <summary>
    /// payBalanceWpf.xaml の相互作用ロジック
    /// </summary>
    public partial class payBalanceWpf : Window
    {
        private ObservableCollection<OddzSetClass> SampleDataCollection;

        public payBalanceWpf()
        {
            InitializeComponent();

            SampleDataCollection = new ObservableCollection<OddzSetClass>()
            {
                new OddzSetClass("ディープインパクト", false),
                new OddzSetClass("キズナ", true),
                new OddzSetClass("ビアンフェ", true),
            };

            OddzGridView.ItemsSource = SampleDataCollection;
        }
            
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    class OddzSetClass
    {
        private String name;
        private Boolean setFlg;

        public bool SetFlg { get => setFlg; set => setFlg = value; }
        public string Name { get => name; set => name = value; }

        public OddzSetClass(String name, Boolean Flg)
        {
            SetFlg = Flg;
            Name = name;
        }
    }
}
