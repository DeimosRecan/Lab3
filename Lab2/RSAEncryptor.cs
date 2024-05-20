using LCD;
using Mod;
using Soloway;
using System.Text;

namespace _RSAEncryptor
{
    public class RSA
    {
        private long? q = null, p = null;

        public RSA(long q, long p) { 
            this.q = q; 
            this.p = p;
        }

        public RSA() { }

        public static (long e, long d, long n) GenerateKeys(long q, long p)
        {
            if (!SolowayTest.Test((long)p, 10) || !SolowayTest.Test((long)q, 10))
            {
                throw new ArgumentException("q or p was not a prime number");
            }

            long n = (long)p * (long)q;
            long phi = (long)(p - 1) * (long)(q - 1);

            long d = 2;

            long? e = null;
            Random rand = new Random();
            while (e == null) { 
            e = rand.NextInt64(3, n);
            while((LCD.LCD.GetLCD((long)e, phi) != 1))
            {
                e++;
                if (e < phi)
                    {
                        e = null;
                        break;
                    }
            }
            }
            Console.WriteLine($"E = {e}, phi = {phi}");

            while (true)
            {
                if (((e * d) % phi == 1) && (e != d))
                {
                    break;
                }
                else
                {
                    d++;
                }
            }
            Console.WriteLine($"D = {d}");
            return ((long)e, d, n);
        }

        public static (long e, long d, long n) ReadKeysFromFile(string openKeyPath, string closeKeyPath)
        {
            string openKeyText = File.ReadAllText(openKeyPath);
            string[] openKey = openKeyText.Split(" ");
            string closeKeyText = File.ReadAllText(closeKeyPath);
            string[] closeKey = closeKeyText.Split(" ");

            if (openKey[1] != closeKey[1])
            {
                throw new Exception("n != n");
            }

            return (long.Parse(openKey[0]), long.Parse(closeKey[0]), long.Parse(closeKey[1]));
        }

        public static void Encrypt(string filePath, string encryptedFilePath, string openKeyFilePath)
        {
            string openKeyText = File.ReadAllText(openKeyFilePath);

            string[] openKey = openKeyText.Split(" ");
            long e = long.Parse(openKey[0]);
            long n = long.Parse(openKey[1]);

            if (!File.Exists(filePath))
            {
                throw new Exception("file doesn't exist");
            }

            string fileText = File.ReadAllText(filePath).ToLower();
            fileText = string.Join("", fileText.Where(s => Alphabet.abc.Keys.Contains(s)));

            Console.WriteLine($"TEXT = {fileText}");

            StringBuilder encodedText = new StringBuilder();

            foreach(var symb in fileText)
            {
                encodedText.Append(Alphabet.abc[symb]);
            }

            Console.WriteLine(encodedText.ToString());
            int encodedTextLength = encodedText.Length;

            List<long> cis = new List<long>();

            for(int i = 0; i < encodedTextLength; i++)
            {
                long mi = (long)char.GetNumericValue(encodedText[i]);
                {
                    for(int j = i+1;  j < encodedTextLength; j++)
                    {
                        if((mi * 10 + (long)char.GetNumericValue(encodedText[j])) < n)
                        {
                            mi *= 10;
                            mi += (long)char.GetNumericValue(encodedText[j]);
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                long ci = ModFunction.GetMod(mi, e, n);

                Console.WriteLine($"mi = {mi}, pow {e}, mod {n} = {ci}");

                cis.Add(ci);
            }

            string encryptedText = string.Join(" ", cis);
            File.WriteAllText(encryptedFilePath, encryptedText);
        }

        public static void Decrypt(string encryptedFilePath, string decryptedFilePath, string closeKeyFilePath)
        {

            string closeKeyText = File.ReadAllText(closeKeyFilePath);
            string[] closeKey = closeKeyText.Split(" ");
            long d = long.Parse(closeKey[0]);
            long n = long.Parse(closeKey[1]);

            string encryptedText = File.ReadAllText(encryptedFilePath);
            List<long> cis = encryptedText.Split(" ").Select(ci => long.Parse(ci)).ToList();

            StringBuilder encodedText = new StringBuilder();
            List<char> decodedText = new List<char>();

            foreach(long ci in cis) {
                long mi = ModFunction.GetMod(ci, d, n);
                encodedText.Append(mi.ToString());
            }

            for(int i = 0; i < encodedText.Length-1; i+=2)
            {
                int code = (int)char.GetNumericValue(encodedText[i]) * 10 + (int)char.GetNumericValue(encodedText[i + 1]);
                Console.WriteLine($"CODE: {code}");
                char symb = Alphabet.abc.Keys.First(k => Alphabet.abc[k] == code);
                decodedText.Add(symb);
            }

            string decryptedText = string.Join("", decodedText);

            File.WriteAllText(decryptedFilePath, decryptedText);
        }

    }
}