using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace POSIntegratorV2.Controllers
{
    class TrnStockTransferOutController
    {
        public frmMain formMain;
        public String ActivityDate;

        public TrnStockTransferOutController(frmMain form, String ActDate)
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

        // =======================
        // Get Stock Transfer - OT
        // =======================
        public void GetStockTransferOT(String apiUrlHost, String fromBranchCode)
        {
            try
            {
                DateTime dateTimeToday = DateTime.Now;
                String stockTransferDate = Convert.ToDateTime(ActivityDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);

                // ============
                // Http Request
                // ============
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + apiUrlHost + "/api/get/POSIntegration/stockTransferItems/OT/" + stockTransferDate + "/" + fromBranchCode);
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
                    List<TrnStockTransfer> stockTransferLists = (List<TrnStockTransfer>)js.Deserialize(result, typeof(List<TrnStockTransfer>));

                    if (stockTransferLists.Any())
                    {
                        //var newConnectionString = "Data Source=localhost;Initial Catalog=" + database + ";Integrated Security=True";
                        Data.POSDatabaseDataContext posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

                        foreach (var stockTransfer in stockTransferLists)
                        {
                            var currentStockOut = from d in posData.TrnStockOuts where d.Remarks.Equals("ST-" + stockTransfer.BranchCode + "-" + stockTransfer.STNumber) && d.TrnStockOutLines.Count() > 0 select d;
                            if (!currentStockOut.Any())
                            {
                                formMain.logMessages("Saving Stock Transfer (OT): ST-" + stockTransfer.BranchCode + "-" + stockTransfer.STNumber);

                                var defaultPeriod = from d in posData.MstPeriods select d;
                                var defaultSettings = from d in posData.SysSettings select d;

                                var lastStockOutNumber = from d in posData.TrnStockOuts.OrderByDescending(d => d.Id) select d;
                                var stockOutNumberResult = defaultPeriod.FirstOrDefault().Period + "-000001";

                                if (lastStockOutNumber.Any())
                                {
                                    var stockOutNumberSplitStrings = lastStockOutNumber.FirstOrDefault().StockOutNumber;
                                    Int32 secondIndex = stockOutNumberSplitStrings.IndexOf('-', stockOutNumberSplitStrings.IndexOf('-'));
                                    var stockOutNumberSplitStringValue = stockOutNumberSplitStrings.Substring(secondIndex + 1);
                                    var stockOutNumber = Convert.ToInt32(stockOutNumberSplitStringValue) + 000001;
                                    stockOutNumberResult = defaultPeriod.FirstOrDefault().Period + "-" + FillLeadingZeroes(stockOutNumber, 6);
                                }

                                var accounts = from d in posData.MstAccounts
                                               select d;

                                if (accounts.Any())
                                {
                                    Data.TrnStockOut newStockOut = new Data.TrnStockOut
                                    {
                                        PeriodId = defaultPeriod.FirstOrDefault().Id,
                                        StockOutDate = Convert.ToDateTime(stockTransfer.STDate),
                                        StockOutNumber = stockOutNumberResult,
                                        AccountId = accounts.FirstOrDefault().Id,
                                        Remarks = "ST-" + stockTransfer.BranchCode + "-" + stockTransfer.STNumber,
                                        PreparedBy = defaultSettings.FirstOrDefault().PostUserId,
                                        CheckedBy = defaultSettings.FirstOrDefault().PostUserId,
                                        ApprovedBy = defaultSettings.FirstOrDefault().PostUserId,
                                        IsLocked = true,
                                        EntryUserId = defaultSettings.FirstOrDefault().PostUserId,
                                        EntryDateTime = DateTime.Now,
                                        UpdateUserId = defaultSettings.FirstOrDefault().PostUserId,
                                        UpdateDateTime = DateTime.Now,
                                    };

                                    posData.TrnStockOuts.InsertOnSubmit(newStockOut);
                                    posData.SubmitChanges();

                                    if (stockTransfer.ListPOSIntegrationTrnStockTransferItem.Any())
                                    {
                                        foreach (var item in stockTransfer.ListPOSIntegrationTrnStockTransferItem.ToList())
                                        {
                                            var currentItem = from d in posData.MstItems where d.BarCode.Equals(item.ItemCode) select d;
                                            if (currentItem.Any())
                                            {
                                                var currentItemUnit = from d in posData.MstUnits where d.Unit.Equals(item.Unit) select d;
                                                if (currentItemUnit.Any())
                                                {
                                                    Data.TrnStockOutLine newStockOutLine = new Data.TrnStockOutLine
                                                    {
                                                        StockOutId = newStockOut.Id,
                                                        ItemId = currentItem.FirstOrDefault().Id,
                                                        UnitId = currentItemUnit.FirstOrDefault().Id,
                                                        Quantity = item.Quantity,
                                                        Cost = item.Cost,
                                                        Amount = item.Amount,
                                                        AssetAccountId = currentItem.FirstOrDefault().AssetAccountId,
                                                    };

                                                    posData.TrnStockOutLines.InsertOnSubmit(newStockOutLine);

                                                    var updateItem = currentItem.FirstOrDefault();
                                                    updateItem.OnhandQuantity = currentItem.FirstOrDefault().OnhandQuantity - Convert.ToDecimal(item.Quantity);

                                                    posData.SubmitChanges();

                                                    formMain.logMessages(" > " + currentItem.FirstOrDefault().ItemDescription + " * " + item.Quantity.ToString("#,##0.00"));
                                                }
                                            }
                                        }
                                    }

                                    formMain.logMessages("Save Successful!");
                                    //formMain.logMessages();
                                }
                                else
                                {
                                    formMain.logMessages("Cannot Save Stock Transfer (OT): ST-" + stockTransfer.BranchCode + "-" + stockTransfer.STNumber);
                                    formMain.logMessages("Empty Accounts!");
                                    //formMain.logMessages();
                                }
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
            formMain.logMessages("Stock Transfer Out Integration Done.");

        }
    }
}