//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyTruyenTranh.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class THELOAI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public THELOAI()
        {
            this.TRUYENTRANHs = new HashSet<TRUYENTRANH>();
        }
    
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }
        public string GhiChu { get; set; }
        public Nullable<bool> BiXoa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRUYENTRANH> TRUYENTRANHs { get; set; }
    }
}