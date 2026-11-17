using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastsignalv0._2
{
    internal class Program
    {
        static void Main(string[] args)
        {// 1. Константы правил игры
            const int MaxHealth = 100;
            const int MaxOxygen = 100;
            const int OxygenDecreasePerTurn = 8;      // Трата кислорода за ход
            const int MedicalKitHealingPower = 25;    // Лечение от аптечки
            const int RequiredScrapForRepair = 3;     // Нужно лома для ремонта
            const int RequiredEnergyForRepair = 20;   // Нужно энергии для ремонта

            // 2. Переменные состояния игры
            string currentCompartmentName = "Центральный отсек";
            int currentHealth = MaxHealth;
            int currentOxygen = MaxOxygen;
            int energyCount = 50;
            int scrapCount = 0;
            int medicalKitsCount = 1;
            int turnCount = 0;

            bool isTransmitterRepaired = false;
            bool isVictory = false;
            bool isDefeat = false;
            bool isExitRequested = false;

            Random random = new Random();

            // 3. Основной игровой цикл (зависит от переменных состояния: победа, поражение, выход)
            while (!isVictory && !isDefeat && !isExitRequested)
            {
                Console.Clear();

                // Вывод состояния станции и персонажа
                Console.WriteLine($"=== ОТСЕК: {currentCompartmentName} | ХОД: {turnCount} ===");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"Здоровье: {currentHealth}/{MaxHealth} HP | Кислород: {currentOxygen}/{MaxOxygen} O2");
                Console.WriteLine($"Ресурсы:  Металлолом: {scrapCount}/{RequiredScrapForRepair} | Энергия: {energyCount}");
                Console.WriteLine($"Инвентарь: Аптечки: {medicalKitsCount} шт.");
                Console.WriteLine($"Передатчик: {(isTransmitterRepaired ? "ИСПРАВЕН" : "ПОЛОМАН")}");
                Console.WriteLine("--------------------------------------------------");

                // Конструкция if - else if - else для проверки диапазонов состояния (предупреждения)
                if (currentHealth <= 30 || currentOxygen <= 24)
                {
                    Console.WriteLine("[КРИТИЧНО] Жизненные показатели на пределе! Срочно примите меры!");
                }
                else if (currentHealth <= 60 || currentOxygen <= 50)
                {
                    Console.WriteLine("[ВНИМАНИЕ] Запасы кислорода или здоровье снижены.");
                }
                else
                {
                    Console.WriteLine("[НОРМА] Системы жизнеобеспечения скафандра в норме.");
                }
                Console.WriteLine("--------------------------------------------------");

                // Меню действий игрока
                Console.WriteLine("1 - Исследовать Инженерный отсек");
                Console.WriteLine("2 - Вернуться в Центральный отсек");
                Console.WriteLine("3 - Использовать аптечку (+25 HP)");
                Console.WriteLine("4 - Починить передатчик (нужно 3 лома и 20 энергии)");
                Console.WriteLine("5 - Отправить сигнал SOS");
                Console.WriteLine("0 (или exit / q) - Выйти из игры");
                Console.Write("Ваш выбор: ");

                string userInput = Console.ReadLine()?.Trim().ToLower() ?? "";

                // Проверка текстовых команд выхода по ТЗ (exit / q)
                if (userInput == "exit" || userInput == "q")
                {
                    userInput = "0";
                }

                if (!int.TryParse(userInput, out int playerCommand))
                {
                    Console.WriteLine("\n[!] Ошибка: введите номер команды из меню (0-5) или 'exit'.");
                    Console.WriteLine("Нажмите любую клавишу для повтора...");
                    Console.ReadKey();
                    continue;
                }

                bool turnSpent = false; // Флаг, был ли потрачен игровой ход

                // Конструкция switch для выбора конкретной команды игрока
                switch (playerCommand)
                {
                    case 1: // Сценарий 1: Исследование отсека и случайные события
                        currentCompartmentName = "Инженерный отсек";
                        scrapCount += 1;
                        Console.WriteLine("\nВы обыскали Инженерный отсек и нашли 1 ед. металлолома.");

                        // Проверка случайного события (вероятность 30%)
                        int eventChance = random.Next(1, 101);
                        if (eventChance <= 30)
                        {
                            int eventType = random.Next(1, 4);
                            if (eventType == 1)
                            {
                                currentHealth -= 25;
                                Console.WriteLine("[СОБЫТИЕ] Сорвало обшивку! Вы получили травму (-25 HP).");
                            }
                            else if (eventType == 2)
                            {
                                currentOxygen -= 12;
                                Console.WriteLine("[СОБЫТИЕ] Произошла утечка кислорода (-12 O2).");
                            }
                            else
                            {
                                medicalKitsCount += 1;
                                Console.WriteLine("[СОБЫТИЕ] В шкафчике экипажа найдена дополнительная аптечка!");
                            }
                        }

                        turnSpent = true;
                        break;

                    case 2: // Переход в Центральный отсек
                        if (currentCompartmentName == "Центральный отсек")
                        {
                            Console.WriteLine("\nВы уже находитесь в Центральном отсеке.");
                        }
                        else
                        {
                            currentCompartmentName = "Центральный отсек";
                            Console.WriteLine("\nВы вернулись в Центральный отсек к терминалу связи.");
                            turnSpent = true;
                        }
                        break;

                    case 3: // Сценарий 3: Лечение (использование аптечки)
                        if (medicalKitsCount > 0)
                        {
                            if (currentHealth == MaxHealth)
                            {
                                Console.WriteLine("\nЗдоровье уже на максимуме, аптечка не потрачена.");
                            }
                            else
                            {
                                medicalKitsCount--;
                                currentHealth += MedicalKitHealingPower;

                                // Граничное условие: здоровье не выше максимума
                                if (currentHealth > MaxHealth)
                                {
                                    currentHealth = MaxHealth;
                                }

                                Console.WriteLine($"\nВы использовали аптечку. Текущее здоровье: {currentHealth} HP.");
                                turnSpent = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nУ вас закончились аптечки!");
                        }
                        break;

                    case 4: // Сценарий 2 (часть 1): Ремонт передатчика
                        if (currentCompartmentName != "Центральный отсек")
                        {
                            Console.WriteLine("\nПередатчик находится в Центральном отсеке! Сначала вернитесь туда.");
                        }
                        else if (isTransmitterRepaired)
                        {
                            Console.WriteLine("\nПередатчик уже исправен, повторный ремонт не требуется.");
                        }
                        else if (scrapCount >= RequiredScrapForRepair && energyCount >= RequiredEnergyForRepair)
                        {
                            scrapCount -= RequiredScrapForRepair;
                            energyCount -= RequiredEnergyForRepair;
                            isTransmitterRepaired = true;
                            Console.WriteLine("\n[УСПЕХ] Передатчик восстановлен! Теперь можно отправить сигнал SOS.");
                            turnSpent = true;
                        }
                        else
                        {
                            Console.WriteLine("\nНедостаточно ресурсов! Нужно 3 ед. металлолома и 20 ед. энергии.");
                        }
                        break;

                    case 5: // Сценарий 2 (часть 2): Отправка сигнала SOS (Победа)
                        if (currentCompartmentName != "Центральный отсек")
                        {
                            Console.WriteLine("\nТерминал отправки SOS находится в Центральном отсеке!");
                        }
                        else if (!isTransmitterRepaired)
                        {
                            Console.WriteLine("\nПередатчик сломан! Сигнал SOS отправить невозможно.");
                        }
                        else
                        {
                            isVictory = true;
                        }
                        break;

                    case 0: // Корректный выход из игры
                        isExitRequested = true;
                        break;

                    default: // Обработка нераспознанной числовой командыgit init

                        Console.WriteLine("\n[!] Неизвестная команда! Выберите пункт меню от 0 до 5.");
                        break;
                }

                // Расчёт расхода кислорода и счётчика ходов, если действие заняло ход
                if (turnSpent && !isVictory && !isExitRequested)
                {
                    turnCount++;
                    currentOxygen -= OxygenDecreasePerTurn;
                }

                // Граничные условия: показатели не должны уходить в отрицательные значения
                if (currentHealth < 0)
                {
                    currentHealth = 0;
                }
                if (currentOxygen < 0)
                {
                    currentOxygen = 0;
                }
                if (scrapCount < 0)
                {
                    scrapCount = 0;
                }
                if (energyCount < 0)
                {
                    energyCount = 0;
                }

                // Проверка условий поражения
                if (currentHealth == 0 || currentOxygen == 0)
                {
                    isDefeat = true;
                }

                if (!isVictory && !isDefeat && !isExitRequested)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }

            // 4. Экран завершения партии (итоги)
            Console.Clear();
            Console.WriteLine("==================================================");
            if (isVictory)
            {
                Console.WriteLine("ПОБЕДА! Аварийный сигнал SOS успешно отправлен на Землю!");
            }
            else if (isDefeat)
            {
                Console.WriteLine("ПОРАЖЕНИЕ! Жизненные показатели упали до нуля.");
            }
            else
            {
                Console.WriteLine("ВЫХОД ИЗ ИГРЫ. Сессия завершена игроком.");
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Итоговая статистика: прожито ходов — {turnCount}");
            Console.WriteLine($"Остаток: {currentHealth} HP | {currentOxygen} O2 | Лом: {scrapCount} | Энергия: {energyCount}");
            Console.WriteLine("==================================================");
            Console.WriteLine("Нажмите любую клавишу для закрытия окна...");
            Console.ReadKey();
        }
    }
}
