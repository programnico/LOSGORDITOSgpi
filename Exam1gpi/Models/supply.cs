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

    public partial class supply
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public supply()
        {
            this.settingProductSupply = new HashSet<settingProductSupply>();
            this.supplyMovement = new HashSet<supplyMovement>();
        }
    
        public int codSupply { get; set; }
        [Display(Name = "Nombre")]
        [RegularExpression(@"^[a-zA-Z0-9 .\ ]{1,40}$", ErrorMessage = "Solo se permiten letras y numeros")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string nameSupply { get; set; }
        [Display(Name = "Existencias")]
        public Nullable<decimal> stock { get; set; }
        [Display(Name = "Habilitar")]
        public bool enable { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<settingProductSupply> settingProductSupply { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supplyMovement> supplyMovement { get; set; }
    }
}
