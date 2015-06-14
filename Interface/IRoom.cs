namespace Interface 
{
    // Made by chaogeb
    public interface IRoom
    {
        string ID           { get; }
        string RoomNum      { get; set; }
        RoomType RType      { get; set; }
        RoomStatus RStatus  { get; set; }
    }

    // Made by chaogeb
    public interface IRoomPrice
    {
        RoomType RType      { get; set; }
        double Price        { get; set; }
    }

    public interface IAvaliableRoom
    {
        RoomType RType      { get; set; }
        double Price        { get; set; }
        int Remain          { get; set; }

        bool Chosen { get; set; }
        int ChosenNum { get; set; }
    }
}
