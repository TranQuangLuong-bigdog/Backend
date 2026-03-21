using KhoaNoiVuCNTT.Data;
using KhoaNoiVuCNTT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KhoaNoiVuCNTT.Controllers
{
    public class VaiTroController : Controller
    {
        private readonly ThongTinNoiBoContext _context;
        public VaiTroController(ThongTinNoiBoContext context)
        {
            _context = context;
        }
        
        // ==========================================
        // 1. GET: VaiTro (Hiển thị danh sách Vai trò)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            // Sắp xếp theo tên Vai trò theo bảng chữ cái
            var danhSachVaiTro = await _context.VaiTro
                .OrderBy(v => v.TenVaiTro)
                .ToListAsync();

            return View(danhSachVaiTro);
        }

        // ==========================================
        // 2. GET: VaiTro/Create (Giao diện thêm mới)
        // ==========================================
        public IActionResult Create()
        {
            return View();
        }

        // ==========================================
        // 3. POST: VaiTro/Create (Xử lý lưu dữ liệu)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaVaiTro,TenVaiTro")] VaiTro vaiTro)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Tên vai trò đã tồn tại chưa để tránh trùng lặp
                bool isExist = await _context.VaiTro.AnyAsync(v => v.TenVaiTro.ToLower() == vaiTro.TenVaiTro.ToLower());
                if (isExist)
                {
                    ModelState.AddModelError("TenVaiTro", "Tên vai trò này đã tồn tại trong hệ thống.");
                    return View(vaiTro);
                }

                _context.Add(vaiTro);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm mới vai trò thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(vaiTro);
        }

        // ==========================================
        // 4. GET: VaiTro/Edit/5 (Giao diện sửa)
        // ==========================================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vaiTro = await _context.VaiTro.FindAsync(id);
            if (vaiTro == null) return NotFound();

            return View(vaiTro);
        }

        // ==========================================
        // 5. POST: VaiTro/Edit/5 (Xử lý lưu khi sửa)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaVaiTro,TenVaiTro")] VaiTro vaiTro)
        {
            if (id != vaiTro.MaVaiTro) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra trùng tên khi sửa (nhưng phải bỏ qua chính nó)
                    bool isExist = await _context.VaiTro.AnyAsync(v => v.TenVaiTro.ToLower() == vaiTro.TenVaiTro.ToLower() && v.MaVaiTro != id);
                    if (isExist)
                    {
                        ModelState.AddModelError("TenVaiTro", "Tên vai trò này đã tồn tại trong hệ thống.");
                        return View(vaiTro);
                    }

                    _context.Update(vaiTro);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật vai trò thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaiTroExists(vaiTro.MaVaiTro)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vaiTro);
        }

        // ==========================================
        // 6. GET: VaiTro/Delete/5 (Giao diện xác nhận xóa)
        // ==========================================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vaiTro = await _context.VaiTro.FirstOrDefaultAsync(m => m.MaVaiTro == id);
            if (vaiTro == null) return NotFound();

            return View(vaiTro);
        }

        // ==========================================
        // 7. POST: VaiTro/Delete/5 (Xử lý xóa an toàn)
        // ==========================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vaiTro = await _context.VaiTro.FindAsync(id);
            if (vaiTro == null) return NotFound();

            try
            {
                _context.VaiTro.Remove(vaiTro);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa vai trò thành công!";
            }
            catch (DbUpdateException)
            {
                // BẮT LỖI KHÓA NGOẠI: Cực kỳ quan trọng để hệ thống không bị crash
                TempData["ErrorMessage"] = $"Không thể xóa vai trò '{vaiTro.TenVaiTro}' vì đang có Cán bộ được phân quyền này. Vui lòng đổi vai trò của các cán bộ đó trước.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VaiTroExists(int id)
        {
            return _context.VaiTro.Any(e => e.MaVaiTro == id);
        }
    }
}