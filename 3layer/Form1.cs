using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.Models;
using System.Data.Entity;
using BLL;

namespace _3layer
{
    public partial class Form1 : Form
    {
        private SinhVienService sinhVienService;

        public Form1()
        {
            InitializeComponent();
            sinhVienService = new SinhVienService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadLop_customDTP();
            LoadSinhVienData();
            dgvSinhVien.SelectionChanged += dgvSinhVien_SelectionChanged; // Đăng ký sự kiện

        }

        private void LoadLop_customDTP()
        {
            using (var context = new Model1())
            {
                var lops = context.Lops.ToList();
                cmbLop.DataSource = lops;
                cmbLop.DisplayMember = "TenLop";  // Hiển thị tên lớp
                cmbLop.ValueMember = "MaLop";     // Lưu giá trị là mã lớp
            }

            // Cài đặt chỉ hiển thị ngày
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            dtpNgaySinh.ShowUpDown = true; // Loại bỏ đồng hồ thời gian
        }
        private void LoadSinhVienData()
        {
            using (var context = new Model1())
            {
                // Kết hợp dữ liệu Sinhvien với Lop qua MaLop
                var sinhvienData = context.Sinhviens
                                           .Join(context.Lops, sv => sv.MaLop, lop => lop.MaLop,
                                                 (sv, lop) => new
                                                 {
                                                     MSSV = sv.MaSV,
                                                     HoTen = sv.HotenSV,
                                                     NgaySinh = sv.NgaySinh,
                                                     MaLop = lop.MaLop
                                                 })
                                           .ToList();

                // Chuyển đổi dữ liệu thành DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("MSSV");
                dt.Columns.Add("HoTen");
                dt.Columns.Add("NgaySinh");
                dt.Columns.Add("MaLop");

                foreach (var item in sinhvienData)
                {
                    DataRow row = dt.NewRow();
                    row["MSSV"] = item.MSSV;
                    row["HoTen"] = item.HoTen;
                    row["NgaySinh"] = item.NgaySinh;
                    row["MaLop"] = item.MaLop;
                    dt.Rows.Add(row);
                }

                // Gán DataTable vào DataGridView
                dgvSinhVien.DataSource = dt;
            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            string mssv = txtMaSV.Text.Trim();
            string hoten = txtTenSV.Text;
            string malop = cmbLop.SelectedValue.ToString();
            DateTime ngaysinh = dtpNgaySinh.Value;

            bool result = sinhVienService.AddSinhVien(mssv, hoten, malop, ngaysinh);
            if (result)
            {
                MessageBox.Show("Thêm sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSinhVienData();
            }
            else
            {
                MessageBox.Show("MSSV đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string mssv = txtMaSV.Text.Trim();
            string hoten = txtTenSV.Text;
            string malop = cmbLop.SelectedValue.ToString();
            DateTime ngaysinh = dtpNgaySinh.Value;

            bool result = sinhVienService.UpdateSinhVien(mssv, hoten, malop, ngaysinh);
            if (result)
            {
                MessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSinhVienData();
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string mssv = txtMaSV.Text.Trim();

            bool result = sinhVienService.DeleteSinhVien(mssv);
            if (result)
            {
                MessageBox.Show("Xóa sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSinhVienData();
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
           
        }

        private void btnKLuu_Click(object sender, EventArgs e)
        {
           
        }

        private void dgvSinhVien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count > 0)
            {
                // Lấy dòng được chọn
                DataGridViewRow selectedRow = dgvSinhVien.SelectedRows[0];

                // Cập nhật giá trị vào các TextBox
                txtMaSV.Text = selectedRow.Cells["MSSV"].Value.ToString();
                txtTenSV.Text = selectedRow.Cells["HoTen"].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(selectedRow.Cells["NgaySinh"].Value);

                // Gán giá trị MaLop từ DataGridView vào ComboBox
                cmbLop.SelectedValue = selectedRow.Cells["MaLop"].Value.ToString();
            }
        }
    }
}
