    public class FlashcardSide
    {
        public string value { get;  }
        public string additional_value { get; }
        public string value_3 { get; }

        public FlashcardSide(string value, string additionalValue = null, string value3 = null)
        {
            this.value = value;
            this.additional_value = additionalValue;
            this.value_3 = value3;
        }
        
        
    }
