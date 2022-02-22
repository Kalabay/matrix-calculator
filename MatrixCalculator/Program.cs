using System;
using System.IO;

namespace MatrixCalculator
{
    class Program
    {
        /// <summary>
        /// Метод для разбиения строки по пробелам.
        /// </summary>
        /// <param name="input">строчка, которую надо разбить.</param>
        /// <returns>массив получившегося разбиения.</returns>
        private static string[] MySplit(string input)
        {
            return input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Метод для проверки строки пользователя на корректность.
        /// </summary>
        /// <param name="len">нужное количесвто чисел в строке.</param>
        /// <param name="min">минимальное возможное число.</param>
        /// <param name="max">максимальное возможное число.</param>
        /// <param name="input">входные данные от пользователя.</param>
        /// <returns>результат проверки на корректность.</returns>
        private static (bool, int[]) CheckInput(int len, int min, int max, string[] input)
        {
            var result = new int[len];
            if (input.Length != len)
            {
                return (false, result);
            }
            for (var i = 0; i < len; ++i)
            {
                int num;
                if (!int.TryParse(input[i], out num) || num < min || num > max)
                {
                    return (false, result);
                }
                result[i] = num;
            }
            return (true, result);
        }

        /// <summary>
        /// Метод для третьего варианта считывания матрицы (рандомная генерация).
        /// </summary>
        /// <param name="restrictions">ограничение на размер матрицы.</param>
        /// <returns>итоговая матрица.</returns>
        private static Matrix ThirdRead(int restrictions)
        {
            var matrix = new Matrix();
            Console.WriteLine("Вам предложат ввести размеры матрицы.");
            Console.WriteLine("Затем элементы сгенерируются рандомно.");
            Console.WriteLine("Введите размеры матрицы NxM через пробел.");
            string input = Console.ReadLine();
            while (!CheckInput(2, 1, restrictions, MySplit(input)).Item1)
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку.");
                input = Console.ReadLine();
            }
            matrix.VerticalSize = CheckInput(2, 1, restrictions, MySplit(input)).Item2[0];
            matrix.HorizonSize = CheckInput(2, 1, restrictions, MySplit(input)).Item2[1];
            matrix.Generate();
            return matrix;
        }

        /// <summary>
        /// Метод для вывода инструкции для второго типа ввода.
        /// </summary>
        private static void TextSecondRead()
        {
            var filePath = @"C:\Users\user1234\Documents\matrix.txt";
            Console.WriteLine("Файл должен быть в формате txt.");
            Console.WriteLine("В первой строчке должны быть размеры NxM матрицы через пробел.");
            Console.WriteLine("В каждой последующей строке по 1 строке матрицы.");
            Console.WriteLine("Например:");
            Console.WriteLine("2 3");
            Console.WriteLine("1 -5 7");
            Console.WriteLine("2 3 5");
            Console.WriteLine("Считывание происходит до конца файла.");
            Console.WriteLine("Напишите путь до вашего файла.");
            Console.WriteLine("Например: " + filePath);
        }

        /// <summary>
        /// Метод для заполнения одной сточки матрицы.
        /// </summary>
        /// <param name="i">индекс строки матрицы.</param>
        /// <param name="line">массив значений для заполнения.</param>
        /// <param name="matrix">матрица, которую заполняем.</param>
        private static void FillingMatrix(int i, int[] line, ref Matrix matrix)
        {
            for (var j = 0; j < matrix.HorizonSize; ++j)
            {
                var num = new Fraction();
                num.Numerator = line[j];
                matrix.MatrixTable[i, j] = num;
            }
        }

        /// <summary>
        /// Метод для считывания строк матрицы из файла.
        /// </summary>
        /// <param name="line">ввод пользователя из файла.</param>
        /// <param name="file">файл, из которого считываем.</param>
        /// <param name="matrix">матрица, с которой работаем.</param>
        private static void ReadingLines(string line, ref StreamReader file, ref Matrix matrix)
        {
            var i = 0;
            while (line != null)
            {
                if (!CheckInput(matrix.HorizonSize, -10, 10, MySplit(line)).Item1)
                {
                    file.Close();
                    throw new Exception();
                }
                FillingMatrix(i, CheckInput(matrix.HorizonSize, -10, 10, MySplit(line)).Item2, ref matrix);
                line = file.ReadLine();
                i += 1;
            }
        }

