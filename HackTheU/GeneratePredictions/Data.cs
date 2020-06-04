using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GeneratePredictions
{


    class User
    {
        string firstname;
        string lastname;
        public Dictionary<string, List<Activity>> Days = new Dictionary<string, List<Activity>>();
        public User(string first, string last)
        {
            firstname = first;
            lastname = last;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("User", firstname + lastname);
            foreach (var day in Days)
            {
                writer.WriteStartElement(day.Key);
                foreach (var activity in day.Value)
                {
                    writer.WriteStartElement("ActivityDATA");
                    writer.WriteElementString("Activity", activity.activity);
                    writer.WriteElementString("HappyLevel", activity.happylevel);
                    writer.WriteElementString("Time", activity.time);
                    writer.WriteElementString("Location", activity.location);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }

    }
    class Activity
    {
        public string time;
        public string activity;
        public string happylevel;
        public string location;
        public Activity(string time, string activity, string happylevel, string location)
        {
            this.time = time;
            this.activity = activity;
            this.happylevel = happylevel;
            this.location = location;
        }
    }

    class Data
    {
        public HashSet<User> GetData()
        {
            HashSet<User> Users = new HashSet<User>();
            using (StreamReader results = new StreamReader(@"C:\Users\jodir\Desktop\CS3500\HackTheU\Names.txt"))
            {
                string line = results.ReadLine();
                while (line != null)
                {
                    string[] names = line.Split("\t".ToCharArray());
                    Users.Add(CreateUser(names));
                    line = results.ReadLine();
                }

                results.Close();
            }
            return Users;
        }
        public void Save(string filename)
        {
            HashSet<User> users = GetData();
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true, IndentChars = ("  ") };
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Users");
                    foreach (User user in users)
                        user.WriteXml(writer);
                    writer.WriteEndElement();

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public User CreateUser(string[] name)
        {
            User temp = new User(name[0], name[1]);
            int day = 0;
            for (int i = 0; i < 50000; i++)
            {
                temp.Days.Add("day" + day.ToString(), CreateDay());
                day += 1;
            }
            return temp;
        }
        public List<Activity> CreateDay()
        {
            Random randy = new Random(0);
            List<Activity> activities = new List<Activity>();
            for (int i = 0; i < 100; i++)
            {
                string time = randy.Next(0, 24).ToString() + ":" + randy.Next(0, 60).ToString() + ":" +  randy.Next(0, 60).ToString();
                string activity = randomName(randy);
                string happylevel = (randy.NextDouble() * randy.Next(0, 10)).ToString();
                string location = randomName(randy) + " Location";
                activities.Add(new Activity(time, activity, happylevel, location));
            }
            return activities;
        }
        private String randomName(Random rand)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(rand.Next(26), 1) + (rand.Next(99) + 1);
        }
    }
}
