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
    
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            this.ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }
    
        public int MaDonHang { get; set; }
        public Nullable<int> MaKhachHang { get; set; }
        public Nullable<System.DateTime> NgayDat { get; set; }
        public Nullable<double> TongTien { get; set; }
        public Nullable<int> MaTinhTrang { get; set; }
        public Nullable<int> MaTaiKhoan { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual KhachHang KhachHang { get; set; }
        public virtual TaiKhoan TaiKhoan { get; set; }
        public virtual TinhTrang TinhTrang { get; set; }
    }
}
