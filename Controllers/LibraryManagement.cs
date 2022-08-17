using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Librarymanagement.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace Librarymanagement.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LibraryManagement : ControllerBase
    {
        DbOperation Db = new DbOperation();
        Book bukobj = new Book();
        Lib_Users usobj = new Lib_Users();
        login logobj = new login();
        Encrypter enobj = new Encrypter();
        DataTable dt = new DataTable();
        
        DataSet ds = new DataSet();
 public IConfiguration _configuration;
        //------------------ login Users-----------------------------------------------------------------
 public LibraryManagement(IConfiguration config)
        {
            _configuration = config;

        }
        [HttpPost("SignUpUser")]
        public string Post([FromBody] Lib_Users usobj)
        {
            string msg = string.Empty;
            try
            {
                Encrypter encobj = new Encrypter();
                usobj.EncryptPassword = encobj.MD5Hash1(usobj);
                msg = Db.CreateLogin(usobj);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }


//-----------------------------------------------------------------------------------------------------


        [HttpPost("LoginUser")]
        
        public List<login> Post([FromBody] login logobj)

        {
            Encrypter encobj = new Encrypter();
            logobj.EncryptPassword = encobj.MD5Hash2(logobj);
            dt = Db.GetLogin(logobj);
               List<login> list = new List<login>();
            if(dt!=null){

            }
         
               var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", "1234"),
                        new Claim("DisplayName", "SnehakMohanan"),
                        new Claim("UserName", "Sneha"),
                        new Claim("Email", "snehakmohan1993@gmail.com")
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            var token1=new JwtSecurityTokenHandler().WriteToken(token);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                logobj.LoginId = Convert.ToInt32(dt.Rows[i]["LoginId"]);
                logobj.Name = dt.Rows[i]["Name"].ToString();
                logobj.CreateDate = Convert.ToDateTime(dt.Rows[i]["CreateDate"]);
                logobj.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                logobj.Token=token1.ToString();
                list.Add(logobj);
            }
            return list;
        }
         
        

        //-------------GET BOOK BY ID---------------------------------------------------------------     


        [HttpGet("{Id}")]

        public List<Book> Get(int Id)

        {
            DataTable dt = new DataTable();
            // Book bukobj = new Book();
            string msg = string.Empty;
            bukobj.Book_Id = Id;

            // dt = Db.GetBookDetails(Id, out msg);

            dt = Db.GetBookDetails(bukobj, out msg);
            List<Book> list = new List<Book>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bukobj.Book_Id = Convert.ToInt32(dt.Rows[i]["Book_Id"]);
                bukobj.BookName = dt.Rows[i]["BookName"].ToString();
                bukobj.MRP = Convert.ToInt32(dt.Rows[i]["MRP"]);
                // bukobj.Language_id = Convert.ToInt32(dt.Rows[i]["Language_id"]);
                bukobj.Language = dt.Rows[i]["Language"].ToString();
                // bukobj.Author_Id = Convert.ToInt32(dt.Rows[i]["Author_Id"]);
                bukobj.AuthorName = dt.Rows[i]["AuthorName"].ToString();
                // bukobj.Publisher_id = Convert.ToInt32(dt.Rows[i]["Publisher_id"]);
                bukobj.PublisherName = dt.Rows[i]["PublisherName"].ToString();
                bukobj.Published_Date = Convert.ToDateTime(dt.Rows[i]["Published_Date"]);
                bukobj.Volume = dt.Rows[i]["Volume"].ToString();
                list.Add(bukobj);
                // var customer = await context.Customers.Where(a => a.Id == Id).FirstOrDefaultAsync();

            }

            return list;
        }



        //---------------------VIEW BOOK---------------------------------------------------------------------------



        [HttpGet]
        public List<Book> Get()
        {
            DataTable dt = new DataTable();
            Book bukobj = new Book();
            string msg = string.Empty;

            dt = Db.GetBookView(out msg);
            List<Book> list = new List<Book>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bukobj.Book_Id = Convert.ToInt32(dt.Rows[i]["Book_Id"]);
                bukobj.BookName = dt.Rows[i]["BookName"].ToString();
                bukobj.MRP = Convert.ToInt32(dt.Rows[i]["MRP"]);
                bukobj.Language = dt.Rows[i]["Language"].ToString();
                bukobj.AuthorName = dt.Rows[i]["AuthorName"].ToString();
                bukobj.PublisherName = dt.Rows[i]["PublisherName"].ToString();
                bukobj.Published_Date = Convert.ToDateTime(dt.Rows[i]["Published_Date"]);
                bukobj.Volume = dt.Rows[i]["Volume"].ToString();

                list.Add(bukobj);
            }
            return list;
        }

        //-----------------Update BOOK Details------------------------------------------------------------



        [HttpPut("{Bookid}")]
        public string Put(int Bookid, [FromBody] Book bukobj)
        {
            string msg = string.Empty;

            try
            {
                bukobj.Book_Id = Bookid;
                msg = Db.UpdateBookDetails(bukobj);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        //------------BOOK Details DELETE---------------------------------------------------------


        [HttpDelete("{Book_id}")]
        public string Delete(int Book_id)
        {
            string msg = string.Empty;

            try
            {
                bukobj.Book_Id = Book_id;
                msg = Db.DeleteBookDetails(bukobj);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

    }
}