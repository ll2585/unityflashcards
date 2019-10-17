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
    private enum STATE
    {
        SHOWING_QUESTION,
        SHOWING_ANSWER,
        SHOWING_DONE_PANEL,
        NONE
    }
    private enum MODE
    {
        KOR_TO_ENG_FLASHCARD,
        ENG_TO_KOR_FLASHCARD,
        ENG_TO_KOR_TYPE,
        KOR_TO_KOR_TYPE
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
        animator= flashcard_panel.GetComponent<Animator>();
        cards = Flashcard.test_cards();
        cur_card_index = 0;
        review_known_cards = false;
        cur_mode = MODE.KOR_TO_ENG_FLASHCARD;
        refresh_card();
        show_flashcard_panel();
    }

    public void refresh_card()
    {
        cur_card = cards[cur_card_index];
    }

    public void switch_panel()
    {
        print( Time.deltaTime * 1000);
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
    
    public void show_flashcard_side(bool top=true)
    {
        print("SHPOW");
        if (cur_mode == MODE.KOR_TO_ENG_FLASHCARD)
        {
            cur_card_known.isOn = cur_card.known;
            if (top)
            {
                flashcard_value.text = cur_card.krn_side;
                flashcard_context_1.text = "";
                flashcard_context_2.text = "";
                cur_state = STATE.SHOWING_QUESTION;
            }
            else
            {
                flashcard_value.text = cur_card.eng_side;
                flashcard_context_1.text = cur_card.krn_context;
                flashcard_context_2.text = cur_card.eng_context;
                cur_state = STATE.SHOWING_ANSWER;
            }

            
        } else if (cur_mode == MODE.ENG_TO_KOR_FLASHCARD)
        {
            if (top)
            {
                flashcard_value.text = cur_card.eng_side;
                flashcard_context_1.text = cur_card.krn_context;
                flashcard_context_2.text = "";
                cur_state = STATE.SHOWING_QUESTION;
            }
            else
            {
                flashcard_value.text = cur_card.krn_side;
                flashcard_context_1.text = cur_card.krn_context;
                flashcard_context_2.text = cur_card.eng_context;
                cur_state = STATE.SHOWING_ANSWER;
            }
            
        }
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

    private void set_flashcard_panel_string(string string_game_object_name, string value)
    {
        GameObject top_string = flashcard_panel.transform.Find(string_game_object_name).gameObject;
        if (top_string != null)
        {
            top_string.GetComponent<Text>().text = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        prompt.text = stringToEdit;
        get_input();
    }

    void toggle_known()
    {
        cur_card.known = !cur_card.known;
        cur_card_known.isOn = cur_card.known;
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
                if (!review_known_cards)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (!cards[i].known)
                        {
                            cur_card_index = i;
                            break;
                        }
                    }
                }
                else
                {
                    cur_card_index = 0;
                }
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
        if (cur_card_index == cards.Count - 1)
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
