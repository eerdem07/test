import { KayitOlComponent } from './components/kayit-ol/kayit-ol.component';
import { ProfilComponent } from './components/profil/profil.component';
import { AdminSoruComponent } from './components/admin/admin-soru/admin-soru.component';
import { SoruDetayComponent } from './components/soru-detay/soru-detay.component';
import { KategoriComponent } from './components/kategori/kategori.component';
import { AuthGuard } from './services/AuthGuard';
import { AdminUyeComponent } from './components/admin/admin-uye/admin-uye.component';
import { AdminKategoriComponent } from './components/admin/admin-kategori/admin-kategori.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'kayit-ol',
    component: KayitOlComponent,
  },
  {
    path: 'kategori/:katId',
    component: KategoriComponent,
  },
  {
    path: 'admin/kategori',
    component: AdminKategoriComponent,
    canActivate: [AuthGuard],
    data: {
      yetkiler: ['Admin'],
      gerigit: '/login',
    },
  },
  {
    path: 'admin/soru',
    component: AdminSoruComponent,
  },
  {
    path: 'admin/soru/:katId',
    component: AdminSoruComponent,
  },
  {
    path: 'admin/uye',
    component: AdminUyeComponent,
  },
  {
    path: 'soru-detay/:id',
    component: SoruDetayComponent,
  },
  {
    path: 'profil/:id',
    component: ProfilComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
