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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class UserStory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserStory()
        {
            this.AttachmentHistories = new HashSet<AttachmentHistory>();
            this.UserStoryAttachments = new HashSet<UserStoryAttachment>();
            this.UserStoryHistories = new HashSet<UserStoryHistory>();
            this.AspNetUsers = new HashSet<AspNetUser>();
            this.Sprints = new HashSet<Sprint>();
        }
    
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public string state { get; set; }
        public string creator { get; set; }
        public Nullable<int> projectId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<AttachmentHistory> AttachmentHistories { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<UserStoryAttachment> UserStoryAttachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<UserStoryHistory> UserStoryHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<Sprint> Sprints { get; set; }
    }
}
