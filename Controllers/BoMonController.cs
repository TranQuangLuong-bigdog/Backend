using KhoaNoiVuCNTT.Data;
using KhoaNoiVuCNTT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KhoaNoiVuCNTT.Controllers
{

    public class BoMonController : Controller
    {

        private readonly ThongTinNoiBoContext _context;

        public BoMonController(ThongTinNoiBoContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. GET: BoMon (Hiển thị danh sách bộ môn)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            // Sắp xếp theo tên Bộ môn cho dễ nhìn
            var danhSachBoMon = await _context.BoMon
                .OrderBy(b => b.TenBoMon)
                .ToListAsync();

            return View(danhSachBoMon);
        }

        // ==========================================
        // 2. GET: BoMon/Create (Giao diện thêm mới)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // ==========================================
        // 3. POST: BoMon/Create (Xử lý lưu dữ liệu)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaBoMon,TenBoMon")] BoMon boMon)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên bộ môn đã tồn tại chưa để tránh trùng lặp
                bool isExist = await _context.BoMon.AnyAsync(b => b.TenBoMon.ToLower() == boMon.TenBoMon.ToLower());
                if (isExist)
                {
                    ModelState.AddModelError("TenBoMon", "Tên bộ môn này đã tồn tại trong hệ thống.");
                    return View(boMon);
                }

                _context.Add(boMon);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm mới bộ môn thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(boMon);
        }

        // ==========================================
        // 4. GET: BoMon/Edit/5 (Giao diện sửa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var boMon = await _context.BoMon.FindAsync(id);
            if (boMon == null) return NotFound();

            return View(boMon);
        }

        // ==========================================
        // 5. POST: BoMon/Edit/5 (Xử lý lưu khi sửa)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("MaBoMon,TenBoMon")] BoMon boMon)
        {
            if (id != boMon.MaBoMon) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra trùng tên khi sửa (loại trừ chính nó)
                    bool isExist = await _context.BoMon.AnyAsync(b => b.TenBoMon.ToLower() == boMon.TenBoMon.ToLower() && b.MaBoMon != id);
                    if (isExist)
                    {
                        ModelState.AddModelError("TenBoMon", "Tên bộ môn này đã tồn tại trong hệ thống.");
                        return View(boMon);
                    }

                    _context.Update(boMon);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật tên bộ môn thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoMonExists(boMon.MaBoMon)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(boMon);
        }

        // ==========================================
        // 6. GET: BoMon/Delete/5 (Giao diện xác nhận xóa)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var boMon = await _context.BoMon.FirstOrDefaultAsync(m => m.MaBoMon == id);
            if (boMon == null) return NotFound();

            return View(boMon);
        }

        // ==========================================
        // 7. POST: BoMon/Delete/5 (Xử lý xóa an toàn)
        // ==========================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boMon = await _context.BoMon.FindAsync(id);
            if (boMon == null) return NotFound();

            try
            {
                _context.BoMon.Remove(boMon);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa bộ môn thành công!";
            }
            catch (DbUpdateException)
            {
                // Bắt lỗi khóa ngoại: Không cho xóa nếu đang có cán bộ thuộc bộ môn này
                TempData["ErrorMessage"] = $"Không thể xóa bộ môn '{boMon.TenBoMon}' vì đang có Cán bộ trực thuộc. Vui lòng chuyển Cán bộ sang bộ môn khác trước khi xóa.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BoMonExists(int id)
        {
            return _context.BoMon.Any(e => e.MaBoMon == id);
        }
    }
}