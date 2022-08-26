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
using Microsoft.AspNetCore.Authorization;
using MyCustomApiResponses;
using PaginationFilter;

namespace Librarymanagement.Controllers
{

    //[Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class LibraryManagement : ControllerBase
    {
        DbOperation Db = new DbOperation();
        Book bukobj = new Book();
        Lib_Users usobj = new Lib_Users();
        login logobj = new login();
        Encrypter enobj = new Encrypter();
        Author Aobj=new Author();
        Languagee Lobj=new Languagee();

        Publisher Pobj=new Publisher();

        GetBook Gtobj=new GetBook();
    


        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

       
             public IConfiguration _configuration;
        
            public LibraryManagement(IConfiguration config)
           {
            _configuration = config;

           }


    //------------------SIGNUP USERS-----------------------------------------------------------------

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


//------------------LOGIN USERS-----------------------------------------------------------------

        [AllowAnonymous]
        
        [HttpPost("LoginUser")]
        
        public List<login> Post([FromBody] login logobj)

        {
            
            Encrypter encobj = new Encrypter();
            logobj.EncryptPassword = encobj.MD5Hash2(logobj);
                        
            //create claims details based on the user information
               var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", logobj.Username),
                        new Claim("Password",logobj.EncryptPassword )
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            var token1=new JwtSecurityTokenHandler().WriteToken(token); //token in token1

          
            logobj.Token=token1; //token pass to field token in the database
            
            dt = Db.GetLogin(logobj);
            

            List<login> list = new List<login>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try 
                {
                 logobj.LoginId = Convert.ToInt32(dt.Rows[i]["LoginId"]);
                logobj.Name = dt.Rows[i]["Name"].ToString();
                logobj.CreateDate = Convert.ToDateTime(dt.Rows[i]["CreateDate"]);
                logobj.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                logobj.Token=token1?.ToString();
                list.Add(logobj);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                
            }
            return list;
        }
         
     //----------------------ADD BOOK----------------------------------------------------------------
      [HttpPost("AddBook")]

        public async Task<MyCustomApiResponse> Post(Book bukobj)
        {

            var header = (string) HttpContext.Request.Headers["Authorization"];
            DbOperation DbObj = new DbOperation();
            string Tokens=string.Empty;
            Tokens=DbObj.Tokenvalidation(header);


            string Tokens1 = "Token is Valid";
            await Task.Delay(1);



            string msg = string.Empty;

            if(Tokens==Tokens1)
               {
                try
               {
                 
              msg = Db.AddBookDetails(bukobj);

              }
            catch (Exception ex)
              {
                msg = ex.Message;
              }
             return new MyCustomApiResponse("Success",200);
              }
             else

             {
               return new MyCustomApiResponse("UnAuthorized",401);
             }
        
            
            
        }
         
              

        //-------------1.GET BOOK BY ID---------------------------------------------------------------     


        [HttpGet("{Id}")]

        public async Task<MyCustomApiResponse> Get(int Id)
        {
            var header = (string) HttpContext.Request.Headers["Authorization"];
           
            DbOperation DbObj = new DbOperation();

            string Tokens=string.Empty; //Backend Token
             
            Tokens=DbObj.Tokenvalidation(header);//Token pass to Header

            
           

            string Tokens1 = "Token is Valid";

            await Task.Delay(1);


           
            DataTable dt = new DataTable();
          
            string msg = string.Empty;

            Gtobj.Book_Id = Id;

            dt = Db.GetBookDetails(Gtobj, out msg);

            List<GetBook> list = new List<GetBook>();


        if(Tokens==Tokens1)
        {

        if(dt.Rows.Count>0)
           
            {
           
             for (int i = 0; i < dt.Rows.Count; i++)
           
            {
                Gtobj.Book_Id = Convert.ToInt32(dt.Rows[i]["Book_Id"]); 
                Gtobj.BookName = dt.Rows[i]["BookName"].ToString();
                Gtobj.MRP = Convert.ToInt32(dt.Rows[i]["MRP"]);
                
                Gtobj.Language = dt.Rows[i]["Language"].ToString();
                Gtobj.AuthorName = dt.Rows[i]["AuthorName"].ToString();
                
                Gtobj.PublisherName = dt.Rows[i]["PublisherName"].ToString();
                Gtobj.Published_Date = Convert.ToDateTime(dt.Rows[i]["Published_Date"]);
                Gtobj.Volume = dt.Rows[i]["Volume"].ToString();
                list.Add(Gtobj);
              

            }
            
             return new MyCustomApiResponse(list,"Success",200);
            
            }

        else
           {
                 
             return new MyCustomApiResponse(list,"Invalid Book ID",200);

            }

           }
    else
     {
         return new MyCustomApiResponse(list,"UnAuthorized",401);
     }

  
        }





        //---------------------1.VIEW BOOK---------------------------------------------------------------------------



        [HttpGet]
    

        public async Task<MyCustomApiResponse> Get(  [FromQuery] Pagination1 filter )
        {
            var header = (string) HttpContext.Request.Headers["Authorization"];
             var validFilter = new Pagination1(filter.PageNumber, filter.PageSize);
             
            DbOperation DbObj = new DbOperation();
           
            string Tokens=string.Empty;
           
            Tokens=DbObj.Tokenvalidation(header);


             
            
            string Tokens1 = "Token is Valid";

            await Task.Delay(1);


            DataTable dt = new DataTable();
            string msg = string.Empty;
            dt = Db.GetBookView(out msg,validFilter.PageNumber,validFilter.PageSize);
            List<GetBook> list = new List<GetBook>();


         if(Tokens==Tokens1)

        {

            if(dt.Rows.Count>0)
           
            {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GetBook Gtobj=new GetBook();

                Gtobj.Book_Id = Convert.ToInt32(dt.Rows[i]["Book_Id"]);
                Gtobj.BookName = dt.Rows[i]["BookName"].ToString();
                Gtobj.MRP = Convert.ToInt32(dt.Rows[i]["MRP"]);
                Gtobj.Language = dt.Rows[i]["Language"].ToString();
                Gtobj.AuthorName = dt.Rows[i]["AuthorName"].ToString();
                Gtobj.PublisherName = dt.Rows[i]["PublisherName"].ToString();
                Gtobj.Published_Date = Convert.ToDateTime(dt.Rows[i]["Published_Date"]);
                Gtobj.Volume = dt.Rows[i]["Volume"].ToString();
                list.Add(Gtobj);
            }
            return new MyCustomApiResponse(list,"Success",200);
            
            }


        else
           {
                 
             return new MyCustomApiResponse(list,"Not Success",200);

            }

           }
    else
     {
         return new MyCustomApiResponse(list,"UnAuthorized",401);
     }
        }

        
        
        
        
        
        
