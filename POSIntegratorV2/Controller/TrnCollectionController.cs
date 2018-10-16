﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace POSIntegratorV2.Controllers
{
    class TrnCollectionController
    {
        public frmMain formMain;
        //public String ActivityDate;

        public TrnCollectionController(frmMain form)
        {
            formMain = form;
            //ActivityDate = ActDate;
        }
        // ==============
        // Get Collection
        // ==============
        public void GetCollection(String apiUrlHost, String branchCode, String userCode)
        {
            try
            {
                //var newConnectionString = "Data Source=localhost;Initial Catalog=" + database + ";Integrated Security=True";
                Data.POSDatabaseDataContext posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

                var collections = from d in posData.TrnCollections where d.PostCode == null && d.CollectionNumber != "NA" && d.IsLocked == true select d;
                if (collections.Any())
                {
                    var collection = collections.FirstOrDefault();

                    var listPayTypes = new List<String>();
                    if (collection.TrnCollectionLines.Any())
                    {
                        foreach (var collectionLine in collection.TrnCollectionLines)
                        {
                            if (collectionLine.Amount > 0)
                            {
                                listPayTypes.Add(collectionLine.MstPayType.PayType + ": " + collectionLine.Amount.ToString("#,##0.00"));
                            }
                        }
                    }

                    String[] payTypes = listPayTypes.ToArray();
                    List<TrnCollectionLines> listCollectionLines = new List<TrnCollectionLines>();

                    if (collection.TrnSale != null)
                    {
                        foreach (var salesLine in collection.TrnSale.TrnSalesLines)
                        {
                            listCollectionLines.Add(new TrnCollectionLines()
                            {
                                ItemManualArticleCode = salesLine.MstItem.BarCode,
                                Particulars = salesLine.MstItem.ItemDescription,
                                Unit = salesLine.MstUnit.Unit,
                                Quantity = salesLine.Quantity,
                                Price = salesLine.Price,
                                Discount = salesLine.MstDiscount.Discount,
                                DiscountAmount = salesLine.DiscountAmount,
                                NetPrice = salesLine.NetPrice,
                                Amount = salesLine.Amount,
                                VAT = salesLine.MstTax.Tax,
                                SalesItemTimeStamp = salesLine.SalesLineTimeStamp.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture)
                            });
                        }

                        var collectionData = new TrnCollection()
                        {
                            SIDate = collection.CollectionDate.ToShortDateString(),
                            BranchCode = branchCode,
                            CustomerManualArticleCode = collection.TrnSale.MstCustomer.CustomerCode,
                            CreatedBy = userCode,
                            Term = collection.TrnSale.MstTerm.Term,
                            DocumentReference = collection.CollectionNumber,
                            ManualSINumber = collection.TrnSale.SalesNumber,
                            Remarks = "User: " + collection.MstUser4.UserName + ", " + String.Join(", ", payTypes),
                            ListPOSIntegrationTrnSalesInvoiceItem = listCollectionLines.ToList()
                        };

                        String json = new JavaScriptSerializer().Serialize(collectionData);

                        formMain.logMessages("Sending Collection: " + collectionData.DocumentReference);
                        formMain.logMessages("Amount: " + collectionData.ListPOSIntegrationTrnSalesInvoiceItem.Sum(d => d.Amount).ToString("#,##0.00"));
                        SendCollection(apiUrlHost, json);
                    }
                }
            }
            catch (Exception e)
            {
                formMain.logMessages(e.Message);
            }
            formMain.logMessages("Collection Integration Done.");
        }

        // ===============
        // Send Collection
        // ===============
        public void SendCollection( String apiUrlHost, String json)
        {
            try
            {
                // ============
                // Http Request
                // ============
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + apiUrlHost + "/api/add/POSIntegration/salesInvoice");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                // ====
                // Data
                // ====
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    TrnCollection collection = new JavaScriptSerializer().Deserialize<TrnCollection>(json);
                    streamWriter.Write(new JavaScriptSerializer().Serialize(collection));
                }

                // ================
                // Process Response
                // ================
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        //var newConnectionString = "Data Source=localhost;Initial Catalog=" + database + ";Integrated Security=True";
                        Data.POSDatabaseDataContext posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

                        TrnCollection collection = new JavaScriptSerializer().Deserialize<TrnCollection>(json);
                        var currentCollection = from d in posData.TrnCollections where d.CollectionNumber.Equals(collection.DocumentReference) select d;
                        if (currentCollection.Any())
                        {
                            var updateCollection = currentCollection.FirstOrDefault();
                            updateCollection.PostCode = result.Replace("\"", "");
                            posData.SubmitChanges();
                        }

                        formMain.logMessages("Send Succesful!");
                    }
                }
            }
            catch (WebException we)
            {
                var resp = new StreamReader(we.Response.GetResponseStream()).ReadToEnd();

                formMain.logMessages(resp.Replace("\"", ""));
            }
        }
    }
}