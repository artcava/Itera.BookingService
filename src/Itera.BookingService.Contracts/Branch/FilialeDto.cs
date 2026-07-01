namespace Itera.BookingService.Contracts.Branch;

public class FilialeDto
{
    public int BranchID { get; set; }
    public string? Description { get; set; }
    public int FranchiseID { get; set; }
    public FilialeExtraDataDto ExtraData { get; set; } = new();
    public List<FilialeOrariOperativiDto> TimeTableBranch { get; set; } = [];
    public FilialeOrariOperativiDto? TimeTableDaySelected { get; set; }
    public List<FilialeOrariOperativiDto> TimeTableVariation { get; set; } = [];
    public bool KeyBox { get; set; }
    public short StateID { get; set; }
    public bool ExcludeVAL { get; set; }
}

public class FilialeExtraDataDto
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
    public List<FilialeFasciaOrarioDto> TimeSlotRetire { get; set; } = [];
    public List<FilialeFasciaOrarioDto> TimeSlotDelivery { get; set; } = [];
    public List<FilialeRiposoSettimanaleDto> WeeklyDayOfRest { get; set; } = [];
    public List<FilialeGiornoChiusuraExtraDto> ClosingDayExtra { get; set; } = [];
    public int FilialeMacroAreaID { get; set; }
    public int FilialeAreaID { get; set; }
    public int? Location { get; set; }
    public bool RentalCar { get; set; }
}

public class FilialeFasciaOrarioDto
{
    public int TimeSlot { get; set; }
    public string? Description { get; set; }
    public short ChangerDay { get; set; }
    public bool Selected { get; set; }
    public string? DayPeriodID { get; set; }
    public string? Start { get; set; }
    public string? End { get; set; }
}

public class FilialeRiposoSettimanaleDto
{
    public int DayOfWeek { get; set; }
    public string? Description { get; set; }
}

public class FilialeGiornoChiusuraExtraDto
{
    public int DayClosureExtraID { get; set; }
    public DateTime Day { get; set; }
    public string? DayFormatted { get; set; }
}

public class FilialeOrariOperativiDto
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