        /// <summary>
        /// Метод для второго варианта считывания матрицы (из файла).
        /// </summary>
        /// <param name="restrictions">ограничение на размер матрицы.</param>
        /// <returns>итоговая матрица.</returns>
        private static Matrix SecondRead(int restrictions)
        {
            var matrix = new Matrix();
            TextSecondRead();
            string filePath = Console.ReadLine();
            while (true)
            {
                try
                {
                    StreamReader file = new StreamReader(filePath);
                    string line = file.ReadLine();
                    matrix.VerticalSize = CheckInput(2, 1, restrictions, MySplit(line)).Item2[0];
                    matrix.HorizonSize = CheckInput(2, 1, restrictions, MySplit(line)).Item2[1];
                    if (!CheckInput(2, 1, restrictions, MySplit(line)).Item1)
                    {
                        file.Close();
                        throw new Exception();
                    }
                    matrix.MatrixTable = new Fraction[matrix.VerticalSize, matrix.HorizonSize];
                    line = file.ReadLine();
                    ReadingLines(line, ref file, ref matrix);
                    file.Close();
                    break;
                }
                catch
                {
                    Console.WriteLine("Некорректные данные или файла не существует.");
                    Console.WriteLine("Повторите попытку ввода файла.");
                    filePath = Console.ReadLine();
                }
            }
            Console.WriteLine("Матрица.");
            matrix.Print();
            return matrix;
        }

        /// <summary>
        /// Метод для первого варианта считывания матрицы (из консоли).
        /// </summary>
        /// <param name="restrictions">ограничение на размер матрицы.</param>
        /// <returns>итоговая матрица.</returns>
        private static Matrix FirstRead(int restrictions)
        {
            var matrix = new Matrix();
            Console.WriteLine("Введите размеры матрицы NxM через пробел.");
            string input = Console.ReadLine();
            while (!CheckInput(2, 1, restrictions, MySplit(input)).Item1)
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку.");
                input = Console.ReadLine();
            }
            matrix.VerticalSize = CheckInput(2, 1, 10, MySplit(input)).Item2[0];
            matrix.HorizonSize = CheckInput(2, 1, 10, MySplit(input)).Item2[1];
            matrix.MatrixTable = new Fraction[matrix.VerticalSize, matrix.HorizonSize];
            for (var i = 1; i <= matrix.VerticalSize; ++i)
            {
                Console.WriteLine("Введите " + i + " строчку матрицы, цифры через пробел.");
                input = Console.ReadLine();
                while (!CheckInput(matrix.HorizonSize, -10, 10, MySplit(input)).Item1)
                {
                    Console.WriteLine("Некорректный ввод. Повторите попытку.");
                    input = Console.ReadLine();
                }
                int[] result = CheckInput(matrix.HorizonSize, -10, 10, MySplit(input)).Item2;
                FillingMatrix(i - 1, result, ref matrix);
            }
            return matrix;
        }

