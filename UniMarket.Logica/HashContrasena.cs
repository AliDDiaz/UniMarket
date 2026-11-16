using System;
using System.Security.Cryptography;

namespace UniMarket.Logica
{
    public static class HashContrasena
    {
        private const int TamanoSal = 16;
        private const int TamanoHash = 32;
        private const int Iteraciones = 100000;

        public static string Generar(string contrasena)
        {
            byte[] sal = new byte[TamanoSal];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(sal);

            byte[] hash = Calcular(contrasena, sal, Iteraciones);
            return $"{Iteraciones}.{Convert.ToBase64String(sal)}.{Convert.ToBase64String(hash)}";
        }

        public static bool Verificar(string contrasena, string guardado)
        {
            if (string.IsNullOrEmpty(guardado)) return false;

            var partes = guardado.Split('.');
            if (partes.Length != 3) return false;
            if (!int.TryParse(partes[0], out int iteraciones)) return false;

            try
            {
                byte[] sal = Convert.FromBase64String(partes[1]);
                byte[] esperado = Convert.FromBase64String(partes[2]);
                byte[] calculado = Calcular(contrasena, sal, iteraciones);
                return SonIguales(esperado, calculado);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] Calcular(string contrasena, byte[] sal, int iteraciones)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                contrasena, sal, iteraciones, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(TamanoHash);
            }
        }

        private static bool SonIguales(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int diferencia = 0;
            for (int i = 0; i < a.Length; i++)
                diferencia |= a[i] ^ b[i];
            return diferencia == 0;
        }
    }
}