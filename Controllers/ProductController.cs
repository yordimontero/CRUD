using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using CRUD.Models;

namespace CRUD.Controllers
{
    public class ProductController : Controller
    {

        String connectionString = @"Data Source = DESKTOP-1LQG6ND; Initial Catalog = CrudDb; Integrated Security = True";

        [HttpGet]
        public ActionResult Index()
        {

            DataTable dataTableProduct = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) {

                sqlConnection.Open();

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Product", sqlConnection);

                sqlDataAdapter.Fill(dataTableProduct);

            }

            return View(dataTableProduct);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductModel());
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel productModel)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();

                string query = "INSERT INTO Product VALUES(@ProductName, @Price, @Count)";

                SqlCommand sqlCommando = new SqlCommand(query, sqlConnection);

                sqlCommando.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlCommando.Parameters.AddWithValue("@Price", productModel.Price);
                sqlCommando.Parameters.AddWithValue("@Count", productModel.Count);

                sqlCommando.ExecuteNonQuery();

            }
            
            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {

            ProductModel productModel = new ProductModel();

            DataTable dataTable = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();

                string query = "SELECT * FROM Product WHERE ProductID = @ProductID";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@ProductID", id);

                sqlDataAdapter.Fill(dataTable);

            }

            if (dataTable.Rows.Count == 1)
            {

                productModel.ProductID = Convert.ToInt32(
                        dataTable.Rows[0][0].ToString()
                    );

                productModel.ProductName = dataTable.Rows[0][1].ToString();

                productModel.Price = Convert.ToDecimal(
                        dataTable.Rows[0][2].ToString()
                    );

                productModel.Count = Convert.ToInt32(
                        dataTable.Rows[0][3].ToString()
                    );

                return View(productModel);

            } 
            else
            {
                return RedirectToAction("Index");
            }

        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductModel productModel)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string query = "UPDATE Product SET ProductName = @ProductName, Price = @Price, Count = @Count WHERE ProductID = @ProductID";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@ProductID", productModel.ProductID);
                sqlCommand.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlCommand.Parameters.AddWithValue("@Price", productModel.Price);
                sqlCommand.Parameters.AddWithValue("@Count", productModel.Count);

                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();

            }

            return RedirectToAction("Index");

        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();

                string query = "DELETE FROM Product WHERE ProductID = @ProductID";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@ProductID", id);

                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();

            }

            return RedirectToAction("Index");

        }

    }
}
