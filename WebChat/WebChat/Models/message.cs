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
    
    public partial class message
    {
        public string id { get; set; }
        public string cus_send_id { get; set; }
        public string cus_receive_id { get; set; }
        public string message1 { get; set; }
        public System.DateTimeOffset send_time { get; set; }
        public int message_status { get; set; }
    
        public virtual customer customer { get; set; }
        public virtual customer customer1 { get; set; }
    }
}