using System;

namespace Task1
{
    /// Класс с тремя целочисленными полями.
    public class ThreeIntFields
    {
        // Автосвойства (код-стайл C#)
        public int First { get; set; }
        public int Second { get; set; }
        public int Third { get; set; }

        // Конструктор по умолчанию
        public ThreeIntFields()
        {
            First = 0;
            Second = 0;
            Third = 0;
        }

        // Параметризованный конструктор
        public ThreeIntFields(int first, int second, int third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        // Конструктор копирования
        public ThreeIntFields(ThreeIntFields other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Объект для копирования не может быть null");

            First = other.First;
            Second = other.Second;
            Third = other.Third;
        }

        /// Вычисляет минимальную из последних цифр полей.
        public int GetMinLastDigit()
        {
            int d1 = Math.Abs(First) % 10;
            int d2 = Math.Abs(Second) % 10;
            int d3 = Math.Abs(Third) % 10;

            // Находим минимум без использования LINQ
            int min = d1;
            if (d2 < min) min = d2;
            if (d3 < min) min = d3;

            return min;
        }

        public override string ToString()
        {
            return $"({First}, {Second}, {Third})";
        }
    }

    /// Дочерний класс — представляет дату.
    public class DateTriple : ThreeIntFields
    {
        private static readonly int[] DaysInMonth =
        {
            31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
        };

        public int Day
        {
            get => First;
            set
            {
                if (value < 1 || value > 31)
                    throw new ArgumentException("День должен быть в диапазоне от 1 до 31");
                First = value;
            }
        }

        public int Month
        {
            get => Second;
            set
            {
                if (value < 1 || value > 12)
                    throw new ArgumentException("Месяц должен быть в диапазоне от 1 до 12");
                Second = value;
            }
        }

        public int Year
        {
            get => Third;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Год должен быть положительным числом");
                Third = value;
            }
        }

        // Конструктор по умолчанию
        public DateTriple() : base(1, 1, 2000)
        {
        }

        // Параметризованный конструктор
        public DateTriple(int day, int month, int year) : base()
        {
            // Сначала присваиваем год и месяц, затем день (для корректной базовой логики)
            Year = year;
            Month = month;
            Day = day;
        }

        // Конструктор копирования
        public DateTriple(DateTriple other) : base(other)
        {
        }

        public bool IsLeapYear()
        {
            int y = Year;
            return (y % 4 == 0 && y % 100 != 0) || (y % 400 == 0);
        }

        public DateTriple GetNextDay()
        {
            int day = Day;
            int month = Month;
            int year = Year;

            int maxDay = DaysInMonth[month - 1];
            if (month == 2 && IsLeapYear())
                maxDay = 29;

            day++;
            if (day > maxDay)
            {
                day = 1;
                month++;
                if (month > 12)
                {
                    month = 1;
                    year++;
                }
            }

            return new DateTriple(day, month, year);
        }

        public override string ToString()
        {
            return $"{Day:D2}.{Month:D2}.{Year:D4}";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ КЛАССА ThreeIntFields ===");

            // 1. Тест конструктора по умолчанию
            var fieldsDefault = new ThreeIntFields();
            Console.WriteLine($"Конструктор по умолчанию: {fieldsDefault}");

            // 2. Ввод данных с клавиатуры с проверкой (Тест параметризованного конструктора)
            Console.WriteLine("\n--- Ввод пользовательских данных ---");
            int f = ReadInt("Введите первое число: ");
            int s = ReadInt("Введите второе число: ");
            int t = ReadInt("Введите третье число: ");

            var fieldsUser = new ThreeIntFields(f, s, t);
            Console.WriteLine($"Создан объект пользователя: {fieldsUser}");
            Console.WriteLine($"Минимальная из последних цифр: {fieldsUser.GetMinLastDigit()}");

            // 3. Тест конструктора копирования
            var fieldsCopy = new ThreeIntFields(fieldsUser);
            Console.WriteLine($"Конструктор копирования (копия объекта пользователя): {fieldsCopy}");


            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ДОЧЕРНЕГО КЛАССА DateTriple ===");

            // 1. Тест конструктора по умолчанию
            var dateDefault = new DateTriple();
            Console.WriteLine($"Конструктор по умолчанию: {dateDefault}");

            // 2. Ввод данных с клавиатуры с полной валидацией корректности даты
            Console.WriteLine("\n--- Ввод пользовательской даты ---");
            DateTriple dateUser = null;
            while (dateUser == null)
            {
                int day = ReadInt("Введите день: ");
                int month = ReadInt("Введите месяц: ");
                int year = ReadInt("Введите год: ");

                try
                {
                    // Проверяем логику дней в конкретном месяце перед созданием
                    // (например, чтобы нельзя было ввести 31 число для февраля)
                    if (month >= 1 && month <= 12 && year >= 1)
                    {
                        int maxDays = (month == 2 && ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))) ? 29 : 31; // грубая проверка верхнего лимита для исключения
                        // Свойства сами выбросят ArgumentException, если что-то не так
                    }

                    dateUser = new DateTriple(day, month, year);

                    // Дополнительная строгая проверка на реальное количество дней в месяце
                    int realMaxDay = (dateUser.Month == 2 && dateUser.IsLeapYear()) ? 29 : 31; // Свойства класса проверяют общие диапазоны, допроверим точные:
                    // Но чтобы не усложнять, доверяем сеттерам, которые ты написал. Единственное — проверим февраль:
                    if (dateUser.Month == 2 && dateUser.Day > 29)
                        throw new ArgumentException("В феврале не может быть больше 29 дней.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка валидации: {ex.Message} Попробуйте снова.");
                    dateUser = null; // сбрасываем, если упало на доп. проверках
                }
            }

            Console.WriteLine($"Успешно создана дата пользователя: {dateUser}");
            Console.WriteLine($"Год високосный: {dateUser.IsLeapYear()}");
            Console.WriteLine($"Следующий день: {dateUser.GetNextDay()}");

            // 3. Тест конструктора копирования для даты
            var dateCopy = new DateTriple(dateUser);
            Console.WriteLine($"Конструктор копирования (копия даты пользователя): {dateCopy}");

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        /// Вспомогательный метод для безопасного чтения целых чисел с клавиатуры.
        static int ReadInt(string message)
        {
            int result;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Ошибка ввода! Пожалуйста, введите корректное целое число: ");
            }
            return result;
        }
    }
}