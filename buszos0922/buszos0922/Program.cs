using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace buszos0922
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string[]> validTicketUsers = new List<string[]>();
            List<string[]> data = ReadTwo();
            
            Console.WriteLine("Number of travellers: "+NumberOfTravellers(data));
            Console.WriteLine("Numver of cancelled travels: "+TimeCalculator(data,validTicketUsers));
            Console.WriteLine("Most travellers got on at this bus stop: "+MostTravellersGotOn(data));
            int[] discount = DiscountUsers(validTicketUsers);
            Console.WriteLine($"Discount users: discount travellers: {discount[0]}, free passengers: {discount[1]} ");
            Console.WriteLine("Number of days between 2020.09.24-2020.12.25(Until Xmas): "+NumberOfDays(2020, 09, 24, 2020, 12, 25));
            SaveToFile(validTicketUsers);
        }
        static List<string[]> ReadTwo() // 1.
        {
            List<string[]> list = new List<string[]>();
            StreamReader sr = new StreamReader("utasadat.txt");
            string[] stuff;
            
            while (!sr.EndOfStream)
            {
                stuff = sr.ReadLine().Split(' ');
                list.Add(stuff);
            }

            return list;
        }
        static int NumberOfTravellers(List<string[]> l) // 2.
        {
            List<int> uniqueIdList = new List<int>();
            int num /*,stop*/;
            for (int i = 0; i < l.Count; i++)
            {
                num = Convert.ToInt32(l[i][2]);
               // stop = Convert.ToInt32(l[i][0]);                      ?? 29 vagy 30 az utolsó?
                if (!uniqueIdList.Contains(num) /* && stop!=29*/)
                {
                    uniqueIdList.Add(num);
                }
            }
            
            return uniqueIdList.Count;
        }
        static int TimeCalculator(List<string[]> l,List<string[]>x) //3.
        {
            int nonValidTickets=0;

            string[] date;
            string month, day /*,hour, min*/;
            int year=0, yearToMinutes, monthAndDayToMinutes/*,hourAndMinsToMins*/,todayInMinutes;

            date = l[0][1].Split('-');
            year = Convert.ToInt32(Convert.ToString(date[0][0])+ Convert.ToString(date[0][1]+ Convert.ToString(date[0][2])+ Convert.ToString(date[0][3])));
            yearToMinutes = year*365*24*60;
            month = Convert.ToString(date[0][4]) + Convert.ToString(date[0][5]);
            day = Convert.ToString(date[0][6]) + Convert.ToString(date[0][7]);
            monthAndDayToMinutes = MonthAndDayToMin(month,day);
            //hour = Convert.ToString(date[1][0]) + Convert.ToString(date[1][1]);
            //min = Convert.ToString(date[1][2]) + Convert.ToString(date[1][3]);
            //hourAndMinsToMins = HoursAndMinsToMin(hour, min);

            todayInMinutes = yearToMinutes + monthAndDayToMinutes /*+hourAndMinsToMins*/;

            string validYear, validMonth, validDay;


            int validUntil;

            for (int i = 0; i < l.Count; i++)
            {
                if (l[i][4].Length>6)
                {

                    validYear = Convert.ToString(l[i][4][0]) + Convert.ToString(l[i][4][1]) + Convert.ToString(l[i][4][2]) + Convert.ToString(l[i][4][3]);  //év string
                    validMonth = Convert.ToString(l[i][4][4]) + Convert.ToString(l[i][4][5]);                                                               //hónap string
                    validDay = Convert.ToString(l[i][4][6]) + Convert.ToString(l[i][4][7]);                                                                 //nap string

                    validUntil = Convert.ToInt32(validYear) * 365 * 24 * 60 + MonthAndDayToMin(validMonth,validDay);

                    if (validUntil<todayInMinutes)
                    {
                        nonValidTickets++;
                    }
                    
                }
                if (l[i][4]=="0")
                {
                    nonValidTickets++;
                }
                else
                {
                    x.Add(l[i]);
                }
            }



            return nonValidTickets;
        } 
        static int MonthAndDayToMin(string month,string day)
        {
            int days = 0;
            if (month=="01")
            {
                days=31;
            }
            else if (month == "02")
            {
                days = 28;
            }
            else if (month == "03")
            {
                days = 31;
            }
            else if (month == "04")
            {
                days = 30 ;
            }
            else if (month == "05")
            {
                days = 31;
            }
            else if (month == "06")
            {
                days = 30;
            }
            else if (month == "07")
            {
                days = 31;
            }
            else if (month == "08")
            {
                days = 31;
            }
            else if (month == "09")
            {
                days = 30;
            }
            else if (month == "10")
            {
                days = 31;
            }
            else if (month == "11")
            {
                days = 30;
            }
            else if (month == "12")
            {
                days = 31 ;
            }
            return (days*24*60)+(Convert.ToInt32(day)*24*60);
        }
        static int MostTravellersGotOn(List<string[]> l) // 4.
        {
            List<int> busStops = new List<int>();
            for (int i = 0; i < l.Count; i++)
            {
                busStops.Add(Convert.ToInt32(l[i][0]));
            }

            var most = busStops.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

            return most;
            //int num = 0;                                                  //biztos ami biztos megszámoltattam vele úgy, hogy lássam, mert nem hittem neki
            //int busStop = 0;
            //for (int i = 0; i < l.Count; i++)
            //{
            //    if (Convert.ToInt32(l[i][0])==busStop)
            //    {
            //        num ++;
            //    }
            //    else
            //    {
            //        Console.WriteLine(busStop+": "+num);
            //        busStop++;
            //        num = 0;
            //    }
            //}





            //return 0;
        }
        static int HoursAndMinsToMin(string hour,string minute)
        {
            int minuteAndHours = 0;
            if (hour=="00")
            {
                minuteAndHours = (0 * 60) + Convert.ToInt32(minute);
            }
            else if (hour =="01")
            {
                minuteAndHours = (1 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "02")
            {
                minuteAndHours = (2 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "03")
            {
                minuteAndHours = (3 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "04")
            {
                minuteAndHours = (4 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "05")
            {
                minuteAndHours = (5 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "06")
            {
                minuteAndHours = (6 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "07")
            {
                minuteAndHours = (7 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "08")
            {
                minuteAndHours = (8 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "09")
            {
                minuteAndHours = (9 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "10")
            {
                minuteAndHours = (10 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "11")
            {
                minuteAndHours = (11 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "12")
            {
                minuteAndHours = (12 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "13")
            {
                minuteAndHours = (13 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "14")
            {
                minuteAndHours = (14 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "15")
            {
                minuteAndHours = (15 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "16")
            {
                minuteAndHours = (16 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "17")
            {
                minuteAndHours = (17 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "18")
            {
                minuteAndHours = (18 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "19")
            {
                minuteAndHours = (19 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "20")
            {
                minuteAndHours = (20 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "21")
            {
                minuteAndHours = (21 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "22")
            {
                minuteAndHours = (22 * 60) + Convert.ToInt32(minute);
            }
            else if (hour == "23")
            {
                minuteAndHours = (23 * 60) + Convert.ToInt32(minute);
            }
            return minuteAndHours;
        }
        static int[] DiscountUsers(List<string[]> l) // 5.
        {
            int[] returnArray=new int[2];
            int first=0, second=0;
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i][3]=="TAB" || l[i][3] == "NYB")
                {
                    first++;
                }
                else if (l[i][3] == "NYP" || l[i][3] == "RVS" || l[i][3] == "GYK")
                {
                    second++;
                }
            }
            returnArray[0] = first;
            returnArray[1] = second;

            return returnArray;
        }        
        static int NumberOfDays(int e1,int h1, int n1, int e2, int h2, int n2) // 6.
        {
            int d1, d2, numberOfDays;
            h1 = (h1 + 9)%12;
            e1 = e1 - (h1 / 10);
            d1 = (365 * e1) + (e1 / 4) - (e1 / 100) + (e1 / 400) + (h1*306 + 5) / 10 + n1 - 1;

            h2 = (h2 + 9) % 12;
            e2 = e2 - (h2 / 10);
            d2 = (365 * e2) + (e2 / 4) - (e2 / 100) + (e2 / 400) + (h2 * 306 + 5) / 10 + n2 - 1;

            return numberOfDays = d2 - d1;
        }

        static void SaveToFile(List<string[]>l) // 7.
        {
            List<string[]> writeList = new List<string[]>();
            string first, second, third;
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i][3]!="JGY")
                {
                    first = "";
                    second = "";
                    third = "";
                    for (int j = 0; j < 4; j++)
                    {
                        first += l[i][4][j];
                    }
                    for (int j = 0; j < 2; j++)
                    {
                        second += l[i][4][j + 4];
                    }
                    for (int j = 0; j < 2; j++)
                    {
                        third += l[i][4][j + 6];
                    }
                    if (NumberOfDays(Convert.ToInt32(first), Convert.ToInt32(second), Convert.ToInt32(third),2019, 03, 29) == 3 || NumberOfDays(Convert.ToInt32(first), Convert.ToInt32(second), Convert.ToInt32(third), 2019, 03, 29) == 2 ||  NumberOfDays(Convert.ToInt32(first), Convert.ToInt32(second), Convert.ToInt32(third), 2019, 03, 29)==1)
                    {
                        writeList.Add(l[i]);
                    }

                }
            }
            string path = "figyelmeztetes.txt";
            string insert = "";
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                for (int i = 0; i < writeList.Count; i++)
                {
                    insert = writeList[i][4].Insert(4, "-");
                    insert = insert.Insert(7, "-");

                    outputFile.WriteLine(writeList[i][2]+" "+insert);
                }

            }
            
        }
    }
}
