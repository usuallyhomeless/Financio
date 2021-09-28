export class Debittransaction {
    id?:number;
    financiouser?:number;
    productid?:number;
    schemeid?:number;
    transactiondatetime?:Date /* default current_timestamp */
    installmentamount:number=0
    lastpaymentdatetime?:Date /* null */
    balanceleft:number=0
    isactive:boolean=false;
}
