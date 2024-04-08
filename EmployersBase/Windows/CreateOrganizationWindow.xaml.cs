using EmployersBase.Entities;
using System;
using System.Collections.Generic;
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

namespace EmployersBase.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateOrganization.xaml
    /// </summary>
    public partial class CreateOrganizationWindow : Window
    {
        public CreateOrganizationWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// adds organization to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateOrganizationButtonClick(object sender, RoutedEventArgs e)
        {
            BaseContext db = new BaseContext();
            var organizationEntity = new OrganizationsEntity()
            {
                Name = OrganizationNameField.Text,
                INN = OrganizationINNField.Text,
                LegalAdress = OrganizationLegalAdressField.Text,
                ActualAdress = OrganizationActualAdressField.Text
            };
            db.Organizations.Add(organizationEntity);
            db.SaveChanges();
        }
    }
}
