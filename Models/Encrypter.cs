using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;



namespace Librarymanagement.Models
{
    public class Encrypter
    {
      
        public string MD5Hash1(Lib_Users usobj)

        {
           
            MD5 md5 = new MD5CryptoServiceProvider();


            //compute hash from the bytes of text  

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(usobj.Password));

            //get hash result after compute it  

            byte[]? result1 = md5.Hash;


            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < result1?.Length; i++)

            {
                //change it into 2 hexadecimal digits  

                //for each byte  

                strBuilder.Append(result1[i].ToString("x2"));

            }

            return strBuilder.ToString();
        }

        public string MD5Hash2(login logobj)

        {
           
            MD5 md5 = new MD5CryptoServiceProvider();


            //compute hash from the bytes of text  

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(logobj.Password));

            //get hash result after compute it  

            byte[]? result1 = md5.Hash;


            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < result1?.Length; i++)

            {
                //change it into 2 hexadecimal digits  

                //for each byte  

                strBuilder.Append(result1[i].ToString("x2"));

            }

            return strBuilder.ToString();
        }
    }
}