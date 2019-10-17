
    using System.Collections.Generic;
    using System.IO;
    public class Flashcard
    {
        public string krn_side { get; set; }
        public string eng_side { get; set; }
        public string krn_def { get; set; }
        public string krn_context { get; set; }
        public string eng_context { get; set; }
        public bool known { get; set; }

        public Flashcard(string krn_side, string eng_side, string krn_def, string krn_context, string eng_context)
        {
            this.krn_side = krn_side;
            this.eng_side = eng_side;
            this.krn_def = krn_def;
            this.krn_context = krn_context;
            known = false;
            this.eng_context = eng_context;
        }
        public static List<Flashcard> test_cards () {
            List<Flashcard> result = new List<Flashcard>();
            result.Add(new Flashcard("시시하다", "1. trifling; trivial; insignificant [Negligible, neither special nor important.]",
                "별다르거나 중요하지 않고 하찮다.", "시시하다.", "Boring."));
            using(var reader = new StreamReader(@"test.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    string krn = values[0];
                    string eng = values[1];
                    string krn_def = values[2];
                    result.Add(new Flashcard(krn, eng,
                        krn_def, "---.", "---."));
                }
            }
            
            
            return result;
        }
    }
    
    
