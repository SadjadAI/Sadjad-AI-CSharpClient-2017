using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace Sadjad_AI_CSharpClient_2017
{
    class Program
    {
        static void Main(string[] args)
        {
            Ai AI = new Ai();

            var socket = IO.Socket(AI.ServerAdr);

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected...");
                socket.Emit("init", AI.Token);
            });

            while(true)
            {
                socket.On("result", (data) =>
                {
                    try
                    {
                        JObject OResource = JObject.FromObject(data);
                        Int32 Score = Convert.ToInt32(OResource["score"]);
                        Int32 TurnNumber = Convert.ToInt32(OResource["turn_number"]);

                        JArray AResource = JArray.Parse(OResource["data"].ToString());

                        List<ABlast.Cell> Map = new List<ABlast.Cell>();

                        foreach(var Items in AResource)
                        {
                            foreach(var Item in Items)
                            {
                                if (Item["type"].ToString() == "black")
                                {
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.JailedBird, (Int32)ABlast.ColorType.Black, (Int32)ABlast.Direction.NoDirection, 0);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "red")
                                {
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.JailedBird, (Int32)ABlast.ColorType.Red, (Int32)ABlast.Direction.NoDirection, 0);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "green")
                                {
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.JailedBird, (Int32)ABlast.ColorType.Green, (Int32)ABlast.Direction.NoDirection, 0);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "blue")
                                {
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.JailedBird, (Int32)ABlast.ColorType.Blue, (Int32)ABlast.Direction.NoDirection, 0);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "yellow")
                                {
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.JailedBird, (Int32)ABlast.ColorType.Yellow, (Int32)ABlast.Direction.NoDirection, 0);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "bomb")
                                {
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.Bomb, (Int32)ABlast.ColorType.NoColor, (Int32)ABlast.Direction.NoDirection, 0);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "laser")
                                {
                                    Int32 color = 0;
                                    switch (Item["color"].ToString())
                                    {
                                        case "red":
                                            color = (Int32)ABlast.ColorType.Red;
                                            break;
                                        case "yellow":
                                            color = (Int32)ABlast.ColorType.Yellow;
                                            break;
                                        case "blue":
                                            color = (Int32)ABlast.ColorType.Blue;
                                            break;
                                        case "green":
                                            color = (Int32)ABlast.ColorType.Green;
                                            break;
                                        case "black":
                                            color = (Int32)ABlast.ColorType.Black;
                                            break;
                                    }
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.Laser, (Int32)color, (Int32)ABlast.Direction.NoDirection, 0);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "rocket")
                                {
                                    Int32 direction = 0;
                                    switch (Item["direction"].ToString())
                                    {
                                        case "0":
                                            direction = 0;
                                            break;
                                        case "1":
                                            direction = 1;
                                            break;
                                    }

                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.Rocket, (Int32)ABlast.ColorType.NoColor, (Int32)direction, 0);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "glass")
                                {
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.Glass, (Int32)ABlast.ColorType.NoColor, (Int32)ABlast.Direction.NoDirection, Convert.ToInt32(Item["life_time"].ToString()));
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "wood")
                                {
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.Wood, (Int32)ABlast.ColorType.NoColor, (Int32)ABlast.Direction.NoDirection, Convert.ToInt32(Item["life_time"].ToString()));
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "rock")
                                {
                                    ABlast.Cell cell = new ABlast.Cell((Int32)ABlast.Type.Rock, (Int32)ABlast.ColorType.NoColor, (Int32)ABlast.Direction.NoDirection, Convert.ToInt32(Item["life_time"].ToString()));
                                    Map.Add(cell);
                                }
                            }
                        }

                        ABlast.Board Board = new ABlast.Board(Score, TurnNumber, Map);

                        Int32[] touch = new Int32[2];

                        AI.doTurn(ref touch, Board);

                        socket.Emit("touch", AI.Token, touch[0], touch[1]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                });
            }
        }
    }
}

namespace ABlast
{
    enum Type
    {
        JailedBird = 0,
        Bomb,
        Rocket,
        Laser,
        Glass,
        Wood,
        Rock
    }

    enum ColorType
    {
        NoColor = 0,
        Red = 1,
        Blue = 2,
        Yellow = 3,
        Black = 4,
        Green = 5
    }

    enum Direction
    {
        VERTICAL = 0,
        HORIZONTAL = 1,
        NoDirection = 2
    }

    public class Cell
    {
        public Cell(Int32 Type, Int32 Direction, Int32 Color, Int32 LifeTime)
        {
            this.Type = Type;
            this.Color = Color;
            this.Direction = Direction;
            this.LifeTime = LifeTime;
        }
        private Int32 Type;
        private Int32 Direction;
        private Int32 Color;
        private Int32 LifeTime;

