using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace POSIntegratorV2.FileWatcherController
{
    class TrnCollectionFileWatcherController
    {
        //public frmMain formMain;

        //public TrnSalesInvoiceFileWatcherController(frmMain form, System.Windows.Forms.DateTimePicker txtDate)
        //{
        //    formMain = form;
        //}

        //// ===============
        //// Send Collection
        //// ===============
        //public void SendCollection( String apiUrlHost, String json)
        //{
        //    try
        //    {
        //        // ============
        //        // Http Request
        //        // ============
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + apiUrlHost + "/api/add/POSIntegration/salesInvoice");
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";

        //        // ====
        //        // Data
        //        // ====
        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            TrnCollection collection = new JavaScriptSerializer().Deserialize<TrnCollection>(json);
        //            streamWriter.Write(new JavaScriptSerializer().Serialize(collection));
        //        }

        //        // ================
        //        // Process Response
        //        // ================
        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            var result = streamReader.ReadToEnd();
        //            if (result != null)
        //            {
        //                //var newConnectionString = "Data Source=localhost;Initial Catalog=" + database + ";Integrated Security=True";
        //                Data.POSDatabaseDataContext posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

        //                TrnCollection collection = new JavaScriptSerializer().Deserialize<TrnCollection>(json);
        //                var currentCollection = from d in posData.TrnCollections where d.CollectionNumber.Equals(collection.DocumentReference) select d;
        //                if (currentCollection.Any())
        //                {
        //                    var updateCollection = currentCollection.FirstOrDefault();
        //                    updateCollection.PostCode = result.Replace("\"", "");
        //                    posData.SubmitChanges();
        //                }
        //                //formMain.logMessagesFM("Send Succesful!");
        //            }
        //        }
        //    }
        //    catch (WebException we)
        //    {
        //        var resp = new StreamReader(we.Response.GetResponseStream()).ReadToEnd();

        //        //formMain.logMessages(resp.Replace("\"", ""));
        //    }
        //}
    }
}