using AdoNetTask.Business.Services;
using AdoNetTask.Business.Helpers;
using AdoNetTask.Core.Entities;

Console.WriteLine("Ado Net Task:\n");
TaskService taskService = new TaskService();

bool runApp = true;
while (runApp)
{
Start:
    Console.WriteLine($"1 - Add object to database\n" +
                      $"2 - Show object not added to DB\n" +
                      $"3 - Show number of user's posts\n" +
                      $"--------------------------------\n" +
                      $"0 - Exit");
    string? option = Console.ReadLine();
    int optionNumber;
    bool isInt = int.TryParse(option, out optionNumber);
    if (isInt)
    {
        if (optionNumber >= 0 && optionNumber <= 3)
        {
            switch (optionNumber)
            {
                case (int)MenuEnum.AddDataToDb:
                    try
                    {
                        Console.WriteLine("Enter post's ID you want to add to DB:");
                        int postId = Convert.ToInt32(Console.ReadLine());
                        Post post = await taskService.GetObjectByIdAsync(postId);
                        if (post != null)
                        {
                            await taskService.AddObjToDbAsync(post);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Failed.");
                            Console.ResetColor();
                            goto Start;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        goto Start;
                    }
                    break;
                case (int)MenuEnum.ShowDatasNotInDb:
                    try
                    {
                        await taskService.PostsNotFoundInDbAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(ex);
                        Console.ResetColor();
                        goto Start;
                    }
                    break;
                case (int)MenuEnum.CountPosts:
                    try
                    {
                        Console.WriteLine("Enter User Id:");
                        int userId = Convert.ToInt32(Console.ReadLine());
                        int result = await taskService.GetPostCountAsync(userId);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"User with {userId} ID currently has {result} posts.");
                        Console.ResetColor();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        goto Start;
                    }
                    break;
                default:
                    runApp = false;
                    break;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Please, enter correct option number.");
            Console.ResetColor();
        }

    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Please, enter correct format to choose an option.");
        Console.ResetColor();
    }

}



