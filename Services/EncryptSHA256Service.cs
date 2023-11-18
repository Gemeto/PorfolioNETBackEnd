using System.Security.Cryptography;
using System.Text;

namespace PorfolioWeb.Services
{
    public class EncryptSHA256Service
    {
        private readonly ASCIIEncoding _encoding;
        private readonly StringBuilder _sb;
        private readonly SHA256 _sha256;
        public EncryptSHA256Service()
        {
            _encoding = new ASCIIEncoding();
            _sb = new StringBuilder();
            _sha256 = SHA256.Create();
        }
        public async Task<string> GetSHA256(string str)
        {
            byte[]? stream = null;
            Stream hash = new MemoryStream(_encoding.GetBytes(str));
            stream = await _sha256.ComputeHashAsync(hash);
            for (int i = 0; i < stream.Length; i++) _sb.AppendFormat("{0:x2}", stream[i]);
            return _sb.ToString();
        }

    }
}
