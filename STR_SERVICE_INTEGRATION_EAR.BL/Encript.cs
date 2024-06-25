using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class Encript
    {
        private static string clave_secret { get; set; }
        private static string aesIV { get; set; }
        public Encript()
        {
            clave_secret = ConfigurationManager.AppSettings["aesSecret"];
            aesIV = ConfigurationManager.AppSettings["aesIV"];
        }

        public string DesencriptaPass(string pass)
        {
            // Contraseña que desencriptar (en texto claro)
            string encryptedPassword = pass;

            // Convertir la clave secreta y el IV de cadenas a bytes
            List<Byte[]> lista = SetKey(clave_secret, aesIV);

            byte[] secretKeyBytes = lista[0];
            byte[] ivBytes = lista[1];

            string decryptedPassword;
            // Crear un objeto Aes para cifrar
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = secretKeyBytes;
                aesAlg.IV = ivBytes;

                // Crear un cifrador
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Convertir la contraseña en texto claro a bytes
                byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);

                // Cifrar la contraseña
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                // Convertir la contraseña cifrada a Base64 (para almacenamiento)
                decryptedPassword = Encoding.UTF8.GetString(decryptedBytes);

                Console.WriteLine("Contraseña cifrada: " + decryptedPassword);


            }
            return decryptedPassword;

        }
        public bool ValidarCredenciales(string passActual, string password)
        {

            // Contraseña que deseas encriptar (en texto claro)
            string plainTextPassword = password;

            // Convertir la clave secreta y el IV de cadenas a bytes
            List<Byte[]> lista = SetKey(clave_secret, aesIV);

            byte[] secretKeyBytes = lista[0];
            byte[] ivBytes = lista[1];

            string encryptedPassword;
            // Crear un objeto Aes para cifrar
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = secretKeyBytes;
                aesAlg.IV = ivBytes;

                // Crear un cifrador
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Convertir la contraseña en texto claro a bytes
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainTextPassword);

                // Cifrar la contraseña
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

                // Convertir la contraseña cifrada a Base64 (para almacenamiento)
                encryptedPassword = Convert.ToBase64String(encryptedBytes);

                Console.WriteLine("Contraseña cifrada: " + encryptedPassword);

            }
            if (encryptedPassword == passActual)
                return true;
            return false;
        }


        public static List<Byte[]> SetKey(string securityAESSecret, string securityAESIV)
        {

            byte[] secretKey;
            byte[] iv;
            // Convierte la cadena securityAESSecret en bytes UTF-8
            byte[] keyBytes = Encoding.UTF8.GetBytes(securityAESSecret);

            // Calcula un resumen hash SHA-1 de la clave secreta
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashedKeyBytes = sha1.ComputeHash(keyBytes);

                // Ajusta la longitud de la clave a 16 bytes
                Array.Resize(ref hashedKeyBytes, 16);

                // Asigna la clave secreta
                secretKey = hashedKeyBytes;
            }

            // Convierte la cadena securityAESIV en bytes UTF-8
            byte[] ivBytes = Encoding.UTF8.GetBytes(securityAESIV);

            // Ajusta la longitud del IV a 16 bytes
            Array.Resize(ref ivBytes, 16);

            // Asigna el IV
            iv = ivBytes;

            return new List<Byte[]> { secretKey, iv };
        }
    }
}