        //-----------------3.UPDATE BOOK DETAILS------------------------------------------------------------

       
        [HttpPut("{Book_Id}")]
         public async Task<MyCustomApiResponse> Put(int Book_Id,[FromBody] Book bukobj)

        {
            
            var header = (string) HttpContext.Request.Headers["Authorization"];
            Console.WriteLine(header);
            
            DbOperation DbObj = new DbOperation();
            
            string Tokens=string.Empty;

             Console.WriteLine(Tokens);
             
            Tokens=DbObj.Tokenvalidation(header);

            Console.WriteLine(Tokens);

            
            string Tokens1 = "Token is Valid";

           await Task.Delay(1);



           
           
           string msg = string.Empty;

            if(Tokens==Tokens1)
             {
                try
               {
               

               bukobj.Book_Id = Book_Id;
                
               msg = Db.UpdateBookDetails(bukobj);
               Console.WriteLine(msg);

               }
               catch (Exception ex)
              
               {
                msg = ex.Message;
               }

              return new MyCustomApiResponse("Successfully Update Book",200);
             }
            else

             {
               return new MyCustomApiResponse("UnAuthorized",401);
             }

           
        }



        
        

        //------------4.DELETE BOOK DETAILS---------------------------------------------------------


        [HttpDelete("{Book_id}")]

