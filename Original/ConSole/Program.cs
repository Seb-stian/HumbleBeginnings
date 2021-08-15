using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace ConSole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Console";
            Color(ConsoleColor.Green);

            bool FirstTime = true;
            string Name = System.Environment.UserName;
            string DocumentsPath = @"C:\Users\" + Name + @"\Desktop\";
            DirectoryInfo Info = Directory.CreateDirectory(DocumentsPath + "Console");
            string FolderPath = Info.FullName;
            if (!IsDirectoryEmpty(FolderPath))
                FirstTime = false;

            FolderPath = FolderPath + @"\";

            if (FirstTime)
            {
                Console.WriteLine("--- Vítejte v konzoli! ---");
                Console.WriteLine("Všechny projekty se budou ukládat na Plochu");
                Console.WriteLine("Tip: Pro zjištění příkazů napište na řádek kódu: help");
                Console.WriteLine("Stiskněte libovolnou klávesu");
                Console.ReadKey();
            }

            bool Exit = false;
            while (!Exit)
            {
                // 1
                Color(ConsoleColor.Green);
                Console.Title = "Console";
                Console.Clear();
                Console.WriteLine("Vytvořit nový projekt [n]");
                Console.WriteLine("Otevřít projekt [o]");
                Console.WriteLine("Spustit projekt [s]");
                Console.WriteLine("Ukončit konzoli [k]");

                ConsoleKeyInfo Key = new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false);
                bool Next = false;
                while (Next == false)
                {
                    if (Key.KeyChar.ToString().ToUpper() == "N" || Key.KeyChar.ToString().ToUpper() == "O" || Key.KeyChar.ToString().ToUpper() == "S" || Key.KeyChar.ToString().ToUpper() == "K")
                        Next = true;
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Vytvořit nový projekt [n]");
                        Console.WriteLine("Otevřít projekt [o]");
                        Console.WriteLine("Spustit projekt [s]");
                        Console.WriteLine("Ukončit konzoli [k]");
                    }
                    Key = Console.ReadKey();
                }
                Console.Clear();

                string Char = Key.KeyChar.ToString().ToUpper();
                string OpenCreated = "";
                string ProjectName = "";
                if (Char == "N")
                {
                    Console.Write("Zadejte název nového projektu: ");
                    ProjectName = Console.ReadLine() + ".txt";
                    CreateProject(ProjectName, FolderPath);
                    Console.WriteLine("Projekt byl vytvořen!");
                    OpenCreated = FolderPath + ProjectName;
                    Console.ReadKey();
                }
                string Input;
                if (Char == "O")
                {
                    Input = "";
                    string ProjectPath = "";
                    if (OpenCreated != "") // jestli se otevírá právě vytvořený projekt
                    {
                        ProjectPath = OpenCreated;
                    }
                    else //jesli se otevírá již vytvořený projekt
                    {
                        bool Exist = false;
                        while (!Exist)
                        {
                            Console.Clear();
                            Console.Write("Zadejte název projektu k otevření: ");
                            Input = Console.ReadLine();
                            if (File.Exists(FolderPath + Input + ".txt"))
                            {
                                Exist = true;
                                ProjectPath = FolderPath + Input + ".txt";
                            }
                        }
                    }
                    // zjistit jestli je projekt prázdný
                    bool Empty = true;
                    using (StreamReader sr = new StreamReader(ProjectPath))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            if (s.Length != 0)
                                Empty = false;
                        }
                    }

                    Console.Clear();
                    Console.WriteLine(Input + ProjectName);
                    string[] radky = new string[500];

                    int radku;
                    bool Continue = true;
                    int a = 0;
                    if (!Empty) // jestli není projekt prázdný
                    {
                        using (StreamReader sr = new StreamReader(ProjectPath))
                        {
                            radku = 0;
                            while (sr.ReadLine() != null) // zjistí počet řádků
                            {
                                radku++;
                            }
                        }
                        if (radku > radky.Length)
                        {
                            Console.WriteLine("Soubor je příliš dlouhý!");
                            Continue = false;
                        }
                        else
                        {
                            using (StreamReader sr = new StreamReader(ProjectPath))
                            {
                                for (int i = 1; i != radku + 1; i++)
                                {
                                    try
                                    {
                                        radky[i] = sr.ReadLine();
                                    }
                                    catch { }
                                }
                            }
                        }
                        a = 1;
                        while (radky[a] != null)
                        {
                            Console.Write(a + " ");
                            Console.WriteLine(radky[a]);
                            a++;
                        }
                    }
                    else a = 1;
                    if (Continue)
                    {
                        bool End = false;
                        int Radek = a;
                        while (!End)
                        {
                            Console.Write(Radek + " ");
                            radky[Radek] = Console.ReadLine();
                            string s = radky[Radek];
                            if (radky[Radek] == "exit")
                            {
                                radky[Radek] = null;
                                End = true;
                            }
                            else if (radky[Radek] == "back")
                            {
                                radky[Radek] = null;
                                radky[Radek - 1] = null;
                                Radek = Radek - 1;
                                Vypis(radky, ProjectName, Input);
                            }
                            else if (radky[Radek] == "help")
                            {
                                radky[Radek] = null;
                                Help(radky, ProjectName, Input);
                            }
                            else if (radky[Radek].Contains("save"))
                            {
                                radky[Radek] = null;
                                Vypis(radky, ProjectName, Input);
                                Save(radky, ProjectPath);
                                Vypis(radky, ProjectName, Input);
                            }
                            else if (radky[Radek].StartsWith("redo"))
                            {
                                radky[Radek] = null;
                                int wNumber;
                                try
                                {
                                    wNumber = int.Parse(s.Remove(0, 4));
                                    int AntiBug;
                                    int Bug = 0;
                                    if (wNumber > Radek - 1)
                                        AntiBug = 1 / Bug;
                                    if (wNumber < 10)
                                        Console.SetCursorPosition(2, wNumber);
                                    else if (wNumber < 100)
                                        Console.SetCursorPosition(3, wNumber);
                                    else
                                        Console.SetCursorPosition(4, wNumber);
                                    radky[wNumber] = Console.ReadLine();
                                    Console.SetCursorPosition(0, Radek);
                                    Vypis(radky, ProjectName, Input);
                                }
                                catch
                                {
                                    Vypis(radky, ProjectName, Input);
                                }
                            }
                            else if (radky[Radek].StartsWith("insert"))
                            {
                                radky[Radek] = null;
                                int wNumber;
                                try
                                {
                                    wNumber = int.Parse(s.Remove(0, 6)) + 1;
                                    int AntiBug;
                                    int Bug = 0;
                                    if (wNumber > Radek - 2)
                                        AntiBug = 1 / Bug;
                                    radky = Move(radky, wNumber);
                                    Vypis(radky, ProjectName, Input);
                                    if (wNumber > Radek - 1)
                                        AntiBug = 1 / Bug;
                                    if (wNumber < 10)
                                        Console.SetCursorPosition(2, wNumber);
                                    else if (wNumber < 100)
                                        Console.SetCursorPosition(3, wNumber);
                                    else
                                        Console.SetCursorPosition(4, wNumber);
                                    radky[wNumber] = Console.ReadLine();
                                    Radek = Radek - 1;
                                    Vypis(radky, ProjectName, Input);
                                    Radek = Radek + 2;
                                    Console.SetCursorPosition(0, Radek);
                                }
                                catch
                                {
                                    Vypis(radky, ProjectName, Input);
                                }
                            }
                            else
                                Radek++;
                        }
                    }
                }
                else if (Char == "S")
                {
                    bool Usable = true;
                    bool Legit = false;
                    string Project = "";
                    string FileName = "";
                    while (!Legit)
                    {
                        Console.Clear();
                        Console.Write("Zadejte název projektu k spuštění: ");
                        FileName = Console.ReadLine();
                        Project = FileName + ".txt";
                        Project = FolderPath + Project;
                        try
                        {
                            using (StreamReader sr = new StreamReader(Project)) { }
                            Legit = true;
                        }
                        catch
                        {
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("Debugging...");
                    List<string> Bugs = new List<string>();
                    using (StreamReader sr = new StreamReader(Project)) //check if knowed commands
                    {
                        string r = "";
                        int Radek = 1;
                        while ((r = sr.ReadLine()) != null)
                        {
                            bool Ok = false;
                            if (r.StartsWith("print"))
                                Ok = true;
                            else if (r.StartsWith("scan"))
                                Ok = true;
                            else if (r.StartsWith("math "))
                                Ok = true;
                            else if (r.StartsWith("number "))
                                Ok = true;
                            else if (r.StartsWith("text "))
                                Ok = true;
                            else if (r.StartsWith("set "))
                                Ok = true;
                            else if (r.StartsWith("if "))
                                Ok = true;
                            else if (r == "clear")
                                Ok = true;
                            else if (r.StartsWith("while "))
                                Ok = true;
                            else if (r == "(" || r == ")")
                                Ok = true;
                            else if (r.StartsWith("/"))
                                Ok = true;
                            else if (r.StartsWith("end"))
                                Ok = true;
                            else if (r.Length == 0)
                                Ok = true;
                            if (!Ok)
                            {
                                Usable = false;
                                Bugs.Add("Neznámý příkaz na řádku " + Radek.ToString());
                            }
                            Radek++;
                        }
                    }
                    using (StreamReader sr = new StreamReader(Project))
                    {
                        string r = "";
                        bool Ended = false;
                        while ((r = sr.ReadLine()) != null)
                        {
                            if (r == "end")
                                Ended = true;
                        }
                        if (!Ended)
                        {
                            Bugs.Add("Neukončený program!");
                            Usable = false;
                        }
                    }
                    if (Usable)
                    {
                        Console.Clear();
                        Color(ConsoleColor.White);
                        Console.Title = FileName;
                        string Line = "";
                        int Radek = 1;
                        using (StreamWriter sw = new StreamWriter("ram.txt")) { }
                        using (StreamReader sr = new StreamReader(Project))
                        {
                            bool End = false;
                            while (!End && (Line = sr.ReadLine()) != null)
                            {
                                string[] Word = Line.Split(' ');
                                if (Line == "print")
                                    Console.WriteLine();
                                else if (Line.StartsWith("print "))
                                {
                                    string Text = Line.Replace("print ", "");
                                    if (Text.StartsWith("\"") && Text.EndsWith("\""))
                                    {
                                        Console.WriteLine(Text.Replace("\"", ""));
                                    }
                                    else
                                    {
                                        using (StreamReader ram = new StreamReader("ram.txt"))
                                        {
                                            string r = "";
                                            while ((r = ram.ReadLine()) != null)
                                            {
                                                r = r.Replace("number ", "");
                                                r = r.Replace("text ", "");
                                                if (r.StartsWith(Text))
                                                {
                                                    r = r.Replace(Text + " ", "");
                                                    Console.WriteLine(r);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (Line == "end")
                                {
                                    Console.ReadKey();
                                    End = true;
                                }
                                else if (Line == "scan")
                                {
                                    Console.ReadLine();
                                }
                                else if (Line.StartsWith("scan "))
                                {
                                    string r = Line.Replace("scan ", "");
                                    if (JeCislo(r))
                                    {
                                        try
                                        {
                                            PridejCislo(r, int.Parse(Console.ReadLine()));
                                        }
                                        catch
                                        {
                                            Console.Clear();
                                            Color(ConsoleColor.Red);
                                            Console.WriteLine("Neočekávaný vstup!");
                                            Console.ReadKey();
                                            End = true;
                                        }
                                    }
                                    else PridejText(r, Console.ReadLine());
                                }
                                else if (Line.StartsWith("number "))
                                {
                                    string Text = Line.Replace("number ", "");
                                    PridejCislo(Text, 0);
                                }
                                else if (Line.StartsWith("text "))
                                {
                                    string Text = Line.Replace("text ", "");
                                    PridejText(Text, "");
                                }
                                else if (Line.StartsWith("math "))
                                {
                                    string All = Line.Remove(0, 5);
                                    string[] h = All.Split(' ');
                                    if (JeCislo(h[0]))
                                    {
                                        try
                                        {
                                            int a = int.Parse(Hodnota(h[0]));
                                            int b = 0;
                                            for (int i = 1; i != h.Count(); i++)
                                            {
                                                if (h[i].StartsWith("+"))
                                                {
                                                    try
                                                    {
                                                        b = int.Parse(h[i].Remove(0, 1));
                                                        a = a + b;
                                                    }
                                                    catch
                                                    {
                                                        string f = h[i].Remove(0, 1);
                                                        b = int.Parse(Hodnota(f));
                                                        a = a + b;
                                                    }
                                                }
                                                else if (h[i].StartsWith("-"))
                                                {
                                                    try
                                                    {
                                                        b = int.Parse(h[i].Remove(0, 1));
                                                        a = a - b;
                                                    }
                                                    catch
                                                    {
                                                        string f = h[i].Remove(0, 1);
                                                        b = int.Parse(Hodnota(f));
                                                        a = a - b;
                                                    }
                                                }
                                                else if (h[i].StartsWith("*"))
                                                {
                                                    try
                                                    {
                                                        b = int.Parse(h[i].Remove(0, 1));
                                                        a = a * b;
                                                    }
                                                    catch
                                                    {
                                                        string f = h[i].Remove(0, 1);
                                                        b = int.Parse(Hodnota(f));
                                                        a = a * b;
                                                    }
                                                }
                                                else if (h[i].StartsWith("/"))
                                                {
                                                    try
                                                    {
                                                        b = int.Parse(h[i].Remove(0, 1));
                                                        a = a / b;
                                                    }
                                                    catch
                                                    {
                                                        string f = h[i].Remove(0, 1);
                                                        b = int.Parse(Hodnota(f));
                                                        a = a / b;
                                                    }
                                                }
                                            }
                                            PridejCislo(h[0], a);
                                        }
                                        catch { }
                                    }
                                }
                                else if (Line.StartsWith("set "))
                                {
                                    string[] s = Line.Replace("set ", "").Split(' ');
                                    string name = s[0];
                                    string add = s[1];
                                    int a;
                                    if (int.TryParse(s[1], out a))
                                    {
                                        if (JeCislo(s[0]))
                                        {
                                            PridejCislo(name, a);
                                        }

                                    }
                                    else if (add.StartsWith("\""))
                                    {
                                        string text = Line.Replace("\"", "");
                                        text = text.Remove(0, 4);
                                        text = text.Remove(0, name.Length);
                                        text = text.Remove(0, 1);
                                        PridejText(name, text);
                                    }
                                    else if (JeCislo(s[1]))
                                    {
                                        if (JeCislo(s[0]))
                                        {
                                            PridejCislo(s[0], int.Parse(Hodnota(s[1])));
                                        }
                                        else
                                        {
                                            PridejText(s[0], Hodnota(s[1]));
                                        }
                                    }
                                    else if (!JeCislo(s[1]))
                                    {
                                        if (JeCislo(s[0]))
                                        {
                                            PridejCislo(s[0], int.Parse(Hodnota(s[1])));
                                        }
                                        else
                                        {
                                            PridejText(s[0], Hodnota(s[1]));
                                        }
                                    }
                                }
                                else if (Line.StartsWith("if "))
                                {
                                    bool Continue;
                                    string s = Line.Remove(0, 3);
                                    string[] h = s.Split(' ');
                                    if (Plati(h))
                                        Continue = true;
                                    else
                                    {
                                        int ignore = -1;
                                        string r = "";
                                        bool NextCommand = false;
                                        while (!NextCommand)
                                        {
                                            r = sr.ReadLine();
                                            if (r == "(")
                                                ignore++;
                                            else if (r == ")" && ignore == 0)
                                                NextCommand = true;
                                            else if (r == ")")
                                                ignore--;
                                        }
                                    }
                                }
                                else if (Line == "clear")
                                {
                                    Console.Clear();
                                }
                                Radek++;
                            }
                        }
                    }
                    else
                    {
                        Color(ConsoleColor.Red);
                        Console.Clear();
                        foreach (string s in Bugs)
                        {
                            Console.WriteLine(s);
                        }
                        Console.ReadKey();
                    }
                }
                else if (Char == "K")
                {
                    Exit = true;
                }
            }
            // 2
            Console.ReadKey();
        }
        static string[] Move(string[] s, int a)
        {
            using (StreamWriter sw = new StreamWriter("help.txt"))
            {
                for (int i = 0; i != a; i++)
                {
                    try { sw.WriteLine(s[i]); }
                    catch { }
                }
                sw.WriteLine();
                int count = 1;
                while (s[count] != null)
                {
                    count++;
                }
                for (int i = a; i != count + 2; i++) //
                {
                    try { sw.WriteLine(s[i]); }
                    catch { }
                }
                sw.Flush();
            }
            using (StreamReader sr = new StreamReader("help.txt"))
            {
                int i = 0;
                string r = "";
                while ((r = sr.ReadLine()) != null)
                {
                    s[i] = r;
                    i++;
                }
            }
            return s;
        }
        static void Help(string[] s, string a, string b)
        {
            using (StreamWriter sw = new StreamWriter("help.txt"))
            {
                int i = 1;
                while (s[i] != null)
                {
                    sw.WriteLine(s[i]);
                    i++;
                }
                sw.Flush();
            }
            Console.Clear();
            Console.WriteLine("Help:");
            Console.WriteLine("save - Uloží projekt\nexit - Zpět do menu");
            Console.WriteLine("\nhelp - Nápověda\n\n");
            Console.WriteLine("back - Vrátí se zpět na předchozý řádek\nredo x - Přesune se na x-tý řádek\n");
            Console.WriteLine("insert x - Vloží nový řádek za x-tý řádek\n\n");
            string[] Commands = new string[11] 
            {
                "print x - Vypíše text/číslo x",
                "scan x - Uloží vstup do x",
                "number - Proměnná reprezentující číslo",
                "text - Proměnná reprezentující text",
                "/komentář",
                "clear - vyčistí konzoli",
                "set x y - Uloží do x hodnotu y",
                "math x +1 *2 /3 -4 - číslo x se změní na {[(x + 1) * 2] / 3} - 4",
                "\nPříklad: set n t - Do čísla n bude dosazen text t",
                "\nif x = y or x < y and y ! x\n(\n) - Pokuď platí, proběhnou operace mezi závorkami",
                "\nwhile x = y or x < y and y ! x\n(\n) - Dokuď platí, budou se operace mezi závorkami opakovat\n"
            };
            for (int i = 0; i != 10; i++)
            {
                Console.WriteLine(Commands[i]);
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine(a + b);
            using (StreamReader sr = new StreamReader("help.txt"))
            {
                string r = "";
                int i = 1;
                while ((r = sr.ReadLine()) != null)
                {
                    Console.Write(i + " ");
                    Console.WriteLine(r);
                    i++;
                }
            }
        }
        static void Color(ConsoleColor c)
        {
            Console.ForegroundColor = c;
        }
        static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        static void CreateProject(string name, string path)
        {
            using (StreamWriter sw = new StreamWriter(path + name))
            {
                sw.Flush();
            }
        }
        static void Vypis(string[] s, string a, string b)
        {
            Console.Clear();
            Console.WriteLine(a + b);
            int i = 1;
            while (s[i] != null)
            {
                Console.Write(i + " ");
                Console.WriteLine(s[i]);
                i++;
            }
        }
        static void Save(string[] s, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                int i = 1;
                while (s[i] != null)
                {
                    sw.WriteLine(s[i]);
                    i++;
                }
                sw.Flush();
            }
        }
        static void PridejCislo(string s, int i)
        {
            using (StreamReader sr = new StreamReader("ram.txt"))
            {
                using (StreamWriter sw = new StreamWriter("help.txt"))
                {
                    string r = "";
                    string h = "";
                    while ((r = sr.ReadLine()) != null)
                    {
                        h = r.Replace("number ", "");
                        if (!h.StartsWith(s))
                        {
                            sw.WriteLine(r);
                        }
                    }
                    sw.Flush();
                    sw.Close();
                }
                sr.Close();

            }
            using (StreamWriter sw = new StreamWriter("ram.txt"))
            {
                sw.WriteLine("number " + s + " " + i);
                using (StreamReader sr = new StreamReader("help.txt"))
                {
                    string r = "";
                    while ((r = sr.ReadLine()) != null)
                    {
                        sw.WriteLine(r);
                    }
                    sr.Close();
                }
                sw.Flush();
            }
        }
        static bool JeCislo(string s)
        {
            using (StreamReader sr = new StreamReader("ram.txt"))
            {
                string r = "";
                while ((r = sr.ReadLine()) != null)
                {
                    string[] h = r.Split(' ');
                    if (h[1] == s)
                    {
                        if (h[0] == "number")
                            return true;
                        else return false;
                    }
                }
            }
            return false;
        }
        static void PridejText(string s, string o)
        {
            using (StreamReader sr = new StreamReader("ram.txt"))
            {
                using (StreamWriter sw = new StreamWriter("help.txt"))
                {
                    string r = "";
                    string h = "";
                    while ((r = sr.ReadLine()) != null)
                    {
                        h = r.Replace("text ", "");
                        if (!h.StartsWith(s))
                        {
                            sw.WriteLine(r);
                        }
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter("ram.txt"))
            {
                sw.WriteLine("text " + s + " " + o);
                using (StreamReader sr = new StreamReader("help.txt"))
                {
                    string r = "";
                    while ((r = sr.ReadLine()) != null)
                    {
                        sw.WriteLine(r);
                    }
                }
            }
        }
        static string Hodnota(string s)
        {
            string value = "";
            using (StreamReader sr = new StreamReader("ram.txt"))
            {
                string r = "";
                while ((r = sr.ReadLine()) != null)
                {
                    string[] h = r.Split(' ');
                    if (h[1] == s)
                        value = h[2];
                }
            }
            return value;
        }
        static bool Plati(string[] s)
        {
            bool Value = false;
            int outVal = 0;
            string a = "";
            string b = "";
            if (int.TryParse(s[0], out outVal) && !int.TryParse(s[2], out outVal)) // 1 : a
            {
                a = s[0];
                b = Hodnota(s[2]);
            }
            else if (!int.TryParse(s[0], out outVal) && int.TryParse(s[2], out outVal)) // a : 1
            {
                if (s[2].StartsWith("\"") && s[2].EndsWith("\""))
                {
                    b = s[2].Replace("\"", "");
                }
                else
                {
                    b = s[2];
                }
                a = Hodnota(s[0]);
            }
            else if (int.TryParse(s[0], out outVal) && int.TryParse(s[2], out outVal)) // 1 : 1
            {
                a = s[0];
                b = s[2];
            }
            else if (!int.TryParse(s[0], out outVal) && !int.TryParse(s[2], out outVal)) // a : b
            {
                if (s[2].StartsWith("\"") && s[2].EndsWith("\"") && s[0].StartsWith("\"") && s[0].EndsWith("\""))
                {
                    a = s[0].Replace("\"", "");
                    b = s[2].Replace("\"", "");
                }
                else if (s[2].StartsWith("\"") && s[2].EndsWith("\""))
                {
                    b = s[2].Replace("\"", "");
                    a = Hodnota(s[0]);
                }
                else if (s[0].StartsWith("\"") && s[0].EndsWith("\""))
                {
                    a = s[0].Replace("\"", "");
                    b = Hodnota(s[2]);
                }
                else
                {
                    a = Hodnota(s[0]);
                    b = Hodnota(s[2]);
                }
            }
            switch (s[1])
            {
                case "=":
                    if (a == b)
                        Value = true;
                    break;

                case "!":
                    if (a != b)
                        Value = true;
                    break;

                case "<":
                    if (int.Parse(a) < int.Parse(b))
                        Value = true;
                    break;

                case ">":
                    if (int.Parse(a) > int.Parse(b))
                        Value = true;
                    break;
            }
            return Value;
        }
    }
}
