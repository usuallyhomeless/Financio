import { Injectable } from '@angular/core';
import { Financiouser } from './Models/financiouser';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CardType } from './Models/cardtype';
import { Bank } from './Models/bank';
import { Ifsc } from './Models/Ifcs';
import { Product } from './Models/product';
import { JsonResponse } from './Models/common';
import { UserAndCard } from './Models/userandcard';
import { Card } from './Models/card';
import { LoginUserModel } from './Models/loginuserModel';
import { Observable, from, of, Subject, BehaviorSubject } from 'rxjs';
import { CompleteUserDetail } from './Models/complete-user-detail';
import { Scheme } from './Models/scheme';
import { DashboardDetails } from './Models/dashboard-details';
import { DebittransactionDetails } from './Models/debittransaction-details';
import { CredittransactionDetails } from './Models/credittransaction-details';
import { DebitTransactionModel } from './Models/debittransactionmodel';
import { ResetPassword } from './Models/resetPassword';

@Injectable({
  providedIn: 'root',
})
export class ApicallService {
  private ApiUrl = 'http://localhost:5000/api';
  
  completeUserDetailsSubject: BehaviorSubject<CompleteUserDetail> = new BehaviorSubject<CompleteUserDetail>({});

  constructor(private httpClient: HttpClient) {
    var token = sessionStorage.getItem('token');
    if (token) {
      this.getCompleteUserDetailsOnLogin().subscribe(
        (response) => {
          this.completeUserDetailsSubject.next(response);
        },
        (response) => {}
      );
    }
  }

  getHttpHeaders(): object {
    var token = sessionStorage.getItem('token');
    var headers = { 'Content-Type': 'application/json', Authorization: '' };
    if (token) {
      headers['Authorization'] = token;
    }
    return { headers: new HttpHeaders(headers) };
  }

  // ------------ Register Component-----------

  getAllCardType(): Observable<CardType[]> {
    return this.httpClient.get<CardType[]>(this.ApiUrl + '/cardtypes/');
  }

  getAllBank(): Observable<Bank[]> {
    return this.httpClient.get<Bank[]>(this.ApiUrl + '/banks/');
  }

  getIfcsByBankId(id: number): Observable<Ifsc[]> {
    return this.httpClient.get<Ifsc[]>(
      this.ApiUrl + `/ifscs/getbybankid/${id}`
    );
  }

