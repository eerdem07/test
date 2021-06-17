import { Cevap } from '../models/Cevap';
import { Uye } from './../models/Uye';
import { Soru } from '../models/Soru';
import { Kategori } from './../models/Kategori';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  apiUrl = 'https://localhost:44360/api/';

  constructor(public http: HttpClient) {}

  /* Oturum İşlemleri Başla */

  tokenAl(kadi: string, parola: string) {
    var data =
      'username=' + kadi + '&password=' + parola + '&grant_type=password';
    var reqHeader = new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded',
    });
    return this.http.post(this.apiUrl + '/token', data, { headers: reqHeader });
  }

  oturumKontrol() {
    if (localStorage.getItem('token')) {
      return true;
    } else {
      return false;
    }
  }

  yetkiKontrol(yetkiler) {
    var uyeYetkileri: string[] = JSON.parse(
      localStorage.getItem('uyeYetkileri')
    );
    var sonuc: boolean = false;
    if (uyeYetkileri) {
      yetkiler.forEach((element) => {
        if (uyeYetkileri.indexOf(element) > -1) {
          sonuc = true;
          return false;
        }
      });
    }

    return sonuc;
  }

  /* Oturum İşlemleri Bitiş */

  /*  API */

  // Kategori API Başlangıç

  KategoriListe() {
    return this.http.get(this.apiUrl + 'kategori-liste/');
  }
  KategoriById(katId: number) {
    return this.http.get(this.apiUrl + 'kategori-liste-by-id/' + katId);
  }
  KategoriEkle(kat: Kategori) {
    return this.http.post(this.apiUrl + 'kategori-ekle/', kat);
  }
  KategoriDuzenle(kat: Kategori) {
    return this.http.put(this.apiUrl + 'kategori-duzenle/', kat);
  }
  KategoriSil(katId: number) {
    return this.http.delete(this.apiUrl + 'kategori-sil/' + katId);
  }

  // Kategori API Bitiş

  // Soru API Başlangıç

  SoruListe() {
    return this.http.get(this.apiUrl + 'soru-liste');
  }
  SoruListeByKatId(katId: number) {
    return this.http.get(this.apiUrl + 'soru-listele-by-kat-id/' + katId);
  }
  SoruListeByUyeId(uyeId: string) {
    return this.http.get(this.apiUrl + 'soru-listele-by-uye-id/' + uyeId);
  }
  SoruById(soruId: any) {
    return this.http.get(this.apiUrl + 'soru-liste-by-id/' + soruId);
  }
  SoruEkle(soru: Soru) {
    soru.tarih = new Date(Date.now()).toLocaleString('tr');
    soru.uyeId = localStorage.getItem('uid');
    return this.http.post(this.apiUrl + 'soru-ekle/', soru);
  }
  SoruDuzenle(soru: Soru) {
    return this.http.put(this.apiUrl + 'soru-duzenle/', soru);
  }
  SoruSil(soruId: number) {
    return this.http.delete(this.apiUrl + 'soru-sil/' + soruId);
  }

  // SoruListeSonEklenenler(s: number) {
  //   return this.http.get(this.apiUrl + "makalelistesoneklenenler/" + s)
  // }

  // Soru API Bitiş

  // Üye API Başlangıç

  UyeListe() {
    return this.http.get(this.apiUrl + 'uye-liste/');
  }

  UyeById(uyeId: string) {
    return this.http.get(this.apiUrl + 'uye-by-id/' + uyeId);
  }
  UyeEkle(uye: Uye) {
    console.log(uye);
    return this.http.post(this.apiUrl + 'uye-ekle/', uye);
  }
  UyeDuzenle(uye: Partial<Uye>) {
    console.log(uye);
    return this.http.put(this.apiUrl + 'uye-duzenle/', uye);
  }
  UyeSil(uyeId: string) {
    return this.http.delete(this.apiUrl + 'uye-sil/' + uyeId);
  }

  // Üye API Bitiş

  // Cevap API Başlangıç

  CevapListeBySoruId(soruId: number) {
    return this.http.get(this.apiUrl + 'cevap-liste-by-soru-id/' + soruId);
  }

  CevapEkle(cevap: Cevap) {
    return this.http
      .post(this.apiUrl + 'cevap-ekle/', cevap)
      .subscribe((d) => {});
  }
  CevapDuzenle(cevap: Cevap) {
    return this.http.put(this.apiUrl + 'cevap-duzenle/', cevap);
  }
  CevapSil(cevapId: number) {
    return this.http.delete(this.apiUrl + 'cevap-sil/' + cevapId);
  }

  // Cevap API Bitiş
}
