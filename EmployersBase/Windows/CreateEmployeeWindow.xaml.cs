using EmployersBase.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EmployersBase.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateEmployee.xaml
    /// </summary>
    public partial class CreateEmployeeWindow : Window
    {

        private string SelectedOrganization {  get; set; }
        public CreateEmployeeWindow(string selectedOrganization)
        {
            this.SelectedOrganization = selectedOrganization;
            InitializeComponent();
        }
        /// <summary>
        /// adds employee to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateEmployeeButtonClick(object sender, RoutedEventArgs e)
        {
            EmployesEntity employesEntity = new EmployesEntity()
            {
                DateOfBirth = DateOfBirthField.SelectedDate.Value.Date,
                LastName = LastNameField.Text,
                FirstName = FirstNameField.Text,
                MiddleName = MiddleNameField.Text,
                PassportSerial = PassportSerialField.Text,
                PassportNumber = PassportNumberField.Text
            };
            BaseContext db = new BaseContext();
            var organization = db.Organizations.ToList().FirstOrDefault(x => x.Name == SelectedOrganization);
            if (organization == null) { return; }
            if (organization.Employes == null) organization.Employes = new List<EmployesEntity>();

            

            organization.Employes.Add(employesEntity);

            db.SaveChanges();

        }
    }
}
