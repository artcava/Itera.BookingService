namespace Itera.BookingService.Contracts.Legacy.Branch;

public class Filiale
{
    public int BranchID { get; set; }
    public string? Description { get; set; }
    public int FranchiseID { get; set; }
    public FilialeExtraData ExtraData { get; set; } = new();
    public List<FilialeOrariOperativi> TimeTableBranch { get; set; } = [];
    public FilialeOrariOperativi? TimeTableDaySelected { get; set; }
    public List<FilialeOrariOperativi> TimeTableVariation { get; set; } = [];
    public bool KeyBox { get; set; }
    public short StateID { get; set; }
    public bool ExcludeVAL { get; set; }
}

public class FilialeExtraData
{
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? Region { get; set; }
    public string? Telephone { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public double? CordX { get; set; }
    public double? CordY { get; set; }
    public double? SpnX { get; set; }
    public double? SpnY { get; set; }
    public string? MapsLink { get; set; }
    public string? TimeOfficeDescription { get; set; }
    public string? TimeDeliveryDescription { get; set; }
    public string? CommercialManager { get; set; }
    public string? AdministrationManager { get; set; }
    public string? ParkingClient { get; set; }
    public List<FilialeFasciaOrario> TimeSlotRetire { get; set; } = [];
    public List<FilialeFasciaOrario> TimeSlotDelivery { get; set; } = [];
    public List<FilialeRiposoSettimanale> WeeklyDayOfRest { get; set; } = [];
    public List<FilialeGiornoChiusuraExtra> ClosingDayExtra { get; set; } = [];
    public int FilialeMacroAreaID { get; set; }
    public int FilialeAreaID { get; set; }
    public int? Location { get; set; }
    public bool RentalCar { get; set; }
}

public class FilialeFasciaOrario
{
    public int TimeSlot { get; set; }
    public string? Description { get; set; }
    public short ChangerDay { get; set; }
    public bool Selected { get; set; }
    public string? DayPeriodID { get; set; }
    public string? Start { get; set; }
    public string? End { get; set; }
}

public class FilialeRiposoSettimanale
{
    public int DayOfWeek { get; set; }
    public string? Description { get; set; }
}

public class FilialeGiornoChiusuraExtra
{
    public int DayClosureExtraID { get; set; }
    public DateTime Day { get; set; }
    public string? DayFormatted { get; set; }
}

public class FilialeOrariOperativi
{
    public string? Date { get; set; }
    public int Day { get; set; }
    public string? DayDescription { get; set; }
    public List<FasciaOraria> TimeSlotDay { get; set; } = [];
}

public class FasciaOraria
{
    public string? Start { get; set; }
    public string? End { get; set; }
    public string? DayPeriodID { get; set; }
}
