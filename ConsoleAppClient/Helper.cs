using MenuLib;
using RestClientLib;
using StaffManagement.Models;

public static class Helper
{
    public static string BaseUrl { get; set; } = "https://localhost:7293/";

    public static MenuBank MenuBank { get; set; } = new MenuBank()
    {
        Title = "Staff Management",
        Menus = new List<Menu>()
        {
            new Menu(){ Text= "Viewing", Action=ViewingProducts},
            new Menu(){ Text= "Creating", Action=CreatingProducts},
            new Menu(){ Text= "Updating", Action=UpdatingProducts},
            new Menu(){ Text= "Deleting", Action=DeletingProducts},
            new Menu(){ Text= "Exiting", Action = ExitingProgram}
        }
    };
    public static void ExitingProgram()
        {
            Console.WriteLine("\n[Exiting Program]");
            Environment.Exit(0);
        }
    private static void DeletingProducts()
    {
        Task.Run(async () =>
        {
            RestClient restClient = new(BaseUrl);
            Console.WriteLine("\n[Deleting Product]");
            while (true)
            {
                Console.Write("Product Id/Code: ");
                var id = Console.ReadLine() ?? "";
                var endpoint = $"api/v1/staffs/{id}";
                var result = await restClient.DeleteAsync<Result<string>>(endpoint);
                if (result!.Data != null)
                {
                    Console.WriteLine($"Successfully delete the product with id/code, {id}");
                }
                else
                {
                    Console.WriteLine($"Failed to delete a product with id/code, {id}");
                }

                if (WaitForEscPressed("ESC to stop or any key for more deleting ..."))
                {
                    break;
                }
            }
        }).Wait();
    }
    private static void UpdatingProducts()
    {
        Task.Run(async () =>
        {
            RestClient restClient = new(BaseUrl);
            Console.WriteLine("\n[Updating Products]");
            while (true)
            {
                Console.Write("Product Id(required): ");
                var key = Console.ReadLine() ?? "";
                var endpoint = "api/v1/staffs";
                Console.Write("New Name (optional)  : ");
                var name = Console.ReadLine();

                var result = await restClient.PutAsync<Staff, Result<string>>(endpoint, new Staff()
                {
                    
                });

                if (result!.Data !=null)
                {
                    Console.WriteLine($"Successfully update the product with id/code, {key}");
                }
                else
                {
                    Console.WriteLine($"Failed to update the product with id/code, {key}");
                }

                Console.WriteLine();
                if (WaitForEscPressed("ESC to stop or any key for more updating...")) break;
            }
        }).Wait();
    }
    private static bool WaitForEscPressed(string text)
    { 
        Console.Write(text);;
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        Console.WriteLine(keyInfo.KeyChar);
        return keyInfo.Key == ConsoleKey.Escape;
    }
    private static void CreatingProducts()
    {
        Task.Run(async () =>
        {
            RestClient restClient = new(BaseUrl);
            Console.WriteLine("\n[Creating Product]");
            var endpoint = "api/products";
            while (true)
            {
                var req = CreateStaff();
                if (req != null)
                {
                    var result = await restClient.PostAsync<Staff, Result<string>>(endpoint, req);
                    var id = result!.Data;
                    if (!string.IsNullOrEmpty(id))
                        Console.WriteLine($"Successfully created a new product with id, {id}");
                    else
                        Console.WriteLine($"Failed to create a new product code, ");
                }

                Console.WriteLine();
                if (WaitForEscPressed("ESC to stop or any key for more creating...")) break;
            }
        }).Wait();
    }
    static Staff? CreateStaff()
    {
        string data = Console.ReadLine() ?? "";
        var dataParts = data.Split("/");
        if (dataParts.Length < 3)
        {
            Console.WriteLine("Invalid create product's data");
            return null;
        }
        var name = dataParts[0].Trim();
        var phoneNumber = dataParts[1].Trim();
        var address = dataParts[2].Trim();
       
        return new Staff() {  Name = name, PhoneNumber = phoneNumber, Address = address  };

    }
    private static  void ViewingProducts()
    {
        Task.Run(async () =>
        {
            RestClient restClient = new(BaseUrl);
            Console.WriteLine("\n[Viewing Products]");
            var endpoint = "api/v1/staffs";
            var result = await restClient.GetAsync<Result<List<Staff>>>(endpoint) ?? new();
            var all = result!.Data??new();
            var count = all.Count;
            Console.WriteLine($"Products: {count}");
            if (count == 0) return;

            Console.WriteLine($"{"Id",-36} {"Code",-10} {"Name",-30} {"Category",-20}");
            Console.WriteLine(new string('=', 36 + 1 + 10 + 1 + 30 + 1 + 20));
            foreach (var prd in all)
            {
                Console.WriteLine($"{prd.Id,-36} {prd.Name,-10} {prd.PhoneNumber,-30} {prd.Address,-20}");
            }
        }).Wait();
    }
    
    
}
