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

    public partial class Sprint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sprint()
        {
            this.SprintHistories = new HashSet<SprintHistory>();
            this.UserStories = new HashSet<UserStory>();
        }
    
        public int Id { get; set; }
        public Nullable<int> number { get; set; }
        public string topic { get; set; }
        public Nullable<System.DateTime> sDate { get; set; }
        public Nullable<System.DateTime> eDate { get; set; }
        public string state { get; set; }
        public Nullable<int> OriginalSprintNumber { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SprintHistory> SprintHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<UserStory> UserStories { get; set; }
    }
}