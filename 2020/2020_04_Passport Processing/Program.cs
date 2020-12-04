using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2020_04_Passport_Processing
{
    class Program
    {
        // Part 1: 209 too low.
        // Bug: wasn't adding the last passport - included setting another passport after looping through the raw data.

        // Part 2: 132 too high.

        static List<string> passportDataRaw = new List<string>();
        static List<Passport> passports = new List<Passport>();

        static void Main(string[] args)
        {
            int validPassports = 0;

            ImportPassportData();
            ParsePassports();

            validPassports = passports.Where(w => w.IsValid()).Count();

            Console.WriteLine(Passport.ToCSVHeader());
            foreach (Passport p in passports.Where(w => w.IsValid()))
                Console.WriteLine(p.ToCSV());

            Console.WriteLine($"Valid passports: {validPassports}/{passports.Count}");

            Console.Read();
            Environment.Exit(0);
        }

        static void ImportPassportData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day4_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                StringBuilder sb = new StringBuilder();
                while ((s = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        sb.Append(s + " ");
                    }

                    else
                    {
                        passportDataRaw.Add(sb.ToString());
                        sb.Clear();
                    }
                }

                passportDataRaw.Add(sb.ToString());
                sb.Clear();
            }
        }

        static void ParsePassports()
        {
            foreach (string s in passportDataRaw)
            {
                Passport p = new Passport(s, true);
                passports.Add(p);
            }
        }

        public class Passport
        {
            /*
            byr (Birth Year)
            iyr (Issue Year)
            eyr (Expiration Year)
            hgt (Height)
            hcl (Hair Color)
            ecl (Eye Color)
            pid (Passport ID)
            cid (Country ID)
            */

            #region Fields
            string[] passportData = new string[8];
            public bool Validate = false;
            #endregion

            #region Properties
            /*
            byr (Birth Year) - four digits; at least 1920 and at most 2002.
            iyr (Issue Year) - four digits; at least 2010 and at most 2020.
            eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
            hgt (Height) - a number followed by either cm or in:
            If cm, the number must be at least 150 and at most 193.
            If in, the number must be at least 59 and at most 76.
            hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            pid (Passport ID) - a nine-digit number, including leading zeroes.
            cid (Country ID) - ignored, missing or not.
            */
            public string BirthYear
            {
                get
                {
                    return this.passportData[0];
                }

                set
                {
                    if (Validate)
                    {
                        if (Convert.ToInt32(value) >= 1920 && Convert.ToInt32(value) <= 2002)
                            this.passportData[0] = value;
                    }

                    else
                        this.passportData[0] = value;
                }
            }

            public string IssueYear
            {
                get
                {
                    return this.passportData[1];
                }

                set
                {
                    if (Validate)
                    {
                        if (Convert.ToInt32(value) >= 2010 && Convert.ToInt32(value) <= 2020)
                            this.passportData[1] = value;
                    }

                    else
                        this.passportData[1] = value;
                }
            }

            public string ExpirationYear
            {
                get
                {
                    return this.passportData[2];
                }

                set
                {
                    if (Validate)
                    {
                        if (Convert.ToInt32(value) >= 2020 && Convert.ToInt32(value) <= 2030)
                            this.passportData[2] = value;
                    }

                    else
                        this.passportData[2] = value;
                }
            }

            public string Height
            {
                get
                {
                    return this.passportData[3];
                }

                set
                {
                    if (Validate)
                    {
                        string unit = value.Substring(value.Length - 2, 2);
                        int val = 0;
                        Int32.TryParse(value.Substring(0, value.Length - 2), out val);

                        switch (unit)
                        {
                            case "cm":
                                if (val >= 150 && val <= 193)
                                    this.passportData[3] = value;
                                break;
                            case "in":
                                if (val >= 59 && val <= 76)
                                    this.passportData[3] = value;
                                break;
                        }
                    }

                    else
                        this.passportData[3] = value;
                }
            }

            public string HairColor
            {
                get
                {
                    return this.passportData[4];
                }

                set
                {
                    if (Validate)
                    {
                        if (value.Substring(0, 1) != "#" || value.Length != 7)
                            return;

                        string pattern = @"#[0-9a-fA-F]{6}";
                        MatchCollection mc = Regex.Matches(value, pattern);

                        if (mc.Count == 1)
                            this.passportData[4] = value;
                    }

                    else
                        this.passportData[4] = value;
                }
            }

            public string EyeColor
            {
                get
                {
                    return this.passportData[5];
                }

                set
                {
                    if (Validate)
                    {
                        switch (value)
                        {
                            case "amb":
                            case "blu":
                            case "brn":
                            case "gry":
                            case "grn":
                            case "hzl":
                            case "oth":
                                this.passportData[5] = value;
                                break;
                        }
                    }

                    else
                        this.passportData[5] = value;
                }
            }

            public string PassportID
            {
                get
                {
                    return this.passportData[6];
                }

                set
                {
                    if (Validate)
                    {
                        string pattern = @"[0-9]{9}";
                        MatchCollection mc = Regex.Matches(value, pattern);

                        if (mc.Count == 1 && value.Length == 9)
                            this.passportData[6] = value;
                    }

                    else
                        this.passportData[6] = value;
                }
            }

            public string CountryID
            {
                get
                {
                    return this.passportData[7];
                }

                set
                {
                    this.passportData[7] = value;
                }
            }
            #endregion

            #region Constructors
            public Passport(bool validate = false)
            {
                Validate = validate;
            }

            public Passport(string s, bool validate = false)
            {
                Validate = validate;
                ParsePassportData(s);
            }
            #endregion

            #region Methods
            void ParsePassportData(string s)
            {
                string[] dataArray = s.Split(' ');

                foreach (string dataItem in dataArray)
                {
                    string[] dataObject = dataItem.Split(':');

                    switch (dataObject[0])
                    {
                        case "byr":
                            this.BirthYear = dataObject[1];
                            break;
                        case "iyr":
                            this.IssueYear = dataObject[1];
                            break;
                        case "eyr":
                            this.ExpirationYear = dataObject[1];
                            break;
                        case "hgt":
                            this.Height = dataObject[1];
                            break;
                        case "hcl":
                            this.HairColor = dataObject[1];
                            break;
                        case "ecl":
                            this.EyeColor = dataObject[1];
                            break;
                        case "pid":
                            this.PassportID = dataObject[1];
                            break;
                        case "cid":
                            this.CountryID = dataObject[1];
                            break;
                    }
                }
            }

            public bool IsValid()
            {
                if (!String.IsNullOrEmpty(this.BirthYear) &&
                    !String.IsNullOrEmpty(this.IssueYear) &&
                    !String.IsNullOrEmpty(this.ExpirationYear) &&
                    !String.IsNullOrEmpty(this.Height) &&
                    !String.IsNullOrEmpty(this.HairColor) &&
                    !String.IsNullOrEmpty(this.EyeColor) &&
                    !String.IsNullOrEmpty(this.PassportID))

                    return true;

                return false;
            }

            public string ToCSV()
            {
                return $"{this.IsValid()}, {this.BirthYear}, {this.IssueYear}, {this.ExpirationYear}, {this.Height}, {this.HairColor}, {this.EyeColor}, {this.PassportID}, {this.CountryID}";
            }

            public static string ToCSVHeader()
            {
                return "IsValid?, Birth Year, Issue Year, Expiration Year, Height, Hair Color, Eye Color, Passport ID, Country ID";
            }
            #endregion

            #region Overrides
            public override string ToString()
            {
                return $"Valid: {this.IsValid()} b: {this.BirthYear} i: {this.IssueYear} e: {this.ExpirationYear} h: {this.Height} hc: {this.HairColor} ec: {this.EyeColor} pid: {this.PassportID} cid: {this.CountryID}";
            }
            #endregion
        }
    }
}