using KhoaNoiVuCNTT.Data;
using KhoaNoiVuCNTT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KhoaNoiVuCNTT.Controllers
{
    public class CanBoController : Controller
    {
        private readonly ThongTinNoiBoContext _context;

        public CanBoController(ThongTinNoiBoContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. GET: CanBo (Hiển thị danh sách)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            var canBos = _context.CanBo
                .Include(c => c.MaBoMonNavigation)
                .Include(c => c.MaVaiTroNavigation);

            // Truyền dữ liệu cho bộ lọc Dropdown ở View Index
            ViewData["MaBoMon"] = new SelectList(await _context.BoMon.ToListAsync(), "MaBoMon", "TenBoMon");
            ViewData["MaVaiTro"] = new SelectList(await _context.VaiTro.ToListAsync(), "MaVaiTro", "TenVaiTro");

            return View(await canBos.ToListAsync());
        }

        // ==========================================
        // 2. GET: CanBo/Create (Giao diện thêm mới)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["MaBoMon"] = new SelectList(await _context.BoMon.ToListAsync(), "MaBoMon", "TenBoMon");
            ViewData["MaVaiTro"] = new SelectList(await _context.VaiTro.ToListAsync(), "MaVaiTro", "TenVaiTro");
            return View();
        }

        // ==========================================
        // 3. POST: CanBo/Create (Xử lý lưu dữ liệu thêm mới)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCanBo,HoTen,Email,MatKhau,MaBoMon,MaVaiTro")] CanBo canBo)
        {
            if (ModelState.IsValid)
            {
                // Xử lý an toàn: Nếu không nhập mật khẩu thì gán mặc định là 123456
                if (string.IsNullOrEmpty(canBo.MatKhau))
                {
                    canBo.MatKhau = "123456";
                }

                _context.Add(canBo);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm mới cán bộ thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi nhập liệu, load lại dữ liệu cho Dropdown để trả về View
            ViewData["MaBoMon"] = new SelectList(await _context.BoMon.ToListAsync(), "MaBoMon", "TenBoMon", canBo.MaBoMon);
            ViewData["MaVaiTro"] = new SelectList(await _context.VaiTro.ToListAsync(), "MaVaiTro", "TenVaiTro", canBo.MaVaiTro);
            return View(canBo);
        }
        // ==========================================
        // 4. GET: CanBo/Edit/5 (Giao diện sửa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var canBo = await _context.CanBo.FindAsync(id);
            if (canBo == null) return NotFound();

            ViewData["MaBoMon"] = new SelectList(await _context.BoMon.ToListAsync(), "MaBoMon", "TenBoMon", canBo.MaBoMon);
            ViewData["MaVaiTro"] = new SelectList(await _context.VaiTro.ToListAsync(), "MaVaiTro", "TenVaiTro", canBo.MaVaiTro);
            return View(canBo);
        }

        // ==========================================
        // 5. POST: CanBo/Edit/5 (Xử lý lưu dữ liệu khi sửa)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCanBo,HoTen,Email,MatKhau,MaBoMon,MaVaiTro")] CanBo canBo)
        {
            if (id != canBo.MaCanBo) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(canBo);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CanBoExists(canBo.MaCanBo)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaBoMon"] = new SelectList(await _context.BoMon.ToListAsync(), "MaBoMon", "TenBoMon", canBo.MaBoMon);
            ViewData["MaVaiTro"] = new SelectList(await _context.VaiTro.ToListAsync(), "MaVaiTro", "TenVaiTro", canBo.MaVaiTro);
            return View(canBo);
        }

        // ==========================================
        // 6. GET: CanBo/Delete/5 (Giao diện xác nhận xóa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var canBo = await _context.CanBo
                .Include(c => c.MaBoMonNavigation)
                .Include(c => c.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaCanBo == id);

            if (canBo == null) return NotFound();

            return View(canBo);
        }

        // ==========================================
        // 7. POST: CanBo/Delete/5 (Xử lý xóa an toàn)
        // ==========================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var canBo = await _context.CanBo.FindAsync(id);
            if (canBo == null) return NotFound();

            try
            {
                _context.CanBo.Remove(canBo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa cán bộ khỏi hệ thống.";
            }
            catch (DbUpdateException)
            {
                // BẮT LỖI KHÓA NGOẠI TẠI ĐÂY: Tránh lỗi sập trang (Exception User-Unhandled)
                TempData["ErrorMessage"] = $"Không thể xóa cán bộ '{canBo.HoTen}' vì dữ liệu của họ đang được sử dụng ở bảng Thông báo, Tài liệu hoặc Lịch công tác.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Hàm kiểm tra tồn tại
        private bool CanBoExists(int id)
        {
            return _context.CanBo.Any(e => e.MaCanBo == id);
        }
    }
}