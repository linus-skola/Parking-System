using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2_Komplettering
{
    class Program
    {
        static ParkingLot parkingL = new ParkingLot();

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "Prague Parking System";

            parkingL.IniArray("database.csv");

            while (true)
            {
                Console.Clear();
                Header();
                int free = 0;
                free = parkingL.FreeSlots();
                Console.WriteLine(" 1. Add vehicle | 2. Move vehicle | 3. Remove vehicle | 4. Currently available slots ({0}) | 5. Search vehicle\n", free);
                Console.Write(" Choose option: ");
                string option = Console.ReadLine();

                //CHECK INPUT
                while (true)
                {
                    if (option == "1" || option == "2" || option == "3" || option == "4" || option == "5" || option == "6")
                    {
                        break;
                    }
                    else
                    {
                        Console.Write("\n Not a valid number, try again: ");
                        option = Console.ReadLine();
                    }
                }

                switch (option)
                {
                    case "1":
                        Console.Clear();
                        AddVehicle();
                        Console.Clear();
                        break;

                    case "2":
                        Console.Clear();
                        MoveVehicle();
                        Console.Clear();
                        break;

                    case "3":
                        Console.Clear();
                        RemoveVehicle();
                        Console.Clear();
                        break;

                    case "4":
                        Console.Clear();
                        parkingL.ListVehicle();
                        Console.Clear();
                        break;

                    case "5":
                        Console.Clear();
                        SearchVehicle();
                        Console.Clear();
                        break;

                    //JUST FOR DEBUGGING RECEIPT, PARKING TIME = 23:59 (incl. 5 free minutes) 
                    case "6":
                        Console.Clear();
                        DateTime yesterday = DateTime.Now.AddHours(-24).AddMinutes(-4);
                        string argu = "CAR, ABC123, " + yesterday;
                        //parkingL.PrintReceipt(argu, 50);
                        Console.Clear();
                        break;

                    default:
                        Console.WriteLine("\n Choose one of the options!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddVehicle()
        {
            string reg = "";
            int VehOption = 0;
            Header();
            Console.WriteLine(" ADD VEHICLE");
            Console.WriteLine(" 1. Car | 2. MC | Leave blank to return to menu\n");
            Console.Write(" Choose vehicle type: ");

            string input = Console.ReadLine();

            //CHECK INPUT-------------------------------
            while (true)
            {
                if (input == "1" || input == "2" || input == "3")
                {
                    VehOption = Convert.ToInt32(input);
                    break;
                }
                else if (input != "")
                {
                    Console.Write("\n Not a valid number, try again: ");
                    input = Console.ReadLine();
                }
                else
                {
                    return;
                }
            }

            switch (VehOption)
            {
                case 1:
                    while (string.IsNullOrEmpty(reg))
                    {
                        Console.Write("\n Registration number: ");
                        reg = Console.ReadLine().ToUpper();
                        reg.Replace(" ", "");

                        if (string.IsNullOrEmpty(reg))
                        {
                            return;
                        }

                        //IF REG ALREADY EXISTS
                        else if (parkingL.DuplicateVehicle(reg) == true)
                        {
                            Console.WriteLine("\n ERROR!\n A vehicle with this registration number already exists.\n1");
                            reg = null;
                        }
                    }
                    Vehicle car = new Vehicle(Type.Car, reg, DateTime.Now);
                    parkingL.AddVehicle(car);
                    break;
                    

                case 2:
                    while (string.IsNullOrEmpty(reg))
                    {
                        Console.Write("\n Registration number: ");
                        reg = Console.ReadLine().ToUpper();
                        reg.Replace(" ", "");

                        if (string.IsNullOrEmpty(reg))
                        {
                            return;
                        }

                        //IF REG ALREADY EXISTS
                        else if (parkingL.DuplicateVehicle(reg) == true)
                        {
                            Console.WriteLine("\n ERROR!\n A vehicle with this registration number already exists.\n");
                            reg = null;
                        }
                    }
                    Vehicle mc = new Vehicle(Type.MC, reg, DateTime.Now);
                    parkingL.AddVehicle(mc);
                    break;
            }
        }

        static void MoveVehicle()
        {
            string reg;
            string input = "";
            int slot = 0;
            Header();
            Console.WriteLine(" MOVE VEHICLE");
            Console.WriteLine(" Leave blank to return to menu\n");
            while (true)
            {
                Console.Write(" Registration number: ");
                reg = Console.ReadLine().ToUpper();

                if (string.IsNullOrEmpty(reg))
                {
                    return;
                }
                else if (parkingL.VehicleExsist(reg) == false)
                {
                    Console.WriteLine("\n ERROR!\n Registration number not found!\n");
                    reg = null;
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.Write("\n To parking slot: ");
                input = Console.ReadLine();
                Int32.TryParse(input, out slot);

                if (string.IsNullOrEmpty(input))
                {
                    return;
                }
                else if (parkingL.ValidSlot(slot) == false)
                {
                    Console.WriteLine("\n ERROR!\n Positing not found! Please try again.\n");
                }
                else
                {
                    break;
                }
            }
            parkingL.MoveVehicle(reg, slot);
        }

        static void RemoveVehicle()
        {
            Header();
            string reg;
            Console.WriteLine(" REMOVE VEHICLE");
            Console.WriteLine(" Leave blank to return to menu\n");

            while (true)
            {
                Console.Write(" Registration number: ");
                reg = Console.ReadLine().ToUpper();

                if (string.IsNullOrEmpty(reg))
                {
                    return;
                }
                else if (parkingL.VehicleExsist(reg) == false)
                {
                    Console.WriteLine("\n ERROR!\n Registration number not found!\n");
                    reg = null;
                }
                else
                {
                    break;
                }
            }

            string vehinfo = parkingL.VehicleInfo(reg);
            int slot = parkingL.RemoveVehicle(reg);
            Console.Write("\n Vehicle has been successfully removed from database.\n Press any key to print receipt...");
            Console.ReadKey();

            Console.Clear();
            parkingL.PrintReceipt(vehinfo, slot);
        }

        static void SearchVehicle()
        {
            Header();
            Console.WriteLine(" SEARCH VEHICLE");
            Console.WriteLine(" Leave empty to return to menu\n");
            string reg;

            while (true)
            {
                Console.Write(" Registration number: ");
                reg = Console.ReadLine().ToUpper();

                if (string.IsNullOrEmpty(reg))
                {
                    return;
                }
                else if (parkingL.VehicleExsist(reg) == false)
                {
                    Console.WriteLine("\n ERROR!\n Registration number not found!\n");
                    reg = null;
                }
                else
                {
                    break;
                }
            }
            Console.Clear();
            parkingL.SearchVehicle(reg);            
        }
        
        static void Header()
        {
            Console.WriteLine(" --------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" =========================================== Prague Parking System ============================================");
            Console.WriteLine(" --------------------------------------------------------------------------------------------------------------");
        }
    }
}
