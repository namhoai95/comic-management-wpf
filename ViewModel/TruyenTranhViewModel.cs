using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Paging;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class TruyenTranhViewModel : ViewModelBase
    {
        private int itemsNumberPage = 4;

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

        public TruyenTranhViewModel()
        {
            PagingCollection = new PagingCollectionView(GetAllTruyenTranh(), itemsNumberPage);
        }

        public ObservableCollection<TRUYENTRANH> GetAllTruyenTranh()
        {
            var truyenTranhs = TruyenTranhDB.Instance.TRUYENTRANHs.ToList();

            ObservableCollection<TRUYENTRANH> list = new ObservableCollection<TRUYENTRANH>();
            foreach (var truyenTranh in truyenTranhs)
            {
                list.Add(truyenTranh);
            }

            return list;
        }

        public static void AddTruyenTranh(TRUYENTRANH truyenTranh)
        {
            TruyenTranhDB.Instance.TRUYENTRANHs.Add(truyenTranh);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static TRUYENTRANH GetTruyenTranhById(int id)
        {
            return TruyenTranhDB.Instance.TRUYENTRANHs.Where(e => e.MaTruyenTranh == id).Select(e => e).SingleOrDefault();
        }

        public static void UpdateTruyenTranh(TRUYENTRANH truyenTranh)
        {
            TruyenTranhDB.Instance.TRUYENTRANHs.AddOrUpdate(truyenTranh);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static void DeleteTruyenTranh(TRUYENTRANH truyenTranh)
        {
            TruyenTranhDB.Instance.TRUYENTRANHs.Remove(truyenTranh);
            TruyenTranhDB.Instance.SaveChanges();
        }

        private ObservableCollection<TRUYENTRANH> SearchTruyenTranh(string query)
        {
            var truyenTranhs = TruyenTranhDB.Instance.TRUYENTRANHs.Where(e => e.TenTruyenTranh.Contains(query)
                                                                              || e.TACGIA.HoTen.Contains(query)
                                                                              || e.NHAXUATBAN.TenNXB.Contains(query)
                                                                              || e.THELOAI.TenTheLoai.Contains(query))
                                                                              .Select(e => e)
                                                                              .ToList();

            ObservableCollection<TRUYENTRANH> list = new ObservableCollection<TRUYENTRANH>();
            foreach (var truyenTranh in truyenTranhs)
            {
                list.Add(truyenTranh);
            }

            return list;
        }

        public static List<TRUYENTRANH> GetListTruyenTranh()
        {
            return TruyenTranhDB.Instance.TRUYENTRANHs.Where(e => e.BiXoa == false).Select(e => e).ToList();
        }

        private bool CanNext()
        {
            if (PagingCollection.TotalPage == 0)
            {
                return false;
            }
            return PagingCollection.CurrentPage != PagingCollection.TotalPage;
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

        private void Search(string keywords)
        {
            if (String.IsNullOrEmpty(keywords) || keywords == "")
            {
                PagingCollection = new PagingCollectionView(GetAllTruyenTranh(), itemsNumberPage);
                CheckCommand();
            }
            else
            {
                PagingCollection = new PagingCollectionView(SearchTruyenTranh(keywords), itemsNumberPage);
                CheckCommand();
            }
        }

        
    }
}
