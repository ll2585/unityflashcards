using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashcards : MonoBehaviour
{
    private string stringToEdit;
    private TouchScreenKeyboard keyboard;
    public InputField input;
    public Text prompt;
    private string cur_solution = "가";
    private STATE cur_state;
    private string entered_answer;

    public Button show_answer_button;
    public Button right_answer_button;
    public Button wrong_answer_button;
    public Button test;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject flashcard_panel;
    private Animator animator;
    private List<Flashcard> cards;
    private List<Flashcard> cards_to_show;
    private Flashcard cur_card;

    public Text flashcard_context_1;
    public Text flashcard_context_2;
    public Text flashcard_value;

    private int cur_card_index;

    private MODE cur_mode;
    public Toggle cur_card_known;

    public GameObject done_panel;
    public Toggle review_known_toggle;
    private bool review_known_cards;

    public Text card_num_text;
    private enum STATE
    {
        SHOWING_QUESTION,
        SHOWING_ANSWER,
        SHOWING_DONE_PANEL,
        NONE
    }

    private enum MODE
    {
        KOR_TO_ENG_FLASHCARD, // korean -> english def + korean context + eng context
        ENG_TO_KOR_FLASHCARD, // english def -> korean + korean context + eng context
        ENG_TO_KOR_TYPE, // english def -> korean + korean context + eng context
        KOR_TO_KOR_TYPE // korean def -> korean + korean context
    }

    // Start is called before the first frame update
    void Start()
    {
        cur_state = STATE.SHOWING_QUESTION;
        stringToEdit = $"Please type in '{cur_solution}'";
        input.onEndEdit.AddListener(input_edited);
        show_answer_button.onClick.AddListener(show_answer_button_clicked);
        wrong_answer_button.onClick.AddListener(wrong_answer);
        right_answer_button.onClick.AddListener(right_answer);
        test.onClick.AddListener(switch_panel);
        animator = flashcard_panel.GetComponent<Animator>();
        cards = Flashcard.test_cards();
        cards_to_show = cards;

        cur_card_index = 0;
        review_known_cards = false;
        cur_mode = MODE.KOR_TO_ENG_FLASHCARD;
        refresh_card();
        show_flashcard_panel();
    }

    public void refresh_card()
    {
        cur_card = cards_to_show[cur_card_index];
    }

    public void switch_panel()
    {
        print(Time.deltaTime * 1000);
        if (flashcard_panel != null)
        {

            if (animator != null)
            {
                bool show_bottom = animator.GetBool("show_bottom");

                animator.SetBool("show_bottom", !show_bottom);
                print(Time.deltaTime * 1000);
            }
        }
    }

    public void show_flashcard_side(bool top = true)
    {
        FlashcardSide front_side = cur_card.front;
        FlashcardSide back_side = cur_card.back;

        if (cur_mode == MODE.KOR_TO_ENG_FLASHCARD)
        {
            front_side = cur_card.front;
            back_side = cur_card.back;
        } else if (cur_mode == MODE.ENG_TO_KOR_FLASHCARD)
        {
            front_side = cur_card.front;
            back_side = cur_card.back;
        }

        FlashcardSide side_shown = front_side;
        cur_card_known.isOn = cur_card.known;
        if (top)
        {
            
            cur_state = STATE.SHOWING_QUESTION;
        }
        else
        {
            side_shown = back_side;
            cur_state = STATE.SHOWING_ANSWER;
        }

        flashcard_value.text = side_shown.value;
        flashcard_context_1.text = side_shown.additional_value;
        flashcard_context_2.text = side_shown.value_3;

    }
    public void show_flashcard_panel()
    {
        done_panel.SetActive(false);
        if (cur_mode == MODE.KOR_TO_ENG_FLASHCARD)
        {
            flashcard_panel.SetActive(true);

        } else if (cur_mode == MODE.ENG_TO_KOR_FLASHCARD)
        {
            flashcard_panel.SetActive(true);
        }

        show_flashcard_side(true);
    }

    // Update is called once per frame
    void Update()
    {
        prompt.text = stringToEdit;
        get_input();
    }

    void toggle_known()
    {
        foreach (Flashcard card in cards)
        {
            if (card == cur_card)
            {
                cur_card.known = !cur_card.known;
                cur_card_known.isOn = cur_card.known;
            }
        }
        
    }
    
    void get_input()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (cur_state == STATE.SHOWING_QUESTION)
            {
                show_flashcard_side(false);
            } 
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (cur_state == STATE.SHOWING_ANSWER)
            {
                show_flashcard_side(true);
            }
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            print(cur_state);
            if (cur_state == STATE.SHOWING_DONE_PANEL)
            {
                review_known_cards = review_known_toggle.isOn;
                cards_to_show = new List<Flashcard>();
                for (int i = 0; i < cards.Count; i++)
                {
                    if ((!review_known_cards && !cards[i].known) || review_known_cards)
                    {
                        cards_to_show.Add(cards[i]);
                    }
                }

                Utils.Shuffle(cards_to_show);
                cur_card_index = 0;
                refresh_card();
                print("show flashcard");
                show_flashcard_panel();
            }
            else
            {
                next_card();
                if (cur_state != STATE.SHOWING_DONE_PANEL)
                {
                    show_flashcard_side(true);
                }
                
            }
            
            
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            last_card();
            show_flashcard_side(true);
            
        }else if (Input.GetKeyDown(KeyCode.Space))
        {
            toggle_known();
        }
    }
    
    void last_card()
    {
        
        if (cur_card_index == 0)
        {
            
        }
        else
        {
            cur_card_index -= 1;
            refresh_card();
        }
        
    }

    void next_card()
    {
        if (cur_card_index == cards_to_show.Count - 1)
        {
            show_all_done_panel();
        }
        else
        {
            cur_card_index += 1;
            refresh_card();
        }
        
    }

    void show_all_done_panel()
    {
        flashcard_panel.SetActive(false);
        done_panel.SetActive(true);
        review_known_toggle.isOn = review_known_cards;
        cur_state = STATE.SHOWING_DONE_PANEL;
        print(cur_state);
    }

    void OnGUI()
    {
        show_answer_button.gameObject.SetActive(cur_state == STATE.SHOWING_QUESTION);
        right_answer_button.gameObject.SetActive(cur_state == STATE.SHOWING_ANSWER);
        wrong_answer_button.gameObject.SetActive(cur_state == STATE.SHOWING_ANSWER);
    }

    void show_answer_button_clicked()
    {
        answer_submitted();
    }
    void input_edited(string new_val)
    {
        entered_answer = new_val;

    }

    void answer_submitted()
    {
        cur_state = STATE.SHOWING_ANSWER;
        print(entered_answer);
        if (entered_answer == cur_solution)
        {
            print("RIGHT!~");
        }
    }

    void right_answer()
    {
        print("U CLICKED ON RIGHT ANSWER");
    }
    
    void wrong_answer()
    {
        print("U CLICKED ON WRONG ANSWER");
    }
}