  postCreateUserAndCard(message: UserAndCard): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(
      this.ApiUrl + '/financiousers/createuserandcard/',
      JSON.stringify(message),
      this.getHttpHeaders()
    );
  }

  // ------------ Login Component-----------

  postlogin(logindetails: LoginUserModel): Observable<JsonResponse> {
    return from(
      this.httpClient.post<JsonResponse>(
          this.ApiUrl + '/auth/login',
          JSON.stringify(logindetails),
          this.getHttpHeaders()
        )
        .toPromise()
        .then(
          (response) => {
            if (response.status == 200 && response.message) {
              sessionStorage.setItem('token', response.message);
              this.getCompleteUserDetailsOnLogin().subscribe((response) => {
                this.completeUserDetailsSubject.next(response);
              });
              return {status: response.status, message: 'Succesfully Logged in'};
            } else {
              return { status: response.status, message: response.message};
            }
          },
          (response) => {
            return { status: 500, message: response.error}
          }
        )
    );
  }

  isLoggedin(): Observable<Boolean> {
    var token = sessionStorage.getItem('token');
    if (!token) return of(false);
    return from(
      this.httpClient
        .get<JsonResponse>(
          this.ApiUrl + '/financiousers/isloggedin',
          this.getHttpHeaders()
        )
        .toPromise()
        .then(
          (response) => {
            if (response.status == 200) return true;
            return false;
          },
          (response) => {
            return false;
          }
        )
    );
  }

  getCompleteUserDetailsOnLogin(): Observable<CompleteUserDetail> {
    return this.httpClient.get<CompleteUserDetail>(
      this.ApiUrl + '/financiousers/getuserdetailsbyauth',
      this.getHttpHeaders()
    );
  }

  logout(): void {
    sessionStorage.clear();
    this.completeUserDetailsSubject.next({});
  }

  // ---------- Product List  ----------

  getAllProduct(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(this.ApiUrl + '/products/');
  }

  // ---------- Single Product -----------

  getProductById(id: number): Observable<Product> {
    return this.httpClient.get<Product>(this.ApiUrl + '/products/' + id);
  }

  getAllScheme(): Observable<Scheme[]> {
    return this.httpClient.get<Scheme[]>(this.ApiUrl + '/schemes/');
  }

  // auth
  getCheckProductBuy(productid: number): Observable<JsonResponse> {
    return this.httpClient.get<JsonResponse>(
      this.ApiUrl + `/debittransactions/checkproductbuytransaction/${productid}`,
      this.getHttpHeaders()
    );
  }

  //auth
  getCheckProductPay(productid: number): Observable<JsonResponse> {
    return this.httpClient.get<JsonResponse>(
      this.ApiUrl + `/debittransactions/checkproductpaytransaction/${productid}`,
      this.getHttpHeaders()
    );
  }

  //auth
  postbuyproduct(
    productid: number,
    schemeid: number
  ): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(
      this.ApiUrl + `/debittransactions/buyproduct/${productid}/scheme/${schemeid}`,
      JSON.stringify({}),
      this.getHttpHeaders()
    );
  }

  //auth
  postPayProduct(productid: number): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(
      this.ApiUrl + `/credittransactions/repay/product/${productid}`,
      JSON.stringify({}),
      this.getHttpHeaders()
    );
  }

  //auth
  getDebitTransactionbyAuth(
    productid: number
  ): Observable<DebitTransactionModel> {
    return this.httpClient.get<DebitTransactionModel>(
      this.ApiUrl + `/debittransactions/getdebittransactionbyauth/${productid}`,
      this.getHttpHeaders()
    );
  }

  // ---------- DashBoard -----------------

  getDashboardDetails(): Observable<DashboardDetails> {
    return this.httpClient.get<DashboardDetails>(
      this.ApiUrl + '/financiousers/dashboard/',
      this.getHttpHeaders()
    );
  }

  getDebitTransactions(): Observable<DebittransactionDetails[]> {
    return this.httpClient.get<DebittransactionDetails[]>(
      this.ApiUrl + '/debittransactions/list/',
      this.getHttpHeaders()
    );
  }

  getCreditTransactions(): Observable<CredittransactionDetails[]> {
    return this.httpClient.get<CredittransactionDetails[]>(
      this.ApiUrl + '/credittransactions/list/',
      this.getHttpHeaders()
    );
  }

  // --------------- Admin user details ------------------

  //implement auth
  getAllUser(): Observable<Financiouser[]> {
    return this.httpClient.get<Financiouser[]>(
      this.ApiUrl + '/financiousers/',
      this.getHttpHeaders()
    );
  }

  getAllCard(): Observable<Card[]> {
    return this.httpClient.get<Card[]>(
      this.ApiUrl + '/cards/',
      this.getHttpHeaders()
    );
  }

  //----------- Complete user details ---------------------

  getUserDetailsById(id: number): Observable<CompleteUserDetail> {
    return this.httpClient.get<CompleteUserDetail>(
      this.ApiUrl + `/financiousers/completedetails/${id}`,
      this.getHttpHeaders()
    );
  }

  getCardByUserId(id: number): Observable<Card> {
    return this.httpClient.get<Card>(
      this.ApiUrl + `/cards/user/${id}`,
      this.getHttpHeaders()
    );
  }

  updateCard(id: number, card: CardType): Observable<any> {
    return this.httpClient.put<any>(
      this.ApiUrl + '/cards/' + id,
      JSON.stringify(card),
      this.getHttpHeaders()
    )
  }

  /// ------------------ password change --------------------------

  postChangePassword(changePassword: any): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(
      this.ApiUrl + '/financiousers/change-password', 
      JSON.stringify(changePassword),
      this.getHttpHeaders());
  }

  //----------------------- forgot password ------------------------

  postForgotPassword(email: string): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(
      this.ApiUrl + `/auth/forgot-password/${email}`,
      JSON.stringify({}),
      this.getHttpHeaders()
    );
  }

  postResetPassword(resetPassword: ResetPassword): Observable<JsonResponse>{
    return this.httpClient.post<JsonResponse>(
      this.ApiUrl + `/auth/reset-password`,
      JSON.stringify(resetPassword),
      this.getHttpHeaders()
    )
  }

  //-------------------------------------------------
}
