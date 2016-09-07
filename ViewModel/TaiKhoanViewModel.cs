using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Paging;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class TaiKhoanViewModel : ViewModelBase
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
        
        public TaiKhoanViewModel()
        {
            PagingCollection = new PagingCollectionView(GetAllTaiKhoan(), itemsNumberPage);
        }

        public ObservableCollection<TaiKhoan> GetAllTaiKhoan()
        {
            var taiKhoans = TruyenTranhDB.Instance.TaiKhoans.ToList();
            ObservableCollection<TaiKhoan> list = new ObservableCollection<TaiKhoan>();
            foreach (var taiKhoan in taiKhoans)
            {
                list.Add(taiKhoan);
            }

            return list;
        }

        public static void AddTaiKhoan(TaiKhoan taiKhoan)
        {
            TruyenTranhDB.Instance.TaiKhoans.Add(taiKhoan);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static TaiKhoan GetTaiKhoanById(int id)
        {
            return TruyenTranhDB.Instance.TaiKhoans.Where(e => e.MaTaiKhoan == id).Select(e => e).SingleOrDefault();
        }

        public static void UpdateTaiKhoan(TaiKhoan taiKhoan)
        {
            TruyenTranhDB.Instance.TaiKhoans.AddOrUpdate(taiKhoan);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static List<TaiKhoan> GetListTaiKhoan()
        {
            return TruyenTranhDB.Instance.TaiKhoans.ToList();
        }

        public static void DeleteTaiKhoan(TaiKhoan taiKhoan)
        {
            TruyenTranhDB.Instance.TaiKhoans.Remove(taiKhoan);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static ObservableCollection<TaiKhoan> SearchTaiKhoan(string query)
        {
            var taiKhoans = TruyenTranhDB.Instance.TaiKhoans.Where(e => e.TenTaiKhoan.ToLower().Contains(query.ToLower())
                                                                     || e.TenNhanVien.ToLower().Contains(query.ToLower()))
                .Select(e => e)
                .ToList();

            ObservableCollection<TaiKhoan> list = new ObservableCollection<TaiKhoan>();

            foreach (var taiKhoan in taiKhoans)
            {
                list.Add(taiKhoan);    
            }

            return list;
        }

        public static TaiKhoan CheckTaiKhoan(string tenTaiKhoan, string matKhau)
        {
            var result =
                TruyenTranhDB.Instance.TaiKhoans.Where(e => e.TenTaiKhoan == tenTaiKhoan && e.MatKhau == matKhau)
                    .Select(e => e)
                    .FirstOrDefault();

            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
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
                PagingCollection = new PagingCollectionView(GetAllTaiKhoan(), itemsNumberPage);
                CheckCommand();
            }
            else
            {
                PagingCollection = new PagingCollectionView(SearchTaiKhoan(keywords), itemsNumberPage);
                CheckCommand();
            }
        }
    }
}
