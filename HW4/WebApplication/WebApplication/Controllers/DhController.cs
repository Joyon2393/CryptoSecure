using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;

namespace WebApplication.Controllers
{
    
    [Authorize]
    
    public class DhController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DhController(ApplicationDbContext context)
        {

            _context = context;
        }
        
        public string GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return claim?.Value ?? "";
        }
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            return View(await _context.Dh.Where(c => c.UserId == userId).ToListAsync());

        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dh = await _context.Dh
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dh == null)
            {
                return NotFound();
            }

            return View(dh);
        }
        // GET: Dh/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: DH/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dh dh)
        {
            ValidateDh(dh);

            if (ModelState.IsValid)
            {
                dh.comA = PowWithMod(Convert.ToUInt64(dh.baseG), Convert.ToUInt64(dh.secretA), Convert.ToUInt64(dh.moduP));
                dh.comB = PowWithMod(Convert.ToUInt64(dh.baseG), Convert.ToUInt64(dh.secretB), Convert.ToUInt64(dh.moduP));
                dh.secretIntA=PowWithMod(Convert.ToUInt64(dh.comB), Convert.ToUInt64(dh.secretA), Convert.ToUInt64(dh.moduP));
                dh.secretIntB=PowWithMod(Convert.ToUInt64(dh.comA), Convert.ToUInt64(dh.secretB), Convert.ToUInt64(dh.moduP));
                _context.Add(dh);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dh);
        }
        // GET: dh/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dh = await _context.Dh.FindAsync(id);
            if (dh == null)
            {
                return NotFound();
            }

            return View(dh);
        }
        // POST: Dh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dh dh)
        {
            if (id != dh.Id)
            {
                return NotFound();
            }

            ValidateDh(dh);
            dh.UserId = GetUserId();
            if (ModelState.IsValid)
            {
                dh.comA = PowWithMod(Convert.ToUInt64(dh.baseG), Convert.ToUInt64(dh.secretA),
                    Convert.ToUInt64(dh.moduP));
                dh.comB = PowWithMod(Convert.ToUInt64(dh.baseG), Convert.ToUInt64(dh.secretB),
                    Convert.ToUInt64(dh.moduP));
                dh.secretIntA = PowWithMod(Convert.ToUInt64(dh.comB), Convert.ToUInt64(dh.secretA),
                    Convert.ToUInt64(dh.moduP));
                dh.secretIntB = PowWithMod(Convert.ToUInt64(dh.comA), Convert.ToUInt64(dh.secretB),
                    Convert.ToUInt64(dh.moduP));
                try
                {
                    _context.Update(dh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DhExists(dh.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(dh);
        }

// GET: Dh/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dh = await _context.Dh
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dh == null)
            {
                return NotFound();
            }

            return View(dh);
        }
        


        public void ValidateDh(Dh dh)
        {
            if (dh.moduP == 0 )
            {
                ModelState.AddModelError(nameof(dh.moduP), "Value can not be null");

            }
            if (dh.baseG == 0 )
            {
                ModelState.AddModelError(nameof(dh.baseG), "Value can not be null");

            }
            if(!CheckingPrime(Convert.ToUInt64(dh.moduP)))
            {
                ModelState.AddModelError(nameof(dh.moduP),"Value have to be prime");
            }
            
            if(!IsPrimitiveRoot(Convert.ToUInt64(dh.baseG),Convert.ToUInt64(dh.moduP)))
            {
                ModelState.AddModelError(nameof(dh.moduP),"Invalid Input");
            }
            
        }
        static bool CheckingPrime(ulong n)
        {
            string v = n.ToString();
            int m, flag = 0;
            m = (int) (n / 2);
            if (IsInt(v))
            {
                for (int i = 2; i <= m; i++)
                {
                    if (n % (ulong) i == 0)
                    {
                        flag = 1;
                        break;
                    }

                }

                if (flag == 0)
                {
                    return true;
                }

                return false;

            }

            return false;

        }

        static Boolean IsInt(string a)
        {
            int result;
            bool parsedSuccessfully = int.TryParse(a, out result);
            if (parsedSuccessfully == false)
            {
                return false;
            }

            return true;
        }

        static bool IsPrimitiveRoot(ulong r, ulong p)
            {
                for (ulong i = 1; i < p; i++)
                {
                    if (i * Math.Log(r) > Math.Log(ulong.MaxValue))
                        throw new System.ArgumentException("Overflow Error.");
                    if ((ulong) Math.Pow(r, i) % p == 1)
                    {
                        if (i == p - 1)
                            return true;
                        
                        return false;
                    }
                }

                return false;



            }

        public ulong PowWithMod(ulong a, ulong b, ulong c)
        {
            ulong x = a % c;
            int k = CountBits(b) - 2;
            /* Left-to-Right binary method */
            while (k >= 0)
            {
                if (2 * Math.Log(x) > Math.Log(ulong.MaxValue))
                    throw new System.ArgumentException("Overflow Error.");
                x = (x * x) % c;
                if ((b >> k & 1) == 1)
                {
                    if (Math.Log(x) + Math.Log(a) > Math.Log(ulong.MaxValue))
                        throw new System.ArgumentException("Overflow Error.");
                    x = (x * a) % c;
                }

                k--;
            }

            return x;


        }
        public static int CountBits(ulong r)
        {
            int k = 0;
            while (r > 0)
            {
                r >>= 1;
                k++;
            }

            return k;
        }


        private bool DhExists(int id)
        {
            return _context.Dh.Any(e => e.Id == id);
        }
    }
}