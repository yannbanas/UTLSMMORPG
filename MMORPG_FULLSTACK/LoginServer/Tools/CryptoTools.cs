using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Tools
{
    public static class CryptoTools
    {
        // Génère une clé secrète aléatoire pour le chiffrement ou HMAC
        public static byte[] GenerateKey(int length)
        {
            using RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomKey = new byte[length];
            rng.GetBytes(randomKey);
            return randomKey;
        }

        // Génère un HMAC pour une donnée donnée avec une clé secrète
        public static byte[] GenerateHMAC(byte[] data, byte[] key)
        {
            using HMACSHA256 hmac = new HMACSHA256(key);
            return hmac.ComputeHash(data);
        }

        // Génère un nonce (nombre utilisé une seule fois) sécurisé
        public static byte[] GenerateSecureNonce(int length)
        {
            return GenerateKey(length); // Utilise la même méthode que pour la clé car elle génère des bytes aléatoires
        }

        // Convertit un tableau de bytes en chaîne hexadécimale pour le stockage ou l'affichage
        public static string BytesToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        // Convertit une chaîne hexadécimale en tableau de bytes
        public static byte[] HexStringToBytes(string hexString)
        {
            int length = hexString.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return bytes;
        }
    }
}
