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

    public partial class person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public person()
        {
            this.orders = new HashSet<orders>();
        }
    
        public int codPerson { get; set; }
        [Display(Name = "Rol")]
        public int codRole { get; set; }
        [Display(Name = "Nombre")]
        [RegularExpression(@"^[a-zA-Z .\ ]{1,40}$", ErrorMessage = "Solo se permiten letras")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string namePerson { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        public string email { get; set; }
        [Display(Name = "Contraseņa")]
        [Required(ErrorMessage = "La contraseņa es requerida")]
        public string password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orders> orders { get; set; }
        public virtual roles roles { get; set; }
    }
}
