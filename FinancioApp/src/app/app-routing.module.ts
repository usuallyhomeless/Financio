import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProductlistComponent } from './components/productlist/productlist.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { SingleproductComponent } from './components/singleproduct/singleproduct.component';
import { LoginComponent } from './components/login/login.component';
import { TermsandconditionsComponent } from './components/termsandconditions/termsandconditions.component';
import { AdminuserdetailsComponent } from './components/adminuserdetails/adminuserdetails.component';
import { CompleteuserdetailsComponent } from './components/completeuserdetails/completeuserdetails.component';
import { AboutusComponent } from './components/aboutus/aboutus.component';
import { ChangepasswordComponent } from './components/changepassword/changepassword.component';
import { ForgotpasswordComponent } from './components/forgotpassword/forgotpassword.component';

const routes: Routes = [
  { path: '', component: ProductlistComponent },
  { path: 'register', component: RegistrationComponent },
  { path: 'login', component: LoginComponent },
  { path: 'login/:next?', component: LoginComponent },
  { path: 'change-password', component: ChangepasswordComponent},
  { path: 'forgot-password', component: ForgotpasswordComponent},
  { path: 'admin', component: AdminuserdetailsComponent },
  { path: 'productlist', component: ProductlistComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'products/:id', component: SingleproductComponent },
  { path: 'termsandconditions', component: TermsandconditionsComponent },  
  { path: 'completeuserdetails/:id', component: CompleteuserdetailsComponent },
  { path: 'aboutus', component: AboutusComponent },  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
