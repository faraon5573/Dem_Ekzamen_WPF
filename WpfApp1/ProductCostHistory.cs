//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductCostHistory
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public System.DateTime ChangeDate { get; set; }
        public decimal CostValue { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
