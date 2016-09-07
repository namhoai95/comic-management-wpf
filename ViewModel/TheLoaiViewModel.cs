using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Log;
using QuanLyTruyenTranh.Helper.Paging;
using QuanLyTruyenTranh.Helper.Service;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class TheLoaiViewModel : ViewModelBase
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

        private string tenTheLoai;
        public string TenTheLoai
        {
            get { return tenTheLoai; }
            set
            {
                tenTheLoai = value;
                OnPropertyChanged("TenTheLoai");
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

        private string ghiChu;
        public string GhiChu
        {
            get { return ghiChu; }
            set
            {
                ghiChu = value;
                OnPropertyChanged("GhiChu");

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

        private THELOAI _TheLoaiNew;

        public THELOAI TheLoaiNew
        {
            get { return _TheLoaiNew; }
            set
            {
                _TheLoaiNew = value;
                OnPropertyChanged("TheLoaiNew");
            }
        }

        private THELOAI _TheLoaiSelected;
        public THELOAI TheLoaiSelected
        {
            get { return _TheLoaiSelected; }
            set
            {
                _TheLoaiSelected = value;
                OnPropertyChanged("TheLoaiSelected");
            }
        }

       
        public TheLoaiViewModel()
        {
            PagingCollection = new PagingCollectionView(GetAllTheLoai(), itemsNumberPage);
        }

        public ObservableCollection<THELOAI> GetAllTheLoai()
        {
            var theLoais = TruyenTranhDB.Instance.THELOAIs.ToList();

            ObservableCollection<THELOAI> list = new ObservableCollection<THELOAI>();
            foreach (var theLoai in theLoais)
            {
                list.Add(theLoai);
            }

            return list;
        }

        private void AddTheLoai()
        {
            if (String.IsNullOrEmpty(TheLoaiNew.TenTheLoai))
            {
                MessageError = "Vui lòng nhập tên thể loại!";
            }
            else
            {
                Logger log = new Logger("Màn hình thêm thể loại");
                TheLoaiNew.BiXoa = false;
                TruyenTranhDB.Instance.THELOAIs.Add(TheLoaiNew);
                TruyenTranhDB.Instance.SaveChanges();
                WindowService.CloseFormThemTheLoai();
                log.LogInfo(string.Format("Tài khoản {0} đã thêm thể loại {1}", AccountLogin.AccountLogged.TenTaiKhoan, TheLoaiNew.TenTheLoai));
                MessageSuccess = "Thêm thành công";
                TextColor = "#009688";
                TruyenTranhDB.Release();
                PagingCollection = new PagingCollectionView(GetAllTheLoai(), itemsNumberPage);
                PagingCollection.Refresh();
                CheckCommand();
                CountTime();
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

        public static THELOAI GetTheLoaiById(int id)
        {
            return TruyenTranhDB.Instance.THELOAIs.Where(e => e.MaTheLoai == id).Select(e => e).FirstOrDefault();
        }

        private void UpdateTheLoai()
        {
            if (String.IsNullOrEmpty(TenTheLoai))
            {
                MessageError = "Vui lòng nhập tên thể loại!";
                CapNhatCommand.RaiseCanExecuteChanged();
            }
            else
            {
                Logger log = new Logger("Màn hình cập nhật thể loại");

                TheLoaiSelected.TenTheLoai = TenTheLoai;
                TheLoaiSelected.GhiChu = GhiChu;
                TheLoaiSelected.BiXoa = BiXoa;

                TruyenTranhDB.Instance.THELOAIs.AddOrUpdate(TheLoaiSelected);
                TruyenTranhDB.Instance.SaveChanges();
                WindowService.CloseFormCapNhatTheLoai();

                log.LogInfo(string.Format("Tài khoản {0} đã cập nhật thể loại {1}", AccountLogin.AccountLogged.TenTaiKhoan, TheLoaiSelected.MaTheLoai));

                MessageSuccess = "Cập nhật thành công";
                TextColor = "#009688";
                PagingCollection.Refresh();
                CheckCommand();
                CountTime();
            }
        }

        private void DeleteTheLoai()
        {
            if (TheLoaiSelected == null)
            {
                MessageSuccess = "Vui lòng chọn thể loại để xóa";
                TextColor = "#ff0000";
            }
            else
            {
                if (CheckTheLoaiHaveTruyenTranh())
                {
                    Logger log = new Logger("Màn hình thêm thể loại");

                    string tenTheLoai = TheLoaiSelected.TenTheLoai;

                    TruyenTranhDB.Instance.THELOAIs.Remove(TheLoaiSelected);
                    TruyenTranhDB.Instance.SaveChanges();
                    TruyenTranhDB.Release();

                    log.LogInfo(string.Format("Tài khoản {0} đã xóa thể loại {1}", AccountLogin.AccountLogged.TenTaiKhoan, tenTheLoai));
                    PagingCollection = new PagingCollectionView(GetAllTheLoai(), itemsNumberPage);

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

        private bool CheckTheLoaiHaveTruyenTranh()
        {
            if (TruyenTranhDB.Instance.TRUYENTRANHs.Where(e => e.MaTheLoai == TheLoaiSelected.MaTheLoai).Count() > 0)
            {
                return false;
            }

            return true;
        }

        private ObservableCollection<THELOAI> SearchTheLoai(string query)
        {
            var theLoais = TruyenTranhDB.Instance.THELOAIs.Where(e => e.TenTheLoai.ToLower().Contains(query.ToLower())).Select(e => e).ToList();
            ObservableCollection<THELOAI> list = new ObservableCollection<THELOAI>();

            foreach (var theLoai in theLoais)
            {
                list.Add(theLoai);
            }

            return list;
        }

        public static List<THELOAI> GetListTheLoai()
        {
            return TruyenTranhDB.Instance.THELOAIs.ToList();
        }


        private void LoadFormUpdateTheLoai()
        {
            if (TheLoaiSelected != null)
            {
                TenTheLoai = TheLoaiSelected.TenTheLoai;
                GhiChu = TheLoaiSelected.GhiChu;
                BiXoa = (bool)TheLoaiSelected.BiXoa;
                WindowService.ShowFormCapNhatTheLoai(this);
            }
            else
            {
                MessageSuccess = "Vui lòng chọn thể loại để cập nhật";
                TextColor = "#ff0000";
            }
        }

        private void ShowFormTheLoai()
        {
            TheLoaiNew = new THELOAI();
            WindowService.ShowFormThemTheLoai(this);
        }

        private void CloseFormThem()
        {
            WindowService.CloseFormThemTheLoai();
        }

        private void CloseFormCapNhat()
        {
            WindowService.CloseFormCapNhatTheLoai();
        }

        private bool CanUpdate()
        {
            if (String.IsNullOrEmpty(TenTheLoai) || TenTheLoai == "")
            {
                return false;
            }

            return true;
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

        private void CheckCommand()
        {
            FirstPageCommand.RaiseCanExecuteChanged();
            LastPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
            PreviousPageCommand.RaiseCanExecuteChanged();
        }

        private RelayCommand _MouseDoubleClickCommand = null;
        public RelayCommand MouseDoubleClickCommand
        {
            get
            {
                if (_MouseDoubleClickCommand == null)
                {
                    _MouseDoubleClickCommand = new RelayCommand(LoadFormUpdateTheLoai);
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
                    _CapNhatCommand = new RelayCommand(() => UpdateTheLoai(), () => CanUpdate());
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
                    _CapNhatCommandUC = new RelayCommand(() => LoadFormUpdateTheLoai());
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
                    _ShowFormThemCommand = new RelayCommand(() => ShowFormTheLoai());
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
                    _ThemCommand = new RelayCommand(() => AddTheLoai());
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

        private void Search(string keywords)
        {
            if (String.IsNullOrEmpty(keywords) || keywords == "")
            {
                PagingCollection = new PagingCollectionView(GetAllTheLoai(), itemsNumberPage);
                CheckCommand();
            }
            else
            {
                PagingCollection = new PagingCollectionView(SearchTheLoai(keywords), itemsNumberPage);
                CheckCommand();
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
            if (TheLoaiSelected != null)
            {
                Caption = "Thông báo";
                Message = string.Format("Bạn có chắc chắn xóa muốn xóa tác giả {0}?", TheLoaiSelected.TenTheLoai);
                MsgBoxButton = MessageBoxButton.YesNo;
                MsgBoxImage = MessageBoxImage.Warning;
                OkCommand = new RelayCommand(
                    () =>
                    {
                        DeleteTheLoai();
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
                MessageSuccess = "Vui lòng chọn thể loại để xóa";
                TextColor = "#ff0000";
            }
        }
    }
}
