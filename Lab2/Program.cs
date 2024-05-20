using _RSAEncryptor;


//const string openTextFilePath = "..\\..\\..\\..\\openText.txt";
//const string encryptedTextFilePath = "..\\..\\..\\..\\encryptedText.txt";
//const string openKeyFilePath = "..\\..\\..\\..\\openKey.txt"
//const string closeKeyFilePath = "..\\..\\..\\..\\closeKey.txt"
//const string keysDirPath = "..\\..\\..\\..\\";
//const string decryptedTextFilePath = "..\\..\\..\\..\\decryptedText.txt";


string cmd = "";
string openTextFilePath, encryptedTextFilePath, keysDirPath, decryptedTextFilePath;
long q, p;

while (true)
{
    Console.Write("enter command (encrypt, decrypt, cls, exit, generate): ");
    cmd = Console.ReadLine().Trim();

    switch (cmd)
    {
        case "generate":
            {
                Console.WriteLine("q, p: ");
                string qp = Console.ReadLine();
                string[] _qp = qp.Split(' ');
                q = long.Parse(_qp[0]);
                p = long.Parse(_qp[1]);
                if (q == p)
                {
                    Console.WriteLine("q must not be equal to p");
                    break;
                }
                long e, d, n;
                try { 
                (e, d, n) = RSA.GenerateKeys(q, p);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }

                while (true) { 
                Console.Write("save keys? (y/n): ");

                    string save_keys_answ = Console.ReadLine().Trim().ToLower();
                    if (save_keys_answ == "y")
                    {
                        save_open_key:
                        Console.WriteLine("open key file path: ");
                        string open_key_file_path = Console.ReadLine();
            
                        try
                        {
                            string _openKey = $"{e} {n}";
                            File.WriteAllText(open_key_file_path, _openKey);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            goto save_open_key;
                        }

                    save_close_key:
                        Console.WriteLine("close key file path: ");
                        string close_key_file_path = Console.ReadLine();

                        try
                        {
                            string _closeKey = $"{d} {n}";
                            File.WriteAllText(close_key_file_path, _closeKey);
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex.Message);
                            goto save_close_key;
                        }
                    }
                    else if (save_keys_answ == "n")
                    {
                        break;
                    }
                    break;
                }
                break;
            }
        case "encrypt":
            {
                Console.Write("open text file path: ");
                openTextFilePath = Console.ReadLine();

                Console.WriteLine("encrypted text file path: ");
                encryptedTextFilePath = Console.ReadLine();

                Console.WriteLine("open key file path: ");
                string openKeyFilePath = Console.ReadLine();

                try { 
                RSA.Encrypt(openTextFilePath, encryptedTextFilePath, openKeyFilePath);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }

                break;
            }
        case "decrypt":
            {
                Console.WriteLine("encrypted text file path: ");
                encryptedTextFilePath = Console.ReadLine();

                Console.WriteLine("decrypted text file path: ");
                decryptedTextFilePath = Console.ReadLine();

                Console.WriteLine("close key file path: ");
                string closeKeyFilePath = Console.ReadLine();

                try { 
                RSA.Decrypt(encryptedTextFilePath, decryptedTextFilePath, closeKeyFilePath);
                }
                catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                    break;
                }

                break;
            }
        case "cls":
            {
                System.Console.Clear();
                break;
            }
        case "exit":
            {
                return;
            }
    }
}
