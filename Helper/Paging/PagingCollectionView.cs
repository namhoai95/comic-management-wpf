using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using QuanLyTruyenTranh.ViewModel;

namespace QuanLyTruyenTranh.Helper.Paging
{
    public class PagingCollectionView : CollectionView
    {
        private readonly IList iList;
        private readonly int itemsPerPage;

        private int currentPage = 1;


        public PagingCollectionView(IList iList, int itemsPerPage) : base(iList)
        {
            this.iList = iList;
            this.itemsPerPage = itemsPerPage;
            TotalPage = CalculateTotalPages();
        }

        public override int Count
        {
            get
            {
                if (this.iList.Count == 0)
                {
                    return 0;
                }

                if (this.CurrentPage < this.TotalPage)
                {
                    return this.itemsPerPage;
                }
                else
                {
                    var itemsLeft = this.iList.Count % itemsPerPage;
                    if (0 == itemsLeft)
                    {
                        return this.itemsPerPage;
                    }
                    else
                    {
                        return itemsLeft;
                    }
                }
                return 0;
            }
        }

        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));
            }
        }

        public int ItemsPerPage { get { return this.itemsPerPage; } }

        private int totalPage;
        public int TotalPage
        {
            get { return totalPage; }
            private set
            {
                totalPage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPage"));
            }
        }

        private int CalculateTotalPages()
        {
            return this.iList.Count % this.itemsPerPage == 0
                        ? this.iList.Count / this.itemsPerPage
                        : this.iList.Count / this.itemsPerPage + 1;

        }

        private int EndIndex
        {
            get
            {
                var end = this.currentPage * this.itemsPerPage - 1;
                return (end > this.iList.Count) ? this.iList.Count : end;
            }
        }

        private int StartIndex
        {
            get { return (this.currentPage - 1) * this.itemsPerPage; }
        }

        public override object GetItemAt(int index)
        {
            var offset = index % this.itemsPerPage;
            return this.iList[this.StartIndex + offset];
        }

        
    }
}
