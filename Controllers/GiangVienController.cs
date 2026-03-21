using KhoaNoiVuCNTT.Data;
using KhoaNoiVuCNTT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KhoaNoiVuCNTT.Controllers
{
    public class GiangVienController : Controller
    {
        private readonly ThongTinNoiBoContext _context;

        public GiangVienController(ThongTinNoiBoContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. GET: GiangVien (Hiển thị danh sách Giảng viên)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            // Sắp xếp danh sách theo Tên giảng viên (Bảng chữ cái)
            var danhSachGiangVien = await _context.GiangVien
                .OrderBy(g => g.TenGiangVien)
                .ToListAsync();

            return View(danhSachGiangVien);
        }

        // ==========================================
        // 2. GET: GiangVien/Create (Giao diện thêm mới)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // ==========================================
        // 3. POST: GiangVien/Create (Xử lý lưu dữ liệu)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGiangVien,TenGiangVien,Tuoi,GioiTinh,MonDay")] GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giangVien);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm mới giảng viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(giangVien);
        }

        // ==========================================
        // 4. GET: GiangVien/Edit/5 (Giao diện sửa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var giangVien = await _context.GiangVien.FindAsync(id);
            if (giangVien == null) return NotFound();

            return View(giangVien);
        }

        // ==========================================
        // 5. POST: GiangVien/Edit/5 (Xử lý lưu khi sửa)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGiangVien,TenGiangVien,Tuoi,GioiTinh,MonDay")] GiangVien giangVien)
        {
            if (id != giangVien.MaGiangVien) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giangVien);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thông tin giảng viên thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiangVienExists(giangVien.MaGiangVien)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(giangVien);
        }

        // ==========================================
        // 6. GET: GiangVien/Delete/5 (Giao diện xác nhận xóa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var giangVien = await _context.GiangVien.FirstOrDefaultAsync(m => m.MaGiangVien == id);
            if (giangVien == null) return NotFound();

            return View(giangVien);
        }

        // ==========================================
        // 7. POST: GiangVien/Delete/5 (Xử lý xóa an toàn)
        // ==========================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giangVien = await _context.GiangVien.FindAsync(id);
            if (giangVien != null)
            {
                try
                {
                    _context.GiangVien.Remove(giangVien);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đã xóa thông tin giảng viên thành công!";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra, không thể xóa giảng viên này.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GiangVienExists(int id)
        {
            return _context.GiangVien.Any(e => e.MaGiangVien == id);
        }
    }
}