        // public string Delete(int Book_id)
        public async Task<MyCustomApiResponse> Delete(int Book_id)
        {
            var header = (string) HttpContext.Request.Headers["Authorization"];
            DbOperation DbObj = new DbOperation();
            string Tokens=string.Empty;
            Tokens=DbObj.Tokenvalidation(header);

            Console.WriteLine(header);


           string Tokens1 = "Token is Valid";
           await Task.Delay(1);


            string msg = string.Empty;
           

             if(Tokens==Tokens1)
             {
                 try
               {
                Console.WriteLine(Tokens);
                bukobj.Book_Id = Book_id;
                msg = Db.DeleteBookDetails(bukobj);

               }
              catch (Exception ex)
               {
                msg = ex.Message;
               }
              return new MyCustomApiResponse("Successfully Delete Book",200);
             }
             else
             {
               return new MyCustomApiResponse("UnAuthorized",401);
             }


           
        }

//---------------------------------------AuthorsName----------------------

       [HttpGet("GetAuthors")]
       public async Task<MyCustomApiResponse> Get()
        {
            var header = (string) HttpContext.Request.Headers["Authorization"];
             
            DbOperation DbObj = new DbOperation();
           
            string Tokens=string.Empty;
           
            Tokens=DbObj.Tokenvalidation(header);

            string Tokens1 = "Token is Valid";

            await Task.Delay(1);


            DataTable dt = new DataTable();

            string msg = string.Empty;

            dt = Db.GetAuthors(Aobj,out msg);

            List<Author> list = new List<Author>();


         if(Tokens==Tokens1)

        {

            if(dt.Rows.Count>0)
           
            {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Author Aobj=new Author();

               Aobj.Author_Id = Convert.ToInt32(dt.Rows[i]["Author_Id"]);

                Aobj.AuthorName = dt.Rows[i]["AuthorName"].ToString();
                
                list.Add(Aobj);
            }
            return new MyCustomApiResponse(list,"Success",200);
            
            }

        else
           {
                 
             return new MyCustomApiResponse(list,"Not Success",200);

            }

           }
    else
     {
         return new MyCustomApiResponse(list,"UnAuthorized",401);
     }
        }
    



        //--------------------------------------Language------------------------------------------
         [HttpGet("GetLanguage")]
       public async Task<MyCustomApiResponse> Get1()
        {
            var header = (string) HttpContext.Request.Headers["Authorization"];
             
            DbOperation DbObj = new DbOperation();
           
            string Tokens=string.Empty;
           
            Tokens=DbObj.Tokenvalidation(header);

            string Tokens1 = "Token is Valid";

            await Task.Delay(1);


            DataTable dt = new DataTable();

            string msg = string.Empty;

            dt = Db.GetLanguage(Lobj,out msg);

            List<Languagee> list = new List<Languagee>();


         if(Tokens==Tokens1)

        {

            if(dt.Rows.Count>0)
           
            {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
               Languagee Lobj=new Languagee();

                 Lobj.Language_id = Convert.ToInt32(dt.Rows[i]["Language_id"]);

                Lobj.Language = dt.Rows[i]["Language"].ToString();
                
                list.Add(Lobj);
            }
            return new MyCustomApiResponse(list,"Success",200);
            
            }

        else
           {
                 
             return new MyCustomApiResponse(list,"Not Success",200);

            }

           }
    else
     {
         return new MyCustomApiResponse(list,"UnAuthorized",401);
     }
        }

      //--------------------------------------Publisher------------------------------------------ 

      [HttpGet("GetPublisher")]
       public async Task<MyCustomApiResponse> Get2()
        {
            var header = (string) HttpContext.Request.Headers["Authorization"];
             
            DbOperation DbObj = new DbOperation();
           
            string Tokens=string.Empty;
           
            Tokens=DbObj.Tokenvalidation(header);

            string Tokens1 = "Token is Valid";

            await Task.Delay(1);


            DataTable dt = new DataTable();

            string msg = string.Empty;

            dt = Db.GetPublisher(Pobj,out msg);

            List<Publisher> list = new List<Publisher>();


         if(Tokens==Tokens1)

        {

            if(dt.Rows.Count>0)
           
            {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
               Publisher Pobj=new Publisher();

                 Pobj.Publisher_id = Convert.ToInt32(dt.Rows[i]["Publisher_id"]);

                Pobj.PublisherName = dt.Rows[i]["PublisherName"].ToString();
                
                list.Add(Pobj);
            }
            return new MyCustomApiResponse(list,"Success",200);
            
            }

        else
           {
                 
             return new MyCustomApiResponse(list,"Not Success",200);

            }

           }
    else
     {
         return new MyCustomApiResponse(list,"UnAuthorized",401);
     }
        }




}
 }


























