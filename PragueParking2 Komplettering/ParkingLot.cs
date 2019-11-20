using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PragueParking2_Komplettering
{
    class ParkingLot
    {
        static Vehicle[,] pSlots = new Vehicle[100, 2];

        public void IniArray(string path)
        {
            //CHECK IF FILE EXIST AND CREATE IF NOT
            bool FileCreated = false;
            if (File.Exists(path) == false)
            {
                using (StreamWriter a = File.Exists(path) ? File.AppendText(path) : File.CreateText(path))
                {
                    for (int i = 0; i < 100; i++)
                    {
                        a.WriteLine(pSlots[i, 0] = null);
                        a.WriteLine(pSlots[i, 1] = null);
                    }
                    FileCreated = true;
                }
            }


            //READ ALL LINES IN FILE TO pSlots[,]
            StreamReader reader = new StreamReader(path);
            using (reader)
            {
                if (FileCreated == false) //IF FILE 
                {
                    for (int i = 0; i < 100; i++)
                    {
                        string line = reader.ReadLine();
                        Vehicle veh = ConvertToObject(line);
                        pSlots[i, 0] = veh;

                        line = reader.ReadLine();
                        veh = ConvertToObject(line);
                        pSlots[i, 1] = veh;
                    }
                }
            }
        }

        private void WriteToFile()
        {
            StreamWriter NameWriter = new StreamWriter("database.csv");
            using (NameWriter)
            {
                foreach (object a in pSlots)
                {
                    NameWriter.Write("{0}\n", a);
                }
            }

        }

        public int FreeSlots()
        {
            int free = 0;
            for (int i = 0; i < 100; i++)
            {
                if (pSlots[i, 0] == null)
                {
                    free++;
                }
            }
            return free;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            string option = "";

            while (option != "yes")
            {
                Console.WriteLine("\n The following will be written to database: ");
                Console.WriteLine(" {0}", vehicle);

                Console.Write("\n Continue? (yes/no): ");
                option = Console.ReadLine().ToLower();

                switch (option)
                {
                    case "yes":
                        break;

                    case "no":
                        return;

                    default:
                        Console.WriteLine(" Wrong input!");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }

            for (int i = pSlots.GetLength(0) - 100; i <= pSlots.Length - 1; i++)
            {
                if (pSlots[i, 0] == null)
                {
                    pSlots[i, 0] = vehicle;


                    Console.Write("\n\n Success!\n Registration number: {0}\n Parking slot: {1}\n\n Press any key to return to menu...", vehicle.Reg, i + 1);
                    Console.ReadKey();
                    break;
                }

                else if (pSlots[i, 0].Type.Equals(Type.MC) && pSlots[i, 1] == null && vehicle.Type.Equals(Type.MC))
                {
                    pSlots[i, 1] = vehicle;

                    Console.Write("\n\n Success!\n Registration number: {0}\n Parking slot: {1}\n\n Press any key to return to menu...", vehicle.Reg, i + 1);
                    Console.ReadKey();
                    break;
                }
            }
            WriteToFile();
        }

        public void MoveVehicle(string reg, int pos)
        {
            pos -= 1;
            Vehicle MovingVehicle = null;

            foreach (Vehicle veh in pSlots)
            {
                try
                {
                    if (!veh.Reg.Equals(reg) || veh.Equals(null))
                    {

                    }
                    else if (veh.Reg.Equals(reg))
                    {
                        MovingVehicle = veh;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\n Registration number not found!");
                        Console.ReadKey();
                        break;
                    }
                }
                catch (NullReferenceException)
                {
                    continue;
                }

            }


            for (int i = 0; i < 100; i++)
            {
                if (pSlots[i, 0] == MovingVehicle || pSlots[i, 1] == MovingVehicle)
                {
                    //CAR
                    if (MovingVehicle.Type.Equals(Type.Car))
                    {
                        if (pSlots[pos, 0] == null) //If new pos is empty (only pSlots[i, 0] since its a car)
                        {
                            RemoveVehicle(MovingVehicle.Reg); //Removes vehicle from old position

                            pSlots[pos, 0] = MovingVehicle; //Moving the vehicle to new position

                            Console.Write("\n SUCCESS!\n Press any key to return to menu...");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.Write("\n Already vehicles in this position!");
                            Console.ReadKey();
                            break;
                        }
                    }


                    //MC
                    else if (MovingVehicle.Type.Equals(Type.MC))
                    {
                        if (pSlots[pos, 0] == null) //If new pos is empty in pSlots[i, 0]
                        {
                            RemoveVehicle(MovingVehicle.Reg); //Removes vehicle from old position

                            pSlots[pos, 0] = MovingVehicle; //Moving the vehicle to new position

                            Console.Write("\n SUCCESS!\n Press any key to return to menu...");
                            Console.ReadKey();
                            break;
                        }


                        else if (pSlots[pos, 1] == null) //If new pos is empty in pSlots[i, 1]
                        {
                            RemoveVehicle(MovingVehicle.Reg); //Removes vehicle from old position

                            pSlots[pos, 1] = MovingVehicle; //Moving the vehicle to new position

                            Console.Write("\n SUCCESS!\n Press any key to return to menu...");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.Write("\n Already vehicles in this position!");
                            Console.ReadKey();
                            break;
                        }
                    }
                }

            }
            WriteToFile();
        }

        public int RemoveVehicle(string reg)
        {
            int slot = 0;
            for (int i = 0; i < 100; i++)
            {
                slot = i;
                try
                {
                    if (pSlots[i, 0].Reg.Equals(reg))
                    {
                        pSlots[i, 0] = null;

                        try
                        {
                            if (pSlots[i, 1].Type.Equals(Type.MC)) //Move MC in pSlots[i, 1] to [i, 0]
                            {
                                pSlots[i, 0] = pSlots[i, 1];
                                pSlots[i, 1] = null;
                            }
                        }
                        catch (NullReferenceException)
                        {
                            break;
                        }

                        break;
                    }

                    else if (pSlots[i, 1].Reg.Equals(reg))
                    {
                        pSlots[i, 1] = null;
                        break;
                    }
                }
                catch (NullReferenceException) //Catch if pSlots[i, 0/1] is null and continues
                {
                    continue;
                }
                
            }
            WriteToFile();
            return slot;
        }

        public void ListVehicle()
        {
            for (int i = 0; i < pSlots.GetLength(0); i++)
            {
                string s = pSlots[i, 0] + " | " + pSlots[i, 1];

                Console.WriteLine($" {(i + 1),-6} {"-",-6} {s,1:C}");
            }
            Console.Write("\n Press any key to return to menu...\n");
            Console.ReadKey();
        }

        public void SearchVehicle(string reg)
        {
            Console.WriteLine(" Searching for vehicle...");
            string vehinfo = VehicleInfo(reg);
            int slot = 0;
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    if (pSlots[i, 0].Reg.Equals(reg) || pSlots[i, 1].Reg.Equals(reg))
                    {
                        slot = i;
                        break;
                    }
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }
            Console.Clear();
            PrintReceipt(vehinfo, slot);
        }

        public void PrintReceipt(string vehinfo, int slot)
        {
            string[] strlist = vehinfo.Split(',');
            string type = strlist[0].Trim().ToUpper();
            string reg = strlist[1].Trim();
            DateTime arrival = Convert.ToDateTime(strlist[2]);
            double TotalPrice = 0;

            DateTime departure = DateTime.Now;
            TimeSpan ParkedTime = departure.AddMinutes(-5) - arrival;
            double ParkedHours = ParkedTime.TotalHours;
            string Time = ParkedTime.ToString(@"dd") + " days, " + ParkedTime.ToString(@"hh") + " hours, " + ParkedTime.ToString(@"mm") + " min";

            slot += 1;

            //IF PARKED TIME IS LESS THAN 5 MINUTES
            if (ParkedTime.TotalMinutes < 0)
            {
                Console.WriteLine($"{"\n ---------------",-6} {"RECEIPT"} {"---------------",6}\n");

                Console.WriteLine($"{" Reg. number: ",-15} {"{0}"}", reg);
                Console.WriteLine($"{" Arrival: ",-15} {"{0}"}", arrival);
                Console.WriteLine($"{" Departure: ",-15} {"{0}"}", departure);
                Console.WriteLine($"{" Parking slot: ",-15} {"{0}"}", slot);

                Console.WriteLine("\n ------------\n");

                Console.WriteLine($"{" Parked time: ",-15} {"< 5 minutes"}");
                Console.WriteLine($"{" Total price: ",-15} {"{0:C}"}", TotalPrice);

                Console.WriteLine("\n ---------------------------------------\n");

                Console.ReadKey();
            }

            else if (ParkedTime.TotalMinutes >= 0)
            {
                //SETTING LOWEST PRICE = 2h (If type == "CAR", TotalPrice = 40. Else TotalPrice = 20
                TotalPrice = type == "CAR" ? 40 : 20;

                if (type == "CAR")
                {
                    TotalPrice += TotalPrice + Math.Round((ParkedHours - 4) + 0.5) * 20;
                }

                else
                {
                    TotalPrice += TotalPrice + Math.Round((ParkedHours - 4) + 0.5) * 10;
                }

                Console.WriteLine($"{"\n ----------------",-6} {"RECEIPT"} {"----------------",6}\n");

                Console.WriteLine($"{" Reg. number: ",-15} {"{0}"}", reg);
                Console.WriteLine($"{" Arrival: ",-15} {"{0}"}", arrival);
                Console.WriteLine($"{" Departure: ",-15} {"{0}"}", departure);
                Console.WriteLine($"{" Parking slot: ",-15} {"{0}"}", slot);

                Console.WriteLine("\n ------------\n");

                Console.WriteLine($"{" Parked time: ",-15} {Time}");
                Console.WriteLine($"{" Total price: ",-15} {"{0:C}"}", TotalPrice);

                Console.WriteLine("\n -----------------------------------------\n");

                Console.ReadKey();

            }
        }

        private Vehicle ConvertToObject(string opt)
        {
            string[] props = opt.Split(',');

            if (props.Length > 1)
            {
                string value = props[0];
                string reg = props[1].Trim();
                Type type;
                Enum.TryParse(value, out type);
                DateTime date = Convert.ToDateTime(props[2].Trim());

                return new Vehicle(type, reg, date);
            }
            else
            {
                return null;
            }
        }

        public bool DuplicateVehicle(string reg)
        {
            bool duplicated = false;

            foreach (Vehicle veh in pSlots)
            {
                if (veh == null)
                {
                    duplicated = false;
                }
                else if (veh.Reg.Equals(reg))
                {
                    duplicated = true;
                    break;
                }
                else
                {
                    duplicated = false;
                }
            }
            return duplicated;
        }

        public bool VehicleExsist(string reg)
        {
            bool exsist = false;

            foreach (Vehicle veh in pSlots)
            {
                if (veh == null)
                {
                    exsist = false;
                }
                else if (veh.Reg.Equals(reg))
                {
                    exsist = true;
                    break;
                }
                else
                {
                    exsist = false;
                }
            }
            return exsist;
        }

        public string VehicleInfo(string reg)
        {
            string vehinfo = "";

            foreach (Vehicle veh in pSlots)
            {
                try
                {
                    if (veh.Reg.Equals(reg))
                    {
                        vehinfo = veh.ToString();
                        break;
                    }
                }
                catch (NullReferenceException)
                {
                    continue;
                }
                
            }
            return vehinfo;
        }

        public bool ValidSlot(int pos)
        {
            //IF POSITION OUT OF BOUNDS IS SENT, SEND FORMATEXCEPTION
            pos -= 1;
            bool valid = false;

            if (pos > pSlots.GetLength(0) || pos < 1)
            {
                valid = false;
            }
            else
            {
                valid = true;
            }

            return valid;
        }
    }
}