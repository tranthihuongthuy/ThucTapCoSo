using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    public class TaoKhoa
    {
        protected BigInteger p, q, N, n, e, d;

        public TaoKhoa(BigInteger p, BigInteger q)
        {
            this.p = p;
            this.q = q;
            this.N = p * q;
            this.n = (p - 1) * (q - 1);
            this.e = TimE(this.n);
            this.d = TimD(this.e, n);
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

        protected virtual BigInteger TimD(BigInteger e, BigInteger n)
        {
            BigInteger d = 0;
            for (BigInteger k = 2; ; k++)
            {
                d = (k * n + 1) / e;

                if ((d * e) % n == 1 && d != e) // Thêm điều kiện để đảm bảo d khác e
                {
                    return d;
                }
            }
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

        public Tuple<BigInteger, BigInteger> LayKhoaCongKhai()
        {
            return Tuple.Create(e, N);
        }

        public Tuple<BigInteger, BigInteger> LayKhoaRieng()
        {
            return Tuple.Create(d, N);
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

                if ((d * e) % n == 1 && d != e) // Thêm điều kiện để đảm bảo d khác e
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

                if ((d * e) % n == 1 && d != e) // Thêm điều kiện để đảm bảo d khác e
                {
                    return d;
                }
            }
        }
        public BigInteger GiaiMaBaoMat(BigInteger encryptedMessage)
        {
            return BigInteger.ModPow(encryptedMessage, d, N);
        }
        public BigInteger GiaiMaChungThuc(BigInteger encryptedMessage)
        {
            return BigInteger.ModPow(encryptedMessage, e, N);
        }
    }

    class ChuongTrinh
    {
        static void Main()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Nhập giá trị p (số nguyên tố): ");
            BigInteger p = BigInteger.Parse(Console.ReadLine());

            Console.WriteLine("Nhập giá trị q (số nguyên tố khác p): ");
            BigInteger q = BigInteger.Parse(Console.ReadLine());

            var taoKhoa = new TaoKhoa(p, q);
            var khoaCongKhai = taoKhoa.LayKhoaCongKhai();
            var khoaRieng = taoKhoa.LayKhoaRieng();

            Console.WriteLine("Khóa công khai: " + khoaCongKhai);
            Console.WriteLine("Khóa riêng tư: " + khoaRieng);
            Console.WriteLine("n: " + taoKhoa.GetN());
            Console.WriteLine("N: " + taoKhoa.GetNModulus());
            Console.WriteLine("e: " + taoKhoa.GetE());
            Console.WriteLine("d: " + taoKhoa.GetD());

            int i = 1024;

            var maHoa = new RSAEncryptor(p, q);
            var giaiMa = new RSADecryptor(p, q);
            BigInteger randomM = maHoa.TaoSoNgauNhien();
            Console.WriteLine("Số ngẫu nhiên M thỏa mãn điều kiện: " + randomM);

            BigInteger encryptedMessage = maHoa.MaHoaBaoMat(randomM);
            Console.WriteLine("Thông điệp đã mã hóa: " + encryptedMessage);
            BigInteger decryptedMessage = giaiMa.GiaiMaBaoMat(encryptedMessage);
            Console.WriteLine("Thông điệp đã giải mã: " + decryptedMessage);

            BigInteger encrypted = maHoa.MaHoaChungThuc(randomM);
            Console.WriteLine("Thông điệp đã mã hóa: " + encrypted);
            BigInteger decrypted = giaiMa.GiaiMaChungThuc(encrypted);
            Console.WriteLine("Thông điệp đã giải mã: " + decryptedMessage);

            Console.ReadKey();
        }
    }
}



