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
    public class LichCongTacController : Controller
    {
        private readonly ThongTinNoiBoContext _context;

        public LichCongTacController(ThongTinNoiBoContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. GET: LichCongTac (Hiển thị danh sách Lịch)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            // Lấy thông tin người tổ chức (Cán bộ) và sắp xếp theo thời gian bắt đầu (mới nhất/sắp tới lên đầu)
            var danhSachLich = _context.LichCongTac
                .Include(l => l.MaNguoiToChucNavigation)
                .OrderByDescending(l => l.ThoiGianBatDau);

            return View(await danhSachLich.ToListAsync());
        }

        // ==========================================
        // 2. GET: LichCongTac/Create (Giao diện thêm mới)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            // Dropdown chọn người tổ chức (Lấy từ bảng Cán bộ)
            ViewData["MaNguoiToChuc"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen");
            return View();
        }

        // ==========================================
        // 3. POST: LichCongTac/Create (Xử lý lưu dữ liệu)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLich,TieuDe,MoTa,ThoiGianBatDau,ThoiGianKetThuc,DiaDiem,MaNguoiToChuc")] LichCongTac lichCongTac)
        {
            // KIỂM TRA LOGIC THỜI GIAN: Kết thúc phải sau Bắt đầu
            if (lichCongTac.ThoiGianBatDau >= lichCongTac.ThoiGianKetThuc)
            {
                ModelState.AddModelError("ThoiGianKetThuc", "Thời gian kết thúc phải diễn ra sau thời gian bắt đầu.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(lichCongTac);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm lịch công tác thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi, trả lại dữ liệu cho Dropdown
            ViewData["MaNguoiToChuc"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen", lichCongTac.MaNguoiToChuc);
            return View(lichCongTac);
        }

        // ==========================================
        // 4. GET: LichCongTac/Edit/5 (Giao diện sửa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lichCongTac = await _context.LichCongTac.FindAsync(id);
            if (lichCongTac == null) return NotFound();

            ViewData["MaNguoiToChuc"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen", lichCongTac.MaNguoiToChuc);
            return View(lichCongTac);
        }

        // ==========================================
        // 5. POST: LichCongTac/Edit/5 (Xử lý lưu khi sửa)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLich,TieuDe,MoTa,ThoiGianBatDau,ThoiGianKetThuc,DiaDiem,MaNguoiToChuc")] LichCongTac lichCongTac)
        {
            if (id != lichCongTac.MaLich) return NotFound();

            // KIỂM TRA LOGIC THỜI GIAN
            if (lichCongTac.ThoiGianBatDau >= lichCongTac.ThoiGianKetThuc)
            {
                ModelState.AddModelError("ThoiGianKetThuc", "Thời gian kết thúc phải diễn ra sau thời gian bắt đầu.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lichCongTac);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật lịch công tác thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LichCongTacExists(lichCongTac.MaLich)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaNguoiToChuc"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen", lichCongTac.MaNguoiToChuc);
            return View(lichCongTac);
        }

        // ==========================================
        // 6. GET: LichCongTac/Delete/5 (Giao diện xác nhận xóa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lichCongTac = await _context.LichCongTac
                .Include(l => l.MaNguoiToChucNavigation) // Lấy tên người tổ chức để hiện ra View
                .FirstOrDefaultAsync(m => m.MaLich == id);

            if (lichCongTac == null) return NotFound();

            return View(lichCongTac);
        }

        // ==========================================
        // 7. POST: LichCongTac/Delete/5 (Xử lý xóa)
        // ==========================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lichCongTac = await _context.LichCongTac.FindAsync(id);
            if (lichCongTac != null)
            {
                try
                {
                    _context.LichCongTac.Remove(lichCongTac);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đã hủy lịch công tác thành công!";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra, không thể xóa lịch công tác này.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LichCongTacExists(int id)
        {
            return _context.LichCongTac.Any(e => e.MaLich == id);
        }
    }
}
