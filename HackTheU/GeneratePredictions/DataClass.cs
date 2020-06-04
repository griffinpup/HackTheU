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
            writer.WriteElementString("User", firstname + " " + lastname);
            foreach (var day in Days)
            {
                writer.WriteStartElement(day.Key);
                foreach (var activity in day.Value)
                {
                    writer.WriteStartElement("ActivityDATA");
                    writer.WriteElementString("Activity", activity.activity);
                    writer.WriteElementString("HappyLevel", activity.happiness);
                    writer.WriteElementString("Time", activity.time);
                    writer.WriteElementString("Location", activity.location);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }

    }
    public class Activity
    {
        public string time;
        public string activity;
        public string happiness;
        public string location;
        public Activity(string time, string activity, string happylevel, string location)
        {
            this.time = time;
            this.activity = activity;
            this.happiness = happylevel;
            this.location = location;
        }
    }

    class Data
    {
        const int days = 10;
        const int activitynum = 5;
        static Random randy = new Random(new Random().Next());
        public HashSet<User> GetData(string filename)
        {
            HashSet<User> Users = new HashSet<User>();
            using (StreamReader results = new StreamReader(filename))
            {
                string line = results.ReadLine();
                while (line != null)
                {
                    string[] names = line.Split("\t".ToCharArray());
                    Users.Add(CreateRandomUser(names));
                    line = results.ReadLine();
                }

                results.Close();
            }
            return Users;
        }
        public void SaveTestData(string filename, HashSet<User> users, bool random)
        {
            if (random)
                users = GetData(@"C:\Users\jodir\Desktop\CS3500\HackTheU\Names.txt");
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
                    writer.WriteEndDocument();

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public HashSet<User> ReadData(string filename)
        {
            HashSet<User> users = new HashSet<User>();

            //XmlDocument doc = new XmlDocument();
            //doc.Load(filename);
            //foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            //{
            //    string blah = node.Name;
            //}
            //return users;

            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {

                    bool done = false;
                    while (!done && reader.Read())
                    {
                        if (reader.IsStartElement())
                        {

                            switch (reader.Name)
                            {
                                case "User":
                                    string[] name = reader.ReadInnerXml().Split(" ".ToCharArray());
                                    User temp = new User(name[0], name[1]);
                                    while (reader.Read())
                                    {
                                        if (reader.IsStartElement())
                                        {

                                            switch (reader.Name.Substring(0, 3))
                                            {
                                                case "day":
                                                    string day = reader.Name;
                                                    List<Activity> activities = new List<Activity>();
                                                    reader.ReadToDescendant("ActivityDATA");
                                                    while (reader.Name != day)
                                                    {

                                                        reader.ReadToDescendant("Activity");
                                                        string activity = reader.ReadInnerXml();
                                                        reader.ReadToNextSibling("HappyLevel");
                                                        string happpylevel = reader.ReadInnerXml();
                                                        reader.ReadToNextSibling("Time");
                                                        string time = reader.ReadInnerXml();
                                                        reader.ReadToNextSibling("Location");
                                                        string location = reader.ReadInnerXml();
                                                        activities.Add(new Activity(time, activity, happpylevel, location));
                                                        reader.MoveToContent();
                                                        reader.ReadToNextSibling("ActivityDATA");
                                                    }
                                                    temp.Days.Add(day, activities);
                                                    break;
                                                case "Use":
                                                    done = true;
                                                    break;
                                            }
                                        }
                                    }
                                    users.Add(temp);
                                    break;

                            }
                        }
                    }

                }
                return users;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public HashSet<User> TopTwenty(HashSet<User> users)
        {
            List<KeyValuePair<string, double>> days = new List<KeyValuePair<string, double>>();
            foreach (var user in users)
            {
                foreach (var day in user.Days)
                {
                    double total = 0;
                    int actnum = 0;
                    foreach (var activity in day.Value)
                    {
                        total += double.Parse(activity.happiness);
                        actnum += 1;
                    }
                    total = total / actnum;
                    days.Add(new KeyValuePair<string, double>(day.Key, total));
                }
                days.Sort((KeyValuePair<string, double> x, KeyValuePair<string, double> y) => x.Value.CompareTo(y.Value));
                for (int i = 0; i < (int)(days.Count * .8); i++)
                {
                    user.Days.Remove(days[i].Key);
                }

            }
            return users;

        }

        public User CreateRandomUser(string[] name)
        {
            User temp = new User(name[0], name[1]);
            int day = 0;
            for (int i = 0; i < days; i++)
            {
                temp.Days.Add("day" + day.ToString(), CreateDay());
                day += 1;
            }
            return temp;
        }
        public List<Activity> CreateDay()
        {
            
            List<Activity> activities = new List<Activity>();
            for (int i = 0; i < activitynum; i++)
            {
                string time = randy.Next(0, 24).ToString() + ":" + randy.Next(0, 60).ToString() + ":" + randy.Next(0, 60).ToString();
                string activity = /*randomName(randy)*/ (i + randy.Next(0, 10)).ToString();
                string happylevel = /*(randy.NextDouble() * randy.Next(0, 100)).ToString()*/ (activity+1).ToString()/*(i + 2).ToString()*/;
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
