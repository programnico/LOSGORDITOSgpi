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

    public partial class sales
    {
        public int codSale { get; set; }
        public Nullable<System.DateTime> dateSale { get; set; }
        [Display(Name = "Pedido")]
        public int codOrder { get; set; }
        [Display(Name = "Precio")]
        public Nullable<decimal> price { get; set; }
    
        public virtual orders orders { get; set; }
    }
}
