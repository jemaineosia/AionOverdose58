using System.Security.Cryptography;

namespace AionOverdose58.Shared.Utility;

/// <summary>
/// Utility class for Aion password encryption using the game's proprietary algorithm.
/// </summary>
public static class AionEncrypt
{
    /// <summary>
    /// Encrypts a password and returns it as a byte array.
    /// </summary>
    /// <param name="password">The plain text password to encrypt.</param>
    /// <returns>The encrypted password as a byte array.</returns>
    public static byte[] EncryptPasswordInByte(string password)
    {
        var encryptedPass = EncryptPassword(password);

        byte[] result = Enumerable.Range(0, encryptedPass.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(encryptedPass.Substring(x, 2), 16))
                     .ToArray();

        return result;
    }

    /// <summary>
    /// Encrypts a web password using SHA1 and Base64 encoding.
    /// </summary>
    /// <param name="password">The plain text password to encrypt.</param>
    /// <returns>The encrypted password as a Base64 string.</returns>
    public static string EncryptWebPassword(string password)
    {
        byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        using SHA1 sha = SHA1.Create();
        byte[] hashBytes = sha.ComputeHash(passwordBytes);
        string passwordEnc = Convert.ToBase64String(hashBytes);

        return passwordEnc;
    }

    /// <summary>
    /// Encrypts a password using Aion's proprietary encryption algorithm.
    /// </summary>
    /// <param name="str">The plain text password to encrypt.</param>
    /// <returns>The encrypted password as a hexadecimal string.</returns>
    public static string EncryptPassword(string str)
    {
        int nBytes = str.Length;
        int i = 0;
        int[] key = new int[17];
        int[] dst = new int[17];

        while (i < nBytes)
        {
            i++;
            key[i] = str[i - 1];
            dst[i] = key[i];
        }

        long rslt = key[1] + key[2] * 256 + key[3] * 65536 + key[4] * 16777216;
        long one = rslt * 213119 + 2529077;
        one = one - (long)(one / 4294967296) * 4294967296;

        rslt = key[5] + key[6] * 256 + key[7] * 65536 + key[8] * 16777216;
        long two = rslt * 213247 + 2529089;
        two = two - (long)(two / 4294967296) * 4294967296;

        rslt = key[9] + key[10] * 256 + key[11] * 65536 + key[12] * 16777216;
        long three = rslt * 213203 + 2529589;
        three = three - (long)(three / 4294967296) * 4294967296;

        rslt = key[13] + key[14] * 256 + key[15] * 65536 + key[16] * 16777216;
        long four = rslt * 213821 + 2529997;
        four = four - (int)(four / 4294967296) * 4294967296;

        key[4] = (int)(one / 16777216);
        key[3] = (int)((one - key[4] * 16777216) / 65536);
        key[2] = (int)((one - key[4] * 16777216 - key[3] * 65536) / 256);
        key[1] = (int)((one - key[4] * 16777216 - key[3] * 65536 - key[2] * 256));

        key[8] = (int)(two / 16777216);
        key[7] = (int)((two - key[8] * 16777216) / 65536);
        key[6] = (int)((two - key[8] * 16777216 - key[7] * 65536) / 256);
        key[5] = (int)((two - key[8] * 16777216 - key[7] * 65536 - key[6] * 256));

        key[12] = (int)(three / 16777216);
        key[11] = (int)((three - key[12] * 16777216) / 65536);
        key[10] = (int)((three - key[12] * 16777216 - key[11] * 65536) / 256);
        key[9] = (int)((three - key[12] * 16777216 - key[11] * 65536 - key[10] * 256));

        key[16] = (int)(four / 16777216);
        key[15] = (int)((four - key[16] * 16777216) / 65535);
        key[14] = (int)((four - key[16] * 16777216 - key[15] * 65536) / 256);
        key[13] = (int)((four - key[16] * 16777216 - key[15] * 65536 - key[14] * 256));

        dst[1] = dst[1] ^ key[1];
        i = 1;
        while (i < 16)
        {
            i++;
            dst[i] = (byte)(dst[i] ^ dst[i - 1] ^ key[i]);
        }

        i = 0;
        while (i < 16)
        {
            i++;
            if (dst[i] == 0)
            {
                dst[i] = 102;
            }
        }

        i = 0;
        string encrypt = "";

        while (i < 16)
        {
            i++;
            if (dst[i] < 16)
            {
                encrypt += "0" + dst[i].ToString("x");
            }
            else
            {
                encrypt += dst[i].ToString("x");
            }
        }

        return encrypt;
    }
}
