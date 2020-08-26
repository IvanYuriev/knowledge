using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AsyncAwait.Services
{

    internal class Hasher
    {
        private static HashAlgorithm _hasher = System.Security.Cryptography.SHA256.Create();
        public string GetBase64Hash(byte[] responseBytes)
        {
            if (responseBytes.Length <= 0) return String.Empty;

            int salt = -4_000_000;
            var byteCopy = new byte[responseBytes.Length + 4];
            responseBytes.CopyTo(byteCopy, 4);
            ApplySalt(salt, byteCopy);
            var hash = _hasher.ComputeHash(byteCopy);
            var foundFlag = "N_";
            while (salt++ < 4_000_000)
            {
                if (hash.Take(8).All(b => b == 0))
                {
                    foundFlag = "F_";
                    break;
                }
                ApplySalt(salt, byteCopy);
                hash = _hasher.ComputeHash(byteCopy);
            }
            return foundFlag + Convert.ToBase64String(hash);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ApplySalt(int salt, byte[] bytes)
        {
            bytes[0] = (byte) (salt >> 24);
            bytes[1] = (byte) (salt >> 16);
            bytes[2] = (byte) (salt >> 8);
            bytes[3] = (byte) (salt);
        }
    }

}