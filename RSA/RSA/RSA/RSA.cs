using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    public class RSA
    {
        private BigInteger p, q, N, n, e, d;
        public RSA(BigInteger p, BigInteger q)
        {
            this.p = p;
            this.q = q;
            this.N = p * q;
            this.n = (p - 1) * (q - 1);
            this.e = FindE(this.n);
            this.d = FindD(this.e, n);
        }
        // Hàm tìm số e nguyên tố cùng nhau với n
        private BigInteger FindE(BigInteger n)
        {
            for (BigInteger i = 2; i < n; i++)
            {
                if (BigInteger.GreatestCommonDivisor(i, n) == 1)
                {
                    return i;
                }
            }

            throw new Exception("Không thể tìm số e.");
        }

        // Hàm tìm số d là nghịch đảo của e trong phép modulo n
        private static BigInteger FindD(BigInteger e, BigInteger n)
        {
            BigInteger d = 0;
            for (BigInteger k = 1; ; k++)
            {
                d = (k * n + 1) / e;

                if ((d * e) % n == 1)
                {
                    return d;
                }
            }
        }

        // Lấy khóa công khai (e, N)
        public Tuple<BigInteger, BigInteger> LayKhoaCongKhai()
        {
            return Tuple.Create(e, N);
        }

        // Lấy khóa riêng (d, N)
        public Tuple<BigInteger, BigInteger> LayKhoaRieng()
        {
            return Tuple.Create(d, N);
        }
        
    }
    class ChuongTrinh
    {
        static void Main()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            // Bước 1: Chọn p và q
            Console.WriteLine("Nhập giá trị p (số nguyên tố): ");
            BigInteger p = BigInteger.Parse(Console.ReadLine());

            Console.WriteLine("Nhập giá trị q (số nguyên tố khác p): ");
            BigInteger q = BigInteger.Parse(Console.ReadLine());

            // Bước 2: Tính N
            Console.WriteLine("Sinh khóa: ");
            BigInteger N = p * q;
            Console.WriteLine("N: " + N);
            BigInteger n = (p - 1) * (q - 1);
            Console.WriteLine("n: " + n);
            // Bước 3: Tìm e
            BigInteger e = FindE(n);
            Console.WriteLine("e: " + e);

            // Bước 4: Tìm d (nghịch đảo modulo)
            BigInteger d = FindD(e, n);
            Console.WriteLine("d: " + d);

            // Bước 5: Hủy bỏ n, p và q (Đã được thực hiện trong constructor)
            // Bước 6: Lấy khóa công khai và khóa riêng
            var khoaCongKhai = Tuple.Create(e, N);
            var khoaRieng = Tuple.Create(d, N);
            Console.WriteLine("Khóa công khai: " + khoaCongKhai);
            Console.WriteLine("Khóa riêng tư: " + khoaRieng);

            BigInteger message = 3;
            // Bước 7: Mã hóa và giải mã bảo mật
            BigInteger encryptedMessage = MaHoa(message, e, N);
            BigInteger decryptedMessage = GiaiMa(encryptedMessage, d, N);

            Console.WriteLine("Thông điệp đã mã hóa: " + encryptedMessage);
            Console.WriteLine("Thông điệp đã giải mã: " + decryptedMessage);
            // Bước 7: Mã hóa và giải mã chứng thực
            BigInteger encrypted = MaHoa(message, d, N);
            BigInteger decrypted = GiaiMa(encrypted, e, N);

            Console.WriteLine("Thông điệp đã mã hóa: " + encrypted);
            Console.WriteLine("Thông điệp đã giải mã: " + decrypted);
            Console.ReadKey();
        }


        private static BigInteger FindE(BigInteger n)
        {
            for (BigInteger e = 2; e < n; e++)
            {
                if (BigInteger.GreatestCommonDivisor(e, n) == 1)
                {
                    // Nếu e là số nguyên tố cùng nhau với n, trả về giá trị e
                    if (IsPrime(e))
                    {
                        return e;
                    }
                }
            }

            throw new Exception("Không thể tìm số e.");
        }

        // Hàm kiểm tra xem một số có phải là số nguyên tố không
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

        private static BigInteger FindD(BigInteger e, BigInteger N)
        {
            BigInteger d = 0;
            for (BigInteger k = 1; ; k++)
            {
                d = (k * N + 1) / e;

                if ((d * e) % N == 1)
                {
                    return d;
                }
            }
        }
        private static BigInteger MaHoa(BigInteger M, BigInteger e, BigInteger n)
        {
            BigInteger encryptedMessage = BigInteger.ModPow(M, e, n);
            return encryptedMessage;
        }

        private static BigInteger GiaiMa(BigInteger encryptedMessage, BigInteger d, BigInteger n)
        {
            BigInteger decryptedMessage = BigInteger.ModPow(encryptedMessage, d, n);
            return decryptedMessage;
        }
    }

}



