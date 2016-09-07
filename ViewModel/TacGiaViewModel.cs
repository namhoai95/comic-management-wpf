using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Log;
using QuanLyTruyenTranh.Helper.Paging;
using QuanLyTruyenTranh.Helper.Service;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class TacGiaViewModel : ViewModelBase
    {
        private PagingCollectionView _PagingCollection;
        private int itemsNumberPage = 10;
        public PagingCollectionView PagingCollection
        {
            get { return _PagingCollection; }
            set
            {
                _PagingCollection = value;
                OnPropertyChanged("PagingCollection");
            }
        }

        private string hoTen;
        public string HoTen
        {
            get { return hoTen; }
            set
            {
                hoTen = value;
                OnPropertyChanged("HoTen");
                CapNhatCommand.RaiseCanExecuteChanged();
            }
        }

        private bool biXoa;
        public bool BiXoa
        {
            get { return biXoa; }
            set
            {
                biXoa = value;
                OnPropertyChanged("BiXoa");

            }
        }

        private string moTa;
        public string MoTa
        {
            get { return moTa; }
            set
            {
                moTa = value;
                OnPropertyChanged("MoTa");

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

        private string caption;
        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                OnPropertyChanged("Caption");
            }
        }
        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        private MessageBoxButton msgBoxButton;

        public MessageBoxButton MsgBoxButton
        {
            get { return msgBoxButton; }
            set
            {
                msgBoxButton = value;
                OnPropertyChanged("MsgBoxButton");
            }
        }
        private MessageBoxImage msgBoxImage;

        public MessageBoxImage MsgBoxImage
        {
            get { return msgBoxImage; }
            set
            {
                msgBoxImage = value;
                OnPropertyChanged("MsgBoxImage");
            }
        }

        public RelayCommand OkCommand { get; set; }

        private bool? showConfirmation;

        public bool? ShowConfirmation
        {
            get { return showConfirmation; }
            set
            {
                showConfirmation = value;
                OnPropertyChanged("ShowConfirmation");
            }
        }

        private RelayCommand _confirmCommand;
        public RelayCommand ConfirmCommand
        {
            get
            {
                return _confirmCommand
                  ?? (_confirmCommand = new RelayCommand(
                       () =>
                       {
                           ShowConfirm();
                       }));
            }
        }

        private void ShowConfirm()
        {
            if (TacGiaSelected != null)
            {
                Caption = "Thông báo";
                Message = string.Format("Bạn có chắc chắn xóa muốn xóa tác giả {0}?", TacGiaSelected.HoTen);
                MsgBoxButton = MessageBoxButton.YesNo;
                MsgBoxImage = MessageBoxImage.Warning;
                OkCommand = new RelayCommand(
                    () =>
                    {
                        DeleteTacGia();
                        UserNotificationMessage msg = new UserNotificationMessage
                        {
                            Message = "OK.\rDeleted it.\rYour data is consigned to oblivion.",
                            SecondsToShow = 5
                        };
                        Messenger.Default.Send<UserNotificationMessage>(msg);
                    });
                OnPropertyChanged("OkCommand");
                showConfirmation = true;
                OnPropertyChanged("ShowConfirmation");
            }
            else
            {
                MessageSuccess = "Vui lòng chọn tác giả để xóa";
                TextColor = "#ff0000";
            }
        }

        private TACGIA _TacGiaNew;

        public TACGIA TacGiaNew
        {
            get { return _TacGiaNew; }
            set
            {
                _TacGiaNew = value;
                OnPropertyChanged("TacGiaNew");
            }
        }

        private TACGIA _TacGiaSelected;
        public TACGIA TacGiaSelected
        {
            get { return _TacGiaSelected; }
            set
            {
                _TacGiaSelected = value;
                OnPropertyChanged("TacGiaSelected");
            }
        }

        private string messageSuccess;
        public string MessageSuccess
        {
            get { return messageSuccess; }
            set
            {
                messageSuccess = value;
                OnPropertyChanged("MessageSuccess");
            }
        }

        private string textColor;
        public string TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                OnPropertyChanged("TextColor");
            }
        }

        private string messageError;
        public string MessageError
        {
            get { return messageError; }
            set
            {
                messageError = value;
                OnPropertyChanged("MessageError");
            }
        }

        private RelayCommand _MouseDoubleClickCommand = null;
        public RelayCommand MouseDoubleClickCommand
        {
            get
            {
                if (_MouseDoubleClickCommand == null)
                {
                    _MouseDoubleClickCommand = new RelayCommand(LoadFormUpdateTacGia);
                }

                return _MouseDoubleClickCommand;
            }
        }

        private RelayCommand _CapNhatCommand = null;

        public RelayCommand CapNhatCommand
        {
            get
            {
                if (_CapNhatCommand == null)
                {
                    _CapNhatCommand = new RelayCommand(() => UpdateTacGia(), () => CanUpdate());
                }

                return _CapNhatCommand;
            }
        }

        private RelayCommand _CapNhatCommandUC = null;

        public RelayCommand CapNhatCommandUC
        {
            get
            {
                if (_CapNhatCommandUC == null)
                {
                    _CapNhatCommandUC = new RelayCommand(() => LoadFormUpdateTacGia());
                }

                return _CapNhatCommandUC;
            }
        }

        private RelayCommand _ShowFormThemCommand = null;

        public RelayCommand ShowFormThemCommand
        {
            get
            {
                if (_ShowFormThemCommand == null)
                {
                    _ShowFormThemCommand = new RelayCommand(() => ShowFormTacGia());
                }

                return _ShowFormThemCommand;
            }
        }

        private RelayCommand _ThemCommand = null;
        public RelayCommand ThemCommand
        {
            get
            {
                if (_ThemCommand == null)
                {
                    _ThemCommand = new RelayCommand(() => AddTacGia());
                }

                return _ThemCommand;
            }
        }

        private RelayCommand _CloseFormThemCommand = null;
        private RelayCommand _CloseFormCapNhatCommand = null;
        public RelayCommand CloseFormThemCommand
        {
            get
            {
                if (_CloseFormThemCommand == null)
                {
                    _CloseFormThemCommand = new RelayCommand(() => CloseFormThem());
                }

                return _CloseFormThemCommand;
            }
        }

        public RelayCommand CloseFormCapNhatCommand
        {
            get
            {
                if (_CloseFormCapNhatCommand == null)
                {
                    _CloseFormCapNhatCommand = new RelayCommand(() => CloseFormCapNhat());
                }
                return _CloseFormCapNhatCommand;
            }
        }

        public TacGiaViewModel()
        {
            PagingCollection = new PagingCollectionView(GetAllTacGia(), itemsNumberPage);
        }

        public ObservableCollection<TACGIA> GetAllTacGia()
        {
            var tgs = TruyenTranhDB.Instance.TACGIAs.ToList();
            ObservableCollection<TACGIA> list = new ObservableCollection<TACGIA>();
            foreach (var tg in tgs)
            {
                list.Add(tg);
            }

            return list;
        }

        public static List<TACGIA> GetListTacGia()
        {
            return TruyenTranhDB.Instance.TACGIAs.ToList();
        }

        public static TACGIA GetTacGiaById(int id)
        {
            return TruyenTranhDB.Instance.TACGIAs.Where(i => i.MaTG == id).Select(i => i).SingleOrDefault();
        }

        private void AddTacGia()
        {
            if (String.IsNullOrEmpty(TacGiaNew.HoTen))
            {
                MessageError = "Vui lòng nhập họ tên tác giả !";
                TextColor = "#ff0000";
            }
            else
            {
                Logger logger = new Logger("Màn hình thêm tác giả");

                TacGiaNew.BiXoa = false;

                TruyenTranhDB.Instance.TACGIAs.Add(TacGiaNew);
                TruyenTranhDB.Instance.SaveChanges();

                WindowService.CloseFormThemTacGia();
                logger.LogInfo(string.Format("Tài khoản {0} đã thêm tác giả {1}", AccountLogin.AccountLogged.TenTaiKhoan, TacGiaNew.HoTen));

                MessageSuccess = "Thêm thành công";
                TextColor = "#009688";
                TruyenTranhDB.Release();
                PagingCollection = new PagingCollectionView(GetAllTacGia(), itemsNumberPage);
                PagingCollection.Refresh();
                CheckCommand();
                CountTime();
            }

        }

        private void UpdateTacGia()
        {
            if (String.IsNullOrEmpty(HoTen))
            {
                MessageError = "Vui lòng nhập tên tác giả !";
            }
            else
            {
                Logger logger = new Logger("Màn hình cập nhật tác giả");
                TacGiaSelected.HoTen = HoTen;
                TacGiaSelected.MoTa = MoTa;
                TacGiaSelected.BiXoa = BiXoa;
                TruyenTranhDB.Instance.TACGIAs.AddOrUpdate(TacGiaSelected);
                TruyenTranhDB.Instance.SaveChanges();
                WindowService.CloseFormCapNhatTacGia();

                logger.LogInfo(string.Format("Tài khoản {0} đã cập nhật tác giả {1}", AccountLogin.AccountLogged.TenTaiKhoan, TacGiaSelected.MaTG));

                MessageSuccess = "Cập nhật thành công";
                TextColor = "#009688";
                PagingCollection.Refresh();
                CheckCommand();
                CountTime();
            }

        }

        private void DeleteTacGia()
        {
            if (TacGiaSelected == null)
            {
                MessageSuccess = "Vui lòng chọn tác giả để xóa";
                TextColor = "#ff0000";
            }
            else
            {
                if (CheckTacGiaHaveTruyenTranh())
                {
                    Logger logger = new Logger("Màn hình danh sách tác giả");
                    string tenTacGia = TacGiaSelected.HoTen;
                    TruyenTranhDB.Instance.TACGIAs.Remove(TacGiaSelected);
                    TruyenTranhDB.Instance.SaveChanges();
                    TruyenTranhDB.Release();
                    logger.LogInfo(string.Format("Tài khoản {0} đã xóa tác giả {1}", AccountLogin.AccountLogged.TenTaiKhoan, tenTacGia));
                    PagingCollection = new PagingCollectionView(GetAllTacGia(), itemsNumberPage);
                    MessageSuccess = "Xóa thành công";
                    TextColor = "#009688";
                    PagingCollection.Refresh();
                    CheckCommand();
                    CountTime();
                }
                else
                {
                    MessageSuccess = "Không thể xóa tác giả";
                    TextColor = "#ff0000";
                }
            }
        }

        private void CountTime()
        {
            var t = new Timer();
            t.Interval = 2500;
            t.Tick += (s, e) =>
            {
                MessageSuccess = "";
                t.Stop();
            };
            t.Start();
        }

        private ObservableCollection<TACGIA> SearchTacGia(string query)
        {
            var tacGias =
                TruyenTranhDB.Instance.TACGIAs.Where(i => i.HoTen.ToLower().Contains(query.ToLower()))
                    .Select(i => i)
                    .ToList();

            ObservableCollection<TACGIA> list = new ObservableCollection<TACGIA>();
            foreach (var tacGia in tacGias)
            {
                list.Add(tacGia);
            }

            return list;
        }
        private bool CanUpdate()
        {
            if (String.IsNullOrEmpty(HoTen) || HoTen == "")
            {
                MessageError = "Tên tác giả không được rỗng";
                return false;
            }

            return true;
        }

        private void LoadFormUpdateTacGia()
        {
            if (TacGiaSelected != null)
            {
                HoTen = TacGiaSelected.HoTen;
                MoTa = TacGiaSelected.MoTa;
                BiXoa = (bool)TacGiaSelected.BiXoa;
                WindowService.ShowFormCapNhatTacGia(this);
            }
            else
            {
                MessageSuccess = "Vui lòng chọn tác giả để cập nhật";
                TextColor = "#ff0000";
            }
        }

        private void ShowFormTacGia()
        {
            TacGiaNew = new TACGIA();
            WindowService.ShowFormThemTacGia(this);
        }

        private void CloseFormThem()
        {
            WindowService.CloseFormThemTacGia();
        }

        private void CloseFormCapNhat()
        {
            WindowService.CloseFormCapNhatTacGia();
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
                PagingCollection = new PagingCollectionView(GetAllTacGia(), itemsNumberPage);
                CheckCommand();
            }
            else
            {
                PagingCollection = new PagingCollectionView(SearchTacGia(keywords), itemsNumberPage);
                CheckCommand();
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

        private bool CheckTacGiaHaveTruyenTranh()
        {
            if (TruyenTranhDB.Instance.TRUYENTRANHs.Where(e => e.MaTG == TacGiaSelected.MaTG).Count() > 0)
            {
                return false;
            }

            return true;
        }
    }
}
