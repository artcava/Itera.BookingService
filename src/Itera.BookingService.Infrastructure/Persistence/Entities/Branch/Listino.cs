namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class Listino
{
    public int ListinoID { get; set; }
}

public class ListinoBrand
{
    public int ListinoBrandID { get; set; }
    public int ListinoID { get; set; }
}

public class ListinoGiorni
{
    public int ListinoGiorniID { get; set; }
    public int ListinoID { get; set; }
}

public class ListinoKm
{
    public int ListinoKmID { get; set; }
    public int ListinoID { get; set; }
}

public class ListinoValori
{
    public int ListinoValoriID { get; set; }
    public int ListinoID { get; set; }
}

public class ListinoFranchigia
{
    public int ListinoFranchigiaID { get; set; }
    public int ListinoID { get; set; }
}
