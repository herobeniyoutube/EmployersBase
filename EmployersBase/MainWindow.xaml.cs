using EmployersBase.Entities;
using EmployersBase.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
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
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Globalization;

namespace EmployersBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string selectedOrganization = "";
        public static string SelectedOrganization { get { return selectedOrganization; } }    
       
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        /// <summary>
        /// Gets all organizations from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var db = new BaseContext();
            OrganizationsSelectionComboBox.ItemsSource = db.Organizations.ToList();
        }
        /// <summary>
        /// Opens window which creates new organization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateOrganizationWindowButtonClick(object sender, RoutedEventArgs e)
        {
            CreateOrganizationWindow window = new CreateOrganizationWindow();
            window.ShowDialog();
        }
        /// <summary>
        /// Opens window which adds new employee to an organization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateEmployeeWindowButtonClick(object sender, RoutedEventArgs e)
        {
            CreateEmployeeWindow window = new CreateEmployeeWindow(SelectedOrganization);
            window.Owner = this;
            window.ShowDialog();
        }
        /// <summary>
        /// Makes buttons for cooperation with db sheet visible after selection of organization changed. Sets current selected organization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrganizationsSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrganizationsSelectionComboBox.SelectedItem == null) 
            {
                AddEmployeeButton.Visibility = Visibility.Collapsed;
                return;
            }
            AddEmployeeButton.Visibility = Visibility.Visible;
            ExportOrganizationButton.Visibility = Visibility.Visible;
            ImportOrganizationButton.Visibility = Visibility.Visible;
            selectedOrganization = OrganizationsSelectionComboBox.SelectedValue.ToString();
        }
        /// <summary>
        /// exports list of employees to csv from selected organization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportOrganization_Click(object sender, RoutedEventArgs e)
        {
            var db = new BaseContext();
            var list = db.Organizations.Include(x => x.Employes).FirstOrDefault(x => x.Name == SelectedOrganization);
            if (list == null)
            {
                MessageBox.Show("В компании нет сотрудников");
                return;
            }
            var employeeList = list.Employes.ToList();

            using (var writer = new StreamWriter("newCsv.csv",false, Encoding.GetEncoding("windows-1251")))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("ru-RU"))
                {
                    HasHeaderRecord = true,
                    Delimiter = ","
                };

                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    csv.WriteRecords(employeeList);
                }
            }
        }
        /// <summary>
        /// import and replaces WHOLE organization employee list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportOrganizationButton_Click(object sender, RoutedEventArgs e)
        {
            var employesImported = new List<EmployesEntity>();
            string path = "";

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "(*.csv)|*.csv";
            bool? dialogResult = dialog.ShowDialog();

            if (dialogResult == true) 
            {
                path = $"{dialog.FileName}{dialog.DefaultExt}";
            }
            else
            {
                return;
            }

            try
            {
                using (var reader = new StreamReader(path, Encoding.GetEncoding("windows-1251")))
                {
                    var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("ru-RU"))
                    {
                        HasHeaderRecord = true,
                        Delimiter = ","
                    };

                    using (var csv = new CsvReader(reader, csvConfig))
                    {
                        var record = new EmployesEntity();
                        var records = csv.EnumerateRecords(record);

                        foreach (var r in records)
                        {
                            employesImported.Add(new EmployesEntity()
                            {
                                LastName = r.LastName,
                                FirstName = r.FirstName,
                                MiddleName = r.MiddleName,
                                DateOfBirth = r.DateOfBirth,
                                PassportSerial = r.PassportSerial,
                                PassportNumber = r.PassportNumber
                            });
                        }
                    }
                    var db = new BaseContext();
                    var organization = db.Organizations.Include(x => x.Employes).FirstOrDefault(x => x.Name == SelectedOrganization);

                    if (organization == null) { return; }
                    if (organization.Employes == null) organization.Employes = new List<EmployesEntity>();

                    organization.Employes = employesImported;
                    db.SaveChanges();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Неправильный формат файла/его содержимое");
            }
        }
    }
}