//===================================================================================================================



//-----------------------VIEW BOOK------------------------------------------------------------
//  [HttpGet]
//         public List<Book> Get()
//         {
//             var header = (string) HttpContext.Request.Headers["Authorization"];
             
//             DbOperation DbObj = new DbOperation();
           
//             string Tokens=string.Empty;
           
//             Tokens=DbObj.Tokenvalidation(header);




//             DataTable dt = new DataTable();
//             string msg = string.Empty;
//             dt = Db.GetBookView(out msg);
//             List<Book> list = new List<Book>();
//             for (int i = 0; i < dt.Rows.Count; i++)
//             {
//                 Book bukobj=new Book();

//                 bukobj.Book_Id = Convert.ToInt32(dt.Rows[i]["Book_Id"]);
//                 bukobj.BookName = dt.Rows[i]["BookName"].ToString();
//                 bukobj.MRP = Convert.ToInt32(dt.Rows[i]["MRP"]);
//                 bukobj.Language = dt.Rows[i]["Language"].ToString();
//                 bukobj.AuthorName = dt.Rows[i]["AuthorName"].ToString();
//                 bukobj.PublisherName = dt.Rows[i]["PublisherName"].ToString();
//                 bukobj.Published_Date = Convert.ToDateTime(dt.Rows[i]["Published_Date"]);
//                 bukobj.Volume = dt.Rows[i]["Volume"].ToString();

//                 list.Add(bukobj);
//             }
//             return list;
//         }


//-------------DELETE------------------------------------------------------

// [HttpDelete("{Book_id}")]
        
        // public string Delete(int Book_id)
        // {
        //     string msg = string.Empty;

        //     try
        //     {
        //         bukobj.Book_Id = Book_id;
        //         msg = Db.DeleteBookDetails(bukobj);

        //     }
        //     catch (Exception ex)
        //     {
        //         msg = ex.Message;
        //     }
        //     return msg;
        // }
//------------------------------UPDATE-------------------

//    [HttpPut("{Bookid}")]

//         public string Put(int Bookid, [FromBody] Book bukobj)
//         {
//             string msg = string.Empty;

//             try
//             {
//                 bukobj.Book_Id = Bookid;
//                 msg = Db.UpdateBookDetails(bukobj);

//             }
//             catch (Exception ex)
//             {
//                 msg = ex.Message;
//             }
//             return msg;
//         }
       //-------------------------GETBYID----------------------------------- 


       // [HttpGet("{Id}")]

//         public List<Book> Get(int Id)

//         {
//             DataTable dt = new DataTable();
//             // Book bukobj = new Book();
//             string msg = string.Empty;
//             bukobj.Book_Id = Id;

//             // dt = Db.GetBookDetails(Id, out msg);

