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
    public class DebittransactionsController : ControllerBase
    {
        private readonly financioContext _context;

        public DebittransactionsController(financioContext context)
        {
            _context = context;
        }        

        // * GET: api/debittransactions/list/

        [HttpGet("list")]
        [Authorize]
        public Object GetDebittransactionList()
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            var details = (from userobj in this._context.Financiouser
                           join debitobj in this._context.Debittransaction
                           on userobj.Id equals debitobj.Financiouser
                           where userobj.Id == SubjectUser.Id
                           select new
                           {
                               userid = SubjectUser.Id,
                               debitproductname = debitobj.Product.Productname,
                               debitamountpaid = debitobj.Product.Cost - debitobj.Balanceleft,
                               debitbalance = debitobj.Balanceleft
                           });
            return details;
        }                

        // * GET : api/debittransactions/getdebittransactionbyauth/2

        [HttpGet("getdebittransactionbyauth/{productid}")]
        [Authorize]
        public Debittransaction getCreditTransactionByAuth(int productid)
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            var Obj = this._context.Debittransaction.Where(x => x.Financiouser == SubjectUser.Id && x.Productid == productid && x.Isactive == true).FirstOrDefault();
            if (Obj == null) return Obj;
            return new Debittransaction { 
                Id = Obj.Id, 
                Financiouser = SubjectUser.Id,
                Productid = Obj.Productid,
                Schemeid = Obj.Schemeid,
                Transactiondatetime = Obj.Transactiondatetime,
                Installmentamount = Obj.Installmentamount,
                Lastpaymentdatetime = Obj.Lastpaymentdatetime,
                Balanceleft = Obj.Balanceleft,
                Isactive = Obj.Isactive
            };
        }

        // * GET: api/debittransactions/checkproductbuytransaction/2

        [HttpGet("checkproductbuytransaction/{productid}")]
        [Authorize]
        public ServerJsonResponse CheckBuyTransaction(int productid)
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            Card SubjectCard = (from x in this._context.Card where x.Financiouser == SubjectUser.Id select x).FirstOrDefault();            
            if (SubjectUser == null || SubjectCard == null) return new ServerJsonResponse() { Status = 404, Message = "User or the related card not found" };

            Product RequestedPoduct = (from x in this._context.Product where x.Id == productid select x).FirstOrDefault();
            if (RequestedPoduct == null) return new ServerJsonResponse() { Status = 404, Message = "Invalid Product" };            

            if (this._context.Debittransaction.Where(x => x.Financiouser == SubjectUser.Id && x.Productid == RequestedPoduct.Id && x.Isactive == true).Count() > 0)
                return new ServerJsonResponse() { Status = 400, Message = "Product Already Purchased" };            

            return new ServerJsonResponse() { Status = 200, Message = "okay" };
        }


        // * GET api/debittransactions/checkproductpaytransaction/2

        [HttpGet("checkproductpaytransaction/{productid}")]
        [Authorize]
        public ServerJsonResponse CheckMonthlyTransaction(int productid)
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            Card SubjectCard = (from x in this._context.Card where x.Financiouser == SubjectUser.Id select x).FirstOrDefault();
            if (SubjectUser == null || SubjectCard == null) return new ServerJsonResponse() { Status = 404, Message = "User or the related card not found" };

            Product RequestedPoduct = (from x in this._context.Product where x.Id == productid select x).FirstOrDefault();
            if (RequestedPoduct == null) return new ServerJsonResponse() { Status = 404, Message = "Invalid Product" };

            Debittransaction SubjectDebitTransaction = this._context.Debittransaction.Where(x => x.Financiouser == SubjectUser.Id && x.Productid == RequestedPoduct.Id && x.Isactive == true).FirstOrDefault();
            
            if(SubjectDebitTransaction == null)
                return new ServerJsonResponse() { Status = 400, Message = "No Debit Transaction found" };

            Credittransaction SubjectCreditTransaction = this._context.Credittransaction.Where(x => x.Debittransactionid == SubjectDebitTransaction.Id).OrderByDescending(x => x.Transactiondatetime).FirstOrDefault();

            if (SubjectCreditTransaction == null)
                return new ServerJsonResponse() { Status = 200, Message = "okay" };

            if (SubjectCreditTransaction.Transactiondatetime.Value.Month == DateTime.Now.Month)
                return new ServerJsonResponse() { Status = 422, Message = "EMI Already Paid for this month" };

            return new ServerJsonResponse() { Status = 200, Message = "okay" };
        }


        // * POST : api/debittransactions/buyproduct/2/scheme/1

        [HttpPost("buyproduct/{productid}/scheme/{schemeid}")]
        [Authorize]
        public ServerJsonResponse PostCredittransaction(int productid, int schemeid)
        {
            using (var transcation = this._context.Database.BeginTransaction())
            {
                try
                {
                    Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
                    Card SubjectCard = (from x in this._context.Card where x.Financiouser == SubjectUser.Id select x).FirstOrDefault();
                    if (SubjectCard.Isactive == false) return new ServerJsonResponse() { Status = 404, Message = "Card Inactive" };

                    if (SubjectUser == null || SubjectCard == null) return new ServerJsonResponse() { Status = 404, Message = "User or the related card not found" };

                    Product RequestedPoduct = (from x in this._context.Product where x.Id == productid select x).FirstOrDefault();
                    if (RequestedPoduct == null) return new ServerJsonResponse() { Status = 404, Message = "Invalid Product" };

                    Scheme RequestedScheme = (from x in this._context.Scheme where x.Id == schemeid select x).FirstOrDefault();
                    if (RequestedScheme == null) return new ServerJsonResponse() { Status = 404, Message = "Invalid Scheme" };

                    if (RequestedPoduct.Cost > SubjectCard.Cardlimit) return new ServerJsonResponse() { Status = 400, Message = "Insufficient Credit Limit" };

                    if (this._context.Debittransaction.Where(x => x.Financiouser == SubjectUser.Id && x.Productid == RequestedPoduct.Id && x.Isactive == true).Count() > 0)
                        return new ServerJsonResponse() { Status = 400, Message = "already subscribed" };


                    this._context.Debittransaction.Add(new Debittransaction()
                    {
                        Financiouser = SubjectUser.Id,
                        Productid = productid,
                        Schemeid = schemeid,
                        Transactiondatetime = DateTime.Now,
                        Installmentamount = RequestedPoduct.Cost / RequestedScheme.Schemeduration,
                        Balanceleft = RequestedPoduct.Cost,
                        Isactive = true,
                    });

                    SubjectCard.Cardlimit = SubjectCard.Cardlimit - RequestedPoduct.Cost;
                    this._context.SaveChanges();
                    transcation.Commit();
                    return new ServerJsonResponse() { Status = 200, Message = "Operation Successful" };
                }
                catch (Exception e)
                {
                    transcation.Rollback();
                    return new ServerJsonResponse() { Status = 500, Message = "Operation Unsuccessful" };
                }
            }
        }
    }
}
