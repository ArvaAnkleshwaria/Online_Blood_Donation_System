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
    
    public partial class BloodBankTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BloodBankTable()
        {
            this.BloodStockTables = new HashSet<BloodStockTable>();
        }
    
        public int BloodBankID { get; set; }
        public string BloodBankName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public int CityID { get; set; }
        public int UserID { get; set; }
    
        public virtual CityTable CityTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BloodStockTable> BloodStockTables { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}
