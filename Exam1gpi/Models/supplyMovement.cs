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

    public partial class supplyMovement
    {
        public int codSupplyMovement { get; set; }
        [Display(Name = "Suministro")]
        public int codSupply { get; set; }
        [Display(Name = "Cantidad")]
        [Range(1, 1000000, ErrorMessage = "Ingrese un n?mero diferente de cero")]
        [Required(ErrorMessage = "La cantidad es requerida")]
        public decimal quantity { get; set; }
        public int tipo { get; set; }
    
        public virtual supply supply { get; set; }
    }
}
