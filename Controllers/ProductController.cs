using NimapInfotechMachineTest.Models;
using NimapInfotechMachineTest.SqlDbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NimapInfotechMachineTest.Controllers
{
    public class ProductController : Controller
    {
        #region
        SqlConnection sqlcon;
        SqlCommand sqlcmd;
        Connection con;
        SqlDataAdapter sqlda;
        #endregion
        public ActionResult Index()
        {
            ViewBag.Category = CategoryList();
            return View();
        }
        public ActionResult Prtial_View()
        {
            return View();
        }
        public List<SelectListItem> CategoryList()
        {
            DataTable dt = new DataTable();
            var SelectList = new List<SelectListItem>();
            SelectList.Add(new SelectListItem { Value = "0", Text = "--Select--" });
            try
            {
                con = new Connection();
                dt = con.FillComboBox("Select * From MCategory Where IsActiv='True'");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SelectList.Add(new SelectListItem { Value = dt.Rows[i]["CategoryId"].ToString(), Text = dt.Rows[i]["CategoryName"].ToString() });

                }

            }
            catch (Exception ex)
            {

            }
            return SelectList;

        }
        public ActionResult SaveOrUpdate(ProductModel model)
        {
            int GetResponce = 0;
            int rtn = 0;
            string Flag = "";
            try
            {
                if (model.ProductId == 0)
                {
                    Flag = "I";
                    rtn = 1;
                    GetResponce = 1;
                }
                else
                {
                    Flag = "U";
                    rtn = 2;
                    GetResponce = 2;
                }
                con = new Connection();
                sqlcon = con.Connect();
                sqlcmd = new SqlCommand();
                sqlcmd.CommandText = "SpProduct";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Connection = sqlcon;
                sqlcmd.Parameters.AddWithValue("@ProductId", model.ProductId);
                sqlcmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                sqlcmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);               
                sqlcmd.Parameters.AddWithValue("@Flag", Flag);
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GetResponce = 3;
            }
            finally
            {
                sqlcmd.Dispose();
                sqlcon.Close();
            }
            if (GetResponce == 1)
            {
                TempData["message"] = "Your Data is Save Successfuly..";
            }
            else if (GetResponce == 2)
            {
                TempData["message"] = "Your Data is Update Successfuly";
            }
            else
            {
                TempData["Error"] = "Opps Somthing Went Wrong !!!";
            }
            return RedirectToAction("index");
        }
        public ActionResult ProductReport()
        {
            List<ProductReportModel> list = new List<ProductReportModel>();
            DataTable dt = new DataTable();
            con = new Connection();
            dt = con.FillComboBox("Select ProductId,ProductName,C.CategoryId,CategoryName From MProduct P inner join MCategory C on C.CategoryId=P.CategoryId");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductReportModel model = new ProductReportModel();

                model.ProductId = Convert.ToInt32(dt.Rows[i]["ProductId"]);
                model.ProductName = dt.Rows[i]["ProductName"].ToString();
                model.CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"]);
                model.CategoryName = dt.Rows[i]["CategoryName"].ToString();
                list.Add(model);
            }
            return View(list);


        }
        public ActionResult EditData(int id)
        {
            ViewBag.Category = CategoryList();

            DataTable dt = new DataTable();
            con = new Connection();
            dt = con.FillComboBox("Select * From MProduct Where ProductId=" + id);

            {
                ProductModel model = new ProductModel();
                {

                    model.ProductId = Convert.ToInt32(dt.Rows[0]["ProductId"]);
                    model.ProductName = dt.Rows[0]["ProductName"].ToString();                  
                    model.CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                }
                return PartialView("index", model);

            }
        }
        public ActionResult ReportDelete(int id)
        {
            ViewBag.Category = CategoryList();

            try
            {
                List<ProductReportModel> list = new List<ProductReportModel>();
                Connection Con = new Connection();
                DataTable dt = new DataTable();
                con = new Connection();
                ProductModel model = new ProductModel();
                model.ProductId = id;
                dt = con.FillComboBox("Delete From MProduct Where ProductId =" + id);
                TempData["Delete"] = "Delete Sucessfully !!";
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
            return View("Index");

        }
    }
}