using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using QuanLyTruyenTranh.Helper.Paging;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class DonHangViewModel : ViewModelBase
    {
        private int itemsNumberPage = 12;
        private PagingCollectionView _PagingCollection;

        public PagingCollectionView PagingCollection
        {
            get { return _PagingCollection; }
            set
            {
                _PagingCollection = value;
                OnPropertyChanged("PagingCollection");
            }
        }

        private string timKiem;
        public string TimKiem
        {
            get { return timKiem; }
            set
            {
                timKiem = value;
                OnPropertyChanged("TimKiem");
                Search(timKiem);
            }
        }


        private DonHang _DonHangSelected;
        public DonHang DonHangSelected
        {
            get { return _DonHangSelected; }
            set
            {
                _DonHangSelected = value;
                OnPropertyChanged("ChiTietDonHang");
            }
        }

        private ObservableCollection<ChiTietDonHang> _ChiTietDonHang;

        public ObservableCollection<ChiTietDonHang> ChiTietDonHang
        {
            get
            {
                if (DonHangSelected == null)
                {
                    return null;
                }
                return this.LoadDetailChiTietDonHang(DonHangSelected);
            }
        }

        public ObservableCollection<ChiTietDonHang> LoadDetailChiTietDonHang(DonHang donHang)
        {
            var list = TruyenTranhDB.Instance.ChiTietDonHangs.Where(e => e.MaDonHang == donHang.MaDonHang).ToList();
            ObservableCollection<ChiTietDonHang> chiTietDonHangs = new ObservableCollection<ChiTietDonHang>();
            foreach (var chiTietDonHang in list)
            {
                chiTietDonHangs.Add(chiTietDonHang);
            }

            return chiTietDonHangs;
        }

        public DonHangViewModel()
        {
            PagingCollection = new PagingCollectionView(GetAllDonHang(), itemsNumberPage);
        }

        public ObservableCollection<DonHang> GetAllDonHang()
        {
            var donHangs = TruyenTranhDB.Instance.DonHangs.ToList();

            ObservableCollection<DonHang> list = new ObservableCollection<DonHang>();

            foreach (var donHang in donHangs)
            {
                list.Add(donHang);
            }

            return list;
        }

        public static void AddDonHang(DonHang donHang)
        {
            TruyenTranhDB.Instance.DonHangs.Add(donHang);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static int GetLastMaDonHang()
        {
            return TruyenTranhDB.Instance.DonHangs.OrderByDescending(e => e.MaDonHang)
                                         .Select(e => e.MaDonHang)
                                         .FirstOrDefault();
        }

        public ObservableCollection<DonHang> SearchDonHang(string keywords)
        {
            var donHangs = TruyenTranhDB.Instance.DonHangs.Where(e => e.KhachHang.TenKhachHang.ToLower().Contains(keywords.ToLower())
                                                                   || e.TaiKhoan.TenNhanVien.ToLower().Contains(keywords.ToLower()))
                                                             .Select(e => e)
                                                             .ToList();
            ObservableCollection<DonHang> list = new ObservableCollection<DonHang>();
            foreach (var khachHang in donHangs)
            {
                list.Add(khachHang);
            }

            return list;
        }

        private bool CanNext()
        {
            if (PagingCollection.TotalPage == 0)
            {
                return false;
            }

            return PagingCollection.CurrentPage != PagingCollection.TotalPage;

        }

        private bool CanPrevious()
        {
            return PagingCollection.CurrentPage != 1;
        }

        private void CheckCommand()
        {
            FirstPageCommand.RaiseCanExecuteChanged();
            LastPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
            PreviousPageCommand.RaiseCanExecuteChanged();
        }

        public void MoveToNextPage()
        {
            PagingCollection.CurrentPage++;
            CheckCommand();
            PagingCollection.Refresh();
        }

        public void MoveToPreviousPage()
        {
            PagingCollection.CurrentPage--;
            CheckCommand();
            PagingCollection.Refresh();
        }

        public void MoveToFirstPage()
        {
            PagingCollection.CurrentPage = 1;
            CheckCommand();
            PagingCollection.Refresh();
        }

        public void MoveToLastPage()
        {
            PagingCollection.CurrentPage = PagingCollection.TotalPage;
            CheckCommand();
            PagingCollection.Refresh();
        }

        private RelayCommand _NextPageCommand = null;
        public RelayCommand NextPageCommand
        {
            get
            {
                if (_NextPageCommand == null)
                {
                    _NextPageCommand = new RelayCommand(() => MoveToNextPage(), () => CanNext());
                }

                return _NextPageCommand;
            }
        }

        private RelayCommand _PreviousPageCommand = null;
        public RelayCommand PreviousPageCommand
        {
            get
            {
                if (_PreviousPageCommand == null)
                {
                    _PreviousPageCommand = new RelayCommand(() => MoveToPreviousPage(), () => CanPrevious());
                }

                return _PreviousPageCommand;
            }
        }

        private RelayCommand _FirstPageCommand = null;
        public RelayCommand FirstPageCommand
        {
            get
            {
                if (_FirstPageCommand == null)
                {
                    _FirstPageCommand = new RelayCommand(() => MoveToFirstPage(), () => CanPrevious());
                }

                return _FirstPageCommand;
            }
        }

        private RelayCommand _LastPageCommand;

        public RelayCommand LastPageCommand
        {
            get
            {
                if (_LastPageCommand == null)
                {
                    _LastPageCommand = new RelayCommand(() => MoveToLastPage(), () => CanNext());
                }

                return _LastPageCommand;
            }
        }

        private void Search(string keywords)
        {
            if (String.IsNullOrEmpty(keywords) || keywords == "")
            {
                PagingCollection = new PagingCollectionView(GetAllDonHang(), itemsNumberPage);
                CheckCommand();
            }
            else
            {
                PagingCollection = new PagingCollectionView(SearchDonHang(keywords), itemsNumberPage);
                CheckCommand();
            }
        }

        private ObservableCollection<DonHang> _DonHang;

        public ObservableCollection<DonHang> DonHang
        {
            get { return _DonHang; }
            set
            {
                _DonHang = value;
                OnPropertyChanged("DonHang");
                DonHang = new ObservableCollection<DonHang>();
            }
        }

        public int ID { get; set; }
        public string TenTruyenTranh { get; set; }
        public int SoLuong { get; set; }
        public double TongTien { get; set; }
        private ObservableCollection<DonHang> _DonHangThongKe;

        public ObservableCollection<DonHang> DonHangThongKe
        {
            get { return _DonHangThongKe; }
            set
            {
                _DonHangThongKe = value;
                OnPropertyChanged("DonHangThongKe");
            }
        }



        public ObservableCollection<DonHang> GetDonHangThongKe(DateTime fromDate, DateTime toDate)
        {
            var donHangs = TruyenTranhDB.Instance.DonHangs.Where(e => e.NgayDat >= fromDate && e.NgayDat <= toDate);
            ObservableCollection<DonHang> list = new ObservableCollection<DonHang>();

            foreach (var donHang in donHangs)
            {
                foreach (var item in donHang.ChiTietDonHangs)
                {
                    TenTruyenTranh = item.TRUYENTRANH.TenTruyenTranh;
                    SoLuong = (int) item.SoLuong;
                    TongTien = (double) (item.SoLuong * item.Gia);
                }
                ID++;
                list.Add(donHang);
            }

            DonHangThongKe = list;
            return DonHangThongKe;
        }
    }
}
