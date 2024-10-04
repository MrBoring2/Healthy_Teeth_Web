using DataMigration;
using Microsoft.EntityFrameworkCore;
Console.OutputEncoding = System.Text.Encoding.UTF8;

using (var db = new HealthyTeethDbContextFactory())
{
        Console.WriteLine("Ожидание");
    var a = db.Visits.Include(p => p.ServiceToVisits).ThenInclude(p => p.Service).ToList();
    foreach (var item in a)
    {
        Console.WriteLine(item.Id);
        foreach (var item2 in item.ServiceToVisits)
        {
            Console.WriteLine(item2.Service.Title + " " + item2.Count);
        }

    }       
    db.Roles.Add(new Entities.Role { Title = "dsad" });
    db.SaveChanges();

    Console.WriteLine("Готово");
}

