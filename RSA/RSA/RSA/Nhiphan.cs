using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Nhiphan
    {
        static void Main()
        {
            // Chuyển đổi số nguyên thành nhị phân
            int decimalNumber = 10;
            string binaryFromInt = Convert.ToString(decimalNumber, 2);
            Console.WriteLine($"{decimalNumber} ở dạng nhị phân là: {binaryFromInt}");

            // Chuyển đổi ký tự ngẫu nhiên thành nhị phân
            char randomChar = GetRandomChar();
            string binaryFromChar = Convert.ToString(randomChar, 2);
            Console.WriteLine($"{randomChar} ở dạng nhị phân là: {binaryFromChar}");
        }
        static char GetRandomChar()
        {
            Random random = new Random();
            int randomNumber = random.Next(65, 91); // Mã ASCII từ 'A' đến 'Z'
            return (char)randomNumber;
        }
    }
}
