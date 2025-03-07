//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class SeekerTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SeekerTable()
        {
            this.RequestTables = new HashSet<RequestTable>();
        }
    
        public int SeekerID { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public int CityID { get; set; }
        public int BloodGroupID { get; set; }
        public string ContactNo { get; set; }
        public string AadharNo { get; set; }
        public int GenderID { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public int UserID { get; set; }
    
        public virtual BloodGroupsTable BloodGroupsTable { get; set; }
        public virtual CityTable CityTable { get; set; }
        public virtual GenderTable GenderTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestTable> RequestTables { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}
