using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TopeServer.al.aldi.utils.security
{
    class EncryptionUtils
    {
        public static X509Certificate2 GenerateCertificate(string certName)
        {
            var keypairgen = new RsaKeyPairGenerator();
            keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 1024));

            AsymmetricCipherKeyPair keypair = keypairgen.GenerateKeyPair();

            var gen = new X509V3CertificateGenerator();

            var CN = new X509Name("CN=" + certName);
            var SN = BigInteger.ProbablePrime(120, new Random());

            gen.SetSerialNumber(SN);
            gen.SetSubjectDN(CN);
            gen.SetIssuerDN(CN);
            gen.SetNotAfter(DateTime.Now.AddYears(3));
            gen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)));
            gen.SetSignatureAlgorithm("SHA1withRSA");
            gen.SetPublicKey(keypair.Public);

            // generate new certificate out of the key pairs private key
            var newCert = gen.Generate(keypair.Private);

            X509Certificate2 cert = new X509Certificate2(DotNetUtilities.ToX509Certificate((Org.BouncyCastle.X509.X509Certificate)newCert));
            
            return ConvertToWindows(newCert, keypair);
        }

        public static X509Certificate2 ConvertToWindows(Org.BouncyCastle.X509.X509Certificate newCert, AsymmetricCipherKeyPair kp)
        {
            var tempStorePwd = Program.FILE_CERT_PASSWORD;
            var tempStoreFile = new FileInfo(Path.GetTempFileName());

            try
            {
                // store key 
                {
                    var newStore = new Pkcs12Store();

                    var certEntry = new X509CertificateEntry(newCert);

                    newStore.SetCertificateEntry(
                        Environment.MachineName,
                        certEntry
                        );

                    newStore.SetKeyEntry(
                        Environment.MachineName,
                        new AsymmetricKeyEntry(kp.Private),
                        new[] { certEntry }
                        );

                    using (var s = tempStoreFile.Create())
                    {
                        newStore.Save(
                            s,
                            tempStorePwd.ToCharArray(),
                            new SecureRandom(new CryptoApiRandomGenerator())
                            );
                    }
                }

                // reload key 
                return new X509Certificate2(tempStoreFile.FullName, tempStorePwd, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
            finally
            {
                tempStoreFile.Delete();
            }
        }


        /// <summary>
        /// Install certificate into the local machine
        /// </summary>
        /// <param name="cerFileName">path of certificate</param>
        public static void InstallCertificate(string cerFileName, string password, StoreName storeName)
        {
            X509Certificate2 certificate = new X509Certificate2(cerFileName, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            X509Store store = new X509Store(storeName, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();
        }

        /// <summary>
        /// Install certificate into the local machine
        /// </summary>
        /// <param name="cerFileName">path of certificate</param>
        public static void InstallCertificate( string password, StoreName storeName)
        {
            InstallCertificate(Program.FILE_CERT_NAME, password, storeName);
        }

        /// <summary>
        /// Install certificate into the local machine
        /// </summary>
        /// <param name="cerFileName">certificate</param>
        public static void InstallCertificate(X509Certificate2 certificate)
        {
            InstallCertificate(certificate, StoreName.TrustedPublisher);
        }

        /// <summary>
        /// Install certificate into the local machine
        /// </summary>
        /// <param name="cerFileName">certificate</param>
        /// /// <param name="storeName">location</param>
        public static void InstallCertificate(X509Certificate2 certificate, StoreName storeName)
        {
            X509Store store = new X509Store(storeName, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();
        }

        public static void SaveCertificateToFile(X509Certificate2 certificate, String password, String fileName)
        {
            File.WriteAllBytes(fileName, certificate.Export(X509ContentType.Pfx, password));
        }

        public static void SaveCertificateToFile(X509Certificate2 certificate, String password)
        {
            SaveCertificateToFile(certificate, password, Program.FILE_CERT_NAME);
        }

        public static void RemoveCertificate(string cerFileName)
        {
            X509Store store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadWrite);
            var certificates = store.Certificates;
            
            foreach (var certificate in certificates)
            {
                var friendlyName = certificate.FriendlyName;
                var xname = certificate.SubjectName.Name; 
                if(xname.Equals("CN="+cerFileName)){
                    store.Remove(certificate);
                }
                
            }

            store.Close();

            store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadWrite);
            certificates = store.Certificates;

            foreach (var certificate in certificates)
            {
                var friendlyName = certificate.FriendlyName;
                var xname = certificate.SubjectName.Name;
                if (xname.Equals("CN=" + cerFileName))
                {
                    store.Remove(certificate);
                }

            }

            store.Close();

            store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadWrite);
            certificates = store.Certificates;

            foreach (var certificate in certificates)
            {
                var friendlyName = certificate.FriendlyName;
                var xname = certificate.SubjectName.Name;
                if (xname.Equals("CN=" + cerFileName))
                {
                    store.Remove(certificate);
                }

            }

            store.Close();

            
        }
    }
}
