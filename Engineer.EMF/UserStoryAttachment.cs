//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Engineer.EMF
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserStoryAttachment
    {
        public int userStoryId { get; set; }
        public int attachId { get; set; }
        public string activties { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public string update_by { get; set; }
        public string state { get; set; }
        public Nullable<bool> @readonly { get; set; }
        public string SVG { get; set; }
    
        public virtual Attachment Attachment { get; set; }
        public virtual UserStory UserStory { get; set; }
    }
}
