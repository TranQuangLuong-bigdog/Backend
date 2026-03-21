using KhoaNoiVuCNTT.Data; // Gọi DbContext
using KhoaNoiVuCNTT.Models; // Gọi Model QuanTriVienGiaoVu
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaNoiVuCNTT.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class QuanTriVienController : BaseQuanTriVien
    {
        private readonly ThongTinNoiBoContext _context;

        // Khởi tạo DbContext
        public QuanTriVienController(ThongTinNoiBoContext context)
        {
            _context = context;
        }

        // 1. HIỂN THỊ DANH SÁCH (READ)
        public async Task<IActionResult> Index()
        {
            var danhSach = await _context.QuanTriVienGiaoVu.ToListAsync();
            return View(danhSach);
        }

        // 2. FORM THÊM MỚI (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 3. XỬ LÝ LƯU THÊM MỚI (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuanTriVienGiaoVu qtv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qtv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qtv);
        }

        // 4. FORM CẬP NHẬT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var qtv = await _context.QuanTriVienGiaoVu.FindAsync(id);
            if (qtv == null) return NotFound();

            return View(qtv);
        }

        // 5. XỬ LÝ LƯU CẬP NHẬT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuanTriVienGiaoVu qtv)
        {
            if (id != qtv.MaNhanVien) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(qtv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qtv);
        }

        // 6. XÓA DỮ LIỆU (POST) - Xóa trực tiếp không cần view xác nhận
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var qtv = await _context.QuanTriVienGiaoVu.FindAsync(id);
            if (qtv != null)
            {
                _context.QuanTriVienGiaoVu.Remove(qtv);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}