        //private X Content;

        public String GetType
        {
            get
            {
                switch (Type)
                {
                    case 1:
                        return "JailedBird";
                        break;
                    case 2:
                        return "Bomb";
                        break;
                    case 3:
                        return "Rocket";
                        break;
                    case 4:
                        return "Laser";
                        break;
                    case 5:
                        return "Glass";
                        break;
                    case 6:
                        return "Wood";
                        break;
                    case 7:
                        return "Rock";
                        break;
                    default:
                        return "NoType";
                        break;
                }
            }
        }
        public String GetDirection
        {
            get
            {
                switch (Direction)
                {
                    case 1:
                        return "Horizontal";
                        break;
                    case 2:
                        return "NoDirection";
                        break;
                    case 0:
                        return "Vertical";
                        break;
                    default:
                        return "Null";
                        break;
                }
            }
        }
        public String GetColor
        {
            get
            {
                switch (Color)
                {
                    case 0:
                        return "NoColor";
                        break;
                    case 1:
                        return "Red";
                        break;
                    case 2:
                        return "Blue";
                        break;
                    case 3:
                        return "Yellow";
                        break;
                    case 4:
                        return "Black";
                        break;
                    case 5:
                        return "Green";
                        break;
                    default:
                        return "Null";
                        break;
                }
            }
        }
        public Int32 GetLifeTime
        {
            get
            {
                return LifeTime;
            }
        }

        /*public X GetContent
        {
            get
            {
                return Content;
            }
        }*/
    }

    /*public class CellResource
    {
        public CellResource()
        {
            Description = "";
        }
        protected String Description;
    }

    public class JailedBird : CellResource
    {
        private Int32 Color;
        public JailedBird(Int32 Color)
        {
            switch(Color)
            {
                case 1:
                    Description = "Red";
                    break;
                case 2:
                    Description = "Blue";
                    break;
                case 3:
                    Description = "Yellow";
                    break;
                case 4:
                    Description = "Black";
                    break;
                case 5:
                    Description = "Green";
                    break;
            }
            this.Color = Color;
        }

        public String GetColor
        {
            get
            {
                return Description;
            }
        }
    }

    public class Bomb : CellResource
    {

    }

    public class Rocket : CellResource
    {
        public Rocket(Int32 Direction)
        {
            switch (Direction)
            {
                case 0:
                    Description = "VERTICAL";
                    break;
                case 1:
                    Description = "HORIZONTAL";
                    break;
            }
            this.Direction = Direction;
        }
        private Int32 Direction;

        public String GetDirection
        {
            get
            {
                return Description;
            }
        }
    }

    public class Laser : CellResource
    {
        private Int32 Color;
        public Laser(Int32 Color)
        {
            switch (Color)
            {
                case 1:
                    Description = "Red";
                    break;
                case 2:
                    Description = "Blue";
                    break;
                case 3:
                    Description = "Yellow";
                    break;
                case 4:
                    Description = "Black";
                    break;
                case 5:
                    Description = "Green";
                    break;
            }
            this.Color = Color;

            
        }

        public String GetColor
        {
            get
            {
                return Description;
            }
        }
    }

    public class Glass : CellResource
    {
        private Int32 LifeTime;
        public Glass(Int32 LifeTime)
        {
            this.LifeTime = LifeTime;
            Description = LifeTime.ToString();
        }

        public Int32 GetLifeTime
        {
            get
            {
                return LifeTime;
            }
        }
    }

    public class Wood : CellResource
    {
        private Int32 LifeTime;
        public Wood(Int32 LifeTime)
        {
            this.LifeTime = LifeTime;
            Description = LifeTime.ToString();
        }

        public Int32 GetLifeTime
        {
            get
            {
                return LifeTime;
            }
        }
    }

    public class Rock : CellResource
    {
        private Int32 LifeTime;
        public Rock(Int32 LifeTime)
        {
            this.LifeTime = LifeTime;
            Description = LifeTime.ToString();
        }

        public Int32 GetLifeTime
        {
            get
            {
                return LifeTime;
            }
        }
    }*/

    public class Board
    {
        private Int32 Score;
        private Int32 TurnNumber;
        private List<Cell> Map;

        public Board(Int32 Score, Int32 TurnNumber, List<Cell> Map)
        {
            this.Score = Score;
            this.TurnNumber = TurnNumber;
            this.Map = Map;
        }

        public Int32 GetScore
        {
            get
            {
                return Score;
            }
        }

        public Int32 GetTurnNumber
        {
            get
            {
                return TurnNumber;
            }
        }

        public List<Cell> GetMap
        {
            get
            {
                return Map;
            }
        }
    }

}