//             dt = Db.GetBookDetails(bukobj, out msg);
//             List<Book> list = new List<Book>();
//             for (int i = 0; i < dt.Rows.Count; i++)
//             {
//                 bukobj.Book_Id = Convert.ToInt32(dt.Rows[i]["Book_Id"]);
//                 bukobj.BookName = dt.Rows[i]["BookName"].ToString();
//                 bukobj.MRP = Convert.ToInt32(dt.Rows[i]["MRP"]);
//                 // bukobj.Language_id = Convert.ToInt32(dt.Rows[i]["Language_id"]);
//                 bukobj.Language = dt.Rows[i]["Language"].ToString();
//                 // bukobj.Author_Id = Convert.ToInt32(dt.Rows[i]["Author_Id"]);
//                 bukobj.AuthorName = dt.Rows[i]["AuthorName"].ToString();
//                 // bukobj.Publisher_id = Convert.ToInt32(dt.Rows[i]["Publisher_id"]);
//                 bukobj.PublisherName = dt.Rows[i]["PublisherName"].ToString();
//                 bukobj.Published_Date = Convert.ToDateTime(dt.Rows[i]["Published_Date"]);
//                 bukobj.Volume = dt.Rows[i]["Volume"].ToString();
//                 list.Add(bukobj);
//                 // var customer = await context.Customers.Where(a => a.Id == Id).FirstOrDefaultAsync();

//             }

//             return list;
//         }
//------------------------ADDBOOK----------------------------------------------------
//   [HttpPost("AddBook")]

//         public string Post([FromBody] Book bukobj)
//         {

//             var header = (string) HttpContext.Request.Headers["Authorization"];
//             DbOperation DbObj = new DbOperation();
//             string Tokens=string.Empty;
//             Tokens=DbObj.Tokenvalidation(header);

//             string msg = string.Empty;

//             try
//             {
               

//                 msg = Db.AddBookDetails(bukobj);

//             }
//             catch (Exception ex)
//             {
//                 msg = ex.Message;
//             }
//             return msg;
            
//         }


//=============================Before pagination====================================================
//  [HttpGet]
//        public async Task<MyCustomApiResponse> Get()
//         {
//             var header = (string) HttpContext.Request.Headers["Authorization"];
             
//             DbOperation DbObj = new DbOperation();
           
//             string Tokens=string.Empty;
           
//             Tokens=DbObj.Tokenvalidation(header);



            
//             string Tokens1 = "Token is Valid";

//             await Task.Delay(1);


//             DataTable dt = new DataTable();
//             string msg = string.Empty;
//             dt = Db.GetBookView(out msg);
//             List<Book> list = new List<Book>();


//          if(Tokens==Tokens1)

//         {

//             if(dt.Rows.Count>0)
           
//             {

//             for (int i = 0; i < dt.Rows.Count; i++)
//             {
//                 Book bukobj=new Book();

//                 bukobj.Book_Id = Convert.ToInt32(dt.Rows[i]["Book_Id"]);
//                 bukobj.BookName = dt.Rows[i]["BookName"].ToString();
//                 bukobj.MRP = Convert.ToInt32(dt.Rows[i]["MRP"]);
//                 bukobj.Language = dt.Rows[i]["Language"].ToString();
//                 bukobj.AuthorName = dt.Rows[i]["AuthorName"].ToString();
//                 bukobj.PublisherName = dt.Rows[i]["PublisherName"].ToString();
//                 bukobj.Published_Date = Convert.ToDateTime(dt.Rows[i]["Published_Date"]);
//                 bukobj.Volume = dt.Rows[i]["Volume"].ToString();
//                 list.Add(bukobj);
//             }
//             return new MyCustomApiResponse(list,"Success",200);
            
//             }

//         else
//            {
                 
//              return new MyCustomApiResponse(list,"Not Success",200);

//             }

//            }
//     else
//      {
//          return new MyCustomApiResponse(list,"UnAuthorized",401);
//      }
//         }

        
        
        