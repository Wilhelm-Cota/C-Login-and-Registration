using EasyRegistration.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace EasyRegistration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public List<EmployeeModel> employeeslist = new List<EmployeeModel>();
        SqlConnection con = new SqlConnection("Data Source=PETER-LAPTOP\\SQLEXPRESS;Initial Catalog=YouTube;Integrated Security=True");
        SqlConnection con1 = new SqlConnection("Data Source=PETER-LAPTOP\\SQLEXPRESS;Initial Catalog=Department;Integrated Security=True");
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserModel userModel)
        {
            string conn = "SELECT * FROM Login WHERE username=@username AND password=@password";
            SqlCommand cmd = new SqlCommand(conn,con);
            cmd.Parameters.AddWithValue("@username", userModel.Username);
            cmd.Parameters.AddWithValue("@password", userModel.Password);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dtable = new DataTable();
            sda.Fill(dtable);

            if(dtable.Rows.Count > 0)
            {
                return RedirectToAction("Welcome");
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserModel userModel)
        {
            string conn = "INSERT INTO Login(username, password) VALUES(@username, @password)";
            SqlCommand cmd = new SqlCommand(conn, con);
           
                cmd.Parameters.AddWithValue("@username", userModel.Username);
                cmd.Parameters.AddWithValue("@password", userModel.Password);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index");
        }

        public IActionResult Welcome()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Details()
        {
            
            string conn = "SELECT * FROM Employees";
            SqlCommand cmd = new SqlCommand(conn, con1);
            con1.Open();
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read()) {
                    EmployeeModel employee = new EmployeeModel();
                    employee.FirstName = rdr.GetString(0);
                    employee.LastName = rdr.GetString(1);
                    employee.Email = rdr.GetString(2);
                    employee.Gender = rdr.GetString(3);
                    employee.Age = rdr.GetInt32(4);
                    employee.Department = rdr.GetString(5);
                    employee.Role = rdr.GetString(6);

                    employeeslist.Add(employee);
                    }
            }con1.Close();
            return RedirectToAction("Details"); 
        }
        public IActionResult AddInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddInfo(EmployeeModel employeeModel)
        {
            string conn = "INSERT INTO Employees(firstname,lastname,email,gender,age,department,role) VALUES(@firstname, @lastname,@email,@gender,@age,@department,@role)";
            SqlCommand cmd = new SqlCommand(conn, con1);

            cmd.Parameters.AddWithValue("@firstname", employeeModel.FirstName );
            cmd.Parameters.AddWithValue("@lastname", employeeModel.LastName);
            cmd.Parameters.AddWithValue("@email", employeeModel.Email);
            cmd.Parameters.AddWithValue("@gender", employeeModel.Gender);
            cmd.Parameters.AddWithValue("@age", employeeModel.Age);
            cmd.Parameters.AddWithValue("@department", employeeModel.Department);
            cmd.Parameters.AddWithValue("@role", employeeModel.Role);

            con1.Open();
            cmd.ExecuteNonQuery();
            con1.Close();
            return RedirectToAction("Details");
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}