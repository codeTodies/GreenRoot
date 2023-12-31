﻿using DoAnNhom1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;

namespace DoAnNhom1.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        ApplicationDbContext database = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
            {
                return View("EmptyCart");
                //return RedirectToAction("Index", "Product");
            }
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }


        public ActionResult AddToCart(int id)
        {
            var _pro = database.Trees.SingleOrDefault(s => s.TreeID == id);

            if (_pro != null)
            {
                // var cart = GetCart();
                // cart.Add_Product_Cart(_pro);
                //Session["Cart"] = cart;
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }


        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.Update_quantity(id_pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }


        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int toltal_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                toltal_quantity_item = cart.Total_quantity();
            ViewBag.QuantityCart = toltal_quantity_item;
            return PartialView("BagCart");
        }

        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro order = new OrderPro();
                order.DateOrder = DateTime.Now;
                database.OrderPro.Add(order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_detail = new OrderDetail();
                    _order_detail.IDOrder = order.ID;
                    _order_detail.IDProduct = item._tree.TreeID;
                    _order_detail.UnitPrice = (double)item._tree.Price;
                    _order_detail.Quantity = item._quantity;
                    database.OrderDetail.Add(_order_detail);
                }
                database.SaveChanges();
                //cart.ClearCart();
                return RedirectToAction("PaymentWithPaypal", "ShoppingCart");
            }
            catch
            {
                return Content("Error, please check info again!");
            }
        }
        public ActionResult FailureView()
        {
            return View();
        }
        public ActionResult CheckOut_Success()
        {
            return View();
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/shoppingcart/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("ShowCart");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("CheckOut_Success");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            Cart cart = Session["Cart"] as Cart;
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc

            foreach (var item in cart.Items)
            {
                itemList.items.Add(new Item()
                {
                    name = item._tree.NameTree,
                    currency = "USD",
                    price = item._tree.Price.ToString(),
                    quantity = item._quantity.ToString(),
                    sku = item._tree.TreeID.ToString(),
                });
            }
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = cart.Total_money().ToString(),
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = cart.Total_money().ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            cart.ClearCart();
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

        //private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        //{
        //    //create itemlist and add item objects to it  
        //    var itemList = new ItemList()
        //    {
        //        items = new List<Item>()
        //    };
        //    //Adding Item Details like name, currency, price etc  
        //    itemList.items.Add(new Item()
        //    {
        //        name = "Item Name comes here",
        //        currency = "USD",
        //        price = "3",
        //        quantity = "1",
        //        sku = "sku"
        //    });
        //    itemList.items.Add(new Item()
        //    {
        //        name = "Item",
        //        currency = "USD",
        //        price = "2",
        //        quantity = "1",
        //        sku = "skux"
        //    });
        //    var payer = new Payer()
        //    {
        //        payment_method = "paypal"
        //    };
        //    // Configure Redirect Urls here with RedirectUrls object  
        //    var redirUrls = new RedirectUrls()
        //    {
        //        cancel_url = redirectUrl + "&Cancel=true",
        //        return_url = redirectUrl
        //    };
        //    // Adding Tax, shipping and Subtotal details  
        //    var details = new Details()
        //    {
        //        tax = "1",
        //        shipping = "1",
        //        subtotal = "5"
        //    };
        //    //Final amount with details  
        //    var amount = new Amount()
        //    {
        //        currency = "USD",
        //        total = "7", // Total must be equal to sum of tax, shipping and subtotal.  
        //        details = details
        //    };
        //    var transactionList = new List<Transaction>();
        //    // Adding description about the transaction  
        //    var paypalOrderId = DateTime.Now.Ticks;
        //    transactionList.Add(new Transaction()
        //    {
        //        description = $"Invoice #{paypalOrderId}",
        //        invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
        //        amount = amount,
        //        item_list = itemList
        //    });
        //    this.payment = new Payment()
        //    {
        //        intent = "sale",
        //        payer = payer,
        //        transactions = transactionList,
        //        redirect_urls = redirUrls
        //    };
        //    // Create a payment using a APIContext  
        //    return this.payment.Create(apiContext);
        //}


    }

}
       
