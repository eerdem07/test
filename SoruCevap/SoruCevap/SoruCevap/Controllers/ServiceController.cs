using SoruCevap.Models;
using SoruCevap.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SoruCevap.Controllers
{
    public class ServiceController : ApiController
    {
        Database1Entities db = new Database1Entities();
        SonucModel sonuc = new SonucModel();

        #region Uye

        [HttpGet]
        [Route("api/uye-liste")]
        public List<UyeModel> uyeListele()
        {
            List<UyeModel> liste = db.Uye.Select(x=> new UyeModel
            {
                userId = x.userId,
                ad =x.ad,
                soyad = x.soyad,
                email =x.email,
                rol=x.rol,

            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/uye-by-id/{id}")]
        public UyeModel uyeListeById(string id)
        {
            UyeModel uye = db.Uye.Where(s => s.userId == id).Select(x => new UyeModel()
            {
                ad = x.ad,
                soyad = x.soyad,
                userId = x.userId,
                email = x.email,
                rol = x.rol
            }).SingleOrDefault();

            return uye;
        }

        [HttpPost]
        [Route("api/uye-ekle")]
        public SonucModel uyeEkle(UyeModel uye)
        {
            if(db.Uye.Count(s=> s.userId == uye.userId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt zaten var";
                return sonuc;
            }
            Uye newUye = new Uye();
            newUye.userId = Guid.NewGuid().ToString();
            newUye.ad = uye.ad;
            newUye.soyad = uye.soyad;
            newUye.sifre = uye.sifre;
            newUye.email = uye.email;
            newUye.rol = uye.rol;

            db.Uye.Add(newUye);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Üye eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/uye-duzenle")]
        public SonucModel uyeDuzenle(UyeModel uye)
        {
            Uye kayit = db.Uye.Where(s => s.userId == uye.userId).SingleOrDefault();
            
            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            kayit.ad = uye.ad;
            kayit.soyad = uye.soyad;
            kayit.email = uye.email;

            if(uye.sifre != null)
            {
                kayit.sifre = uye.sifre;
            }

            if (uye.rol != null)
            {
                kayit.rol = uye.rol;
            }


            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Üye Düzenlendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/uye-sil/{id}")]
        public SonucModel uyeSil(string id)
        {
            Uye kayit = db.Uye.Where(s => s.userId == id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Silinecek üye bulunamadı";
                return sonuc;
            }

            if(db.Soru.Count(s=>s.uyeId== id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Sorusu olan öğrenci silinemez";
                return sonuc;
            }

            db.Uye.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Üye silindi";
            return sonuc;
        }


        #endregion

        #region Soru

        [HttpGet]
        [Route("api/soru-liste")]
        public List<SoruModel> soruListele()
        {
            List<SoruModel> liste = db.Soru.Select(x => new SoruModel
            {
                baslik = x.baslik,
                icerik = x.icerik,
                Id = x.Id,
                katId = x.katId,
                tarih = x.tarih,
                uyeId = x.uyeId
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/soru-listele-by-uye-id/{uyeId}")]
        public List<SoruModel> soruListeleByUserId(string uyeId)
        {
            List<SoruModel> liste = db.Soru.Where(s => s.uyeId == uyeId).Select(x => new SoruModel
            {
                baslik = x.baslik,
                icerik = x.icerik,
                Id = x.Id,
                katId = x.katId,
                tarih = x.tarih,
                uyeId = x.uyeId
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/soru-liste-by-id/{id}")]
        public SoruModel soruById(int id)
        {
            SoruModel kayit = db.Soru.Where(s => s.Id == id).Select(x => new SoruModel()
            {
                baslik = x.baslik,
                Id = x.Id,
                icerik = x.icerik,
                katId = x.katId,
                tarih = x.tarih,
                uyeId = x.uyeId,
            }).SingleOrDefault();

            List<CevapModel> cevaplar = db.Cevap.Where(s => s.soruId == kayit.Id).Select(x=> new CevapModel()
            {
                icerik = x.icerik,
                Id = x.Id,
                uyeId = x.uyeId
            }
            ).ToList();

            UyeModel uye = db.Uye.Where(s => s.userId == kayit.uyeId).Select(x => new UyeModel() {
                ad = x.ad,
                soyad = x.soyad
            }).SingleOrDefault();

            kayit.cevaplar = cevaplar;
            kayit.uye = uye;

            return kayit;
        }   

        [HttpGet]
        [Route("api/soru-listele-by-kat-id/{katId}")]
        public List<SoruModel> soruListeleByKatId(int katId)
        {
            List<SoruModel> liste = db.Soru.Where(s => s.katId == katId).Select(x => new SoruModel()
            {
                baslik = x.baslik,
                Id = x.Id,
                icerik = x.icerik,
                katId = x.katId,
                tarih = x.tarih,
                uyeId = x.uyeId
            }).ToList();

            return liste;
        }


        [HttpPost]
        [Route("api/soru-ekle")]
        public SonucModel soruEkle(SoruModel model)
        {
            if(db.Soru.Count(s=> s.Id == model.Id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu ID'ye soru kayıtlıdır.";
                return sonuc;
            }

            Soru newSoru = new Soru();
            newSoru.baslik = model.baslik;
            newSoru.icerik = model.icerik;
            newSoru.tarih = model.tarih;
            newSoru.katId = model.katId;
            newSoru.uyeId = model.uyeId;
            db.Soru.Add(newSoru);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Soru kaydedildi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/soru-duzenle")]
        public SonucModel soruDuzenle(SoruModel model)
        {
            Soru kayit = db.Soru.Where(s => s.Id == model.Id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            kayit.icerik = model.icerik;
            kayit.baslik = model.baslik;
            kayit.tarih = model.tarih;
            kayit.katId = model.katId;
            kayit.uyeId = model.uyeId;
            db.Soru.Add(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kayıt güncellendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/soru-sil/{id}")]
        public SonucModel soruSil(int id)
        {
            Soru kayit = db.Soru.Where(s => s.Id == id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            if(db.Cevap.Count(s=>s.soruId == id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde cevap olan soru silinemez";
                return sonuc;
            }

            db.Soru.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = false;
            sonuc.mesaj = "Soru silindi";
            return sonuc;
        }

        #endregion

        #region Kategori

        [HttpGet]
        [Route("api/kategori-liste")]
        public List<KategoriModel> kategoriListele()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel()
            {
                katAdi = x.katAdi,
                katId = x.katId
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/kategori-liste-by-id/{id}")]
        public KategoriModel kategoriById(int id)
        {
            KategoriModel kayit = db.Kategori.Where(s => s.katId == id).Select(x => new KategoriModel()
            {
                katId = x.katId,
                katAdi = x.katAdi
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/kategori-ekle")]
        public SonucModel kategoriEkle(KategoriModel model) 
        {
            if(db.Kategori.Count(s=> s.katAdi == model.katAdi)>0) 
            {
                sonuc.islem = false;
                sonuc.mesaj = "Verilen kategori adı kayıtlıdır";
                return sonuc;
            }

            Kategori yeni = new Kategori();
            yeni.katAdi = model.katAdi;
            db.Kategori.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kategori-duzenle")]
        public SonucModel kategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s.katId == model.katId).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori bulunamadı.";
                return sonuc;
            }

            kayit.katAdi = model.katAdi;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategori-sil/{id}")]
        public SonucModel kategoriSil(int id)
        {
            Kategori kayit = db.Kategori.Where(s => s.katId == id).SingleOrDefault();
            
            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori bulunamadı";
                return sonuc;
            }

            if(db.Soru.Count(s=>s.katId == id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Soru bulunan kategori silinemez";
                return sonuc;
            }

            db.Kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori silindi";
            return sonuc;
        }
        #endregion

        #region Cevap

        [HttpGet]
        [Route("api/cevap-liste-by-soru-id/{soruId}")]
        public List<CevapModel> cevapListeleBySoruId(int soruId)
        {
            List<CevapModel> liste = db.Cevap.Where(s => s.soruId == soruId).Select(x=> new CevapModel()
            {
                icerik = x.icerik,
                soruId = x.soruId,
                Id = x.Id,
                uyeId =x.uyeId
            }  ).ToList();

            return liste;
        }

        [HttpPost]
        [Route("api/cevap-ekle")]
        public SonucModel cevapEkle(CevapModel model)
        {
            Cevap yeni = new Cevap();
            yeni.icerik = model.icerik;
            yeni.soruId = model.soruId;
            yeni.uyeId = model.uyeId;
            db.Cevap.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Cevap eklendi";
            return sonuc;
            
        }

        [HttpPut]
        [Route("api/cevap-duzenle")]
        public SonucModel cevapDuzenle(CevapModel model)
        {
            Cevap kayit = db.Cevap.Where(s => s.Id == model.Id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            kayit.icerik = model.icerik;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Cevap düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/cevap-sil/{id}")]
        public SonucModel cevapSil(int id)
        {
            Cevap kayit = db.Cevap.Where(s=>s.Id == id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Silinecek kayıt bulunamadı";
                return sonuc;
            }
            db.Cevap.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Cevap silindi";
            return sonuc;
        }
        #endregion
    }
}
