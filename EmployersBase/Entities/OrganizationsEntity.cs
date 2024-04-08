using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployersBase.Entities
{
    public class OrganizationsEntity
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? INN { get; set; }
        public string? LegalAdress { get; set; }
        public string? ActualAdress { get; set; }
        public List<EmployesEntity> Employes { get; set; }

        public override string ToString() => $"{Name}";
        
    }
}
