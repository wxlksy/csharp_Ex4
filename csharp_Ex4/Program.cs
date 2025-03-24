using System;
using System.IO;
using System.Threading;

namespace MazeGame
{
    /// <summary>
    /// Основной класс программы, который запускает игру.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.StartGame();
        }
    }

    /// <summary>
    /// Класс для управления игровой логикой.
    /// </summary>
    class GameManager
    {
        private char[,] map;
        private int performerX, performerY;
        private int health = 100;

        public void StartGame()
        {
            try
            {
                map = ReadMap("C:\\Users\\1969-23\\Documents\\csharp_Ex4\\csharp_Ex4\\maps\\level01.txt", out performerX, out performerY);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке карты: {ex.Message}");
                return;
            }

            while (true)
            {
                Console.Clear();
                DrawMap(map);
                DrawHealthBar(health);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                MovePerformer(keyInfo);

                if (health <= 0)
                {
                    Console.WriteLine("Вы проиграли!");
                    break;
                }

                if (CheckWinCondition())
                {
                    Console.WriteLine("Вы выиграли!");
                    break;
                }

                Thread.Sleep(100); // Задержка для улучшения визуализации
            }
        }

        private char[,] ReadMap(string filePath, out int performerX, out int performerY)
        {
            performerX = 0;
            performerY = 0;

            string[] newFile = File.ReadAllLines(filePath);
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '■')
                    {
                        performerX = i;
                        performerY = j;
                        map[i, j] = ' '; // Очищаем позицию игрока на карте
                    }
                }
            }
            return map;
        }

        private void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == performerX && j == performerY)
                    {
                        Console.Write('P'); // Игрок
                    }
                    else
                    {
                        Console.Write(map[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }

        private void MovePerformer(ConsoleKeyInfo keyInfo)
        {
            int newX = performerX;
            int newY = performerY;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    newX--;
                    break;
                case ConsoleKey.DownArrow:
                    newX++;
                    break;
                case ConsoleKey.LeftArrow:
                    newY--;
                    break;
                case ConsoleKey.RightArrow:
                    newY++;
                    break;
                case ConsoleKey.P:
                    ShowPath();
                    break;
            }

            if (CanMoveTo(newX, newY))
            {
                performerX = newX;
                performerY = newY;
            }
        }

        private bool CanMoveTo(int x, int y)
        {
            return x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1) && map[x, y] != '║' && map[x, y] != '═';
        }

        private void DrawHealthBar(int health)
        {
            int bars = health / 10;
            Console.Write("Health: [");
            for (int i = 0; i < 10; i++)
            {
                if (i < bars)
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('_');
                }
            }
            Console.WriteLine("]");
        }

        private bool CheckWinCondition()
        {
            // Условие победы, например, достижение определенной позиции
            return performerX == map.GetLength(0) - 1 && performerY == map.GetLength(1) - 1;
        }

        private void ShowPath()
        {
            // Логика для отображения правильного маршрута
            // Например, можно использовать алгоритм поиска пути, такой как A*
            Console.WriteLine("Правильный маршрут показан!");
        }
    }
}
