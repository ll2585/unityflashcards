    public class Term
    {
        public string krn_side { get; set; }
        public string eng_side { get; set; }
        public string krn_def { get; set; }
        public string krn_context { get; set; }
        public string eng_context { get; set; }

        public Term(string krn_side, string eng_side, string krn_def, string krn_context=null, string eng_context=null)
        {
            this.krn_side = krn_side;
            this.eng_side = eng_side;
            this.krn_def = krn_def;
            this.krn_context = krn_context;
            this.eng_context = eng_context;
        }
    }
