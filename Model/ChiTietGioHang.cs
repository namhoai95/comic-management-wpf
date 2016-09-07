namespace QuanLyTruyenTranh.Model
{
    public class ChiTietGioHang
    {
        public TRUYENTRANH TruyenTranh { get; set; }
        public int MaDonHang { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }

        public ChiTietGioHang()
        {
            TruyenTranh = new TRUYENTRANH();
        }
    }
}
