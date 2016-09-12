using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace uzdevums_NET_programmetajam
{
    class Program
    {
        static ConsoleKeyInfo info; // lietotaja ievads

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Uzdevums_NET_programmetajam\t\t\t\t\t\t\tIzstradāja: Deniss.Ivanovs@va.lv");
            Console.ResetColor();

            Console.WriteLine("Izveidot .NET C# konsoles aplikāciju, \nkura realizē divu veidu preču definēšanu un pirkuma noformēšanu, piemērojot atlaidi\n");


            bool loopComplete = false; // lai apturetu programmu/ nodrosinatu atkartotu ievadu no izvelnes
            bool karteIr = false; // vair ir klienta/atlaizu karte

            //precu saraksta deklareshana //veidu/kategoriju saraksta deklaresana
            List<Prece> precu_katalogs = new List<Prece>();   //bez skaita parametra
            List<Veids> veidu_katalogs = new List<Veids>();
            List<Prece> pirkums_list = new List<Prece>();    // skaits parametrs

            //izveidojam kategorijas un veidus  = veids(koda_formats, nosaukuma_garums)
            Veids veids1 = 
                new Veids("^T[0-9]{3}$", "^.{0,200}$"); //a.i.Īpašība: formātā T + 3 (piemēram T001) a.ii. pieļaujamo garumu 200 simboli; [regex]
            Veids veids2 =
                new Veids("^PC[0-9]{4}$", "^.{0,220}$"); //a.i.Īpašība: formātā PC + 4 (piemēram T001) a.ii. pieļaujamo garumu 220 simboli;[regex]
            veidu_katalogs.Add(veids1);
            veidu_katalogs.Add(veids2);

            //instrukcijas
            Console.WriteLine("[F1]"+"\t" +"pievienot preci");
            Console.WriteLine("[F2]" + "\t" + "izveidot pirkumu");
            Console.WriteLine("[F3]" + "\t" + "Izvadīt uz ekrāna čeku");
            Console.WriteLine();
            Console.WriteLine("[F5]" + "\t" + "labot atlaidi");
            Console.WriteLine("[F8]" + "\t" + "Izvadīt preču sarakstu");
            Console.WriteLine("[ESCAPE]" + "\t" + "Iziet \n");
            //Prece rec1 = new Prece("T001", "nosaukum1", "123", "100", 1);  //testam lai nav ar roku javada
            //Prece rec2 = new Prece("T002", "nosaukum2", "123", "100", 2);
            //Prece rec3 = new Prece("T003", "nosaukum3", "123", "100", 3);
            //Prece rec4 = new Prece("T004", "nosaukum4", "123", "100", 4);
            //Prece rec5 = new Prece("T005", "nosaukum5", "123", "100", 1);
            //precu_katalogs.Add(rec1);
            //precu_katalogs.Add(rec2);
            //precu_katalogs.Add(rec3);
            //precu_katalogs.Add(rec4);
            //precu_katalogs.Add(rec5);
            //pirkums_list.Add(rec1);
            //pirkums_list.Add(rec2);
            //pirkums_list.Add(rec3);
            //pirkums_list.Add(rec4);
            //pirkums_list.Add(rec5);

            while (!loopComplete)
            {
                //lietotaja navigācijas taustinji
                Console.WriteLine(" _ _ __ _ _ __ _ _ __ _ _ __ _ _ __ _ _ __ _ _ __ _ _ __ _ _ _");
                info = Console.ReadKey(true);
                switch (info.Key)
                {
                    case ConsoleKey.F1:
                        try
                        {
                            Console.WriteLine("pievienot preci:");
                            Console.WriteLine("\n -ievadiet preces parametrus: kods nosaukums cena atlaide");
                            Console.Write("kods\t \t:");
                            String a = Console.ReadLine();
                            Console.Write("nosaukums\t:");
                            String b = Console.ReadLine();
                            Console.Write("cena\t \t:");
                            String c = Console.ReadLine();
                            Console.Write("atlaide\t \t:");
                            String d = Console.ReadLine();
                            //preces pievienosana
                            Prece prece1 =
                                new Prece(a, b, c, d);
                            // parbaudam kodu formātu un nosaukuma garumi ekssistē pie veidu saraksta
                            Veids kodsValids = kods_pareizs(prece1, veidu_katalogs);
                            if (kodsValids != null)
                            {
                                //ja nosaukums nav par garu
                                if (formats_valids(kodsValids.nos_formats, prece1.nosaukums))
                                {
                                    precu_katalogs.Add(prece1);
                                }
                                else
                                {
                                   // Console.WriteLine("\n -Preces nosaukums parsniedz pieļaujamo garumu!");
                                    throw new ArgumentException("\n -Preces parametrs:nosaukums par garu! pievienojiet preci no jauna!", "nosaukums");
                                }
                            }
                            else
                            //kods neatbilst
                            {
                               // Console.WriteLine("\n -Nepareizs preces koda formāts!");
                                throw new ArgumentException("\n -Nepareizs preces koda formāts! pievienojiet preci no jauna!", "kods");
                            }
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine("\nKluda : {0} .", e.Message);
                        }

                        
                        break;

                    case ConsoleKey.F8:
                        Console.WriteLine("\npreču saraksts:");

                        if (precu_katalogs.Any())
                        {

                            int num = 1;
                            foreach (Prece s in precu_katalogs)
                            {
                                Console.WriteLine("  |" + num + "|" + s.kods + "\t" + s.nosaukums + "\t" + s.cena + "\t" + s.atlaide );
                                num++;
                            }
                        }
                        else
                            Console.WriteLine("\n -Preču sarakst ir tukšs ,lūdzu , pievienojiet preci.");

                        break;

                    case ConsoleKey.F2:
                        Console.WriteLine("izveidot pirkumu:");
                        if (precu_katalogs.Any())
                        {
                            try
                            {
                                // klienta karte
                                ConsoleKeyInfo cki;
                                pirkums_list = new List<Prece>(); // veicot jaunu pirkumu saraksts tiek veidots no jauna
                                do
                                {
                                    Console.WriteLine(" -vai jums ir klienta karte Y/N:");
                                    cki = Console.ReadKey();
                                } while (!((cki.Key == ConsoleKey.Y) || (cki.Key == ConsoleKey.N)));

                                karteIr = (cki.Key == ConsoleKey.Y);
                                do
                                {
                                    // izvelamies ,parbauda preces kodu
                                    Console.WriteLine("\n -izvelaties preces kodu:");
                                    string fkods = Console.ReadLine();
                                    var item = precu_katalogs.Find(x => x.kods == fkods);
                                    if (item == null)
                                        throw new Exception("\n -šāda koda preču sarakstā nav!");
                                    else
                                    {
                                        Console.WriteLine(" -prece [{0}], ievadiet skaitu:", item.nosaukums);
                                        int skaits = Convert.ToInt32(Console.ReadLine());                      //preču skaits konkretam kodam
                                        Prece pirkums1 =
                                            new Prece(item.kods, item.nosaukums, item.cena, item.atlaide, skaits);
                                        if (pirkums1 != null)
                                            pirkums_list.Add(pirkums1);

                                    }
                                    do
                                    {
                                        Console.WriteLine("\n -vai pievienot citu preci pirkumam Y/N:");
                                        cki = Console.ReadKey();
                                    } while (!((cki.Key == ConsoleKey.Y) || (cki.Key == ConsoleKey.N)));


                                } while (cki.Key == ConsoleKey.Y);

                                if (!pirkums_list.Any())
                                    Console.WriteLine("Pirkums neizdevās!");
                            }

                            catch (Exception e)
                            {
                                Console.WriteLine("Kluda : {0} .", e.Message);
                            }


                        }
                        else
                            Console.WriteLine("\n -sarakst ir tukšs pievienojiet preci");
                        break;


                    case ConsoleKey.F3:
                        Console.WriteLine("Izvadīt uz ekrāna čeku:\n");
                        if (pirkums_list.Any())
                        {
                            //pirkuma objekta izveide  
                            Pirkums pirkumaObjekts = new Pirkums();
                            pirkumaObjekts.ceks = pirkums_list;
                            pirkumaObjekts.irKarte = karteIr; // karteIr;
                            decimal summa=00.00m;                      //summas aprekinasanai
                            decimal atlaide=00.00m;                      //atlaides aprekinasanai
                            // cheka izvads uz ekrana 
                   
                            Console.WriteLine(String.Format("|{0,5}|{1,5}|{2,5}|", "Preces kods" + "\t", "Preces nosaukums" + "\t", "cena" + "\t"));

                            foreach (Prece s in pirkumaObjekts.ceks)
                            {
                                for (int i = 1; i <= s.skaits; i++)
                                {
                                    decimal tmp;       // cenas parveidosanai no string
                                    decimal.TryParse(s.cena, out tmp);
                                    int tmp2;         // atlaides parveidosanai no int
                                    int.TryParse(s.atlaide, out tmp2);
                                    Console.WriteLine(String.Format("|{0,5}|{1,5}|{2,5}|", s.kods+ "\t\t", s.nosaukums + "\t\t", tmp.ToString("0.00") + "\t"));
                                    //atlaides aprekinasana
                                    if (pirkumaObjekts.irKarte)
                                    {
                                        atlaide += tmp * tmp2 / 100;
                                        summa += tmp - tmp * tmp2 / 100;
                                    }
                                    else summa += tmp;
                                }
                            }
                            Console.WriteLine(String.Format("{0,5}|{1,5}|{2,5}", "" + "\t\t", "kopa" + "\t\t", summa + "\t\t"));
                            Console.WriteLine(String.Format("{0,5}|{1,5}|-{2,5}", "" + "\t\t", "atlaide" + "\t", atlaide + "\t"));

                        }
                        else Console.WriteLine("\n -izveidojiet pirkumu!");

                            break;
                    case ConsoleKey.F5:
                        if (precu_katalogs.Any())// parbauda vai ir ieraksti
                        {
                            try
                            {
                                Console.WriteLine("\n -labot atlaidi, ievadiet preces kodu:");
                                string fkods = Console.ReadLine();
                                var item = precu_katalogs.Find(x => x.kods == fkods);  //mekle sakritibu
                                if (item == null)
                                    throw new Exception("\n -šada koda preču sarakstā nav!");
                                else
                                {
                                    Console.WriteLine("\n -labot atlaidi[{0}], ievadiet jauno preces atlaidi:", item.atlaide);
                                    Prece jaunaAtlaide =
                                        new Prece(item.kods, item.nosaukums, item.cena, Console.ReadLine());
                                    if (jaunaAtlaide != null)
                                        precu_katalogs[precu_katalogs.FindIndex(x => x.kods == fkods)] = jaunaAtlaide;
                                }
                            }

                            catch (Exception e)
                            {
                                Console.WriteLine("Kluda : {0} .", e.Message);
                            }

                        }
                        else
                            Console.WriteLine("\n -Preču sarakst ir tukšs ,lūdzu , pievienojiet preci.");
                        break;

                    case ConsoleKey.Escape:
                        Console.WriteLine("[ESCAPE]");
                        loopComplete = true;
                        break;
                }
            }
   
        }
        /// <summary>
        /// metode ievadot regex-formatu un preces parametru atgriez true ja formats atbilst
        /// </summary>  
        
        public static bool formats_valids(string formats, string param)
        {
            Match match = Regex.Match(param, @formats, //a.iii. - pozitīvs decimālskaitlis;
                 RegexOptions.None);                  
            return (match.Success);
           
        }
        /// <summary>
        /// mekle vai koda formāts eksistē veidu/kategoriju sarakstā atrodot 1.sakritibu atgriež veida objektu no saraksta, atgriez null objektu ja nav sakritibas
        /// </summary>
        public static Veids kods_pareizs (Prece prece1, List<Veids> veidu_katalogs)
        {
            foreach (Veids x in veidu_katalogs)
            {
                if (formats_valids(x.koda_formats,prece1.kods))
                {
                    return x;
                }
            }          
            return null;        
        }
    }
    /// <summary>
    /// klase Preces - ipašības (kods,nosaukums,cena,atlaide)
    /// </summary>
    class Prece
    {
        private string _kods, _nosaukums, _cena, _atlaide; int _skaits;
        private string kods_tmp, nosaukums_tmp, cena_tmp, atlaide_tmp; int skaits_tmp;

        public string kods
        {
            get
            {
                return _kods;
            }
            set
            {
                if (string.IsNullOrEmpty(kods_tmp))
                {
                   // Console.WriteLine("preces kods netika ievadits!");
                    throw new ArgumentException("kods netika ievadits, pievienojiet preci no jauna!", "kods");
                }
                _kods = kods_tmp;
            }
        }
        public string nosaukums
        {
            get
            {
                return _nosaukums;
            }
            set
            {
                if (string.IsNullOrEmpty(nosaukums_tmp))
                {
                   // Console.WriteLine("preces nosaukums netika ievadits!");
                    throw new ArgumentException("nosaukums netika ievadits, pievienojiet preci no jauna!","nosaukums");
                }
                _nosaukums = nosaukums_tmp;
            }
        }
        public string cena
        {
            get
            {
                return _cena;
            }
            set
            {

                decimal tmp;
                if (!decimal.TryParse(cena_tmp.Replace('.', ','), out tmp))// pienem . , ka dec atdalitaju
                {
                   // Console.WriteLine("cenaa nav pozitīvs decimālskaitlis,ievadiet preces cenu1!");
                    throw new ArgumentException("cenaa nav pozitīvs decimālskaitlis,ievadiet preces cenu!, pievienojiet preci no jauna!", "cena");

                }
                Match match = Regex.Match(cena_tmp, @"^(\d+(?:[\.\,]\d{1,2})?)$", //a.iii. - pozitīvs decimālskaitlis;
                    RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    // Console.WriteLine("nekorekta cena, cenai jabut pozitīvam decimālskaitlim (max 2 cipari aiz komata)!");
                    throw new ArgumentException("cenaa nav pozitīvs decimālskaitlis,ievadiet preces cenu!, pievienojiet preci no jauna!", "cena");

                }
                _cena = cena_tmp;
            }
        }

        public string atlaide
        {
            get
            {
                return _atlaide;
            }
            set
            {
                int tmp;
                if (!int.TryParse(atlaide_tmp, out tmp))
                {
                   // Console.WriteLine("ievadiet korektu atlaidi! (nav skaitlis/par lielu)");
                    throw new ArgumentException("ievadiet korektu atlaidi!(nav skaitlis/par lielu), ja velaties labot Atlaidi nospiežiet [F5]", "atlaide");
                }
                Match match = Regex.Match(atlaide_tmp, @"^[1-9][0-9]*$", //a.iv. pozitīvs naturāls
                   RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                   // Console.WriteLine("ievadiet korektu atlaidi!");
                    throw new ArgumentException("nav pozitīvs naturāls, ievadiet korektu atlaidi, ja velaties labot Atlaidi nospiežiet [F5]", "atlaide");
                }
                _atlaide = atlaide_tmp;
            }
        }
        public int skaits
        {
            get
            {
                return _skaits;
            }
            set
            {
                if (skaits_tmp < 0 )
                {
                  //  Console.WriteLine("ievadiet skaitli lielaks par 1");
                    throw new ArgumentException("ievadiet skaitli lielaks par 0, ja velaties pievienot preci no jauna nospiežiet [F2]", "skaits");
                }
               
                _skaits = skaits_tmp;
            }
        }

        public Prece() { }
        /// <summary>
        /// tiks izmantots pirkuma klases parametra saraksta izveidei, jo satur parametru skaits
        /// </summary>
        public Prece(string kods, string nosaukums, string cena, string atlaide, int skaits)
        {
            kods_tmp = kods;
            nosaukums_tmp = nosaukums;
            cena_tmp = cena;
            atlaide_tmp = atlaide;
            skaits_tmp = skaits;
            this.kods = kods;
            this.nosaukums = nosaukums;
            this.cena = cena;
            this.atlaide = atlaide;
            this.skaits = skaits;
        }

        public Prece(string kods, string nosaukums, string cena, string atlaide)
        {

            kods_tmp = kods;
            nosaukums_tmp = nosaukums;
            cena_tmp = cena;
            atlaide_tmp = atlaide;

            this.kods = kods;
            this.nosaukums = nosaukums;
            this.cena = cena;
            this.atlaide = atlaide;

        }
    }
    /// <summary>
    /// klase pirkums sastāv no saraksta ar precēm un vai irkarte
    /// </summary>
    class Pirkums
    {
        public List<Prece> ceks
        {
            get;
            set;
        }
        public Boolean irKarte
        {
            get;
            set;
        }

    }
    /// <summary>
    /// klase lai definetu precu veidus
    /// </summary>
    class Veids
    {
        public string koda_formats// Tiks izmantota regex funkcija (regular expresions klase)
        {
            get;
            set;
        }
        public string nos_formats// Tiks izmantota regex funkcija (regular expresions klase)
        {
            get;
            set;
        }
        public Veids() { }
        public Veids(string koda_f, string nosauk_f)
        {
            koda_formats = koda_f;
            nos_formats = nosauk_f;
        }
    }
   
}
