using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly CreateUserView createUserView;
    private readonly ListUsersView listUsersView;

    public ManageUsersView(IUserRepository userRepository)
    {
        createUserView = new CreateUserView(userRepository);
        listUsersView = new ListUsersView(userRepository);
    }

    public async Task ShowAsync()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("--- Manage Users ---");
            Console.WriteLine("1. Create user");
            Console.WriteLine("2. List users");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await createUserView.ShowAsync();
                    break;

                case "2":
                    Console.WriteLine("ENTRE EN OPCION 2");
                    listUsersView.Show();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}