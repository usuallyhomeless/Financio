using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancioAPI.Models;
using FinancioAPI.ViewModels;

namespace FinancioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredittransactionsController : ControllerBase
    {
        private readonly financioContext _context;

        public CredittransactionsController(financioContext context)
        {
            _context = context;
        }
                

        // * POST : api/credittransactions/repay/product/2

        [HttpPost("repay/product/{productid}")]
        [Authorize]
        public ServerJsonResponse PostCreditTransaction(int productid)
        {
            using (var transcation = this._context.Database.BeginTransaction())
            {
                try
                {
                    Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
                    Card SubjectCard = (from x in this._context.Card where x.Financiouser == SubjectUser.Id select x).FirstOrDefault();
                    if (SubjectUser == null || SubjectCard == null) return new ServerJsonResponse() { Status = 404, Message = "User or the related card not found" };

                    Debittransaction SubjectDebitTransaction = this._context.Debittransaction.Where(x => x.Financiouser == SubjectUser.Id && x.Productid == productid && x.Isactive == true).Include("Scheme").FirstOrDefault();
                    if (SubjectDebitTransaction == null) return new ServerJsonResponse() { Status = 404, Message = "You Have No active Emi for this product" };

                    Credittransaction SubjectCreditTransactionPast = this._context.Credittransaction.Where(x => x.Debittransactionid == SubjectDebitTransaction.Id).OrderByDescending(x => x.Transactiondatetime).FirstOrDefault();
                    if (SubjectCreditTransactionPast != null && SubjectCreditTransactionPast.Transactiondatetime.Value.Month == DateTime.Now.Month)
                        return new ServerJsonResponse() { Status = 422, Message = "EMI Already Paid for this month" };

                    Credittransaction SubjectCreditTransaction = new Credittransaction()
                    {
                        Debittransactionid = SubjectDebitTransaction.Id,
                        Amount = SubjectDebitTransaction.Installmentamount,
                        Transactiondatetime = DateTime.Now
                    };
                    this._context.Credittransaction.Add(SubjectCreditTransaction);

                    this._context.SaveChanges();

                    SubjectDebitTransaction.Lastpaymentdatetime = SubjectCreditTransaction.Transactiondatetime;
                    SubjectDebitTransaction.Balanceleft = SubjectDebitTransaction.Balanceleft - SubjectDebitTransaction.Installmentamount;

                    this._context.SaveChanges();

                    SubjectCard.Cardlimit = SubjectCard.Cardlimit + SubjectDebitTransaction.Installmentamount;

                    int CreditTransactionCount = this._context.Credittransaction.Where(x => x.Debittransactionid == SubjectDebitTransaction.Id).Count();
                    if (CreditTransactionCount >= SubjectDebitTransaction.Scheme.Schemeduration)
                    {
                        SubjectDebitTransaction.Isactive = false;
                        SubjectCard.Cardlimit = SubjectCard.Cardlimit + SubjectDebitTransaction.Balanceleft;
                        SubjectDebitTransaction.Balanceleft = 0;
                    }

                    this._context.SaveChanges();
                    transcation.Commit();
                    return new ServerJsonResponse() { Status = 200, Message = "Transaction Successful" };
                }
                catch (Exception e)
                {
                    transcation.Rollback();                    
                    return new ServerJsonResponse() { Status = 500, Message = "Transaction Unsuccessful" };
                }
            }
        }

        // * GET : api/debittransactions/list/

        [HttpGet("list")]
        [Authorize]
        public IEnumerable<CredittransactionDetails> GetCredittransactionList()
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            IEnumerable<CredittransactionDetails> details = (from userobj in this._context.Financiouser
                                                             join creditobj in this._context.Credittransaction
                                                             on userobj.Id equals creditobj.Debittransaction.Financiouser
                                                             where userobj.Id == SubjectUser.Id
                                                             select new CredittransactionDetails
                                                             {
                                                                 creditproductname = creditobj.Debittransaction.Product.Productname,
                                                                 credittransactiondate = creditobj.Transactiondatetime,
                                                                 creditamountpaid = creditobj.Debittransaction.Installmentamount
                                                             }).ToList();
            return details;
        }              
    }
}
