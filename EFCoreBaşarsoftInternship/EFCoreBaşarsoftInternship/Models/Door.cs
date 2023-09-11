 namespace EFCoreBaşarsoftInternship.DTOs;


public class Door
{
    public int id { get; set; }
    public string name { get; set; }
    public double x { get; set; }
    public double y { get; set; }
}
public class DoorInfo
{
    public string name { get; set; }
    public double x { get; set; }
    public double y { get; set; }
}
public class DoorInfosUpdateDTO
{
    public string name { get; set; }
    public double x { get; set; }
    public double y { get; set; }
}
