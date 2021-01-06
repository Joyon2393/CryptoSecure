using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;


namespace WebApplication.Controllers
{
    [Authorize]

    public class RsaController : Controller
    {

        private readonly ApplicationDbContext _context;


        public RsaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return claim?.Value ?? "";
        }


        // GET: Rsa
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            return View(await _context.Rsa.Where(r => r.UserId == userId).ToListAsync());

        }

        // GET: Rsa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rsa = await _context.Rsa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rsa == null)
            {
                return NotFound();
            }

            return View(rsa);
        }

        // GET: Rsa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rsa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rsa rsa)
        {
            ValidateRsa(rsa);
            if (ModelState.IsValid)
            {
                var n = rsa.FirstKey * rsa.SecondKey;
                var m = (rsa.FirstKey - 1) * (rsa.SecondKey - 1);

                ulong e;
                for (e = 2; e < ulong.MaxValue; e++)
                {
                    if (GCD(m, e) == 1) break;
                }

                ulong d = 2;
                while (d < m)
                {
                    if ((d * e) % m == 1)
                    {
                        break;
                    }

                    d++;
                }

                byte[] combinedString = rsa_encrypt(rsa.PlainText, e, n);
                rsa.CypherText = System.Convert.ToBase64String(combinedString);
                rsa.UserId = GetUserId();
                _context.Add(rsa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(rsa);
        }

        // GET: Rsa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rsa = await _context.Rsa.FindAsync(id);
            if (rsa == null)
            {
                return NotFound();
            }

            return View(rsa);
        }



        public void ValidateRsa(Rsa rsa)
        {
            if (rsa.FirstKey == 0 || rsa.SecondKey == 0)
            {
                ModelState.AddModelError(nameof(Rsa.FirstKey), "Value can not be null");

            }

            if (string.IsNullOrWhiteSpace(rsa.PlainText))
            {
                ModelState.AddModelError(nameof(Rsa.PlainText), "Please write something!");
            }

            if (!CheckingPrime(rsa.FirstKey))
            {
                ModelState.AddModelError(nameof(Rsa.FirstKey), "Value have to be prime");
            }

            if (!CheckingPrime(rsa.SecondKey))
            {
                ModelState.AddModelError(nameof(Rsa.SecondKey), "Value have to be prime");
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

        static ulong GCD(ulong a, ulong b)
        {
            ulong Remainder;

            while (b != 0)
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }

        public byte[] rsa_encrypt(string text, ulong e, ulong n)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(text);
            var result = new byte[text.Length];
            int i = 0;
            foreach (byte b in inputBytes)
            {
                ulong encryptedChar = ModExponentiation(b, e, n);
                result[i] = (byte) encryptedChar;
                i++;
            }


            return result;
        }

        static ulong ModExponentiation(ulong x, ulong y, ulong p)
        {
            ulong res = 1;

            x = x % p;

            if (x == 0)
                return 0;

            while (y > 0)
            {
                if ((y & 1) == 1)
                    res = (res * x) % p;

                y = y >> 1;
                x = (x * x) % p;
            }

            return res;
        }

        // POST: Rsa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rsa rsa)
        {
            if (id != rsa.Id)
            {
                return NotFound();
            }

            ValidateRsa(rsa);

            rsa.UserId = GetUserId();

            if (ModelState.IsValid)
            {
                var n = rsa.FirstKey * rsa.SecondKey;
                var m = (rsa.FirstKey - 1) * (rsa.SecondKey - 1);

                ulong e;
                for (e = 2; e < ulong.MaxValue; e++)
                {
                    if (GCD(m, e) == 1) break;
                }

                ulong d = 2;
                while (d < m)
                {
                    if ((d * e) % m == 1)
                    {
                        break;
                    }

                    d++;
                }

                byte[] combinedString = rsa_encrypt(rsa.PlainText, e, n);
                rsa.CypherText = System.Convert.ToBase64String(combinedString);
                try
                {
                    _context.Update(rsa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RsaExists(rsa.Id))
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

            return View(rsa);
        }

        // GET: Rsa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rsa = await _context.Rsa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rsa == null)
            {
                return NotFound();
            }

            return View(rsa);
        }

        // POST: Rsa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // TODO: .where(c => c.Id == id && c.UserId == GetUserId()
            var rsa = await _context.Rsa.FindAsync(id);

            _context.Rsa.Remove(rsa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RsaExists(int id)
        {
            return _context.Rsa.Any(e => e.Id == id);
        }
    }
}