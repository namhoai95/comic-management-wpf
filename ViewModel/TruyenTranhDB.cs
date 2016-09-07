using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class TruyenTranhDB
    {
        private static QuanLyTruyenTranhEntities instance = null;

        private TruyenTranhDB()
        {
        }

        public static QuanLyTruyenTranhEntities Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QuanLyTruyenTranhEntities();
                }

                return instance;
            }
        }

        public static void Release()
        {
            if (instance != null)
            {
                instance.Dispose();
                instance = new QuanLyTruyenTranhEntities();
            }
        }
    }
}
