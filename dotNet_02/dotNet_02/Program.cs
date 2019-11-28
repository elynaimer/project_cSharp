using System;
using System.Collections;
using System.Collections.Generic;

namespace project2._2
{
    class Program
    {
        enum menu { request, present, show_number, end };    //enum declaration
        static void Main(string[] args)                   //main
        {
            Schedule schedule = new Schedule();         //matrix declaration


            int length; //date variables
            Date start = new Date();
            int option; //input variable
            bool stop = false;
            while (stop == false)
            {
                PrintWelcome();         //print options for input
                option = int.Parse(Console.ReadLine());
                //  option = Input();      //input
                switch (option)        //switch case
                {
                    case (int)menu.request:                                //case 0 - request
                        length = schedule.request(start);
                        break;      // end case 0 - request

                    case (int)menu.present:             //case 1 - present
                        schedule.present();
                        break;//end case 1 - present

                    case (int)menu.show_number:  //case 2- show_number
                        int count = 0;
                        for (int i = 0; i < 12; i++)
                        {
                            int j = 0;
                            for (; j < 31; j++)
                            {
                                if (schedule.year[i, j] == true)
                                    count++;
                            }

                        }
                        Console.WriteLine("number of days occupied: " + count + " number of days statistically: " + 100 * ((float)count / 372) + " percent");
                        break;        //end case 2 - show_number

                    case (int)menu.end:  // case 3 - end
                        stop = true;
                        Console.WriteLine("Thank you for using our system, goodbye!");
                        Console.Read();
                        break;            // end case 3 - end
                    default:
                        Console.WriteLine("thank you for booking by us, have a nice vacation!");
                        stop = true;
                        Console.Read();
                        break;

                }
            }
            void PrintWelcome() //print options for input function
            {
                Console.WriteLine("please insert number betweeen o-3 depending on the following options");
                Console.WriteLine("0-request for vacation");
                Console.WriteLine("1-printing on screen all occupied dates");
                Console.WriteLine("2-prints number of occupied dates and statistics");
                Console.WriteLine("3- exit the program");
            }      //
        }



    }






