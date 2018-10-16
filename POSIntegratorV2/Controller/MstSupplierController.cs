using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace POSIntegratorV2.Controllers
{
    class MstSupplierController
    {
        public frmMain formMain;
        public String ActivityDate;

        public MstSupplierController(frmMain form, String ActDate)
        {
            formMain = form;
            ActivityDate = ActDate;
        }
        // ============
        // Get Supplier
        // ============
        public void GetSupplier(String apiUrlHost)
        {
            try
            {
                DateTime dateTimeToday = DateTime.Now;
                //String currentDate = dateTimeToday.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String currentDate = Convert.ToDateTime(ActivityDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);

                // ============
                // Http Request
                // ============
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + apiUrlHost + "/api/get/POSIntegration/supplier/" + currentDate);
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
                    List<MstSupplier> supplierLists = (List<MstSupplier>)js.Deserialize(result, typeof(List<MstSupplier>));

                    if (supplierLists.Any())
                    {
                        //var newConnectionString = "Data Source=localhost;Initial Catalog=" + database + ";Integrated Security=True";
                        Data.POSDatabaseDataContext posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

                        foreach (var supplier in supplierLists)
                        {
                            var terms = from d in posData.MstTerms where d.Term.Equals(supplier.Term) select d;
                            if (terms.Any())
                            {
                                var defaultSettings = from d in posData.SysSettings select d;

                                var currentSupplier = from d in posData.MstSuppliers where d.Supplier.Equals(supplier.Article) select d;
                                if (currentSupplier.Any())
                                {
                                    Boolean foundChanges = false;

                                    if (!foundChanges)
                                    {
                                        if (!currentSupplier.FirstOrDefault().Supplier.Equals(supplier.Article))
                                        {
                                            foundChanges = true;
                                        }
                                    }

                                    if (!foundChanges)
                                    {
                                        if (!currentSupplier.FirstOrDefault().Address.Equals(supplier.Address))
                                        {
                                            foundChanges = true;
                                        }
                                    }

                                    if (!foundChanges)
                                    {
                                        if (!currentSupplier.FirstOrDefault().CellphoneNumber.Equals(supplier.ContactNumber))
                                        {
                                            foundChanges = true;
                                        }
                                    }

                                    if (!foundChanges)
                                    {
                                        if (!currentSupplier.FirstOrDefault().MstTerm.Term.Equals(supplier.Term))
                                        {
                                            foundChanges = true;
                                        }
                                    }

                                    if (!foundChanges)
                                    {
                                        if (!currentSupplier.FirstOrDefault().TIN.Equals(supplier.TaxNumber))
                                        {
                                            foundChanges = true;
                                        }
                                    }

                                    if (foundChanges)
                                    {
                                        formMain.logMessages("Updating Supplier: " + currentSupplier.FirstOrDefault().Supplier);
                                        formMain.logMessages("Contact No.: " + currentSupplier.FirstOrDefault().CellphoneNumber);

                                        var updateSupplier = currentSupplier.FirstOrDefault();
                                        updateSupplier.Supplier = supplier.Article;
                                        updateSupplier.Address = supplier.Address;
                                        updateSupplier.CellphoneNumber = supplier.ContactNumber;
                                        updateSupplier.TermId = terms.FirstOrDefault().Id;
                                        updateSupplier.TIN = supplier.TaxNumber;
                                        updateSupplier.UpdateUserId = defaultSettings.FirstOrDefault().PostUserId;
                                        updateSupplier.UpdateDateTime = DateTime.Now;
                                        posData.SubmitChanges();

                                        formMain.logMessages("Update Successful!");
                                    }
                                }
                                else
                                {
                                    formMain.logMessages("Saving Supplier: " + supplier.Article);
                                    formMain.logMessages("Contact No.: " + supplier.ContactNumber);

                                    Data.MstSupplier newSupplier = new Data.MstSupplier
                                    {
                                        Supplier = supplier.Article,
                                        Address = supplier.Address,
                                        TelephoneNumber = "NA",
                                        CellphoneNumber = supplier.ContactNumber,
                                        FaxNumber = "NA",
                                        TermId = terms.FirstOrDefault().Id,
                                        TIN = supplier.TaxNumber,
                                        AccountId = 254,
                                        EntryUserId = defaultSettings.FirstOrDefault().PostUserId,
                                        EntryDateTime = DateTime.Now,
                                        UpdateUserId = defaultSettings.FirstOrDefault().PostUserId,
                                        UpdateDateTime = DateTime.Now,
                                        IsLocked = true,
                                    };

                                    posData.MstSuppliers.InsertOnSubmit(newSupplier);
                                    posData.SubmitChanges();

                                    formMain.logMessages("Save Successful!");
                                }
                            }
                            else
                            {
                                formMain.logMessages("Cannot Save Supplier: " + supplier.Article);
                                formMain.logMessages("Term Mismatch!");
                                formMain.logMessages("Save Failed!");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                formMain.logMessages(e.Message);
            }
            formMain.logMessages("Supplier Integration Done.");
        }
    }
}