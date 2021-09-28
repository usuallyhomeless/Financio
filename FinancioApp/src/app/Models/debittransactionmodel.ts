export class DebitTransactionModel {
    id? : number;
    financiouser?: number;
    productid?: number;
    schemeid?: number;
    transactiondatetime?: string;
    installmentamount?: number;
    lastpaymentdatetime?: string;
    balanceleft?: number;
    isactive?: boolean;
    financiouserNavigation ?: any;
    product ?: any;
    scheme ?: any;
    credittransaction ?: any;
}