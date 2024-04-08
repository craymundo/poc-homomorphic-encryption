using Microsoft.Research.SEAL;
using System;

namespace Exercise
{
    class Program
    {
        static void Main(string[] args)
        {
           

            EncryptionParameters parms = new EncryptionParameters(SchemeType.BFV);

            ulong polyModulusDegree = 4096;
            parms.PolyModulusDegree = polyModulusDegree;
            parms.CoeffModulus = CoeffModulus.BFVDefault(polyModulusDegree);
            parms.PlainModulus = new Modulus(1024);

            SEALContext context = new SEALContext(parms);

             KeyGenerator keygen = new KeyGenerator(context);
             SecretKey secretKey = keygen.SecretKey;
            keygen.CreatePublicKey(out PublicKey publicKey);


            Encryptor encryptor = new Encryptor(context, publicKey);
            Decryptor decryptor = new Decryptor(context, secretKey);
            Evaluator evaluator = new Evaluator(context);

            int num1 = 5;
            int num2 = 7;

            Plaintext plaintext1 = new Plaintext(num1.ToString());
            Plaintext plaintext2 = new Plaintext(num2.ToString());

            Ciphertext ciphertext1 = new Ciphertext();
            Ciphertext ciphertext2 = new Ciphertext();
            encryptor.Encrypt(plaintext1, ciphertext1);
            encryptor.Encrypt(plaintext2, ciphertext2);

            // Realizar la multiplicación homomórfica
            Ciphertext encryptedResult = new Ciphertext();
            evaluator.Multiply(ciphertext1, ciphertext2, encryptedResult);

            // Descifrar el resultado
            Plaintext plaintextResult = new Plaintext();
            decryptor.Decrypt(encryptedResult, plaintextResult);

            int result;
            if (int.TryParse(plaintextResult.ToString(), out result))
            {
                Console.WriteLine("Resultado de la multiplicación homomórfica: " + result);
            }
            else
            {
                Console.WriteLine("No se pudo descifrar el resultado correctamente.");
            }
        }
    }
}
