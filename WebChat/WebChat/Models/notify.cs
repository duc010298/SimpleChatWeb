//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebChat.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class notify
    {
        public System.Guid notify_id { get; set; }
        public System.Guid cus_id { get; set; }
        public System.DateTimeOffset date_create { get; set; }
        public string notify_content { get; set; }
    
        public virtual customer customer { get; set; }
    }
}
