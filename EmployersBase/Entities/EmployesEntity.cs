using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace EmployersBase.Entities
{
    public class EmployesEntity
    {
        public int Id { get; set; }
        public string? LastName  { get; set; }
        public string? FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PassportSerial { get; set; }
        public string? PassportNumber { get; set; }
        [JsonIgnore]
        public OrganizationsEntity Organizations { get; set; }
    }
}
