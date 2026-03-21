using KhoaNoiVuCNTT.Data;
using KhoaNoiVuCNTT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KhoaNoiVuCNTT.Controllers
{
    public class TaiLieuController : Controller
    {
        private readonly ThongTinNoiBoContext _context;

        public TaiLieuController(ThongTinNoiBoContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. GET: TaiLieu (Hiển thị danh sách tài liệu)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            // Include để lấy tên Cán bộ đã tải tài liệu lên
            // OrderByDescending để tài liệu mới nhất hiện lên đầu
            var taiLieus = _context.TaiLieu
                .Include(t => t.MaNguoiTaiLenNavigation)
                .OrderByDescending(t => t.NgayTaiLen);

            return View(await taiLieus.ToListAsync());
        }

        // ==========================================
        // 2. GET: TaiLieu/Create (Giao diện thêm mới)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            // Đổ danh sách Cán bộ ra Dropdown để chọn người tải lên
            ViewData["MaNguoiTaiLen"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen");
            return View();
        }

        // ==========================================
        // 3. POST: TaiLieu/Create (Xử lý lưu dữ liệu)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTaiLieu,TenTaiLieu,DuongDanFile,MaNguoiTaiLen,NgayTaiLen,PhanLoai")] TaiLieu taiLieu)
        {
            if (ModelState.IsValid)
            {
                // TỰ ĐỘNG HÓA & SỬA LỖI SQL DATETIME: 
                // Bắt cả trường hợp null và trường hợp hệ thống tự gán năm 0001
                if (taiLieu.NgayTaiLen == null || taiLieu.NgayTaiLen == DateTime.MinValue)
                {
                    taiLieu.NgayTaiLen = DateTime.Now;
                }

                _context.Add(taiLieu);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm mới tài liệu thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaNguoiTaiLen"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen", taiLieu.MaNguoiTaiLen);
            return View(taiLieu);
        }

        // ==========================================
        // 4. GET: TaiLieu/Edit/5 (Giao diện sửa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var taiLieu = await _context.TaiLieu.FindAsync(id);
            if (taiLieu == null) return NotFound();

            ViewData["MaNguoiTaiLen"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen", taiLieu.MaNguoiTaiLen);
            return View(taiLieu);
        }

        // ==========================================
        // 5. POST: TaiLieu/Edit/5 (Xử lý lưu khi sửa)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTaiLieu,TenTaiLieu,DuongDanFile,MaNguoiTaiLen,NgayTaiLen,PhanLoai")] TaiLieu taiLieu)
        {
            if (id != taiLieu.MaTaiLieu) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Đảm bảo không bị lỗi ngày tháng khi update
                    if (taiLieu.NgayTaiLen == null || taiLieu.NgayTaiLen == DateTime.MinValue)
                    {
                        taiLieu.NgayTaiLen = DateTime.Now;
                    }

                    _context.Update(taiLieu);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thông tin tài liệu thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiLieuExists(taiLieu.MaTaiLieu)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaNguoiTaiLen"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen", taiLieu.MaNguoiTaiLen);
            return View(taiLieu);
        }

        // ==========================================
        // 6. GET: TaiLieu/Delete/5 (Giao diện xác nhận xóa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var taiLieu = await _context.TaiLieu
                .Include(t => t.MaNguoiTaiLenNavigation) // Lấy thêm tên người đăng để hiện ra lúc hỏi xóa
                .FirstOrDefaultAsync(m => m.MaTaiLieu == id);

            if (taiLieu == null) return NotFound();

            return View(taiLieu);
        }

        // ==========================================
        // 7. POST: TaiLieu/Delete/5 (Xử lý xóa)
        // ==========================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taiLieu = await _context.TaiLieu.FindAsync(id);
            if (taiLieu != null)
            {
                _context.TaiLieu.Remove(taiLieu);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa tài liệu khỏi hệ thống!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TaiLieuExists(int id)
        {
            return _context.TaiLieu.Any(e => e.MaTaiLieu == id);
        }
    }
}