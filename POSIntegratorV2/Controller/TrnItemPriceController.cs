using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace POSIntegratorV2.Controllers
{
    class TrnItemPriceController
    {
        public frmMain formMain;
        public String ActivityDate;

        public TrnItemPriceController(frmMain form, String ActDate)
        {
            formMain = form;
            ActivityDate = ActDate;
        }
        // ==============
        // Get Item Price
        // ==============
        public void GetItemPrice(String apiUrlHost, String branchCode)
        {
            try
            {
                DateTime dateTimeToday = DateTime.Now;
                //String currentDate = dateTimeToday.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String currentDate = Convert.ToDateTime(ActivityDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);

                // ============
                // Http Request
                // ============
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + apiUrlHost + "/api/get/POSIntegration/itemPrice/" + branchCode + "/" + currentDate);
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
                    List<TrnArticlePrice> itemPriceLists = (List<TrnArticlePrice>)js.Deserialize(result, typeof(List<TrnArticlePrice>));

                    if (itemPriceLists.Any())
                    {
                        foreach (var itemPrice in itemPriceLists)
                        {
                            //var newConnectionString = "Data Source=localhost;Initial Catalog=" + database + ";Integrated Security=True";
                            Data.POSDatabaseDataContext posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

                            var currentItem = from d in posData.MstItems where d.BarCode.Equals(itemPrice.ItemCode) select d;
                            if (currentItem.Any())
                            {
                                var currentItemPrice = from d in posData.MstItemPrices where d.ItemId == currentItem.FirstOrDefault().Id && d.PriceDescription.Equals("IP-" + itemPrice.BranchCode + "-" + itemPrice.IPNumber + " (" + itemPrice.IPDate + ")") select d;
                                if (!currentItemPrice.Any())
                                {
                                    formMain.logMessages("Saving Item Price: IP-" + itemPrice.BranchCode + "-" + itemPrice.IPNumber + " (" + itemPrice.IPDate + ")");
                                    formMain.logMessages("Current Item: " + currentItem.FirstOrDefault().ItemDescription);

                                    Data.MstItemPrice newPrice = new Data.MstItemPrice()
                                    {
                                        ItemId = currentItem.FirstOrDefault().Id,
                                        PriceDescription = "IP-" + itemPrice.BranchCode + "-" + itemPrice.IPNumber + " (" + itemPrice.IPDate + ")",
                                        Price = itemPrice.Price,
                                        TriggerQuantity = itemPrice.TriggerQuantity
                                    };

                                    posData.MstItemPrices.InsertOnSubmit(newPrice);

                                    var updateCurrentItem = currentItem.FirstOrDefault();
                                    updateCurrentItem.Price = itemPrice.Price;

                                    posData.SubmitChanges();

                                    formMain.logMessages("Save Successful!");
                                }
                            }
                            else
                            {
                                formMain.logMessages("Cannot Save Item Price: IP-" + itemPrice.BranchCode + "-" + itemPrice.IPNumber + " (" + itemPrice.IPDate + ")" + "...");
                                formMain.logMessages("Price: " + itemPrice.Price);
                                formMain.logMessages("Item Not Found!");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                formMain.logMessages(e.Message);
            }

            formMain.logMessages("ItemPrice Integration Done.");
        }
    }
}