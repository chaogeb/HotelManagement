using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;
using Model;


namespace Model
{
    class Initialize
    {
        public Room RoomInitialize()
        {
            string id=Clock.GetRoomCount;
            return new Room(id,"000",0,0,0);
        }
         
    }
}
