using Interface;

namespace Model
{
    //made by 廖开翔
    public class Room : IRoom
    {
        public string ID            { get; private set; }
        public string RoomNum       { get; set; }
        public RoomType RType       { get; set; }
        public RoomStatus RStatus   { get; set; }
        public Room(string id, string roomNum, RoomType type, RoomStatus status)
        {
            ID = id;
            RoomNum = roomNum;
            RType = type;
            RStatus = status;
        }
        public Room()
        {
            ID = IClock.GetRoomID;
        }
    }

    // Made by chaogeb
    public class RoomPrice : IRoomPrice
    {
        public RoomType RType   { get; set; }
        public double Price     { get; set; }

        public RoomPrice(RoomType rType, double price)
        {
            RType = rType;
            Price = price;
        }

    }

    public class AvaliableRoom : IAvaliableRoom
    {
        public RoomType RType   { get; set; }
        public double Price     { get; set; }
        public int Remain       { get; set; }
        public bool Chosen { get; set; }

        public int ChosenNum { get; set; }

        public AvaliableRoom(RoomType rType, double price)
        {
            RType = rType;
            Price = price;
            Remain = 1;
        }

        public void Add()
        {
            Remain++;
        }
        public void Reduce()
        {
            Remain--;
        }
    }
}
