﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace POSIntegratorV2.Controllers
{
    class TrnStockInController
    {
        public frmMain formMain;
        public String ActivityDate;

        public TrnStockInController(frmMain form, String ActDate)
        {
            formMain = form;
            ActivityDate = ActDate;
        }
        // ===================
        // Fill Leading Zeroes
        // ===================
        public String FillLeadingZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        // ============
        // Get Stock In
        // ============
        public void GetStockIn(String apiUrlHost, String branchCode)
        {
            try
            {
                DateTime dateTimeToday = DateTime.Now;
                String stockInDate = Convert.ToDateTime(ActivityDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);

                // ============
                // Http Request
                // ============
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + apiUrlHost + "/api/get/POSIntegration/stockIn/" + stockInDate + "/" + branchCode);
                httpWebRequest.Method = "GET";
                httpWebRequest.Accept = "application/json";

                // ================
                // Process Response
                // ================
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<TrnStockIn> stockInLists = (List<TrnStockIn>)js.Deserialize(result, typeof(List<TrnStockIn>));

                    if (stockInLists.Any())
                    {
                        //var newConnectionString = "Data Source=localhost;Initial Catalog=" + database + ";Integrated Security=True";
                        Data.POSDatabaseDataContext posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

                        foreach (var stockIn in stockInLists)
                        {
                            var currentStockIn = from d in posData.TrnStockIns where d.Remarks.Equals("IN-" + stockIn.BranchCode + "-" + stockIn.INNumber) && d.TrnStockInLines.Count() > 0 select d;
                            if (!currentStockIn.Any())
                            {
                                formMain.logMessages("Saving Stock In: IN-" + stockIn.BranchCode + "-" + stockIn.INNumber);

                                var defaultPeriod = from d in posData.MstPeriods select d;
                                var defaultSettings = from d in posData.SysSettings select d;

                                var lastStockInNumber = from d in posData.TrnStockIns.OrderByDescending(d => d.Id) select d;
                                var stockInNumberResult = defaultPeriod.FirstOrDefault().Period + "-000001";

                                if (lastStockInNumber.Any())
                                {
                                    var stockInNumberSplitStrings = lastStockInNumber.FirstOrDefault().StockInNumber;
                                    Int32 secondIndex = stockInNumberSplitStrings.IndexOf('-', stockInNumberSplitStrings.IndexOf('-'));
                                    var stockInNumberSplitStringValue = stockInNumberSplitStrings.Substring(secondIndex + 1);
                                    var stockInNumber = Convert.ToInt32(stockInNumberSplitStringValue) + 000001;
                                    stockInNumberResult = defaultPeriod.FirstOrDefault().Period + "-" + FillLeadingZeroes(stockInNumber, 6);
                                }

                                Data.TrnStockIn newStockIn = new Data.TrnStockIn
                                {
                                    PeriodId = defaultPeriod.FirstOrDefault().Id,
                                    StockInDate = Convert.ToDateTime(stockIn.INDate),
                                    StockInNumber = stockInNumberResult,
                                    SupplierId = defaultSettings.FirstOrDefault().PostSupplierId,
                                    Remarks = "IN-" + stockIn.BranchCode + "-" + stockIn.INNumber,
                                    IsReturn = false,
                                    PreparedBy = defaultSettings.FirstOrDefault().PostUserId,
                                    CheckedBy = defaultSettings.FirstOrDefault().PostUserId,
                                    ApprovedBy = defaultSettings.FirstOrDefault().PostUserId,
                                    IsLocked = true,
                                    EntryUserId = defaultSettings.FirstOrDefault().PostUserId,
                                    EntryDateTime = DateTime.Now,
                                    UpdateUserId = defaultSettings.FirstOrDefault().PostUserId,
                                    UpdateDateTime = DateTime.Now
                                };

                                posData.TrnStockIns.InsertOnSubmit(newStockIn);
                                posData.SubmitChanges();

                                if (stockIn.ListPOSIntegrationTrnStockInItem.Any())
                                {
                                    foreach (var item in stockIn.ListPOSIntegrationTrnStockInItem.ToList())
                                    {
                                        var currentItem = from d in posData.MstItems where d.BarCode.Equals(item.ItemCode) select d;
                                        if (currentItem.Any())
                                        {
                                            var currentItemUnit = from d in posData.MstUnits where d.Unit.Equals(item.Unit) select d;
                                            if (currentItemUnit.Any())
                                            {
                                                Data.TrnStockInLine newStockInLine = new Data.TrnStockInLine
                                                {
                                                    StockInId = newStockIn.Id,
                                                    ItemId = currentItem.FirstOrDefault().Id,
                                                    UnitId = currentItemUnit.FirstOrDefault().Id,
                                                    Quantity = item.Quantity,
                                                    Cost = item.Cost,
                                                    Amount = item.Amount,
                                                    ExpiryDate = currentItem.FirstOrDefault().ExpiryDate,
                                                    LotNumber = currentItem.FirstOrDefault().LotNumber,
                                                    AssetAccountId = currentItem.FirstOrDefault().AssetAccountId,
                                                    Price = currentItem.FirstOrDefault().Price
                                                };

                                                posData.TrnStockInLines.InsertOnSubmit(newStockInLine);

                                                var updateItem = currentItem.FirstOrDefault();
                                                updateItem.OnhandQuantity = currentItem.FirstOrDefault().OnhandQuantity + Convert.ToDecimal(item.Quantity);

                                                posData.SubmitChanges();

                                                formMain.logMessages(" > " + currentItem.FirstOrDefault().ItemDescription + " * " + item.Quantity.ToString("#,##0.00"));
                                            }
                                        }
                                    }
                                }

                                formMain.logMessages("Save Successful!");
                                //formMain.logMessages();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                formMain.logMessages(e.Message);
                //formMain.logMessages();
            }
            formMain.logMessages("StockIn Integration Done.");

        }
    }
}