using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace {{cookiecutter.project_slug}}.Api.IntegrationTests
{
    public class JwtTokenIssuer
    {
        public static string Issuer = "https://testissuer.example.com";
        public string Jwks { get; private set; }

        readonly X509Certificate2 signingCert;

        public SigningCredentials SigningCredentials => new SigningCredentials(new X509SecurityKey(signingCert), "RS256");

        public JwtTokenIssuer()
        {
            this.signingCert = this.BuildSelfSignedServerCertificate();
            var rsaParams = signingCert.GetRSAPrivateKey().ExportParameters(true);

            Jwks = JsonConvert.SerializeObject(new
            {
                keys = new [] {
                    new {
                        alg = "RS256",
                        kty = "RSA",
                        use = "sig",
                        x5c = new string[] { Convert.ToBase64String(signingCert.RawData) },
                        e = Convert.ToBase64String(rsaParams.Exponent),
                        n = Convert.ToBase64String(rsaParams.Modulus),
                        kid = signingCert.Thumbprint,
                        x5t = signingCert.Thumbprint
                    }
                }
            });
        }

        public string GetToken()
        {
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "TestAudience",
                Issuer = JwtTokenIssuer.Issuer,
                NotBefore = DateTime.Now - TimeSpan.FromHours(1),
                Expires = DateTime.Now + TimeSpan.FromHours(1),
                SigningCredentials = SigningCredentials,
                Subject = new ClaimsIdentity(new Claim[] { new Claim("sub", "user@example.com") })
            };

            var h = new JwtSecurityTokenHandler();
            var token = h.CreateJwtSecurityToken(securityTokenDescriptor);
            return h.WriteToken(token);
        }

        private X509Certificate2 BuildSelfSignedServerCertificate()
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN=https://issuer.com");

            using RSA rsa = RSA.Create(2048);
            var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));


            request.CertificateExtensions.Add(
               new X509EnhancedKeyUsageExtension(
                   new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

            request.CertificateExtensions.Add(sanBuilder.Build());

            var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));

            return certificate;
        }
    }
}
