//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exam1gpi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class productUnitMeasurement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public productUnitMeasurement()
        {
            this.settingProductSupply = new HashSet<settingProductSupply>();
        }
        [Display(Name = "Producto - unidad de medida")]
        public int codProductUnitMeasurement { get; set; }
        [Display(Name = "Producto")]
        public int codProduct { get; set; }
        [Display(Name = "Unidad de medida")]
        public int codUnitMeasurement { get; set; }
    
        public virtual product product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<settingProductSupply> settingProductSupply { get; set; }
        public virtual unitMeasurement unitMeasurement { get; set; }
    }
}
