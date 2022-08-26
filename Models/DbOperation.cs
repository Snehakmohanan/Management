using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Librarymanagement.Models
{
    public class DbOperation
    {
        Lib_Users usobj = new Lib_Users();
        login logobj=new login();
        Book bukobj = new Book();
        Encrypter enobj = new Encrypter();

        Author Aobj=new Author();

        Languagee Lobj=new Languagee();

        Publisher Pobj=new Publisher();
        GetBook Gtobj=new GetBook();
    

        //SqlConnection con = new SqlConnection("Data Source=192.168.12.11;Initial Catalog=LibrarySystemSneha;USER ID=hrconnect;password=mm123");
        SqlConnection con = new SqlConnection("Data Source=MMFL-CO250\\SQLEXPRESS;Initial Catalog=LibrarySystemSnehaa;Integrated Security=True");

        DataTable dt = new DataTable();


        //--------------------LOGIN-----------------------------------------------------------
        

        public string CreateLogin(Lib_Users usobj)
        {
            string msg = string.Empty;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Lib_CreateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", usobj.Name);
                cmd.Parameters.AddWithValue("@Username", usobj.Username);
                cmd.Parameters.AddWithValue("@Password", usobj.EncryptPassword);
                cmd.Parameters.AddWithValue("@Status", usobj.Status);
                // cmd.Parameters.AddWithValue("@CreateDate", usobj.CreateDate);
                // cmd.Parameters.AddWithValue("@Token",logobj.Token);
                
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    msg = "Successfully Add New User";
                }
                else if (i <= 0)
                {
                    msg = "Failed to Add New User";
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            return msg;
        }
        //----------------------------------------------------------------------------------------------------
        public DataTable  GetLogin(login logobj)
        {
            string msg = string.Empty;

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_LOGINPAGE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", logobj.Username);
                cmd.Parameters.AddWithValue("@Password", logobj.EncryptPassword);
                cmd.Parameters.AddWithValue("@Token",logobj.Token);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    msg = "Login Failed";
                }
                else
                {
                    msg = "login success";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                con.Close();

            }

            return dt;

        }



//-----------------------------TOKEN VALIDATION---------------------------------------------------------

        public string  Tokenvalidation(string Token)
        {
            string msg = string.Empty;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_JWTToken", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@Token",Token);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    msg = "Token is Valid";
                }
                else
                {
                    msg = "Token is not valid";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {        
                con.Close();

            }

            return msg;

        }

       //----------------//ADD-BOOK--------------------------------------------------------------------------------------------------

         
         public string AddBookDetails(Book bukobj)
           {
            string msg = string.Empty;
             
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_AddBook", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@BookName", bukobj.BookName);
                cmd.Parameters.AddWithValue("@Language", bukobj.Language);
                cmd.Parameters.AddWithValue("@MRP", bukobj.MRP);
                cmd.Parameters.AddWithValue("@Publisher_Id", bukobj.Publisher_id);
                cmd.Parameters.AddWithValue("@Published_Date", bukobj.Published_Date);
                cmd.Parameters.AddWithValue("@Volume", bukobj.Volume);
                cmd.Parameters.AddWithValue("@status", bukobj.Status);
                cmd.Parameters.AddWithValue("@Author_Id ", bukobj.Author_Id );
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    msg = "Book Added Successfully";

                    //  Console.WriteLine(msg);
                }
                else if (i <= 0)
                {
                    msg = "Book not Added";
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            return msg;
        }



        //--------------//Get Book Details by ID------------------------------------------------------------------------------------
        
        
        public DataTable GetBookDetails(GetBook Gtobj, out string msg)
        {
            msg = string.Empty;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SPLibrarySearchBook", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Book_Id", Gtobj.Book_Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
                con.Close();
                return dt;


            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                con.Close();

            }

            return dt;
        }

        //------------------------ BOOK VIEW---------------------------------------------------------------------------
       

        public DataTable GetBookView(out string msg,int pgno,int pgsize)
        {
            msg = string.Empty;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_LibraryViewBook", con);
                cmd.CommandType = CommandType.StoredProcedure;
                Console.WriteLine(pgno);
                cmd.Parameters.AddWithValue("@PageNo", pgno);
                cmd.Parameters.AddWithValue("@PageSize ", pgsize);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                con.Close();

            }

            return dt;
        }

        //----------------- // UPDATE BOOK-----------------------------------------------------------------------------
       


        public string UpdateBookDetails(Book bukobj)
        {
            string msg = string.Empty;
            
             
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_UpdateBook", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Book_Id", bukobj.Book_Id);
                cmd.Parameters.AddWithValue("@BookName", bukobj.BookName);
                cmd.Parameters.AddWithValue("@Language", bukobj.Language);
                cmd.Parameters.AddWithValue("@MRP", bukobj.MRP);
                cmd.Parameters.AddWithValue("@Publisher_Id", bukobj.Publisher_id);
                cmd.Parameters.AddWithValue("@Published_Date", bukobj.Published_Date);
                cmd.Parameters.AddWithValue("@Volume", bukobj.Volume);
                cmd.Parameters.AddWithValue("@status", bukobj.Status);
                cmd.Parameters.AddWithValue("@Author_Id", bukobj.Author_Id);
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    msg = "Book Updated Successfully";
                     Console.WriteLine(msg);
                }
                else if (i <= 0)
                {
                    msg = "Book not updated";
                }

            }
              catch (Exception ex)
              {
                msg = ex.Message;
              }
             finally
             {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

             }
             return msg;
        }

        //------------------ //BOOK DELETE-----------------------------------------------------------------------------------
       
        public string DeleteBookDetails(Book bukobj)
        {
            string msg = string.Empty;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[SP_DeleteBook]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Book_Id", bukobj.Book_Id);
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    msg = "Data has been Deleted";
                }
                else if (i <= 0)
                {
                    msg = "Process Failed";
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            return msg;
        }
//--------------------------------GetAuthors-------------------------------------------------------------------
  
    
  


         public DataTable GetAuthors(Author Aobj,out string msg)
        {
            msg = string.Empty;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_ViewAuthors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                con.Close();

            }

            return dt;
        }

//--------------------------------GetLanguage-------------------------------------------------------------------
          public DataTable GetLanguage(Languagee Lobj,out string msg)
        {
            msg = string.Empty;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_ViewLanguage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                con.Close();

            }

            return dt;
        }

//--------------------------------GetPublisher-------------------------------------------------------------------
       public DataTable GetPublisher(Publisher Pobj,out string msg)
        {
            msg = string.Empty;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_ViewPublisher", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                con.Close();

            }

            return dt;
        }

    }
    }
