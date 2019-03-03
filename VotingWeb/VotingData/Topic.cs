//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VotingData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Topic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Topic()
        {
            this.Shortcuts = new HashSet<Shortcut>();
            this.Votings = new HashSet<Voting>();
        }
    
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Order { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public Nullable<int> Total { get; set; }
        public bool IsProcedural { get; set; }
        public bool IsSecret { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Session Session { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shortcut> Shortcuts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
