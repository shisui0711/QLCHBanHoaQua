using System;

namespace Framework
{
    public static class ViewHelper
    {
        public static void PrintSuccess(string message)
        {
            Start:
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{"",30}╔{string.Concat(Enumerable.Repeat("═", message.Length + 5))}╗");
                var height = Console.GetCursorPosition();
                Console.WriteLine();
                Console.Write($"{"",30}║  {message}");
                Console.SetCursorPosition(height.Left - 1, height.Top + 1);
                Console.WriteLine("║");
                Console.WriteLine($"{"",30}╚{string.Concat(Enumerable.Repeat("═", message.Length + 5))}╝");
            }
            catch (ArgumentOutOfRangeException)
            {

                ClearWhenOverFlow();
                Console.BufferHeight += 3;
                Console.BufferWidth += 3;
                goto Start;
            }

        }
        public static void PrintError(string message)
        {
            Start:
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{"",30}╔{string.Concat(Enumerable.Repeat("═", message.Length + 5))}╗");
                var height = Console.GetCursorPosition();
                Console.WriteLine();
                Console.Write($"{"",30}║  {message}");
                Console.SetCursorPosition(height.Left - 1, height.Top + 1);
                Console.WriteLine("║");
                Console.WriteLine($"{"",30}╚{string.Concat(Enumerable.Repeat("═", message.Length + 5))}╝");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch (ArgumentOutOfRangeException)
            {
                ClearWhenOverFlow();
                Console.BufferHeight += 3;
                Console.BufferWidth += 3;
                goto Start;
            }

        }
        public static void PrintWarning(string message)
        {
            Start:
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{"",30}╔{string.Concat(Enumerable.Repeat("═", message.Length + 5))}╗");
                var height = Console.GetCursorPosition();
                Console.WriteLine();
                Console.Write($"{"",30}║  {message}");
                Console.SetCursorPosition(height.Left - 1, height.Top + 1);
                Console.WriteLine("║");
                Console.WriteLine($"{"",30}╚{string.Concat(Enumerable.Repeat("═", message.Length + 5))}╝");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch (ArgumentOutOfRangeException)
            {
                ClearWhenOverFlow();
                Console.BufferHeight += 3;
                Console.BufferWidth += 3;
                goto Start;
            }
        }
        public static void ClearPreviousLine(int n)
        {
            try
            {
                for (int i = 0; i < n; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
                }
            }
            catch (Exception)
            {
                Console.Clear();
                PrintError("Lỗi hiển thị.");
                PrintError("Vui lòng để chế độ toàn màn hình");
                Environment.Exit(0);
            }
        }
        private static void ClearWhenOverFlow()
        {
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
        }
        public static T Input<T>(string message, int width = 35)
        {
            Start:
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
            Input:
                Console.Write($"╔{string.Concat(Enumerable.Repeat("═", message.Length + 5))}╦");
                var height = Console.GetCursorPosition();
                Console.Write($"{string.Concat(Enumerable.Repeat("═", width))}╗");
                var height2 = Console.GetCursorPosition();
                Console.WriteLine();
                Console.Write($"║  {message}");
                Console.SetCursorPosition(height.Left - 1, height.Top + 1);
                Console.Write("║ ");
                var pos_input = Console.GetCursorPosition();
                Console.SetCursorPosition(height2.Left - 1, height2.Top + 1);
                Console.WriteLine("║");
                Console.Write($"╚{string.Concat(Enumerable.Repeat("═", message.Length + 5))}╩");
                Console.WriteLine($"{string.Concat(Enumerable.Repeat("═", width))}╝");
                var pos_end = Console.GetCursorPosition();
                Console.SetCursorPosition(pos_input.Left, pos_input.Top);
                Console.CursorVisible = true;
                string temp = Console.ReadLine();
                if (temp == "Exit" || temp == "exit")
                {
                    Console.CursorVisible = false;
                    throw new ExitException(); 
                }
                try
                {
                    T res = (T)Convert.ChangeType(temp, typeof(T));
                    Console.CursorVisible = false;
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    return res;
                }
                catch (InvalidCastException)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    Console.CursorVisible = false;
                    PrintError("Dữ liệu không hợp lệ.");
                    Thread.Sleep(1000);
                    ClearPreviousLine(6);
                    goto Input;
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    Console.CursorVisible = false;
                    PrintError("Dữ liệu không hợp lệ.");
                    Thread.Sleep(1000);
                    ClearPreviousLine(6);
                    goto Input;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                ClearWhenOverFlow();
                Console.BufferHeight += 3;
                Console.BufferWidth += 3;
                goto Start;
            }
            
        }
    }
}