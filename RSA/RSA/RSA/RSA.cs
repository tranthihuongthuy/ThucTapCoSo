using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    public class KeyPair
    {
        public BigInteger PublicKey { get; set; }
        public BigInteger PrivateKey { get; set; }
    }

    public class TaoKhoa
    {
        protected BigInteger p, q, N, n, e, d;

        public TaoKhoa(BigInteger p, BigInteger q)
        {
            KhoiTao(p, q);
        }

        public TaoKhoa()
        {
        }

        protected virtual void KhoiTao(BigInteger p, BigInteger q)
        {
            this.p = p;
            this.q = q;
            this.N = p * q;
            this.n = (p - 1) * (q - 1);
            this.e = TimE(this.n);
            this.d = TimD(this.e, this.n);
        }

        protected virtual BigInteger TimE(BigInteger n)
        {
            for (BigInteger i = 2; i < n; i++)
            {
                if (BigInteger.GreatestCommonDivisor(i, n) == 1 && IsPrime(i))
                {
                    return i;
                }
            }

            throw new Exception("Không thể tìm số e.");
        }

        private static bool IsPrime(BigInteger number)
        {
            if (number <= 1)
            {
                return false;
            }

            for (BigInteger i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        protected virtual BigInteger TimD(BigInteger e, BigInteger n)
        {
            BigInteger d = 0;
            for (BigInteger k = 2; ; k++)
            {
                d = (k * n + 1) / e;

                if ((d * e) % n == 1 && d != e)
                {
                    return d;
                }
            }
        }

        public KeyPair TaoKhoaTuFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Đọc giá trị từ file và chia thành mảng các chuỗi
                string[] values = reader.ReadLine().Split(' ');

                // Chuyển đổi giá trị từ chuỗi sang BigInteger
                BigInteger p = BigInteger.Parse(values[0]);
                BigInteger q = BigInteger.Parse(values[1]);

                KhoiTao(p, q);

                return new KeyPair { PublicKey = e, PrivateKey = d };
            }
        }

        public Tuple<BigInteger, BigInteger> LayKhoaCongKhai()
        {
            return Tuple.Create(e, N);
        }

        public Tuple<BigInteger, BigInteger> LayKhoaRieng()
        {
            return Tuple.Create(d, N);
        }

        public BigInteger GetP()
        {
            return p;
        }

        public BigInteger GetQ()
        {
            return q;
        }

        public BigInteger GetN()
        {
            return n;
        }

        public BigInteger GetNModulus()
        {
            return N;
        }

        public BigInteger GetE()
        {
            return e;
        }

        public BigInteger GetD()
        {
            return d;
        }
    }

    public class RSAEncryptor : TaoKhoa
    {
        public RSAEncryptor(BigInteger p, BigInteger q) : base(p, q)
        {
        }

        protected override BigInteger TimE(BigInteger n)
        {
            for (BigInteger i = 2; i < n; i++)
            {
                if (BigInteger.GreatestCommonDivisor(i, n) == 1 && IsPrime(i))
                {
                    return i;
                }
            }

            throw new Exception("Không thể tìm số e trong RSAEncryptor.");
        }

        private static bool IsPrime(BigInteger number)
        {
            if (number <= 1)
            {
                return false;
            }

            for (BigInteger i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        protected override BigInteger TimD(BigInteger e, BigInteger n)
        {
            BigInteger d = 0;
            for (BigInteger k = 2; ; k++)
            {
                d = (k * n + 1) / e;

                if ((d * e) % n == 1 && d != e)
                {
                    return d;
                }
            }
        }

        public BigInteger MaHoaBaoMat(BigInteger M)
        {
            return BigInteger.ModPow(M, e, N);
        }

        public BigInteger MaHoaChungThuc(BigInteger M)
        {
            return BigInteger.ModPow(M, d, N);
        }

        public BigInteger TaoSoNgauNhien()
        {
            BigInteger maxM = N - 1;

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[maxM.ToByteArray().Length];
                rng.GetBytes(randomBytes);

                BigInteger randomM = new BigInteger(randomBytes);
                randomM = BigInteger.Abs(randomM) % maxM + 1;

                return randomM;
            }
        }
    }

    public class RSADecryptor : TaoKhoa
    {
        public RSADecryptor(BigInteger p, BigInteger q) : base(p, q)
        {
        }

        protected override BigInteger TimE(BigInteger n)
        {
            for (BigInteger i = 2; i < n; i++)
            {
                if (BigInteger.GreatestCommonDivisor(i, n) == 1 && IsPrime(i))
                {
                    return i;
                }
            }

            throw new Exception("Không thể tìm số e trong RSADecryptor.");
        }

        private static bool IsPrime(BigInteger number)
        {
            if (number <= 1)
            {
                return false;
            }

            for (BigInteger i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        protected override BigInteger TimD(BigInteger e, BigInteger n)
        {
            BigInteger d = 0;
            for (BigInteger k = 2; ; k++)
            {
                d = (k * n + 1) / e;

                if ((d * e) % n == 1 && d != e)
                {
                    return d;
                }
            }
        }

        public BigInteger GiaiMaBaoMat(BigInteger encryptedMessage)
        {
            return BigInteger.ModPow(encryptedMessage, d, N);
        }

        public BigInteger GiaiMaChungThuc(BigInteger encrypted)
        {
            return BigInteger.ModPow(encrypted, e, N);
        }
    }

    class ChuongTrinh
    {
        static void Main()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Nhập đường dẫn tới file chứa giá trị p và q: ");
            string filePath = Console.ReadLine();

            var taoKhoa = new TaoKhoa();
            var khoaCongKhaiRieng = taoKhoa.TaoKhoaTuFile(filePath);

            Console.WriteLine("Khóa công khai: " + taoKhoa.LayKhoaCongKhai());
            Console.WriteLine("Khóa riêng tư: " + taoKhoa.LayKhoaRieng());

            Console.WriteLine("p: " + taoKhoa.GetP());
            Console.WriteLine("q: " + taoKhoa.GetQ());
            Console.WriteLine("n: " + taoKhoa.GetN());
            Console.WriteLine("N: " + taoKhoa.GetNModulus());
            Console.WriteLine("e: " + taoKhoa.GetE());
            Console.WriteLine("d: " + taoKhoa.GetD());

            var maHoa = new RSAEncryptor(taoKhoa.GetP(), taoKhoa.GetQ());
            var giaiMa = new RSADecryptor(taoKhoa.GetP(), taoKhoa.GetQ());
            BigInteger randomM = maHoa.TaoSoNgauNhien();
            Console.WriteLine("Số ngẫu nhiên M thỏa mãn điều kiện: " + randomM);

            BigInteger encryptedMessage = maHoa.MaHoaBaoMat(randomM);
            Console.WriteLine("Thông điệp đã mã hóa: " + encryptedMessage);
            BigInteger decryptedMessage = giaiMa.GiaiMaBaoMat(encryptedMessage);
            Console.WriteLine("Thông điệp đã giải mã: " + decryptedMessage);

            BigInteger encrypted = maHoa.MaHoaChungThuc(randomM);
            Console.WriteLine("Thông điệp đã mã hóa: " + encrypted);
            BigInteger decrypted = giaiMa.GiaiMaChungThuc(encrypted);
            Console.WriteLine("Thông điệp đã giải mã: " + decrypted);

            Console.ReadKey();
        }
    }
}



