using System;

namespace MatrixCalculator
{
    /// <summary>
    /// Реализация класса дроби.
    /// </summary>
    class Fraction
    {
        /// <summary>
        /// Переменная в которой хранится числитель.
        /// </summary>
        private long numerator = 0;

        /// <summary>
        /// Переменная в которой хранится знаменатель.
        /// </summary>
        private long denominator = 1;

        /// <summary>
        /// public версия numerator
        /// </summary>
        public long Numerator
        {
            get
            {
                return numerator;
            }
            set
            {
                numerator = value;
            }
        }

        /// <summary>
        /// public версия denominator
        /// </summary>
        public long Denominator
        {
            get
            {
                return denominator;
            }
            set
            {
                denominator = value;
            }
        }

        /// <summary>
        /// Метод для нахождения наибольшего общего делителя.
        /// </summary>
        /// <param name="num1">первое число.</param>
        /// <param name="num2">второе число.</param>
        /// <returns>возвращает НОД двух чисел.</returns>
        private static long Gcd(long num1, long num2)
        {
            // Алгоритм Евклида.
            if (num2 == 0)
            {
                return num1;
            }
            return Gcd(num2, num1 % num2);
        }

        /// <summary>
        /// Метод для приведения дроби к несократимому виду.
        /// </summary>
        private void Reduction()
        {
            long gcd = Gcd(Math.Abs(numerator), Math.Abs(denominator));
            // Делим на НОД, чтобы получить несократимые дроби.
            numerator /= gcd;
            denominator /= gcd;
            // Поддерживаем инвариант, что знаменатель - натуральное число.
            if (denominator < 0)
            {
                numerator *= -1;
                denominator *= -1;
            }
        }


        /// <summary>
        /// Cложение двух дробей.
        /// </summary>
        /// <param name="num1">первая дробь.</param>
        /// <param name="num2">вторая дробь.</param>
        /// <returns>дробь равная сумме двух.</returns>
        public static Fraction operator +(Fraction num1, Fraction num2)
        {
            var sum = new Fraction();
            sum.numerator = num1.numerator * num2.denominator + num2.numerator * num1.denominator;
            sum.denominator = num1.denominator * num2.denominator;
            sum.Reduction();
            return sum;
        }

        /// <summary>
        /// Вычитание двух дробей.
        /// </summary>
        /// <param name="num1">первая дробь.</param>
        /// <param name="num2">вторая дробь.</param>
        /// <returns>дробь равная разнице двух.</returns>
        public static Fraction operator -(Fraction num1, Fraction num2)
        {
            var diff = new Fraction();
            diff.numerator = num1.numerator * num2.denominator - num2.numerator * num1.denominator;
            diff.denominator = num1.denominator * num2.denominator;
            diff.Reduction();
            return diff;
        }


        /// <summary>
        /// Умножение двух дробей.
        /// </summary>
        /// <param name="num1">первая дробь.</param>
        /// <param name="num2">вторая дробь.</param>
        /// <returns>дробь равная произведению двух.</returns>
        public static Fraction operator *(Fraction num1, Fraction num2)
        {
            var mul = new Fraction();
            mul.numerator = num1.numerator * num2.numerator;
            mul.denominator = num1.denominator * num2.denominator;
            mul.Reduction();
            return mul;
        }

        /// <summary>
        /// Произведение дроби и числа.
        /// </summary>
        /// <param name="num1">дробь.</param>
        /// <param name="num2">число на которое надо домножить дробь.</param>
        /// <returns>Дробь домноженная на число.</returns>
        public static Fraction operator *(Fraction num1, int num2)
        {
            var mul = new Fraction();
            mul.numerator = num1.numerator * num2;
            mul.denominator = num1.denominator;
            mul.Reduction();
            return mul;
        }

        /// <summary>
        /// Деление одной дроби на другую.
        /// </summary>
        /// <param name="num1">первая дробь.</param>
        /// <param name="num2">вторая дробь.</param>
        /// <returns>дробь равная частному.</returns>
        public static Fraction operator /(Fraction num1, Fraction num2)
        {
            var div = new Fraction();
            div.numerator = num1.numerator * num2.denominator;
            div.denominator = num1.denominator * num2.numerator;
            div.Reduction();
            return div;
        }

        /// <summary>
        /// Сравнение двух дробей по модулю (на больше).
        /// </summary>
        /// <param name="num1">первая дробь.</param>
        /// <param name="num2">вторая дробь.</param>
        /// <returns>true, если больше, иначе false.</returns>
        public static bool operator >(Fraction num1, Fraction num2)
        {
            return (Math.Abs(num1.numerator) * num2.denominator > num1.denominator * Math.Abs(num2.numerator));
        }

        /// <summary>
        /// Сравнение двух дробей по модулю (на меньше).
        /// </summary>
        /// <param name="num1">первая дробь.</param>
        /// <param name="num2">вторая дробь.</param>
        /// <returns>true, если меньше, иначе false.</returns>
        public static bool operator <(Fraction num1, Fraction num2)
        {
            return (Math.Abs(num1.numerator) * num2.denominator < num1.denominator * Math.Abs(num2.numerator));
        }

        /// <summary>
        /// Проверка на равенство двух дробей.
        /// </summary>
        /// <param name="num1">первая дробь.</param>
        /// <param name="num2">вторая дробь.</param>
        /// <returns>true, если равны, иначе false.</returns>
        public static bool operator ==(Fraction num1, Fraction num2)
        {
            return (num1.numerator * num2.denominator == num1.denominator * num2.numerator);
        }

        /// <summary>
        /// Проверка на неравенство двух дробей.
        /// </summary>
        /// <param name="num1">первая дробь.</param>
        /// <param name="num2">вторая дробь.</param>
        /// <returns>true, если неравны, иначе false.</returns>
        public static bool operator !=(Fraction num1, Fraction num2)
        {
            return (num1.numerator * num2.denominator != num1.denominator * num2.numerator);
        }

        /// <summary>
        /// Проверка на равенство дроби и числа.
        /// </summary>
        /// <param name="num1">дробь.</param>
        /// <param name="num2">число.</param>
        /// <returns>true, если равны, иначе false.</returns>
        public static bool operator ==(Fraction num1, int num2)
        {
            return (num1.numerator == num1.denominator * num2);
        }

        /// <summary>
        /// Проверка на неравенство дроби и числа.
        /// </summary>
        /// <param name="num1">дробь.</param>
        /// <param name="num2">число.</param>
        /// <returns>true, если неравны, иначе false.</returns>
        public static bool operator !=(Fraction num1, int num2)
        {
            return (num1.numerator != num1.denominator * num2);
        }

        /// <summary>
        /// Приводим дробь в string.
        /// </summary>
        /// <returns>дробь в виде строки.</returns>
        public string Str()
        {
            // Проверяем, что дробь - целое число.
            if (denominator == 1)
            {
                return numerator.ToString() + " ";
            }
            return numerator + " / " + denominator + " ";
        }
    }
}
