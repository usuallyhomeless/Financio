import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProductlistComponent } from './components/productlist/productlist.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HttpClientModule } from '@angular/common/http';
import { RegistrationComponent } from './components/registration/registration.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SingleproductComponent } from './components/singleproduct/singleproduct.component';
import { TermsandconditionsComponent } from './components/termsandconditions/termsandconditions.component';
import { CarouselComponent } from './components/carousel/carousel.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { LoginComponent } from './components/login/login.component';
import { AdminuserdetailsComponent } from './components/adminuserdetails/adminuserdetails.component';
import { FooterComponent } from './components/footer/footer.component';
import { CompleteuserdetailsComponent } from './components/completeuserdetails/completeuserdetails.component';
import { AboutusComponent } from './components/aboutus/aboutus.component';
import { ChangepasswordComponent } from './components/changepassword/changepassword.component';
import { ForgotpasswordComponent } from './components/forgotpassword/forgotpassword.component';


@NgModule({
  declarations: [
    AppComponent,
    ProductlistComponent,
    DashboardComponent,
    RegistrationComponent,
    SingleproductComponent,
    TermsandconditionsComponent,
    CarouselComponent,
    NavbarComponent,
    LoginComponent,
    AdminuserdetailsComponent,
    FooterComponent,
    CompleteuserdetailsComponent,
    AboutusComponent,
    ChangepasswordComponent,
    ForgotpasswordComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgbModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
  
})
export class AppModule {}
