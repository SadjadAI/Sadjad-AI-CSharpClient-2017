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

                        List<Object> Map = new List<Object>();

                        foreach(var Items in AResource)
                        {
                            foreach(var Item in Items)
                            {
                                if (Item["type"].ToString() == "black")
                                {
                                    ABlast.JailedBird JB = new ABlast.JailedBird((Int32)ABlast.ColorType.Black);
                                    ABlast.Cell<ABlast.JailedBird> cell = new ABlast.Cell<ABlast.JailedBird>((Int32)ABlast.Type.JailedBird, JB);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "red")
                                {
                                    ABlast.JailedBird JB = new ABlast.JailedBird((Int32)ABlast.ColorType.Red);
                                    ABlast.Cell<ABlast.JailedBird> cell = new ABlast.Cell<ABlast.JailedBird>((Int32)ABlast.Type.JailedBird, JB);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "green")
                                {
                                    ABlast.JailedBird JB = new ABlast.JailedBird((Int32)ABlast.ColorType.Green);
                                    ABlast.Cell<ABlast.JailedBird> cell = new ABlast.Cell<ABlast.JailedBird>((Int32)ABlast.Type.JailedBird, JB);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "blue")
                                {
                                    ABlast.JailedBird JB = new ABlast.JailedBird((Int32)ABlast.ColorType.Blue);
                                    ABlast.Cell<ABlast.JailedBird> cell = new ABlast.Cell<ABlast.JailedBird>((Int32)ABlast.Type.JailedBird, JB);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "yellow")
                                {
                                    ABlast.JailedBird JB = new ABlast.JailedBird((Int32)ABlast.ColorType.Yellow);
                                    ABlast.Cell<ABlast.JailedBird> cell = new ABlast.Cell<ABlast.JailedBird>((Int32)ABlast.Type.JailedBird, JB);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "bomb")
                                {
                                    ABlast.Bomb Bomb = new ABlast.Bomb();
                                    ABlast.Cell<ABlast.Bomb> cell = new ABlast.Cell<ABlast.Bomb>((Int32)ABlast.Type.Bomb, Bomb);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "laser")
                                {
                                    ABlast.Laser Laser;
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

                                    Laser = new ABlast.Laser(color);

                                    ABlast.Cell<ABlast.Laser> cell = new ABlast.Cell<ABlast.Laser>((Int32)ABlast.Type.Laser, Laser);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "rocket")
                                {
                                    ABlast.Rocket Rocket;
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

                                    Rocket = new ABlast.Rocket(direction);

                                    ABlast.Cell<ABlast.Rocket> cell = new ABlast.Cell<ABlast.Rocket>((Int32)ABlast.Type.Rocket, Rocket);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "glass")
                                {
                                        ABlast.Glass Glass;

                                        Glass = new ABlast.Glass(Convert.ToInt32(Item["life_time"].ToString()));

                                        ABlast.Cell<ABlast.Glass> cell = new ABlast.Cell<ABlast.Glass>((Int32)ABlast.Type.Glass, Glass);
                                        Map.Add(cell);
                                    }
                                else if (Item["type"].ToString() == "wood")
                                {
                                    ABlast.Wood Wood;

                                    Wood = new ABlast.Wood(Convert.ToInt32(Item["life_time"].ToString()));

                                    ABlast.Cell<ABlast.Wood> cell = new ABlast.Cell<ABlast.Wood>((Int32)ABlast.Type.Wood, Wood);
                                    Map.Add(cell);
                                }
                                else if (Item["type"].ToString() == "rock")
                                {
                                    ABlast.Rock Rock;

                                    Rock = new ABlast.Rock(Convert.ToInt32(Item["life_time"].ToString()));

                                    ABlast.Cell<ABlast.Rock> cell = new ABlast.Cell<ABlast.Rock>((Int32)ABlast.Type.Rock, Rock);
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
        Red = 1,
        Blue = 2,
        Yellow = 3,
        Black = 4,
        Green = 5
    }

    enum Direction
    {
        VERTICAL = 0,
        HORIZONTAL = 1
    }

    public class Cell <X>
    {
        public Cell(Int32 Type, X Object)
        {
            this.Type = Type;
            this.Content = Object; 
        }
        private Int32 Type;
        private X Content;

        public Int32 GetType
        {
            get
            {
                return Type;
            }
        }

        public X GetContent
        {
            get
            {
                return Content;
            }
        }
    }

    public class CellResource
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
    }

    public class Board
    {
        private Int32 Score;
        private Int32 TurnNumber;
        private List<Object> Map;

        public Board(Int32 Score, Int32 TurnNumber, List<Object> Map)
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

        public List<Object> GetMap
        {
            get
            {
                return Map;
            }
        }
    }

}