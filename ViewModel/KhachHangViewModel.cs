using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyTruyenTranh.Helper.Paging;
using QuanLyTruyenTranh.Model;
using GalaSoft.MvvmLight.Command;

namespace QuanLyTruyenTranh.ViewModel
{
    public class KhachHangViewModel : ViewModelBase
    {
        private int itemsNumberPage = 10;
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

        public KhachHangViewModel()
        {          
            PagingCollection = new PagingCollectionView(GetAllKhachHang(), itemsNumberPage);
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

        public ObservableCollection<KhachHang> GetAllKhachHang()
        {
            var listKhachHang = TruyenTranhDB.Instance.KhachHangs.ToList();
            ObservableCollection<KhachHang> list = new ObservableCollection<KhachHang>();
            foreach (var khachHang in listKhachHang)
            {           
                list.Add(khachHang);
            }

            return list;
        }

        public static void AddKhachHang(KhachHang khachHang)
        {
            TruyenTranhDB.Instance.KhachHangs.Add(khachHang);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static KhachHang GetKhachHangById(int id)
        {
            return TruyenTranhDB.Instance.KhachHangs.Where(e => e.MaKhachHang == id).Select(e => e).SingleOrDefault();
        }

        public static void UpdateKhachHang(KhachHang khachHang)
        {
            TruyenTranhDB.Instance.KhachHangs.AddOrUpdate(khachHang);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static void DeleteKhachHang(KhachHang khachHang)
        {
            TruyenTranhDB.Instance.KhachHangs.Remove(khachHang);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static ObservableCollection<KhachHang> SearchKhachHang(string tenKhachHang)
        {
            var khachHangs =TruyenTranhDB.Instance.KhachHangs.Where(e => e.TenKhachHang.ToLower().Contains(tenKhachHang.ToLower()))
                                                             .Select(e => e)
                                                             .ToList();
            ObservableCollection<KhachHang> list = new ObservableCollection<KhachHang>();
            foreach (var khachHang in khachHangs)
            {
                list.Add(khachHang);
            }

            return list;
        }

        public static List<KhachHang> GetListKhachHang()
        {
            return TruyenTranhDB.Instance.KhachHangs.ToList();
        }

        public static bool CheckKhachHangHaveDonHang(KhachHang khachHang)
        {
            if (TruyenTranhDB.Instance.DonHangs.Where(e => e.MaKhachHang == khachHang.MaKhachHang).Count() > 0)
            {
                return false;
            }

            return true;
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
                PagingCollection = new PagingCollectionView(GetAllKhachHang(), itemsNumberPage);
                CheckCommand();
            }
            else
            {
                PagingCollection = new PagingCollectionView(SearchKhachHang(keywords), itemsNumberPage);
                CheckCommand();
            }
        }
    }
}