    class Schedule
    {
        public bool[,] year;
        public Schedule()
        {
            this.year = new bool[12, 31];
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    year[i, j] = false;
                }
            }
        }
        public void setdate(int i, int j)
        {
            this.year[i, j] = true;
        }
        public bool[,] Year { get => year; set => year = value; }

        public Date FindEndDate(Date date, int length)// enday and endate.month will eb edited in the main
        {
            Date endate = new Date();
            endate.month = date.month;
            endate.day = 0;
            if (date.day + length > 31)
            {
                endate.month++;
                length -= 31 - date.day;
            }
            while (length > 0)
            {
                if (length > 31)
                {
                    endate.month++;
                    length -= 31;
                }
                else
                {
                    if (date.month == endate.month)
                        endate.day = length + date.day;
                    else
                        endate.day = length;
                    length = 0;
                }

            }
            if (endate.month > 12)
            {
                endate.month -= 12;
            }
            return endate;
        }



        public int request(Date start)
        {
            int length = 0;
            Console.WriteLine("enter date for the start of your vacation, pleasse enter the month then the day: ");
            try { start.month = int.Parse(Console.ReadLine()); }
            catch { Console.WriteLine("please enter only numbers with enter between them"); }
            start.day = int.Parse(Console.ReadLine());
            if (start.month > 12 || start.day > 31)      // checking if the date that was entered exists
            {
                Console.WriteLine("ERROR!! invalid date");
                length = -1;
                start.month = 0;
                start.day = 0;
            }
            else
            {
                Console.WriteLine("enter number of days for vacation: ");// the length of the vacation will be entered by user and the dtae for end calculated
                length = int.Parse(Console.ReadLine());
                Date endate = FindEndDate(start, length);// function to calculate the end date for the vacation
                bool flag = false;
                int end2 = 31;
                int p;
                for (int i = start.month - 1; i <= endate.month - 1; i++)// loop to check if the daates are free 
                {// setting verything to minus to access the correct place in the matrix, will ne changed back for printing
                    if (i == endate.month - 1)
                        end2 = endate.day - 1;
                    for (p = 0; p < end2; p++)
                    {
                        if (i == start.month - 1 && p == 0)
                            p = start.day - 1;
                        if (year[i, p])
                            flag = true;
                    }

                }
                if (flag == true)
                {
                    Console.WriteLine("request denied");
                    length = -1;
                }
                else
                {
                    Console.WriteLine("request accepted ");
                    end2 = 31;
                    for (int i = start.month - 1; i <= endate.month - 1; i++)// loop to make these dates unavaileble to the rest of the people
                    {
                        if (i == endate.month - 1)
                            end2 = endate.day;
                        for (p = 0; p < end2; p++)
                        {
                            if (i == start.month - 1 && p == 0)
                                p = start.day - 1;
                            year[i, p] = true;
                        }

                    }

                }
            }

            return length;
        }



        public void present()
        {

            bool vacant = false;
            ArrayList dates = new ArrayList();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (year[i, j] == vacant) ;
                    else if (!vacant)
                    {
                        dates.Add(i);
                        dates.Add(j);
                        vacant = !vacant;
                    }
                    else if (vacant && j != 0)
                    {
                        dates.Add(i);
                        dates.Add(j - 1);
                        vacant = !vacant;
                    }
                    else
                    {
                        dates.Add(i - 1);
                        dates.Add(30);
                        vacant = !vacant;
                    }
                }
            }


            for (int i = 0; i < dates.Count; i += 4)
            {
                Console.WriteLine("start date: " + ((int)dates[i + 1] + 1) + "/" + ((int)dates[i] + 1) + " end date: " + ((int)dates[i + 3] + 1) + "/" + ((int)dates[i + 2] + 1));
            }

        }
    }





    class Date
    {
        public int day;
        public int month;
        public Date(int a = 0, int b = 0)
        {
            this.day = a;
            this.month = b;
        }
        public int Day { get => day; set => day = value; }
        public int Month { get => month; set => month = value; }
    }
    class GuestRequest
    {
        public Date EntryDate, Release;
        public bool IsApproved;
        public GuestRequest()
        { this.IsApproved = false; }
        public bool isapproved { get => IsApproved; set => IsApproved = value; }
        public override string ToString()
        {
            return "day : " + EntryDate.day + "month : " + EntryDate.month;
        }
    }
    class HostingUnit : IComparable
    {
        private static long stSerialKey;
        private int hostingUnitkey;
        public Schedule Diary;
        HostingUnit()
        {
            stSerialKey = this.HostingUnitkey;
            this.hostingUnitkey = 10000000;
        }

        public static long StSerialKey { get => stSerialKey; set => stSerialKey = value; }
        public int HostingUnitkey { get => hostingUnitkey; set => hostingUnitkey = value; }
        public override string ToString()
        {
            string flag = null;
            Int32.TryParse(flag, out hostingUnitkey);

            bool vacant = false;
            ArrayList dates = new ArrayList();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (Diary.year[i, j] == vacant) ;
                    else if (!vacant)
                    {
                        dates.Add(i);
                        dates.Add(j);
                        vacant = !vacant;
                    }
                    else if (vacant && j != 0)
                    {
                        dates.Add(i);
                        dates.Add(j - 1);
                        vacant = !vacant;
                    }
                    else
                    {
                        dates.Add(i - 1);
                        dates.Add(30);
                        vacant = !vacant;
                    }
                }
            }


            for (int i = 0; i < dates.Count; i += 4)
            {
                flag += ("start date: " + ((int)dates[i + 1] + 1) + "/" + ((int)dates[i] + 1) + " end date: " + ((int)dates[i + 3] + 1) + "/" + ((int)dates[i + 2] + 1));
            }
            return flag;
        }

        public bool ApproveRequest(GuestRequest guestReq)
        {
            if (Diary.request(guestReq.EntryDate) < 0)
            {
                return false;
            }
            return true;
        }



        public int GetAnnualBusyDays()
        {
            bool vacant = false;
            int count = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (Diary.year[i, j] == vacant) ;
                    count++;
                }
            }
            return count;
        }
        public float GetAnnualBusyPercentage()
        {
            bool vacant = false;
            int count = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (Diary.year[i, j] == vacant) ;
                    count++;

                }
            }
            return count / 365 * 100;
        }

        public int CompareTo(object obj)
        {
            return this.GetAnnualBusyDays() - (obj as HostingUnit).GetAnnualBusyDays();
        }
    }
    class Host : IEnumerable
    {
        public int hostkey;

        public int Hostkey { get => hostkey; set => hostkey = value; }
        private IList<HostingUnit> list;
        Host(int ID = 10000000, int NumOf = 4)
        {
            this.Hostkey = ID;
            list = new List<HostingUnit>(NumOf);
        }
        public override string ToString()
        {
            String flag = null;
            flag += "Host ID :";
            Int32.TryParse(flag, out hostkey);
            foreach (HostingUnit item in list)
            {
                Int32.TryParse(flag, out item.HostingUnitkey)
                flag += item.HostingUnitkey
            }
            return;
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    }



}