        /// <summary>
        /// Метод для выбора варианта считывания матрицы.
        /// </summary>
        /// <param name="restrictions">ограничение на размер матрицы.</param>
        /// <returns>итоговая матрица.</returns>
        private static Matrix Read(int restrictions)
        {
            Console.WriteLine("Ограничения на матрицу:");
            Console.WriteLine("Не меньше 1x1;");
            Console.WriteLine("Не больше " + restrictions + "x" + restrictions + ";");
            Console.WriteLine("Элементы по модулю не больше 10;");
            Console.WriteLine("Выберете, что хотите сделать:");
            Console.WriteLine("1. Ввести матрицу вручную;");
            Console.WriteLine("2. Считать матрицу из файла;");
            Console.WriteLine("3. Сгенерировать матрицу;");
            Console.WriteLine("Напишите цифру интересующей вас команды.");
            string input = Console.ReadLine();
            int command;
            while (!int.TryParse(input, out command) || command < 1 || command > 3)
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку.");
                input = Console.ReadLine();
            }
            if (command == 1)
            {
                return FirstRead(restrictions);
            }
            else if (command == 2)
            {
                return SecondRead(restrictions);
            }
            return ThirdRead(restrictions);
        }

        /// <summary>
        /// Метод для нахождения и вывода следа матрицы.
        /// </summary>
        /// <returns>возвращает true, так как приложение не закончило работу.</returns>
        private static bool Trace()
        {
            Matrix matrix = Read(10);
            if (matrix.HorizonSize != matrix.VerticalSize)
            {
                Console.WriteLine("Нельзя взять след у неквадратной матрицы.");
                return true;
            }
            Console.WriteLine("Результат.");
            Console.WriteLine(matrix.Trace().Str());
            return true;
        }

        /// <summary>
        /// Метод для нахождения и вывода транспонированной матрицы.
        /// </summary>
        /// <returns>возвращает true, так как приложение не закончило работу.</returns>
        private static bool Transpose()
        {
            Matrix matrix = Read(10);
            matrix.Transpose();
            Console.WriteLine("Результат.");
            matrix.Print();
            return true;
        }

        /// <summary>
        /// Метод для нахождения и вывода суммы двух матриц.
        /// </summary>
        /// <returns>возвращает true, так как приложение не закончило работу.</returns>
        private static bool Plus()
        {
            Matrix matrix1 = Read(10);
            Matrix matrix2 = Read(10);
            if (matrix1.VerticalSize != matrix2.VerticalSize)
            {
                Console.WriteLine("Нельзя сложить.");
                return true;
            }
            if (matrix1.HorizonSize != matrix2.HorizonSize)
            {
                Console.WriteLine("Нельзя сложить.");
                return true;
            }
            matrix1.Plus(matrix2);
            Console.WriteLine("Результат.");
            matrix1.Print();
            return true;
        }

        /// <summary>
        /// Метод для нахождения и вывода разности двух матриц.
        /// </summary>
        /// <returns>возвращает true, так как приложение не закончило работу.</returns>
        private static bool Minus()
        {
            Matrix matrix1 = Read(10);
            Matrix matrix2 = Read(10);
            if (matrix1.VerticalSize != matrix2.VerticalSize)
            {
                Console.WriteLine("Нельзя вычесть.");
                return true;
            }
            if (matrix1.HorizonSize != matrix2.HorizonSize)
            {
                Console.WriteLine("Нельзя вычесть.");
                return true;
            }
            matrix1.Minus(matrix2);
            Console.WriteLine("Результат.");
            matrix1.Print();
            return true;
        }

        /// <summary>
        /// Метод для нахождения и вывода произведения двух матриц.
        /// </summary>
        /// <returns>возвращает true, так как приложение не закончило работу.</returns>
        private static bool MulMatrix()
        {
            Matrix matrix1 = Read(10);
            Matrix matrix2 = Read(10);
            if (matrix1.HorizonSize != matrix2.VerticalSize)
            {
                Console.WriteLine("Нельзя перемножить.");
                return true;
            }
            matrix1.MulMatrix(matrix2);
            Console.WriteLine("Результат.");
            matrix1.Print();
            return true;
        }

        /// <summary>
        /// Метод для нахождения и вывода умножения матрицы на число.
        /// </summary>
        /// <returns>возвращает true, так как приложение не закончило работу.</returns>
        private static bool MulNumber()
        {
            Matrix matrix1 = Read(10);
            int num;
            Console.WriteLine("Введите число от -100 до 100.");
            string input = Console.ReadLine();
            while (!int.TryParse(input, out num) || num < -100 || num > 100)
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку.");
                input = Console.ReadLine();
            }
            matrix1.MulNumber(num);
            Console.WriteLine("Результат.");
            matrix1.Print();
            return true;
        }

        /// <summary>
        /// Метод для нахождения и вывода определителя матрицы.
        /// </summary>
        /// <returns>возвращает true, так как приложение не закончило работу.</returns>
        private static bool Determinant()
        {
            Matrix matrix = Read(7);
            if (matrix.HorizonSize != matrix.VerticalSize)
            {
                Console.WriteLine("Нельзя взять определитель у неквадратной матрицы.");
                return true;
            }
            Console.WriteLine("Результат.");
            Console.WriteLine(matrix.Determinant().Str());
            return true;
        }

        /// <summary>
        /// Метод для нахождения и вывода решения СЛАУ.
        /// </summary>
        /// <returns>возвращает true, так как приложение не закончило работу.</returns>
        private static bool Gauss()
        {
            Console.WriteLine("Введите систему уравнений в формате матрицы.");
            Console.WriteLine("Например уравнения 5 * x1 - 3 * x2 = 2 и x1 + 2 * x2 = 1:");
            Console.WriteLine("2 3");
            Console.WriteLine("5 -3 2");
            Console.WriteLine("1 2 1");
            Matrix matrix1 = Read(7);
            matrix1.Gauss();
            return true;
        }

        /// <summary>
        /// Метод, который отвечает за меню.
        /// </summary>
        /// <returns>true, если продолжить выполнение, иначе false.</returns>
        private static bool Menu()
        {
            try
            {
                Console.WriteLine("Меню.");
                Console.WriteLine("Выберете, что хотите сделать:");
                Console.WriteLine("1. нахождение следа матрицы;");
                Console.WriteLine("2. транспонирование матрицы;");
                Console.WriteLine("3. сумма двух матриц;");
                Console.WriteLine("4. разность двух матриц;");
                Console.WriteLine("5. произведение двух матриц;");
                Console.WriteLine("6. умножение матрицы на число;");
                Console.WriteLine("7. нахождение определителя матрицы;");
                Console.WriteLine("8. решение СЛАУ;");
                Console.WriteLine("9. закончить выполнение программы;");
                Console.WriteLine("Напишите цифру интересующей вас команды.");
                string input = Console.ReadLine();
                int command;
                while (!int.TryParse(input, out command) || command < 1 || command > 9)
                {
                    Console.WriteLine("Некорректный ввод. Повторите попытку.");
                    input = Console.ReadLine();
                }
                switch (command)
                {
                    case 1: return Trace();
                    case 2: return Transpose();
                    case 3: return Plus();
                    case 4: return Minus();
                    case 5: return MulMatrix();
                    case 6: return MulNumber();
                    case 7: return Determinant();
                    case 8: return Gauss();
                    default: return false;
                }
            }
            catch
            {
                Console.WriteLine("Неверный ввод.");
                return true;
            }
        }

        /// <summary>
        /// Main, который каждый раз запускает программу.
        /// </summary>
        static void Main()
        {
            Console.WriteLine("Добро пожаловать в калькулятор матрицы!!!");
            var notEnd = true;
            while (notEnd)
            {
                notEnd = Menu();
            }
        }
    }
}
