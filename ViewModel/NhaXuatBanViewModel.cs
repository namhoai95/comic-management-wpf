using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using QuanLyTruyenTranh.Helper.Paging;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class NhaXuatBanViewModel : ViewModelBase
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

        public NhaXuatBanViewModel()
        {
            PagingCollection = new PagingCollectionView(GetAllNhaXuatBan(), itemsNumberPage);
        }

        public ObservableCollection<NHAXUATBAN> GetAllNhaXuatBan()
        {
            var nhaXuatBans = TruyenTranhDB.Instance.NHAXUATBANs.ToList();
            ObservableCollection<NHAXUATBAN> list = new ObservableCollection<NHAXUATBAN>();
            foreach (var nxb in nhaXuatBans)
            {
                list.Add(nxb);
            }

            return list;
        }

        public static void AddNhaXuatBan(NHAXUATBAN nhaXuatBan)
        {
            TruyenTranhDB.Instance.NHAXUATBANs.Add(nhaXuatBan);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static NHAXUATBAN GetNhaXuatBanById(int id)
        {
            return TruyenTranhDB.Instance.NHAXUATBANs.Where(i => i.MaNXB == id).Select(i => i).SingleOrDefault();
        }

        public static void UpdateNhaXuatBan(NHAXUATBAN nhaXuatBan)
        {
            TruyenTranhDB.Instance.NHAXUATBANs.AddOrUpdate(nhaXuatBan);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static void DeleteNhaXuatBan(NHAXUATBAN nhaXuatBan)
        {
            TruyenTranhDB.Instance.NHAXUATBANs.Remove(nhaXuatBan);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public static ObservableCollection<NHAXUATBAN> SearchNhaXuatBan(string query)
        {
            var nhaXuatBans = TruyenTranhDB.Instance.NHAXUATBANs.Where(i => i.TenNXB.ToLower().Contains(query.ToLower())).Select(i => i).ToList();
            ObservableCollection<NHAXUATBAN> list = new ObservableCollection<NHAXUATBAN>();
            foreach (var nxb in nhaXuatBans)
            {
                list.Add(nxb);
            }

            return list;
        }

        public static List<NHAXUATBAN> GetListNhaXuatBan()
        {
            return TruyenTranhDB.Instance.NHAXUATBANs.ToList();
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
                PagingCollection = new PagingCollectionView(GetAllNhaXuatBan(), itemsNumberPage);
                CheckCommand();
            }
            else
            {
                PagingCollection = new PagingCollectionView(SearchNhaXuatBan(keywords), itemsNumberPage);
                CheckCommand();
            }
        }
    }
}
