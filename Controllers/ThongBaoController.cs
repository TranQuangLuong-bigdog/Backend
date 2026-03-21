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
    public class ThongBaoController : Controller
    {
        private readonly ThongTinNoiBoContext _context;

        public ThongBaoController(ThongTinNoiBoContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. GET: ThongBao (Hiển thị danh sách thông báo)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            // Include để lấy tên người đăng (Cán bộ)
            // OrderByDescending để thông báo mới nhất luôn nổi lên đầu tiên
            var thongBaos = _context.ThongBao
                .Include(t => t.MaNguoiDangNavigation)
                .OrderByDescending(t => t.NgayDang);

            return View(await thongBaos.ToListAsync());
        }

        // ==========================================
        // 2. GET: ThongBao/Create (Giao diện thêm mới)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            // Đổ danh sách Cán bộ ra Dropdown để chọn Ai là người đăng bài
            ViewData["MaNguoiDang"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen");
            return View();
        }

        // ==========================================
        // 3. POST: ThongBao/Create (Xử lý lưu dữ liệu)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThongBao,TieuDe,NoiDung,NgayDang,MaNguoiDang")] ThongBao thongBao)
        {
            if (ModelState.IsValid)
            {
                // TỰ ĐỘNG HÓA: Bắt cả trường hợp null và trường hợp C# tự gán năm 0001
                if (thongBao.NgayDang == null || thongBao.NgayDang == DateTime.MinValue)
                {
                    thongBao.NgayDang = DateTime.Now;
                }
                _context.Add(thongBao);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Đăng thông báo mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaNguoiDang"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen", thongBao.MaNguoiDang);
            return View(thongBao);
        }

        // ==========================================
        // 4. GET: ThongBao/Edit/5 (Giao diện sửa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var thongBao = await _context.ThongBao.FindAsync(id);
            if (thongBao == null) return NotFound();

            ViewData["MaNguoiDang"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen", thongBao.MaNguoiDang);
            return View(thongBao);
        }

        // ==========================================
        // 5. POST: ThongBao/Edit/5 (Xử lý lưu khi sửa)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaThongBao,TieuDe,NoiDung,NgayDang,MaNguoiDang")] ThongBao thongBao)
        {
            if (id != thongBao.MaThongBao) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thongBao);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật nội dung thông báo thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThongBaoExists(thongBao.MaThongBao)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaNguoiDang"] = new SelectList(await _context.CanBo.ToListAsync(), "MaCanBo", "HoTen", thongBao.MaNguoiDang);
            return View(thongBao);
        }

        // ==========================================
        // 6. GET: ThongBao/Delete/5 (Giao diện xác nhận xóa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var thongBao = await _context.ThongBao
                .Include(t => t.MaNguoiDangNavigation) // Kéo theo tên người đăng để hiển thị trên bảng xác nhận
                .FirstOrDefaultAsync(m => m.MaThongBao == id);

            if (thongBao == null) return NotFound();

            return View(thongBao);
        }

        // ==========================================
        // 7. POST: ThongBao/Delete/5 (Xử lý xóa)
        // ==========================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thongBao = await _context.ThongBao.FindAsync(id);
            if (thongBao != null)
            {
                _context.ThongBao.Remove(thongBao);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã gỡ thông báo thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ThongBaoExists(int id)
        {
            return _context.ThongBao.Any(e => e.MaThongBao == id);
        }
    }
}
