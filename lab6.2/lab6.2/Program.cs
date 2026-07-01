using System;

namespace Task2_3
{
    /// Класс, представляющий квадратное уравнение a*x^2 + b*x + c = 0.
    public class QuadraticEquation
    {
        // Упрощено: использование автосвойств с приватными сеттерами/проверками
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        // Конструктор по умолчанию
        public QuadraticEquation() : this(1, 0, 0) { }

        // Параметризованный конструктор
        public QuadraticEquation(double a, double b, double c)
        {
            if (a == 0)
                throw new ArgumentException("Коэффициент a не может быть равен 0, иначе уравнение не является квадратным");
            A = a;
            B = b;
            C = c;
        }

        // Конструктор копирования
        public QuadraticEquation(QuadraticEquation other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Объект для копирования не может быть null");

            A = other.A;
            B = other.B;
            C = other.C;
        }

        /// Дискриминант уравнения.
        public double Discriminant => B * B - 4 * A * C;

         /// Вычисляет корни квадратного уравнения.
        public double[] GetRoots()
        {
            double d = Discriminant;

            if (d < 0) return new double[0];
            if (d == 0) return new[] { -B / (2 * A) };

            double sqrtD = Math.Sqrt(d);
            return new[] { (-B - sqrtD) / (2 * A), (-B + sqrtD) / (2 * A) };
        }

        public override string ToString() => $"{A}x^2 + ({B})x + ({C}) = 0";

        // ===================== Задание 3: перегруженные операции =====================

        // Унарный ++ : увеличивает коэффициенты на 1
        public static QuadraticEquation operator ++(QuadraticEquation eq)
        {
            if (eq == null) throw new ArgumentNullException(nameof(eq));
            return new QuadraticEquation(eq.A + 1, eq.B + 1, eq.C + 1);
        }

        // Унарный -- : уменьшает коэффициенты на 1
        public static QuadraticEquation operator --(QuadraticEquation eq)
        {
            if (eq == null) throw new ArgumentNullException(nameof(eq));
            return new QuadraticEquation(eq.A - 1, eq.B - 1, eq.C - 1);
        }

        // Неявное приведение к double (возвращает дискриминант)
        public static implicit operator double(QuadraticEquation eq)
        {
            if (eq == null) throw new ArgumentNullException(nameof(eq));
            return eq.Discriminant;
        }

        // Явное приведение к bool (true, если корни существуют)
        public static explicit operator bool(QuadraticEquation eq)
        {
            if (eq == null) throw new ArgumentNullException(nameof(eq));
            return eq.Discriminant >= 0;
        }

        // Бинарное равенство ==
        public static bool operator ==(QuadraticEquation eq1, QuadraticEquation eq2)
        {
            if (ReferenceEquals(eq1, eq2)) return true;
            if (eq1 is null || eq2 is null) return false;
            return eq1.A == eq2.A && eq1.B == eq2.B && eq1.C == eq2.C;
        }

        // Бинарное неравенство !=
        public static bool operator !=(QuadraticEquation eq1, QuadraticEquation eq2) => !(eq1 == eq2);

        public override bool Equals(object obj) => obj is QuadraticEquation other && this == other;

        public override int GetHashCode() => HashCode.Combine(A, B, C);
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ЗАДАНИЕ 2: ТЕСТИРОВАНИЕ КЛАССА ===");

            // 1. Тест базовых конструкторов
            var eqDefault = new QuadraticEquation();
            Console.WriteLine($"Конструктор по умолчанию: {eqDefault}");

            // 2. Ввод данных с клавиатуры для пользовательского уравнения
            Console.WriteLine("\n--- Ввод пользовательских коэффициентов ---");
            double aCoeff = ReadDouble("Введите коэффициент a (не 0): ", allowZero: false);
            double bCoeff = ReadDouble("Введите коэффициент b: ", allowZero: true);
            double cCoeff = ReadDouble("Введите коэффициент c: ", allowZero: true);

            var eqUser = new QuadraticEquation(aCoeff, bCoeff, cCoeff);
            Console.WriteLine($"Создано уравнение пользователя: {eqUser}");
            Console.WriteLine($"Дискриминант: {eqUser.Discriminant}");
            PrintRoots(eqUser);

            // 3. Тест конструктора копирования
            var eqCopy = new QuadraticEquation(eqUser);
            Console.WriteLine($"Конструктор копирования (копия пользовательского): {eqCopy}");

            // 4. Проверка обработки ошибок (перехват исключений)
            Console.WriteLine("\n--- Проверка обработки исключений ---");
            try
            {
                var invalid = new QuadraticEquation(0, 5, 5);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Перехвачена ошибка создания при a = 0: {ex.Message}");
            }


            Console.WriteLine("\n=== ЗАДАНИЕ 3: ТЕСТИРОВАНИЕ ОПЕРАЦИЙ ===");

            Console.WriteLine($"Исходное уравнение: {eqUser}");

            // Тест унарных операторов ++ и --
            // Чтобы не получить a = 0 при уменьшении, обернем в try-catch
            try
            {
                var eqInc = ++eqUser;
                Console.WriteLine($"После ++ (коэффициенты +1): {eqInc}");

                var eqDec = --eqInc;
                Console.WriteLine($"После -- (вернули обратно): {eqDec}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Операция ++/-- привела к a = 0: {ex.Message}");
            }

            // Тест приведений типов
            double disc = eqUser;
            Console.WriteLine($"Неявное приведение к double (дискриминант): {disc}");

            bool hasRoots = (bool)eqUser;
            Console.WriteLine($"Явное приведение к bool (есть ли корни?): {hasRoots}");

            // Тест операций сравнения
            Console.WriteLine("\n--- Проверка операций сравнения ---");
            var eqSimilar = new QuadraticEquation(eqUser.A, eqUser.B, eqUser.C);
            Console.WriteLine($"Уравнение 1: {eqUser}");
            Console.WriteLine($"Уравнение 2 (с такими же коэффициентами): {eqSimilar}");
            Console.WriteLine($"Равенство (1 == 2): {eqUser == eqSimilar}");
            Console.WriteLine($"Неравенство (1 != 2): {eqUser != eqSimilar}");

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        /// Вывод корней в консоль.
        static void PrintRoots(QuadraticEquation eq)
        {
            double[] roots = eq.GetRoots();
            if (roots.Length == 0)
                Console.WriteLine("Действительных корней нет.");
            else if (roots.Length == 1)
                Console.WriteLine($"Один корень: x = {roots[0]}");
            else
                Console.WriteLine($"Два корня: x1 = {roots[0]}, x2 = {roots[1]}");
        }

        /// Безопасное чтение вещественных чисел с консоли с валидацией нуля.
        static double ReadDouble(string message, bool allowZero)
        {
            double result;
            Console.Write(message);
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out result))
                {
                    if (!allowZero && result == 0)
                    {
                        Console.Write("Ошибка! Коэффициент не может быть равен 0. Повторите ввод: ");
                        continue;
                    }
                    return result;
                }
                Console.Write("Ошибка ввода! Пожалуйста, введите число: ");
            }
        }
    }
}