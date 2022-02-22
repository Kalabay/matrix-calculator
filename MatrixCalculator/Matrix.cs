using System;

namespace MatrixCalculator
{
    /// <summary>
    /// Реализация класса матрицы.
    /// </summary>
    class Matrix
    {
        /// <summary>
        /// Размер матрицы по вертикали.
        /// </summary>
        private int verticalSize;

        /// <summary>
        /// Размер матрицы по горизонтали.
        /// </summary>
        private int horizonSize;

        /// <summary>
        /// Двумерный массив с элементами матрицы.
        /// </summary>
        private Fraction[,] matrix;

        /// <summary>
        /// public версия verticalSize
        /// </summary>
        public int VerticalSize
        {
            get
            {
                return verticalSize;
            }
            set
            {
                verticalSize = value;
            }
        }

        /// <summary>
        /// public версия horizonSize
        /// </summary>
        public int HorizonSize
        {
            get
            {
                return horizonSize;
            }
            set
            {
                horizonSize = value;
            }
        }

        /// <summary>
        /// public версия matrix
        /// </summary>
        public Fraction[,] MatrixTable
        {
            get
            {
                return matrix;
            }
            set
            {
                matrix = value;
            }
        }

        /// <summary>
        /// Метод делает транспонирование матрицы.
        /// </summary>
        public void Transpose()
        {
            var matrixT = new Fraction[horizonSize, verticalSize];
            for (var i = 0; i < horizonSize; ++i)
            {
                for (var j = 0; j < verticalSize; ++j)
                {
                    matrixT[i, j] = matrix[j, i];
                }
            }
            matrix = matrixT;
            //меняем значения вертикали и горизонтали местами.
            verticalSize ^= horizonSize;
            horizonSize ^= verticalSize;
            verticalSize ^= horizonSize;
        }

        /// <summary>
        /// Метод для сложения двух матриц.
        /// </summary>
        /// <param name="matrixPlus">матрица, которую надо прибавить.</param>
        public void Plus(Matrix matrixPlus)
        {
            for (var i = 0; i < verticalSize; ++i)
            {
                for (var j = 0; j < horizonSize; ++j)
                {
                    matrix[i, j] += matrixPlus.matrix[i, j];
                }
            }
        }

        /// <summary>
        /// Метод для вычитания матриц.
        /// </summary>
        /// <param name="matrixMinus">матрица, которую надо вычесть.</param>
        public void Minus(Matrix matrixMinus)
        {
            for (var i = 0; i < verticalSize; ++i)
            {
                for (var j = 0; j < horizonSize; ++j)
                {
                    matrix[i, j] -= matrixMinus.matrix[i, j];
                }
            }
        }

        /// <summary>
        /// Метод для перемножения матриц.
        /// </summary>
        /// <param name="matrixMul">матрица, на которую надо умножить.</param>
        public void MulMatrix(Matrix matrixMul)
        {
            var matrixProduct = new Fraction[verticalSize, matrixMul.horizonSize];
            for (var i = 0; i < verticalSize; ++i)
            {
                for (var j = 0; j < matrixMul.horizonSize; ++j)
                {
                    var sum = new Fraction();
                    // Перемножение строки и столбца.
                    for (var q = 0; q < horizonSize; ++q)
                    {
                        sum += matrix[i, q] * matrixMul.matrix[q, j];
                    }
                    matrixProduct[i, j] = sum;
                }
            }
            matrix = matrixProduct;
            horizonSize = matrixMul.horizonSize;
        }

        /// <summary>
        /// Метод для умножения матрицы на число.
        /// </summary>
        /// <param name="num">число, на которое надо умножить.</param>
        public void MulNumber(int num)
        {
            for (var i = 0; i < verticalSize; ++i)
            {
                for (var j = 0; j < horizonSize; ++j)
                {
                    matrix[i, j] *= num;
                }
            }
        }

        /// <summary>
        /// Метод, в котором происходит обмен двух строк и зануление вертикали.
        /// </summary>
        /// <param name="line1">индекс первой строки для обмена.</param>
        /// <param name="line2">индекс второй строки для обмена.</param>
        /// <param name="column">индекс столбца, который надо занулить.</param>
        private void SwapAndZeroing(int line1, int line2, int column)
        {
            // Обмен строк матрицы.
            for (var i = 0; i < horizonSize; ++i)
            {
                Fraction numSwap = matrix[line1, i];
                matrix[line1, i] = matrix[line2, i];
                matrix[line2, i] = numSwap;
            }
            // Зануление столбца матрицы.
            for (var i = 0; i < verticalSize; ++i)
            {
                if (i != line2)
                {
                    Fraction attitude = (matrix[i, column] / matrix[line2, column]);
                    for (int j = column; j < horizonSize; ++j)
                    {
                        matrix[i, j] -= attitude * matrix[line2, j];
                    }
                }
            }
            // Делим строку матрицы line2, чтобы matrix[line2, column] равнялось 1.
            for (int j = line2 + 1; j < horizonSize; ++j)
            {
                matrix[line2, j] /= matrix[line2, column];
            }
            matrix[line2, column] /= matrix[line2, column];
        }

