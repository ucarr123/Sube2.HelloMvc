using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Sube2.HelloMvc.Models;
using Sube2.HelloMvc.Models.ViewModels;
using System.Linq;

namespace Sube2.HelloMvc.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly OkulDbContext db;

        public OgrenciController(OkulDbContext context)
        {
            db = context;
        }


        public ViewResult Index()//Action
        {
            return View("AnaSayfa");
        }

        //public ViewResult OgrenciDetay(int id)
        //{
        //    Ogrenci ogrenci = null;
        //    Ogretmen ogrt = null;
        //    if (id == 1)
        //    {
        //        ogrenci = new Ogrenci { Ad = "Ali", Soyad = "Veli", Ogrenciid = 1 };
        //        ogrt = new Ogretmen { Ad = "Hakan", Soyad = "Demir", Ogretmenid = 1 };
        //    }
        //    else if (id == 2)
        //    {
        //        ogrenci = new Ogrenci { Ad = "Ahmet", Soyad = "Mehmet", Ogrenciid = 2 };
        //        ogrt = new Ogretmen { Ad = "Osman", Soyad = "Yılmaz", Ogretmenid = 2 };
        //    }
        //    ViewData["ogr"] = ogrenci;
        //    ViewBag.Student = ogrenci;
        //    ViewBag.Teacher = ogrt;

        //    var dto = new OgrenciDetayDTO { Ogrenci = ogrenci, Ogretmen = ogrt };

        //    return View(dto);
        //}

        public ViewResult OgrenciListe()
        {
            var tumOgrenciler = db.Ogrenciler.ToList();
            return View(tumOgrenciler);
        }
        
        public ViewResult OgrenciListeAjax()
        {
            return View();
        }
        
        public JsonResult OgrenciListeJson(string arananKelime = "")
        {
            var tumOgrenciler = db.Ogrenciler.ToList();
            var sonuc = tumOgrenciler;
            
            if (!string.IsNullOrEmpty(arananKelime))
            {
                sonuc = new List<Ogrenci>();
                
                foreach (var ogr in tumOgrenciler)
                {
                    if (ogr.Ad.Contains(arananKelime) || ogr.Soyad.Contains(arananKelime))
                    {
                        sonuc.Add(ogr);
                    }
                }
            }
            
            return Json(sonuc);
        }
        
        [HttpGet]
        public ViewResult OgrenciEkle()
        {
            return View();
        }

        [HttpPost]
        public ViewResult OgrenciEkle(Ogrenci ogr)
        {
            int saveCount = 0;
            if (ogr != null)
            {
                db.Ogrenciler.Add(ogr);
                saveCount = db.SaveChanges();
            }

            if (saveCount > 0)
            {
                TempData["sonuc"] = true;
            }
            else
            {
                TempData["sonuc"] = false;
            }
            return View();
        }
        
        [HttpGet]
        public ViewResult OgrenciEkleAjax()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult OgrenciEkleAjax(Ogrenci ogr)
        {
            if (ogr != null)
            {
                db.Ogrenciler.Add(ogr);
                db.SaveChanges();
                return Json(new { ok = true });
            }
            
            return Json(new { ok = false });
        }

        [HttpGet]
        public IActionResult OgrenciDetay(int id)
        {
            var bulunanOgr = db.Ogrenciler.Find(id);
            return View(bulunanOgr);
        }

        [HttpPost]
        public IActionResult OgrenciDetay(Ogrenci ogr)
        {
            db.Entry(ogr).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("OgrenciListe");
        }

        public IActionResult OgrenciSil(int id)
        {
            var silinecekOgr = db.Ogrenciler.Find(id);
            db.Ogrenciler.Remove(silinecekOgr);
            db.SaveChanges();
            return RedirectToAction("OgrenciListe");
        }
    }
}

//Controller'dan View'e veri taşıma
//1-ViewData: Key-Value Pair Collection. Key'ler mutlaka tekil olmalıdır.Key'ler string, Value'lar object'dir. Type-Safe değildir.
//2-ViewBag: Arka planda ViewData dictionary'sini kullanır. Bu durumda daha önce ViewData'larda kullanılan key'ler kullanılamaz.
//ViewBag'ler dynamic yapılardır ve içindeki türler Runtime sırasında tespit edilir.

//3-Model
//4-TempData**
