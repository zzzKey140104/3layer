using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL
{
    public class SinhVienService
    {
        public bool AddSinhVien(string mssv, string hoten, string malop, DateTime ngaysinh)
        {
            using (var context = new Model1())
            {
                var existingSinhVien = context.Sinhviens.FirstOrDefault(sv => sv.MaSV == mssv);
                if (existingSinhVien != null)
                    return false; // MSSV đã tồn tại

                var sinhvien = new Sinhvien
                {
                    MaSV = mssv,
                    HotenSV = hoten,
                    MaLop = malop,
                    NgaySinh = ngaysinh
                };

                context.Sinhviens.Add(sinhvien);
                context.SaveChanges();
                return true;
            }
        }

        public bool UpdateSinhVien(string mssv, string hoten, string malop, DateTime ngaysinh)
        {
            using (var context = new Model1())
            {
                var sinhvien = context.Sinhviens.FirstOrDefault(sv => sv.MaSV == mssv);
                if (sinhvien == null)
                    return false;

                sinhvien.HotenSV = hoten;
                sinhvien.MaLop = malop;
                sinhvien.NgaySinh = ngaysinh;
                context.SaveChanges();
                return true;
            }
        }

        public bool DeleteSinhVien(string mssv)
        {
            using (var context = new Model1())
            {
                var sinhvien = context.Sinhviens.FirstOrDefault(sv => sv.MaSV == mssv);
                if (sinhvien == null)
                    return false;

                context.Sinhviens.Remove(sinhvien);
                context.SaveChanges();
                return true;
            }
        }

        public IQueryable<Sinhvien> GetAllSinhVien()
        {
            using (var context = new Model1())
            {
                return context.Sinhviens.AsQueryable();
            }
        }
    }
}