        /// <summary>
        /// Метод для проверки и ввывода ответа.
        /// </summary>
        /// <param name="getLine">массив, в котором хранится индекс строки-ответа для кажого x.</param>
        /// <param name="ans">массив, где хранится решение СЛАУ.</param>
        private void CheckAndPrint(int[] getLine, Fraction[] ans)
        {
            // Подставляем решение во все уравнения.
            for (var i = 0; i < verticalSize; ++i)
            {
                var res = new Fraction();
                for (var j = 0; j < horizonSize - 1; ++j)
                {
                    res += ans[j] * matrix[i, j];
                }
                // Проверяем, что уравнение неверно для нашего решения.
                if (res != matrix[i, horizonSize - 1])
                {
                    Console.WriteLine("Нет решений.");
                    return;
                }
            }
            Console.WriteLine("Решение:");
            // Выводим одно решение.
            for (var i = 0; i < horizonSize - 1; ++i)
            {
                Console.WriteLine("x" + (i + 1) + " = " + ans[i].Str());
            }
            // Проверяем на количество решений.
            for (var i = 0; i < horizonSize - 1; ++i)
            {
                if (getLine[i] == 0)
                {
                    Console.WriteLine("И ещё бесконечное число решений!");
                    return;
                }
            }
        }

        /// <summary>
        /// Метод для нахождения решения (если оно есть).
        /// </summary>
        /// <param name="getLine">массив, в котором хранится индекс строки-ответа для кажого x.</param>
        private void GetAns(int[] getLine)
        {
            // Находим одно решения, оно хранится в ans.
            var ans = new Fraction[horizonSize - 1];
            for (var i = 0; i < horizonSize - 1; ++i)
            {
                if (getLine[i] != 0)
                {
                    ans[i] = matrix[getLine[i] - 1, horizonSize - 1];
                }
                else
                {
                    ans[i] = new Fraction();
                }
            }
            CheckAndPrint(getLine, ans);
        }

        /// <summary>
        /// Метод - алгоритм Гаусса.
        /// </summary>
        public void Gauss()
        {
            // Массив, в котором хранится индекс строки-ответа для кажого x.
            var getLine = new int[horizonSize - 1];
            // Храним на какой линии мы в данный момент.
            var line = 0;
            for (var column = 0; column < horizonSize - 1; ++column)
            {
                int maxLine = column;
                if (line >= verticalSize)
                {
                    break;
                }
                // Ищем максимальный по модулю элемент в столбце.
                for (var i = line; i < verticalSize; ++i)
                {
                    if (matrix[i, column] > matrix[maxLine, column])
                    {
                        maxLine = i;
                    }
                }
                // Проверяем, что в столбце есть элемент неравный 0.
                if (matrix[maxLine, column] != 0)
                {
                    getLine[column] = line + 1;
                    SwapAndZeroing(maxLine, line, column);
                    line += 1;
                }
            }
            GetAns(getLine);
        }

        /// <summary>
        /// Метод для нахождения следа матрицы.
        /// </summary>
        /// <returns>значение следа матрицы.</returns>
        public Fraction Trace()
        {
            int i = 0;
            var trace = new Fraction();
            while (i < verticalSize && i < horizonSize)
            {
                trace += matrix[i, i];
                i += 1;
            }
            return trace;
        }

        /// <summary>
        /// Метод, зануляющий i-ый столбец для поиска определителя.
        /// </summary>
        /// <param name="i">индекс столбца, который надо занулить.</param>
        private void Zeroing(int i)
        {
            // Зануление столбца матрицы.
            for (var j = 0; j < horizonSize; ++j)
            {
                if (j != i)
                {
                    for (int q = verticalSize - 1; q >= i; --q)
                    {
                        matrix[j, q] -= (matrix[j, i] / matrix[i, i]) * matrix[i, q];
                    }
                }
            }
            // Делим строку матрицы i, чтобы matrix[i, i] равнялось 1.
            for (int j = horizonSize - 1; j >= i; --j)
            {
                matrix[i, j] /= matrix[i, i];
            }
        }

        /// <summary>
        /// Метод для нахождения определителя.
        /// </summary>
        /// <returns>значение определителя.</returns>
        public Fraction Determinant()
        {
            var determinant = new Fraction();
            determinant.Numerator = 1;
            // Модифицированный алгоритм Гаусса.
            for (var i = 0; i < horizonSize; ++i)
            {
                int maxLine = i;
                // Ищем максимальный по модулю элемент в столбце.
                for (int j = i; j < verticalSize; ++j)
                {
                    if (matrix[j, i] > matrix[maxLine, i])
                    {
                        maxLine = j;
                    }
                }
                // Обмен строчек <=> изменению знака определителя.
                if (maxLine != i)
                {
                    determinant *= -1;
                }
                determinant = determinant * matrix[maxLine, i];
                if (determinant == 0)
                {
                    return determinant;
                }
                for (var j = 0; j < horizonSize; ++j)
                {
                    Fraction numSwap = matrix[maxLine, j];
                    matrix[maxLine, j] = matrix[i, j];
                    matrix[i, j] = numSwap;
                }
                Zeroing(i);
            }
            return determinant;
        }

        /// <summary>
        /// Метод для генерации рандомной матрицы.
        /// </summary>
        public void Generate()
        {
            Random rand = new Random();
            matrix = new Fraction[verticalSize, horizonSize];
            for (var i = 0; i < verticalSize; ++i)
            {
                for (var j = 0; j < horizonSize; ++j)
                {
                    var num = new Fraction();
                    num.Numerator = rand.Next(-10, 10);
                    num.Denominator = 1;
                    matrix[i, j] = num;
                }
            }
            // Выводим её для пользователя.
            Console.WriteLine("Матрица.");
            Print();
        }

        /// <summary>
        /// Метод для вывода матрицы.
        /// </summary>
        public void Print()
        {
            Console.WriteLine(verticalSize + " " + horizonSize);
            for (var i = 0; i < verticalSize; ++i)
            {
                for (var j = 0; j < horizonSize; ++j)
                {
                    Console.Write(matrix[i, j].Str() + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
