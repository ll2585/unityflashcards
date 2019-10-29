
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public class Flashcard
    { 
        public bool known { get; set; }
        public FlashcardSide front { get; }
        public FlashcardSide back { get; }

        public Flashcard(Term t,FlashcardSide front, FlashcardSide back)
        {
            known = false;
            this.front = front;
            this.back = back;
        }
        public static List<Flashcard> test_cards () {
            List<Flashcard> result = new List<Flashcard>();
            Term a = new Term("시시하다", "1. trifling; trivial; insignificant [Negligible, neither special nor important.]", "별다르거나 중요하지 않고 하찮다.", "시시하다",
                "Boring.");
            FlashcardSide front = new FlashcardSide(a.krn_side);
            FlashcardSide back = new FlashcardSide(a.eng_side, a.krn_context, a.eng_context);
            result.Add(new Flashcard(a, front, back));
            using(var reader = new StreamReader(@"test.csv"))
            {
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    String[] values = CSVParser.Split(line);
                    //var values = line.Split(',');
                    string krn = values[0];
                    string eng = values[1];
                    string krn_def = values[2];
                    Term t = new Term(krn, eng, krn_def);
                    FlashcardSide t_front = new FlashcardSide(t.krn_side);
                    FlashcardSide t_back = new FlashcardSide(t.eng_side, t.krn_context, t.eng_context);
                    Debug.Log(back.value);
                    result.Add(new Flashcard(t, t_front, t_back));
                }
            }
            Debug.Log(result.Count);
            Utils.Shuffle(result);
            return result;
        }
    }
    
    
